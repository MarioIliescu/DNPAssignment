using Entities;
using RepositoryContracts;
namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private Post selected;
    public ManagePostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    public async Task StartAsync(int userId, int postId)
    {
        int? userChoice = 0;
        do
        {
            if (await ViewSinglePostAsync(postId) == null)
            {
                Console.WriteLine("Post Not Found");
                break;
            }
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Update a post");
            Console.WriteLine("2. Delete post");
            Console.WriteLine("3. Exit");
            userChoice = Convert.ToInt32(Console.ReadLine());
                switch (userChoice)
                {
                    case 1:
                    {
                        Console.WriteLine("Enter a title for the post:");
                        string? title = Console.ReadLine();
                        Console.WriteLine("Enter a body for the post:");
                        string? body = Console.ReadLine();
                        Post? post = new()
                        {
                            Title = title,
                            Body = body,
                            UserId = userId,
                            Id = postId
                        };
                        await postRepository.UpdateAsync(post);
                        break;
                    }
                    case 2:
                    {
                        Console.WriteLine("Select an option:");
                        Console.WriteLine("1. Confirm deletion");
                        Console.WriteLine("2. Cancel deletion");
                        int? choice = Convert.ToInt32(Console.ReadLine());
                        if (choice == 1)
                        {
                            await SudoDeletePost(postId);
                        }

                        break;
                    }
                    default:
                        Console.WriteLine("Try again");
                        break;
                    case 3: break;
                }
        } while (userChoice != 3);
    
        await Task.CompletedTask;
    }
    public async Task SudoStartAsync(int userId, int postId)
    {
        int? userChoice = 0;
        do
        {
            if (await ViewSinglePostAsync(postId) == null)
            {
                Console.WriteLine("Post Not Found");
                break;
            }
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Update a post");
            Console.WriteLine("2. Delete post");
            Console.WriteLine("3. Exit");
            userChoice = Convert.ToInt32(Console.ReadLine());
                switch (userChoice)
                {
                    case 1:
                    {
                        Console.WriteLine("Enter a title for the post:");
                        string? title = Console.ReadLine();
                        Console.WriteLine("Enter a body for the post:");
                        string? body = Console.ReadLine();
                        Post? post = new()
                        {
                            Title = title,
                            Body = body,
                            UserId = userId,
                            Id = postId
                        };
                        await postRepository.UpdateAsync(post);
                        break;
                    }
                    case 2:
                    {
                        Console.WriteLine("Select an option:");
                        Console.WriteLine("1. Confirm deletion");
                        Console.WriteLine("2. Cancel deletion");
                        int? choice = Convert.ToInt32(Console.ReadLine());
                        if (choice == 1)
                        {
                            await SudoDeletePost(postId);
                        }

                        break;
                    }
                    default:
                        Console.WriteLine("Try again");
                        break;
                    case 3: break;
                }
        } while (userChoice != 3);
    
        await Task.CompletedTask;
    }

    public async Task<Post> ViewSinglePostAsync(int postId)
    {
        Post post = await postRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found");
        }
        return await Task.FromResult(post);
    }

    public async Task DeletePost(int postId, int userId)
    {
        Post post = await ViewSinglePostAsync(postId);
        if (post.UserId != userId)
        {
            Console.WriteLine("Unable to delete posts of other users");
        }
        else
        {
            await postRepository.DeleteAsync(postId);
        }
        await Task.CompletedTask;
    }
    public async Task SudoDeletePost(int postId)
    {
        await postRepository.DeleteAsync(postId);
        await Task.CompletedTask;
    }
}