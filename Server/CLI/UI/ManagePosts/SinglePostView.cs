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

    public async Task<Post> StartAsync()
    {
        int userChoice = 0;
            Console.WriteLine("Select a post to view by Id");
            Console.WriteLine("0. Exit");
            userChoice = int.Parse(Console.ReadLine()!);
            Post post = await GetSinglePost(userChoice);
        return await Task.FromResult(post);
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
        return await Task.FromResult(post);
    }
    public async Task<Post> GetSinglePostNoOutput(int postId)
    {
        Post post = await postRepository.GetSingleAsync(postId);
        return await Task.FromResult(post);
    }
}