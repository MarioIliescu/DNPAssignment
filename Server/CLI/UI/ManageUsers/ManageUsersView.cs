using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;
    private User selectedUser;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<User> ViewSingleUserAsync(int id)
    {
        selectedUser = await userRepository.GetSingleAsync(id);
        return await Task<User>.FromResult(selectedUser);
    }

    public async Task<User> UpdateUserAsync(string name, string password)
    {
        User user = selectedUser;
        user.Username = name;
        user.Password = password;
        await userRepository.UpdateAsync(user);
        return await Task<User>.FromResult(user);
    }

    public async Task DeleteUserAsync()
    {
        await userRepository.DeleteAsync(selectedUser.Id);
        await Task.CompletedTask;
    }
}