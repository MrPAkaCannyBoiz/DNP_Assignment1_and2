using System;

namespace Entities;

public class Comment
{
    public string? Body { get; set; }
    public int? Id { get; set; }
    public int? PostId { get; set; }
    public int? UserId { get; set; }

    private Comment() { } // for EF Core (EFC), since EFC requires a parameterless constructor

    // as private default constructor is defined, we no longer able to use properties initializer
    public Comment(string? body, int? postId, int? userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}
