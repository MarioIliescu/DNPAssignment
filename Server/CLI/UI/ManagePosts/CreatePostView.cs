using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        await Task.CompletedTask;
    }

    // public async Task CreateAsync()
    // {
    //     Post post = new()
    //     {
    //         
    //     }
    //     postRepository.AddAsync()
    // }
}