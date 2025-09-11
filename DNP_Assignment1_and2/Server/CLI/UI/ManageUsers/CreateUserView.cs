using System;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
}
