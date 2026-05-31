using Assignly.Core.DTOs.CommentDTOs;
using Assignly.Core.DTOs.Results;
using Assignly.Data.Models;

namespace Assignly.Service.Services;

public interface ICommentService
{
    Task<Result<CommentResponse>> GetById(Guid id);
    Task<IEnumerable<Result<CommentResponse>>> GetAllByTaskId(Guid taskId);
    Task<Result<CommentResponse>> Create(CommentRequest request);
    Task<Result<CommentResponse>> Update(Guid id, CommentRequest request);
    Task<Result<CommentResponse>> Delete(Guid id);
}
