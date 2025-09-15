using System;
using Entities;

namespace CLI.UI.ManagePosts;


public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task CreatePost()
    {
        bool Active = true;
        while (Active)
        {
            Console.WriteLine("Write the User ID as poster");
            int? UserIdInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Write your Post Title down");
            string? TitleInput = Console.ReadLine();
            System.Console.WriteLine("Write your Post Content");
            string? BodyInput = Console.ReadLine();
            Post NewPost = new()
            {
                Title = TitleInput,
                Body = BodyInput,
                UserId = UserIdInput
            };
            await postRepository.AddAsync(NewPost);
            System.Console.WriteLine("Added Post with title: " + NewPost.Title + "\nPoster ID: " + NewPost.UserId);
            System.Console.WriteLine("Continue? (press 'n' to stop)");
            string? Choice = Console.ReadLine();
            if (Choice is "n")
            {
                Active = false;
                break;
            }
            else
            {
                continue;
            }
        }
    }   
}
