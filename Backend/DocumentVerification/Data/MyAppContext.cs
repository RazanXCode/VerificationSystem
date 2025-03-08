using Microsoft.EntityFrameworkCore;
using DocumentVerification.Models;

namespace DocumentVerification.Data
{
    public class MyAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<VerificationLog> VerificationLogs { get; set; }

        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the VerificationLog entity
            modelBuilder.Entity<VerificationLog>()
                .HasOne(vl => vl.Document) // VerificationLog has one Document
                .WithMany() // Document can have many VerificationLogs
                .HasForeignKey(vl => vl.DocumentId) // Foreign key
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete

            modelBuilder.Entity<VerificationLog>()
                .HasOne(vl => vl.User) // VerificationLog has one User
                .WithMany() // User can have many VerificationLogs
                .HasForeignKey(vl => vl.VerifiedBy) // Foreign key
                .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete
        }

        // Seed method to populate sample data
        public static void Seed(MyAppContext context)
        {
            if (!context.Users.Any())
            {
                var user1 = new User
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Password = "password123",  
                    Role = "Admin",
                };

                var user2 = new User
                {
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Password = "password456",
                    Role = "User",
                };

                context.Users.AddRange(user1, user2);
                context.SaveChanges();

                // Seed documents
                var document1 = new Document
                {
                    UserId = user1.Id,
                    Title = "Identity Proof",
                    FilePath = "/documents/identity_proof.pdf",
                    VerificationCode = "ABC123",
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                };

                var document2 = new Document
                {
                    UserId = user2.Id,
                    Title = "Address Proof",
                    FilePath = "/documents/address_proof.pdf",
                    VerificationCode = "XYZ789",
                    Status = "Verified",
                    CreatedAt = DateTime.Now.AddDays(-1),
                };

                context.Documents.AddRange(document1, document2);
                context.SaveChanges();

                // Seed verification logs
                var verificationLog1 = new VerificationLog
                {
                    DocumentId = document2.Id,
                    VerifiedBy = user1.Id,
                    Timestamp = DateTime.Now,
                    Status = "Verified",
                };

                context.VerificationLogs.Add(verificationLog1);
                context.SaveChanges();
            }

        }
    }
}