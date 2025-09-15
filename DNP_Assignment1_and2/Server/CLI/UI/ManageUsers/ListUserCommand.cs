using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace CLI.UI.ManageUsers
{
  public class ListUserCommand
  {
    private readonly IUserRepository userRepository;
    public ListUserCommand(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
    public async Task ListUsers()
        {
        IQueryable<User> users = userRepository.GetManyAsync();
            Console.WriteLine("\n__All users list below here__");
            foreach (var user in users)
            {
               Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
               Console.WriteLine();
            }
        }
    }
}
