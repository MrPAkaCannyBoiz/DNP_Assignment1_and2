using System;
using Entities;

namespace CLI.UI.ManageComment;

public class CreateCommentView
{
    private readonly ICommentRepository CommentRepo;
    public CreateCommentView(ICommentRepository CommentRepo)
    {
        this.CommentRepo = CommentRepo;
    }

    public async Task CreateComment()
    {
       bool Active = true;
       while (Active)
       {
           Console.WriteLine("Write the User ID as commenter");
           int? UserIdInput = Convert.ToInt32(Console.ReadLine());
           Console.WriteLine("Write the Post ID to comment on");
           int? PostIdInput = Convert.ToInt32(Console.ReadLine());
           Console.WriteLine("Write your Comment Content");
           string? BodyInput = Console.ReadLine();
           Comment NewComment = new()
           {
               Body = BodyInput,
               PostId = PostIdInput,
               UserId = UserIdInput
           };
           await CommentRepo.AddAsync(NewComment);
           Console.WriteLine("Added Comment with content: " + NewComment.Body 
               + "\nOn Post ID: " + NewComment.PostId 
               + "\nBy User ID: " + NewComment.UserId);
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

}
