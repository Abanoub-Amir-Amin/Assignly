using Assignly.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace Assignly.Service.Services
{
    public interface IAttachmentService
    {
        Task<Attachment> CreateFileAttachmentAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            long fileSize,
            Guid? taskId = null,
            Guid? commentId = null
        );
        Task<Attachment> CreateLinkAttachmentAsync(
            string link,
            Guid? taskId = null,
            Guid? commentId = null
        );
        Task<Attachment?> GetAttachmentByIdAsync(Guid attachmentId);
        Task<(byte[] fileData, string fileName, string contentType)?> GetFileDataAsync(
            Guid attachmentId
        );

        Task<List<Attachment>> GetAttachmentsByTaskIdAsync(Guid taskId);
        Task<List<Attachment>> GetAttachmentsByCommentIdAsync(Guid commentId);
        Task<bool> DeleteAttachmentByIdAsync(Guid attachmentId);
    }
}
