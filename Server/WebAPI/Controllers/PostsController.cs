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
    
    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
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
        return Created($"/posts/{dto.Id}", dto);

    }
    [HttpPut] 
    public async Task<ActionResult> UpdatePost([FromBody] UpdatePostDto request) { 

        Post post = new ()
        {
            Id = request.Id,
            Title = request.Title,
            Body = request.Body,
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
    [HttpDelete("{postId}")] 
    public async Task<ActionResult> DeletePostAsync([FromBody] DeletePostDto request)
    {
        try
        {
            Post? post = await postRepository.GetSingleAsync(request.Id);
            if (post == null)
                return StatusCode(404, "Post not found");

            IQueryable<Comment> comments = commentRepository.GetManyAsync();
            List<Comment> postComments = comments.Where(c => c.PostId == request.Id).ToList();

            foreach (Comment comment in postComments)
            {
                await commentRepository.DeleteAsync(comment.Id);
            }

            await postRepository.DeleteAsync(post.Id); // now post.Id is correct
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet]
    public async Task<ActionResult> GetPostsAsync( [FromQuery] string? nameContains)
    {
        IQueryable<Post> posts =  postRepository.GetManyAsync();
        List<PostDto> postDtos = posts.Select(p => new PostDto(p.Id, p.Title, p.Body, p.UserId)).ToList();
        return Ok(postDtos);
    }
    [HttpGet("{postId}")]
    public async Task<ActionResult<PostWithCommentsDto>> GetSinglePost(int postId)
    {
        Post post = await postRepository.GetSingleAsync(postId);
        if (post == null)
        {
            return StatusCode(404, "Post not found");
        }
        IQueryable<Comment> comments = commentRepository.GetManyAsync();
        List<Comment> postComments = comments.Where(c => c.PostId == postId).ToList();
        List<CommentDto> commentDtos = postComments.Select(c => new CommentDto(c.Id,c.Body,c.UserId,c.PostId)).ToList();
        PostWithCommentsDto dto = new(new PostDto(post.Id, post.Title, post.Body, post.UserId), commentDtos);
        return Ok(dto);
    }
}