using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentVerification.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required, MaxLength(255)]
        public string Title { get; set; }
        
        [Required, MaxLength(500)]
        public string FilePath { get; set; }
        
        [MaxLength(100)]
        public string VerificationCode { get; set; }
        
        [Required, MaxLength(50)]
        public string Status { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }

        // One to many relationship with the VerficationLog table 
        public List<VerificationLog> VerificationLogs { get; set; }
    }
}
