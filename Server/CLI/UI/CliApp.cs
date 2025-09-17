using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        AddDummyData();
    }

    public async Task StartAsync()
    {
        string? userInput;
            Console.WriteLine("Select an option:");
            Console.WriteLine("user: Register as User");
            Console.WriteLine("sudo: Register as SuperUser");
            Console.WriteLine("Any other: Exit");
            userInput = Console.ReadLine();
            Console.WriteLine("Enter username:");
            string? userName = Console.ReadLine();
            Console.WriteLine("Enter password");
            string? password = Console.ReadLine();
            User user = new()
            {
                Username = userName,
                Password = password
            };
            User? currentUser = await userRepository.AddAsync(user);
            switch (userInput)
            {
                case "user": break;
                case "sudo": await HandleSuperUserAsync(currentUser.Id);
                    break;
                default: break;
            }
        
        await Task.CompletedTask;
    }

    private void AddDummyData()
    {
        for (int i = 1; i <= 5; i++)
        {
            userRepository.AddAsync(new User()
            {
                Username = $"User{i}",
                Password = "pass",
                Id = i
            });
            postRepository.AddAsync(new Post()
            {
                Body = $"Post {i}",
                Id = i,
                Title = $"Title{i}",
                UserId = i
            });
            commentRepository.AddAsync(new Comment()
            {
                Body = $"Comment {i}",
                Id = i,
                PostId = i
            });
        }
    }

    private async Task HandleChoiceUserAsync(CreateUserView userView, ListUserView listUserView, ManageUsersView manageUsersView)
    {
        int choiceInput = 0;
        do
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1. Create an user");
            Console.WriteLine("2. List all users");
            Console.WriteLine("3. Manage a user");
            Console.WriteLine("4. Go back");
            choiceInput = Convert.ToInt32(Console.ReadLine());
            switch (choiceInput)
            {
                case 1 : await userView.StartAsync();
                    break;
                case 2 : await listUserView.StartAsync();
                    break;
                case 3 : await manageUsersView.StartAsync();
                    break;
                case 4 : break;
                default: Console.WriteLine("Input not recognized, try again");
                    break;
            } 
        } while (choiceInput.Equals(4));
    }

    private async Task HandleSuperUserAsync(int currentUserId)
    {
        CreatePostView postView = new(postRepository);
        CreateUserView userView = new(userRepository);
        ListUserView listUserView = new ListUserView(userRepository);
        ManageUsersView manageUsersView = new ManageUsersView(userRepository);
        int userInput = 0;
        do
        {
            Console.WriteLine("Select an option by inserting the corresponding number, after insertion enter");
            Console.WriteLine("1. Posts");
            Console.WriteLine("2. Users");
            Console.WriteLine("3. Exit");
            userInput = Convert.ToInt32(Console.ReadLine());
            switch (userInput)
            {
                case 1: break;
                case 2:await HandleChoiceUserAsync(userView, listUserView, manageUsersView);
                    break;
                default:
                {
                    Console.WriteLine("Input not recognized, try again");
                    break;
                }
                case 3: break;
            }

        } while (userInput!=3);
    }
}