using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignly.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignly.Infrastructure.Repositories
{
    internal class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        private readonly AppDBContext _context;

        public AttachmentRepository(AppDBContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Attachment>> GetAttachmentsByCommentIdAsync(Guid commentId)
        {
            var attachments = await _context
                .Attachments.Where(a => a.CommentId == commentId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
            return attachments;
        }

        public Task<List<Attachment>> GetAttachmentsByTaskIdAsync(Guid taskId)
        {
            var attachments = _context
                .Attachments.Where(a => a.TaskId == taskId)
                .OrderByDescending(a => a.UploadedAt)
                .ToListAsync();
            return attachments;
        }
    }
}
