using System;

namespace Entities;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }

    private User() { } // for EF Core (EFC), since EFC requires a parameterless constructor

    // as private default constructor is defined, we no longer able to use properties initializer
    public User(string? userName, string? password)
    {
        UserName = userName;
        Password = password;
    }

    public User(int id, string? userName, string? password)
    {
        Id = id;
        UserName = userName;
        Password = password;
    }
}
