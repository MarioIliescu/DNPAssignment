using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostView
{
    private readonly IPostRepository postRepository;

    public ListPostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
            int userChoice = 0;
            do
            {
                Console.WriteLine("Select an option");
                Console.WriteLine("1. View all posts");
                Console.WriteLine("2. View all posts by a user");
                Console.WriteLine("3. Exit");
                userChoice = int.Parse(Console.ReadLine()!);
                switch (userChoice)
                {
                    case 1:
                    {
                        List <Post> posts= await ViewAllPostsAsync();
                        foreach (var post in posts)
                        {
                            Console.WriteLine(post);
                        }
                        break;
                    }
                    case 2:
                    {
                        Console.WriteLine("Insert User Id");
                        int id = Convert.ToInt32(Console.ReadLine());
                        List <Post> posts= await ViewPostsByUserAsync(id);
                        foreach (var post in posts)
                        {
                            Console.WriteLine(post);
                        }
                        break;
                    }
                    default:
                        Console.WriteLine("Try again");
                        break;
                    case 3: break;
                }
            } while (userChoice!=3);
    }
    public async Task<List<Post>> ViewAllPostsAsync()
    {
        IQueryable<Post> postList = postRepository.GetManyAsync();
        List<Post> list = postList.ToList();
        return await Task.FromResult(list);
    }

    public async Task<List<Post>> ViewPostsByUserAsync(int userId)
    {
        IQueryable<Post> postList = postRepository.GetManyAsync();
        List<Post> userList = postList.Where(p => p.UserId == userId).ToList();
        return await Task.FromResult(userList);
    }
}