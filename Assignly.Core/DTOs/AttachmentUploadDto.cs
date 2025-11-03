using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignly.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Assignly.Core.DTOs
{
    public class AttachmentUploadDto
    {
        public AttachmentTypeEnum AttachmentType { get; set; }
        public IFormFile? File { get; set; }
        public string? Link { get; set; }
        public Guid? TaskId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
