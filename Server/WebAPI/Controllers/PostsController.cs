using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class PostsController :ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    
    public PostsController(IPostRepository postRepository)
        {
        this.postRepository = postRepository;
        }
    
    [HttpPost] 
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request) { 

        Post post = new ()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
        }; 
        Post created = await postRepository.AddAsync(post);

        PostDto dto = new(created.Id, created.Title, created.Body, created.UserId);
        return Created($"/posts/{dto.Id}", created);
    }
    [HttpPut] 
    public async Task<ActionResult> UpdatePost([FromBody] UpdatePostDto request) { 

        Post post = new ()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
        };
        try
        {
            await postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpDelete] 
    public async Task<ActionResult> DeletePostAsync([FromBody] DeletePostDto request) { 
        try
        {
            Post post = await postRepository.GetSingleAsync(request.Id);
            IQueryable<Comment> comments = commentRepository.GetManyAsync();
            List<Comment> postComments = comments.Where(c => c.PostId == request.Id).ToList();
            foreach (Comment comment in postComments)
            {
              await commentRepository.DeleteAsync(comment.Id);
            }
            await postRepository.DeleteAsync(post.Id);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet]
    public async Task<IResult> GetPostsAsync( [FromQuery] string? nameContains)
    {
        IQueryable<Post> posts =  postRepository.GetManyAsync();
        List<PostDto> postDtos = posts.Select(p => new PostDto(p.Id, p.Title, p.Body, p.UserId)).ToList();
        return Results.Ok(postDtos);
    }
}