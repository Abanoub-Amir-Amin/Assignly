using System.ComponentModel.DataAnnotations;
using Assignly.Core.DTOs.AttachmentDTOs;
using Assignly.Data.Models;

namespace Assignly.Core.DTOs.CommentDTOs;

public class CommentResponse
{
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public Guid TaskId { get; set; }
    public ICollection<AttachmentUploadDto>? Attachments { get; set; } =
        new HashSet<AttachmentUploadDto>();
}
