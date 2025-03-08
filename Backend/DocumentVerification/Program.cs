

using Microsoft.EntityFrameworkCore;
using DocumentVerification.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using DocumentVerification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Required for IFormFile
using System.IO; // Required for file handling


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
            policy.WithOrigins("http://localhost:4200") // Corrected port number (Angular default is 4200)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAngularApp");

// Populate data to database 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyAppContext>();
    context.Database.Migrate(); // Ensures the database is created and up to date
    MyAppContext.Seed(context); // Call the Seed method
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
    var file = form.Files["file"]; // Get the uploaded file
    var name = form["name"]; // Get the document name
    var userId = form["userId"]; // Get the user ID (if provided)

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
    var request = await httpContext.Request.ReadFromJsonAsync<VerificationRequest>();
    if (request is null)
        return Results.BadRequest("Invalid request data");

    // Query to get the document's verification status and count by matching VerificationCode
    string query = @"
        SELECT Status
        FROM Documents 
        WHERE Id = @Id AND VerificationCode = @VerificationCode";

    var status = await conn.ExecuteScalarAsync<string>(query, new { request.Id, request.VerificationCode });

    if (status == null)
        return Results.Json(new { message = "Invalid verification code or document not found." }, statusCode: 404); // Not Found

    if (status == "Verified")
        return Results.Ok("Document verified successfully!");

    return Results.Json(new { message = "Document verification failed." }, statusCode: 401); // Unauthorized
});

// Run the application
app.Run();

// Request model for document verification
record VerificationRequest(int Id, string VerificationCode);
