namespace Assignly.Core.DTOs.AttachmentDTOs;

public class AttachmentResponseDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string? FileType { get; set; } // e.g., "image/png", "application/pdf"
    public string AttachmentType { get; set; } //File, Link
    public string? LinkUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string? DownloadLink { get; set; }
}
