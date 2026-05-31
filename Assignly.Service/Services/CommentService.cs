using Assignly.Core.DTOs.CommentDTOs;
using Assignly.Core.DTOs.Results;
using Assignly.Data.Models;
using Assignly.Infrastructure.Repositories;
using AutoMapper;

namespace Assignly.Service.Services;

public class CommentService(ICommentRepository commentRepository, IMapper mapper) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CommentResponse>> Create(CommentRequest request)
    {
        var comment = _mapper.Map<Comment>(request);
        await _commentRepository.Add(comment);
        return Result<CommentResponse>.Success(_mapper.Map<CommentResponse>(comment));
    }

    public async Task<Result<CommentResponse>> Delete(Guid id)
    {
        var comment = await _commentRepository.GetById(id);
        if (comment == null)
        {
            return Result<CommentResponse>.Failure("Comment not found", 404);
        }
        _commentRepository.Delete(id);
        await _commentRepository.SaveChangesAsync();
        return Result<CommentResponse>.Success(null);
    }

    public async Task<IEnumerable<Result<CommentResponse>>> GetAllByTaskId(Guid taskId)
    {
        var comments = await _commentRepository.GetAll();
        var filteredComments = comments.Where(c => c.TaskId == taskId);
        return filteredComments.Select(c =>
            Result<CommentResponse>.Success(_mapper.Map<CommentResponse>(c))
        );
    }

    public async Task<Result<CommentResponse>> GetById(Guid id)
    {
        var comment = await _commentRepository.GetById(id);
        if (comment == null)
        {
            return Result<CommentResponse>.Failure("Comment not found", 404);
        }
        return Result<CommentResponse>.Success(_mapper.Map<CommentResponse>(comment));
    }

    public async Task<Result<CommentResponse>> Update(Guid id, CommentRequest request)
    {
        var comment = await _commentRepository.GetById(id);
        if (comment == null)
        {
            return Result<CommentResponse>.Failure("Comment not found", 404);
        }
        var newComment = _mapper.Map(request, comment);
        _commentRepository.Update(newComment);
        await _commentRepository.SaveChangesAsync();
        return Result<CommentResponse>.Success(_mapper.Map<CommentResponse>(newComment));
    }
}
