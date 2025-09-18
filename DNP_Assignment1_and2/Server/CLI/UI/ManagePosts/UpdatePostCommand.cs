using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManagePosts
{
    public class UpdatePostCommand
    {
        private readonly IPostRepository _postRepository;
        private bool _active = true;
        public UpdatePostCommand(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }

        public async Task Execute()
        {
            while (_active)
            {
                System.Console.WriteLine("Enter the Post ID to update (press h to show post list \n)");
                var postIdInput = Console.ReadLine();
                if (postIdInput is "h")
                {
                    Console.WriteLine("Post list: \n[");
                    IQueryable<Post> posts = _postRepository.GetManyAsync();
                    foreach (Post post in posts)
                    {
                        Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
                    }
                    Console.WriteLine("]");
                }
                else if (!int.TryParse(postIdInput, out _) && postIdInput is not "h")
                // Validate input if it is not an integer (out _ means ignores the parsed value)
                {
                    System.Console.WriteLine("Invalid input. Please enter a valid Post ID. \n");
                }
                else
                {
                    try
                    {
                        Post post = await _postRepository.GetSingleAsync(Convert.ToInt32(postIdInput));
                        Console.WriteLine($"Current title: {post.Title}");
                        Console.WriteLine("Enter new title:");
                        var newTitle = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newTitle))
                        {
                            Console.WriteLine("Title cannot be empty. Update action cancelled. \n");
                        }
                        else
                        {
                            post.Title = newTitle;
                            await _postRepository.UpdateAsync(post);
                            Console.WriteLine($"Post with ID '{postIdInput}' has been updated to title '{newTitle}'. \n");
                            ShowContinueChoice();
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        private void ShowContinueChoice()
        {
            Console.WriteLine("Do you want to update another post? ('y' to continue, press any key to exit)");
            string? choice = Console.ReadLine();
            if (choice is not "y")
            {
                _active = false;
                Console.WriteLine("Exiting update post menu. \n");
            }
        }
    }
}
