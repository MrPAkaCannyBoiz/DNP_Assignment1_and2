using System;
using Entities;

namespace CLI.UI.ManagePosts;


public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private bool _active = true;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task CreatePost()
    {
        _active = true;
        while (_active)
        {
            string? userIdInput = "";
            while (true)
            {
                Console.WriteLine("Write the User ID as poster (press 'n' to cancel)");
                var input = Console.ReadLine();
                if (input is "n")
                {
                    Console.WriteLine("Create Post action cancelled.");
                    _active = false;
                    break;
                }
                else if (!int.TryParse(input, out _))
                {
                    Console.WriteLine("Invalid input. Please enter a valid User ID (press any key to continue)");
                }
                else
                {
                    userIdInput = input;
                    break;
                }  
            }
            if (!_active) break;
            Console.WriteLine("Write your Post Title down");
            string? titleInput = Console.ReadLine();
            CancelCreatePostIfInputIsN(titleInput);
            if (!_active) break;
            Console.WriteLine("Write your Post Content");
            string? bodyInput = Console.ReadLine();
            CancelCreatePostIfInputIsN(bodyInput);
            if (!_active) break;
            Post NewPost = new()
            {
                Title = titleInput,
                Body = bodyInput,
                UserId = Convert.ToInt32(userIdInput)
            };
            await postRepository.AddAsync(NewPost);
            Console.WriteLine("Added Post with title: " + NewPost.Title + "\nPoster ID: " + NewPost.UserId);
            Console.WriteLine("Continue? (press 'n' to stop)");
            string? Choice = Console.ReadLine();
            if (Choice is "n")
            {
                break;
            }
            else
            {
                continue;
            }
        }
    }

    private void CancelCreatePostIfInputIsN(string? input)
    {
        if (input is "n")
        {
            Console.WriteLine("Create Post action cancelled.");
            _active = false;
        }
    }
}
