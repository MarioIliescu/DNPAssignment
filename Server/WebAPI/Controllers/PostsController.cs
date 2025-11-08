using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class PostsController :ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    
    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this._postRepository = postRepository;
        this._commentRepository = commentRepository;
        this._userRepository = userRepository;
    }
    
    [HttpPost] 
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request) { 

        Post post = new ()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
        }; 
        User user = await _userRepository.GetSingleAsync(request.UserId);
        Post created = await _postRepository.AddAsync(post);

        PostDto dto = new(created.Id, created.Title, created.Body, user.Username);
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
            await _postRepository.UpdateAsync(post);
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
            Post? post = await _postRepository.GetSingleAsync(request.Id);

            IQueryable<Comment> comments = _commentRepository.GetManyAsync();
            List<Comment> postComments = comments.Where(c => c.PostId == request.Id).ToList();

            foreach (Comment comment in postComments)
            {
                await _commentRepository.DeleteAsync(comment.Id);
            }

            await _postRepository.DeleteAsync(post.Id); 
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
        IQueryable<Post> posts =  _postRepository.GetManyAsync();
        IQueryable<User> users = _userRepository.GetManyAsync();
        List<PostDto> postDtos = new List<PostDto>();
        foreach (Post post in posts)
        {
            User user = users.SingleOrDefault(u => u.Id == post.UserId);
            postDtos.Add(new PostDto(post.Id, post.Title, post.Body, user.Username));
        }
        return Ok(postDtos);
    }
    [HttpGet("{postId}")]
    public async Task<ActionResult<PostWithCommentsDto>> GetSinglePost(int postId)
    {
        // Get the post
        Post post = await _postRepository.GetSingleAsync(postId);
        // Get comments for the post
        List<Comment> allComments = _commentRepository.GetManyAsync().ToList(); 
        List<Comment> postComments = allComments.Where(c => c.PostId == postId).ToList();
        List<User> users = _userRepository.GetManyAsync().ToList();
        List<CommentDto> commentDtos= new List<CommentDto>();
        // Map to DTOs
        foreach (Comment comment in postComments)
        {
            User user = users.SingleOrDefault(u => u.Id == comment.UserId);
            commentDtos.Add(new CommentDto(comment.Id, comment.Body, user.Username, comment.PostId));
        }

        // Create PostWithCommentsDto
        User postWriter = users.SingleOrDefault(u => u.Id == post.UserId);
        PostWithCommentsDto dto = new(
            new PostDto(post.Id, post.Title, post.Body, postWriter.Username),
            commentDtos
        );

        return Ok(dto);
    }

}