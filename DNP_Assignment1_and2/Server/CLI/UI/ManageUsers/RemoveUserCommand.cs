using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageUsers
{
    public class RemoveUserCommand
    {
        private readonly IUserRepository _userRepository;

        private bool _active = true;

        public RemoveUserCommand(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task Execute()
        {
            while (_active)
            {
                System.Console.WriteLine("Enter the User ID to remove (press h to show user list)");
                var userIdInput = Console.ReadLine();
                if (userIdInput is "h")
                {
                    IQueryable<User> users = _userRepository.GetManyAsync();
                    Console.WriteLine("User list: \n[");
                    foreach (User user in users)
                    {
                        Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
                    }
                    Console.WriteLine("]");
                }
                else if (!int.TryParse(userIdInput, out _) && userIdInput is not "h")
                // Validate input if it is not an integer and it's not "h" (out _ means ignores the parsed value)
                {
                    System.Console.WriteLine("Invalid input. Please enter a valid User ID.");
                }
                else
                {
                    User user = await _userRepository.GetSingleAsync(Convert.ToInt32(userIdInput));
                    Console.WriteLine($"Are you sure to delete user {user.UserName}? " +
                        $"('y' to confirm, press any key to cancel)");
                    string? choice = Console.ReadLine();
                    if (choice is "y")
                    {
                        try
                        {
                            await _userRepository.DeleteAsync(Convert.ToInt32(userIdInput));
                            Console.WriteLine($"User with ID '{userIdInput}' has been removed.");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        ShowContinueChoice();
                    }
                    else
                    {
                        Console.WriteLine("Delete action cancelled.");
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
            else
            {
                return;
            }
        }
    }
}
