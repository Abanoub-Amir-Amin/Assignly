using Assignly.Data.Enums;
using Assignly.Data.Models;
using Assignly.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Assignly.Service.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly AppDBContext _context;
        private readonly string _uploadsPath;

        public AttachmentService(AppDBContext context, IConfiguration config)
        {
            _context = context;
            var uploadsPath =
                config["FileStorage:UploadPath"]
                ?? throw new InvalidOperationException(
                    "FileStorage:UploadsPath configuration is missing."
                );
            _uploadsPath = Path.IsPathRooted(uploadsPath)
                ? uploadsPath
                : Path.Combine(Directory.GetCurrentDirectory(), uploadsPath);
            Directory.CreateDirectory(_uploadsPath);
        }

        public async Task<Attachment> CreateFileAttachmentAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            long fileSize,
            Guid? taskId = null,
            Guid? commentId = null
        )
        {
            if (fileStream == null || fileSize == 0) // Checking for file presence
            {
                throw new ArgumentException("File is required.");
            }
            const long maxFileSize = 2 * 1024 * 1024; // Limiting file size to 2 MB

            if (fileSize > maxFileSize)
            {
                throw new ArgumentException("File size exceeds the 2 MB limit.");
            }

            var attachment = new Attachment // Creating attachment entity to be saved in DB
            {
                FileName = fileName,
                FileType = contentType,
                AttachmentType = AttachmentTypeEnum.File,
                TaskId = taskId,
                CommentId = commentId,
                UploadedAt = DateTime.UtcNow,
            };

            // Generate file to be saved physically on disk
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{attachment.Id}{extension}";
            var filePath = Path.Combine(_uploadsPath, uniqueFileName);

            using (var fileStreamToDisk = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamToDisk);
            }

            // saving file in DB
            attachment.FilePath = filePath;

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<Attachment> CreateLinkAttachmentAsync(
            string link,
            Guid? taskId = null,
            Guid? commentId = null
        )
        {
            if (String.IsNullOrWhiteSpace(link))
            {
                throw new ArgumentException("Link is required.");
            }
            if (
                !Uri.TryCreate(link, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            )
            {
                throw new ArgumentException("Link is not a valid URL.");
            }

            var attachment = new Attachment
            {
                LinkUrl = link,
                FileName = uri.Host,
                AttachmentType = AttachmentTypeEnum.Link,
                TaskId = taskId,
                CommentId = commentId,
                UploadedAt = DateTime.UtcNow,
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<bool> DeleteAttachmentByIdAsync(Guid attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment == null)
            {
                return false;
            }
            if (
                // delete physucal file from disk.
                attachment.AttachmentType == AttachmentTypeEnum.File
                && !string.IsNullOrEmpty(attachment.FilePath)
                && File.Exists(attachment.FilePath)
            )
            {
                File.Delete(attachment.FilePath);
            }
            //delete file/link from DB.
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Attachment?> GetAttachmentByIdAsync(Guid attachmentId)
        {
            return await _context.Attachments.FindAsync(attachmentId);
        }

        public async Task<List<Attachment>> GetAttachmentsByCommentIdAsync(Guid commentId)
        {
            return await _context
                .Attachments.Where(a => a.CommentId == commentId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
        }

        public async Task<List<Attachment>> GetAttachmentsByTaskIdAsync(Guid taskId)
        {
            return await _context
                .Attachments.Where(a => a.TaskId == taskId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
        }

        public async Task<(byte[] fileData, string fileName, string contentType)?> GetFileDataAsync(
            Guid attachmentId
        )
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);

            if (
                attachment == null
                || attachment.AttachmentType != AttachmentTypeEnum.File
                || string.IsNullOrEmpty(attachment.FilePath)
                || !File.Exists(attachment.FilePath)
            )
            {
                return null;
            }
            var fileData = await File.ReadAllBytesAsync(attachment.FilePath);
            var contentType = attachment.FileType ?? "application/octet-stream";
            return (fileData, attachment.FileName, contentType);
        }
    }
}
