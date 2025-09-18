using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManagePosts
{
    public class RemovePostCommand
    {
        private readonly IPostRepository _postRepository;
        private bool _active = true;

        public RemovePostCommand(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }

        public async Task Execute()
        {
            while (_active)
            {
                Console.WriteLine("Enter the ID of the post to remove " +
                    "(press 'h' to list all posts, press 'n' to cancel remove)");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "n":
                        Console.WriteLine("Remove action cancelled.");
                        return;

                    case "h":
                        Console.WriteLine("Post list: \n[");
                        IQueryable<Post> posts = _postRepository.GetManyAsync();
                        foreach (Post post in posts)
                        {
                            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
                        }
                        Console.WriteLine("]");
                        break;

                    default:
                        if (!int.TryParse(input, out int postId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid post ID.");
                        }
                        else
                        {
                            try
                            {
                                await _postRepository.DeleteAsync(postId);
                                Console.WriteLine($"Post with ID {input} has been removed.");
                                return;
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                }
            }
        }
    }
}
