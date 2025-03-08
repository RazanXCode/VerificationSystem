
using System.ComponentModel.DataAnnotations;

namespace DocumentVerification.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        // One to many relationship with the Document table 
        public List<Document> Documents { get; set; }

           // One to many relationship with the VerficationLog table 
        public List<VerificationLog> VerificationLogs { get; set; }

    }
}