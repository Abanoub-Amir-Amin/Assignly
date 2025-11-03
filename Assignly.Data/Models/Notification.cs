using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
