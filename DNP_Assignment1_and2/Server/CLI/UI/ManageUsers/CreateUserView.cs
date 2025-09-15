using System;
using System.ComponentModel;
using Entities;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task CreateUser()
    {
        bool Active = true;
        while (Active)
        {
            System.Console.WriteLine("Write your Username down");
            string? UserNameInput = Console.ReadLine();
            System.Console.WriteLine("Write your Password");
            string? PasswordInput = Console.ReadLine();
            User NewUser = new()
            {
                UserName = UserNameInput,
                Password = PasswordInput
            };
            await userRepo.AddAsync(NewUser);
            System.Console.WriteLine("Added User with username: " + NewUser.UserName);
            System.Console.WriteLine("Continue? (press 'n' to stop)");
            string? Choice = Console.ReadLine();
            if (Choice is "n")
            {
                Active = false;
                break;
            }
            else
            {
                continue;
            }
        }
    }
}
