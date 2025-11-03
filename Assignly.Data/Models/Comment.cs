using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public ICollection<Attachment>? Attachments { get; set; } = new HashSet<Attachment>();
    }
}
