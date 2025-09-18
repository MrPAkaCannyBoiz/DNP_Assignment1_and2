using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageUsers
{
    public class UpdateUserCommand
    {
        private readonly IUserRepository _userRepository;

        private bool _active = true;
        public UpdateUserCommand(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task Execute()
        {
            while (_active)
            {
                System.Console.WriteLine("Enter the User ID to update (press h to show user list \n)");
                var userIdInput = Console.ReadLine();
                if (userIdInput is "h")
                {
                    Console.WriteLine("User list: \n[");
                    IQueryable<User> users = _userRepository.GetManyAsync();
                    foreach (User user in users)
                    {
                        Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
                    }
                    Console.WriteLine("]");
                }
                else if (!int.TryParse(userIdInput, out _) && userIdInput is not "h")
                // Validate input if it is not an integer (out _ means ignores the parsed value)
                {
                    System.Console.WriteLine("Invalid input. Please enter a valid User ID. \n");
                }
                else
                {
                    try
                    {
                        User user = await _userRepository.GetSingleAsync(Convert.ToInt32(userIdInput));
                        Console.WriteLine($"Current username: {user.UserName}");
                        Console.WriteLine("Enter new username:");
                        var newUsername = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newUsername))
                        {
                            Console.WriteLine("Username cannot be empty. Update action cancelled. \n");
                        }
                        else
                        {
                            user.UserName = newUsername;
                            await _userRepository.UpdateAsync(user);
                            Console.WriteLine($"User with ID '{userIdInput}' has been updated to username '{newUsername}'. \n");
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
            Console.WriteLine("Continue? (enter with 'n' to leave)");
            string? choice1 = Console.ReadLine();
            if (choice1 is "n")
            {
                _active = false;
            }
        }
    }
}
