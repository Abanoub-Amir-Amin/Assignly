using Assignly.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Assignly.Data.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? FileType { get; set; } // e.g., "image/png", "application/pdf"
        [EnumDataType(typeof(AttachmentTypeEnum))]
        public AttachmentTypeEnum AttachmentType { get; set; } //File, Link
        public string? FilePath { get; set; }
        public string? LinkUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }

    }
}
