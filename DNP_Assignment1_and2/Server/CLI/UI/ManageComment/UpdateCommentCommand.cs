using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.UI.ManageComment;

public class UpdateCommentCommand
{
    private readonly ICommentRepository _commnetRepo;

    private bool _active;

    public UpdateCommentCommand(ICommentRepository commnetRepo)
    {
        _commnetRepo = commnetRepo;
    }

    public async Task Execute()
    {
        _active = true;
        while (_active)
        {
            string? userInput = "";
            while (true) 
            {
                Console.WriteLine("Write the comment Id (press h to show all post with comment) (press n to cancel)");
                var input = Console.ReadLine();
                if (input is "n")
                {
                    _active = false;
                    Console.WriteLine("Canceled the update");
                    break;
                }
                else if (input is "h")
                {
                    IQueryable<Comment>? comments = _commnetRepo.GetManyAsync();
                    foreach (var comment in comments)
                    {
                        if (comment != null)
                        {
                            Console.WriteLine("== Comment List");
                            Console.WriteLine($"ID: {comment.Id}, Body: {comment.Body}, PostID: {comment.PostId}, UserID: {comment.UserId}");
                        }
                    }
                }
                else if (Int32.TryParse(input, out _))
                {
                    userInput = input;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid command, please try again");
                }
            }
            Console.WriteLine("Write new body (write 'CANCEL' to cancel)");
            string? newBody = Console.ReadLine();
            if (newBody is "CANCEL")
            {
                _active = false;
                Console.WriteLine("Canceled the update");
                break;
            }
            else
            {
                Comment? commentToUpdate = await _commnetRepo.GetSingleAsync(Convert.ToInt32(userInput));
                await _commnetRepo.UpdateAsync(commentToUpdate);
                Console.WriteLine($"Updated the comment id {commentToUpdate.Id}");
                ShowContinueChoice();
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
        else
        {
            return;
        }
    }
}
