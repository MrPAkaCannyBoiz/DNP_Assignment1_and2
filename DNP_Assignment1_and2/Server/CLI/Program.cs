using CLI.UI;
using InMemoryRepositories;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

Cliapp cliapp = new Cliapp()
{
    UserInMemoryRepository = userRepository,
    CommentInMemoryRepository = commentRepository,
    PostInMemoryRepository = postRepository
};

await cliapp.StartAsync();
