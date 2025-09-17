using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        int userChoice = 0;
            Console.WriteLine("Select a post to view by Id");
            Console.WriteLine("0. Exit");
            userChoice = int.Parse(Console.ReadLine()!);
            GetSinglePost(userChoice);
        await Task.CompletedTask;
    }

    public async Task<Post> GetSinglePost(int postId)
    {
        Post post = await postRepository.GetSingleAsync(postId);
        IQueryable<Comment> comments = commentRepository.GetManyAsync();
        List<Comment> postComments = comments.Where(c => c.PostId == postId).ToList();
        Console.WriteLine(post.FullPostToString());
        Console.WriteLine("Comments:");
        foreach (Comment comment in postComments)
        {
            Console.WriteLine(comment.ToString());
        }

        return post;
    }
}