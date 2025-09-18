using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageComment
{
  public class ShowOneCommentCommand
  {
    private readonly ICommentRepository commentRepository;
    public ShowOneCommentCommand(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }
    public async Task ShowOneCommentById()
        {
        Console.WriteLine("Enter the Comment ID to display");
        int CommentIdInput = Convert.ToInt32(Console.ReadLine());
        Comment? comment = await commentRepository.GetSingleAsync(CommentIdInput);
        if (comment != null)
        {
            Console.WriteLine($"ID: {comment.Id}, Body: {comment.Body}, PostID: {comment.PostId}, UserID: {comment.UserId}");
        }
        else
        {
            Console.WriteLine("Comment not found.");
        }
        }
    }
}
