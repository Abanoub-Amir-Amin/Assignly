using System.ComponentModel.DataAnnotations;
using Assignly.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Assignly.Core.DTOs.AttachmentDTOs;

public class AttachmentUploadDto
{
    [EnumDataType(typeof(AttachmentTypeEnum))]
    public AttachmentTypeEnum AttachmentType { get; set; }
    public IFormFile? File { get; set; }
    public string? Link { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? CommentId { get; set; }
}
