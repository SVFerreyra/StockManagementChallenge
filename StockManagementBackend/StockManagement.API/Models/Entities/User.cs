using System.ComponentModel.DataAnnotations;

namespace StockManagement.API.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}