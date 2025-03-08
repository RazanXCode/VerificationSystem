using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentVerification.Models
{
    public class VerificationLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int VerifiedBy { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }

        [ForeignKey("VerifiedBy")]
        public User User { get; set; }
    }
}