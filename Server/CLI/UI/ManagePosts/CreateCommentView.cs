using System.Reflection.Metadata.Ecma335;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }
    public async Task<Comment> CreateCommentAsync(string body, int userId, int postId)
    {
        Comment comment = new()
        {
            Body = body,
            PostId = postId,
            UserId = userId
        };
        Comment created = await commentRepository.AddAsync(comment);
        return await Task.FromResult(created);
    }

    public async Task StartAsync(int userId,int postId)
    {
        Console.WriteLine("Enter the body of the comment");
        string? body = Console.ReadLine();
        await CreateCommentAsync(body, userId, postId);
        await Task.CompletedTask;
    }
}