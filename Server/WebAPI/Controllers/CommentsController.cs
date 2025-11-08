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
    private readonly IUserRepository _userRepository;

    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this._commentRepository = commentRepository;
        this._userRepository = userRepository;
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
        User user = await _userRepository.GetSingleAsync(request.UserId);
        Comment created = await _commentRepository.AddAsync(comment);

        CommentDto dto = new(created.Id, created.Body, user.Username, created.PostId);
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
        IQueryable<User> users = _userRepository.GetManyAsync();
        List<CommentDto> commentDtos = new List<CommentDto>();
        foreach (Comment comment in comments)
        {
            User user = users.SingleOrDefault(u => u.Id == comment.UserId);
            commentDtos.Add(new CommentDto(comment.Id,comment.Body,user.Username,comment.PostId));
        }
        return Results.Ok(commentDtos);
    }
    [HttpGet("post/{postId}")]
    public async Task<IResult> GetCommentsFromPostAsync([FromQuery] int postId)
    {
        IQueryable<Comment> comments = _commentRepository.GetManyAsync();
        IQueryable<User> users = _userRepository.GetManyAsync();
        List<CommentDto> commentDtos = new List<CommentDto>();
        foreach (Comment comment in comments)
        {
            User user = users.SingleOrDefault(u => u.Id == comment.UserId);
            commentDtos.Add(new CommentDto(comment.Id,comment.Body,user.Username,comment.PostId));
        }
        return Results.Ok(commentDtos);
    }

    [HttpGet("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int commentId)
    {
        Comment comment = await _commentRepository.GetSingleAsync(commentId);
        User user = await _userRepository.GetSingleAsync(comment.UserId);
        return await Task.FromResult(new CommentDto(comment.Id,comment.Body,user.Username,comment.PostId));
    }
}