using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignly.Data.Models;

namespace Assignly.Infrastructure.Repositories
{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        public Task<List<Attachment>> GetAttachmentsByCommentIdAsync(Guid commentId);

        public Task<List<Attachment>> GetAttachmentsByTaskIdAsync(Guid taskId);
    }
}
