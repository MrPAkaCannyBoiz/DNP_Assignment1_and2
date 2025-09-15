using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManagePosts
{
   public class ListPostsCommand
   {
     private readonly IPostRepository postRepository;
     public ListPostsCommand(IPostRepository postRepository)
         {
             this.postRepository = postRepository;
         }
     public async Task ListPosts()
         {
         IQueryable<Post> posts = postRepository.GetManyAsync();
             foreach (var post in posts)
             {
                Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Body: {post.Body}, UserID: {post.UserId}");
             }
         }
    }
}
