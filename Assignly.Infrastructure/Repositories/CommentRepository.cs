using Assignly.Data.Models;

namespace Assignly.Infrastructure.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDBContext context)
        : base(context) { }
}
