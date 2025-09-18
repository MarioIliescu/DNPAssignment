using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;
    private User? selectedUser;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<User?> ViewSingleUserAsync(int id)
    {
        selectedUser = await userRepository.GetSingleAsync(id);
        return await Task<User>.FromResult(selectedUser);
    }

    public async Task<User> UpdateUserAsync(string name, string password)
    {
        User? user = selectedUser;
        user.Username = name;
        user.Password = password;
        await userRepository.UpdateAsync(user);
        return await Task.FromResult(user);
    }

    public async Task DeleteUserAsync()
    {
        if (selectedUser != null) await userRepository.DeleteAsync(selectedUser.Id);
        await Task.CompletedTask;
    }

    public async Task StartAsync()
    {
        int selectedUserId = 0;
        int? userChoice = 0;
        do
        {
            Console.WriteLine("Select a user you want to manage by inserting id");
            Console.WriteLine("Enter 0 to exit");
            selectedUserId = int.Parse(Console.ReadLine()!);
            if (selectedUserId == 0)
            {
                break;
            }
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Update user with new username and password");
            Console.WriteLine("2. Delete user");
            Console.WriteLine("3. Exit");
            userChoice = Convert.ToInt32(Console.ReadLine());
            await ViewSingleUserAsync(selectedUserId);
            switch (userChoice)
            {
                case 1:
                {
                    var name = selectedUser?.Username;
                    var password = selectedUser?.Password;
                    Console.WriteLine("Name: ");
                    name = Console.ReadLine();
                    Console.WriteLine("Password: ");
                    password = Console.ReadLine();
                    User? user = new ()
                    {
                        Id = selectedUser.Id,
                        Password = password,
                        Username = name
                    };
                    await userRepository.UpdateAsync(user);
                    break;
                }
                case 2:
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Confirm deletion");
                    Console.WriteLine("2. Cancel deletion");
                    int? choice = Convert.ToInt32(Console.ReadLine());
                    if (choice==1)
                    {
                        await userRepository.DeleteAsync(selectedUserId);
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
}