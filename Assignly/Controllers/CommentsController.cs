using Assignly.Core.DTOs.CommentDTOs;
using Assignly.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        [HttpGet("get-comments/{taskId:alpha}")]
        public async Task<IActionResult> GetAll(string taskId)
        {
            var comments = await _commentService.GetAllByTaskId(new Guid(taskId));
            return Ok(comments);
        }

        [HttpGet("get/{id:alpha}")]
        public async Task<IActionResult> GetById(string id)
        {
            var comment = await _commentService.GetById(new Guid(id));
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CommentRequest request)
        {
            var result = await _commentService.Create(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update/{id:alpha}")]
        public async Task<IActionResult> Update(string id, CommentRequest request)
        {
            var result = await _commentService.Update(new Guid(id), request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id:alpha}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _commentService.Delete(new Guid(id));
            return StatusCode(result.StatusCode, result);
        }
    }
}
