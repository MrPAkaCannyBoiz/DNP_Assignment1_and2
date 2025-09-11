using System;
using InMemoryRepositories;

namespace CLI.UI;

public class Cliapp
{
    public IUserRepository? UserInMemoryRepository { get; set; }
    public ICommentRepository? CommentInMemoryRepository { get; set; }
    public IPostRepository? PostInMemoryRepository { get; set; }

    public Task StartAsync()
    {
        return Task.CompletedTask;
    }

}
