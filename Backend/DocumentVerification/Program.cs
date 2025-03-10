

using Microsoft.EntityFrameworkCore;
using DocumentVerification.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using DocumentVerification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register SqlConnection for Dapper
builder.Services.AddTransient<SqlConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();


app.UseCors("AllowAngularApp");

// Populate data to database 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyAppContext>();
    context.Database.Migrate(); 
    MyAppContext.Seed(context); 
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// POST /api/documents → Upload a document & generate a unique VerificationCode
app.MapPost("/api/documents", async (MyAppContext db, HttpContext httpContext) =>
{
    // Read the form data from the request
    var form = await httpContext.Request.ReadFormAsync();
    var file = form.Files["file"]; 
    var name = form["name"]; 
    var userId = form["userId"]; 

    if (file == null || string.IsNullOrEmpty(name))
    {
        return Results.BadRequest("File and name are required.");
    }

    // Validate the UserId
    if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int validUserId))
    {
        return Results.BadRequest("A valid UserId is required.");
    }

    // Check if the user exists
    var userExists = await db.Users.AnyAsync(u => u.Id == validUserId);
    if (!userExists)
    {
        return Results.BadRequest("The specified user does not exist.");
    }

    // Save the file to the uploads directory
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    var filePath = Path.Combine(uploadsDirectory, file.FileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    // Create the Document object
    var document = new Document
    {
        Title = name,
        FilePath = filePath, // Save the file path
        VerificationCode = Guid.NewGuid().ToString().Substring(0, 8), // Generate a unique code
        CreatedAt = DateTime.Now,
        Status = "Pending", // Set a default status (e.g., "Pending")
        UserId = validUserId // Set the UserId
    };

    // Save the document to the database
    db.Documents.Add(document);
    await db.SaveChangesAsync();

    return Results.Created($"/api/documents/{document.Id}", document);
});


// GET /api/documents/{id} → Retrieve document details
app.MapGet("/api/documents/{userId}", async (MyAppContext db, int userId) =>
{
    var documents = await db.Documents.Where(d => d.UserId == userId).ToListAsync();
    return documents.Any() ? Results.Ok(documents) : Results.NotFound("No documents found for this user.");
});


// POST /api/verify → Verify a document using Dapper
app.MapPost("/api/verify", async (HttpContext httpContext, SqlConnection conn) =>
{
    // Read the verification request from the body
    var request = await httpContext.Request.ReadFromJsonAsync<VerificationRequest>();
    if (request is null)
        return Results.BadRequest("Invalid request data");

    // Query to check the document's status using the VerificationCode and Document Id
    string query = @"
        SELECT Status
        FROM Documents 
        WHERE Id = @Id AND VerificationCode = @VerificationCode";

    // Retrieve the document's current status
    var status = await conn.ExecuteScalarAsync<string>(query, new { request.Id, request.VerificationCode });

    // If the document isn't found or the verification code is incorrect
    if (status == null)
        return Results.Json(new { message = "Invalid verification code or document not found." }, statusCode: 404); // Not Found

    // If the document is already verified
    if (status == "Verified")
        return Results.Ok("Document is already verified.");

    // Query to update the document status to 'Verified'
    string updateQuery = @"
        UPDATE Documents
        SET Status = 'Verified'
        WHERE Id = @Id AND VerificationCode = @VerificationCode";

    // Execute the update
    var rowsAffected = await conn.ExecuteAsync(updateQuery, new { request.Id, request.VerificationCode });

    if (rowsAffected > 0)
        return Results.Ok("Document verified successfully!");
    else
        return Results.Json(new { message = "Document verification failed." }, statusCode: 401); // Unauthorized
});



app.Run();


record VerificationRequest(int Id, string VerificationCode);
