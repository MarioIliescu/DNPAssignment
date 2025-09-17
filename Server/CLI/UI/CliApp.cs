using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
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
    }

    public async Task StartAsync()
    {
        CreatePostView postView = new(postRepository);
        CreateUserView userView = new(userRepository);
        ListUserView listUserView = new ListUserView(userRepository);
        ManageUsersView manageUsersView = new ManageUsersView(userRepository);
        string userInput = "";
        string choiceInput = "";
        do
        {
            Console.WriteLine("Select an option by inserting the corresponding number, after insertion enter");
            Console.WriteLine("1. Posts");
            Console.WriteLine("2. Users");
            Console.WriteLine("4. Exit");
            userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1": break;
                case "2":
                    do
                    {
                        Console.WriteLine("Select an option");
                        Console.WriteLine("1. Create an user");
                        Console.WriteLine("2. List all users");
                        Console.WriteLine("3. Manage a user");
                        Console.WriteLine("4. Go back");
                        choiceInput = Console.ReadLine();
                        switch (choiceInput)
                        {
                            case "1" : userView.StartAsync();
                                break;
                            case "2" : listUserView.StartAsync();
                                break;
                            case "3" : break;
                            case "4" : break;
                            default: Console.WriteLine("Input not recognized, try again");
                                break;
                        } 
                    } while (choiceInput.Equals("4"));
                    break;
                default:
                {
                    Console.WriteLine("Input not recognized, try again");
                    break;
                }
                case "4": break;
            }

        } while (!userInput.Equals("4"));

        await Task.CompletedTask;
    }
}