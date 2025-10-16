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
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
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
        Comment created = await commentRepository.AddAsync(comment);

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
            await commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCommentAsync([FromBody] DeleteCommentDto request)
    {
        try
        {
            Comment? comment = await commentRepository.GetSingleAsync(request.Id);
            if (comment == null)
            {
                return StatusCode(404, "Comment not found");
            }
            await commentRepository.DeleteAsync(comment.Id);
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
        IQueryable<Comment> comments = commentRepository.GetManyAsync();
        List<CommentDto> commentDtos = comments.Select(c => new CommentDto(c.Id,c.Body,c.UserId,c.PostId)).ToList();
        return Results.Ok(commentDtos);
    }

    [HttpGet("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int commentId)
    {
        Comment comment = await commentRepository.GetSingleAsync(commentId);
        if (comment == null)
        {
            return StatusCode(404, "Comment not found");
        }

        return await Task.FromResult(new CommentDto(comment.Id,comment.Body,comment.UserId,comment.PostId));
    }
}