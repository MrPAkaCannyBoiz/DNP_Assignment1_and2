using System;

namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int? UserId { get; set; }

    private Post() { } // for EF Core (EFC), since EFC requires a parameterless constructor

    // as private default constructor is defined, we no longer able to use properties initializer
    public Post(string? title, string? body, int? userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }

    public Post(int id, string? title, string? body, int? userId)
    {
        Id = id;
        Title = title;
        Body = body;
        UserId = userId;
    }
}
