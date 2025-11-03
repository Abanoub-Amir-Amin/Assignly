using Assignly.Core.DTOs;
using Assignly.Data.Enums;
using Assignly.Data.Models;
using Assignly.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttachmentAsync(
            AttachmentUploadDto attachmentUploadDto
        )
        {
            Attachment attachment;
            try
            {
                if (attachmentUploadDto.AttachmentType == AttachmentTypeEnum.File)
                {
                    if (attachmentUploadDto.File == null)
                    {
                        return BadRequest(
                            new { message = "File is required for file attachments." }
                        );
                    }
                    attachment = await _attachmentService.CreateFileAttachmentAsync(
                        attachmentUploadDto.File.OpenReadStream(),
                        attachmentUploadDto.File.FileName,
                        attachmentUploadDto.File.ContentType,
                        attachmentUploadDto.File.Length,
                        attachmentUploadDto.TaskId,
                        attachmentUploadDto.CommentId
                    );
                }
                else if (attachmentUploadDto.AttachmentType == AttachmentTypeEnum.Link)
                {
                    if (String.IsNullOrEmpty(attachmentUploadDto.Link))
                    {
                        return BadRequest(
                            new { message = "Link is required for link attachments." }
                        );
                    }

                    attachment = await _attachmentService.CreateLinkAttachmentAsync(
                        attachmentUploadDto.Link,
                        attachmentUploadDto.TaskId,
                        attachmentUploadDto.CommentId
                    );
                }
                else
                {
                    return BadRequest(new { message = "Invalid attachment type." });
                }
                var attachmentResponse = MapToDto(attachment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttachmentByIdAsync(Guid id)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
            {
                return NotFound(new { message = "Attachment not found." });
            }
            var attachmentResponse = MapToDto(attachment);
            return Ok(attachmentResponse);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var attachment = await _attachmentService.GetFileDataAsync(id);
            if (attachment == null)
            {
                return NotFound(new { message = "Attachment not found." });
            }
            return File(
                attachment.Value.fileData,
                attachment.Value.contentType,
                attachment.Value.fileName
            );
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskIdAsync(Guid taskId)
        {
            var attachments = await _attachmentService.GetAttachmentsByTaskIdAsync(taskId);
            var response = attachments.Select(MapToDto).ToList();
            return Ok(response);
        }

        [HttpGet("comment/{commentId}")]
        public async Task<IActionResult> GetByCommentId(Guid commentId)
        {
            var attachments = await _attachmentService.GetAttachmentsByCommentIdAsync(commentId);
            var response = attachments.Select(MapToDto).ToList();
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(Guid id)
        {
            var result = await _attachmentService.DeleteAttachmentByIdAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Attachment not found." });
            }
            return NoContent();
        }

        public AttachmentResponseDto MapToDto(Attachment attachment)
        {
            return new AttachmentResponseDto
            {
                Id = attachment.Id,
                FileName = attachment.FileName,
                FileType = attachment.FileType,
                AttachmentType = attachment.AttachmentType.ToString(),
                LinkUrl =
                    attachment.AttachmentType == AttachmentTypeEnum.Link
                        ? attachment.LinkUrl
                        : null,
                UploadedAt = attachment.UploadedAt,
                DownloadLink =
                    attachment.AttachmentType == AttachmentTypeEnum.File
                        ? $"/api/attachments/download/{attachment.Id}"
                        : null,
            };
        }
    }
}
