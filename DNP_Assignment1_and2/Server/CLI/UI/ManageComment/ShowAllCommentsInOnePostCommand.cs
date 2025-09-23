using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageComment
{
    public class ShowAllCommentsInOnePostCommand
    {
        private ICommentRepository _commentRepository;
        private IPostRepository _postRepository;
        private bool _active = true;

        public ShowAllCommentsInOnePostCommand(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            this._commentRepository = commentRepository;
            this._postRepository = postRepository;
        }

        public async Task Execute()
        {
            while (_active)
            {
                Console.WriteLine("Write down the Post ID (press 'h' to list all exist post with ID) ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)
                    && (input is not "h" || input is not "n"))
                {
                    Console.WriteLine("Invaild input, only whole number allowed");
                }
                else if (input is "h")
                {
                    Console.WriteLine("Post list: \n[");
                    IQueryable<Post> posts = _postRepository.GetManyAsync();
                    foreach (Post post in posts)
                    {
                        Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
                    }
                    Console.WriteLine("]");
                }
                else if (input is "n")
                {
                    break;
                }
                else
                {
                    int PostIdInput = Convert.ToInt32(input);
                    try
                    {
                        Post? post = await _postRepository.GetSingleAsync(PostIdInput);
                        IQueryable<Comment> comments = _commentRepository.GetManyAsync().Where(c => c.PostId == post.Id);
                        Console.WriteLine("Comment list: \n[");
                        foreach (Comment comment in comments)
                        {
                            Console.WriteLine($"ID: {comment.Id}, Body: {comment.Body}, UserID: {comment.UserId}");
                        }
                        Console.WriteLine("]");
                        ShowContinueChoice();
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
           Console.WriteLine("Continue? (enter with 'n' to leave)");
           string? choice1 = Console.ReadLine();
           if (choice1 is "n")
           {
             _active = false;
           }
          }
    }
}
