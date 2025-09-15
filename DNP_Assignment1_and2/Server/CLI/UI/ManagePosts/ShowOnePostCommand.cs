using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManagePosts
{
  public class ShowOnePostCommand
  {
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    public ShowOnePostCommand(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }
    public async Task ShowOnePostById()
        {
        System.Console.WriteLine("Enter the Post ID to display");
        int PostIdInput = Convert.ToInt32(Console.ReadLine());
        Post? post = await postRepository.GetSingleAsync(PostIdInput);
        if (post != null)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Body: {post.Body}, UserID: {post.UserId}");
            IQueryable<Comment> comments = commentRepository.GetManyAsync().Where(c => c.PostId == PostIdInput);
            Console.WriteLine($"__Comment of post : {post.Title}");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"\nUser ID: {comment.UserId}" +
                        $"\n => {comment.Body}");
                }
            }
        else
        {
            Console.WriteLine("Post not found.");
        }
        
        }
    }
}
