using System.Xml;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUserView
{
    private readonly IUserRepository userRepository;
    private List<User> users = new();
    public ListUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<List<User>> ViewAllUsersAsync()
    {
        IQueryable<User> userList = userRepository.GetManyAsync();
        List<User> list = userList.ToList();
        return await Task.FromResult(list);
    }

    public async Task<User> ViewSingleUserAsync(int id)
    {
        return await Task<User>.FromResult(await userRepository.GetSingleAsync(id));
        
    }

    public async Task StartAsync()
    {
        int userChoice = 0;
        do
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1. View all users");
            Console.WriteLine("2. Get a user by Id");
            Console.WriteLine("3. Exit");
            userChoice = int.Parse(Console.ReadLine()!);
            switch (userChoice)
            {
                case 1:
                {
                    List <User> users= await ViewAllUsersAsync();
                    foreach (var user in users)
                    {
                        Console.WriteLine(user);
                    }
                    break;
                }
                case 2:
                {
                    Console.WriteLine("Insert Id");
                    int id = int.Parse(Console.ReadLine()!);
                    Console.WriteLine(await ViewSingleUserAsync(id));
                    break;
                }
                default:
                    Console.WriteLine("Try again");
                    break;
                case 3: break;
            }
        } while (userChoice!=3);
    }
}