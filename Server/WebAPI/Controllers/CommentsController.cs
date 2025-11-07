using System.Net.Http.Headers;
using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this._commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        Comment comment = new()
        {
            PostId = request.PostId,
            Body = request.Body,
            UserId = request.UserId,
        };
        Comment created = await _commentRepository.AddAsync(comment);

        CommentDto dto = new(created.Id, created.Body, created.UserId, created.PostId);
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateComment([FromBody] UpdateCommentDto request)
    {
        Comment comment = new()
        {
            Id = request.Id,
            Body = request.Body,
        };
        try
        {
            await _commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{commentId}")]
    public async Task<ActionResult> DeleteCommentAsync([FromBody] DeleteCommentDto request)
    {
        try
        {
            Comment? comment = await _commentRepository.GetSingleAsync(request.Id);
            await _commentRepository.DeleteAsync(comment.Id);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<IResult> GetCommentsAsync([FromQuery] string? nameContains)
    {
        IQueryable<Comment> comments = _commentRepository.GetManyAsync();
        List<CommentDto> commentDtos = comments.Select(c => new CommentDto(c.Id,c.Body,c.UserId,c.PostId)).ToList();
        return Results.Ok(commentDtos);
    }
    [HttpGet("post/{postId}")]
    public async Task<IResult> GetCommentsFromPostAsync([FromQuery] int postId)
    {
        IQueryable<Comment> comments = _commentRepository.GetManyAsync();
        comments = comments.Where(c => c.PostId == postId);
        List<CommentDto> commentDtos = comments.Select(c => new CommentDto(c.Id,c.Body,c.UserId,c.PostId)).ToList();
        return Results.Ok(commentDtos);
    }

    [HttpGet("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int commentId)
    {
        Comment comment = await _commentRepository.GetSingleAsync(commentId);

        return await Task.FromResult(new CommentDto(comment.Id,comment.Body,comment.UserId,comment.PostId));
    }
}