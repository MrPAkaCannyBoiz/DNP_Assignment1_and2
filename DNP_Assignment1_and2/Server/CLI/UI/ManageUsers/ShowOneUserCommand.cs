using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageUsers
{
    public class ShowOneUserCommand
    {
        private readonly IUserRepository _userRepository;
        public ShowOneUserCommand(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task Execute()
        {
            bool active = true;
            while (active)
            {
                System.Console.WriteLine("Enter the User ID to show");
                var userIdInput = Console.ReadLine();
                try
                {
                    User user = await _userRepository.GetSingleAsync(Convert.ToInt32(userIdInput));
                    System.Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
                }
                catch (InvalidOperationException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Invalid input. " +
                        "Please enter a valid User ID with only whole number (e.g. '69').");
                }
                Console.WriteLine("Continue? (enter with 'n' to leave)");
                string? choice = Console.ReadLine();
                if (choice is "n")
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
}
