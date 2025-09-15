using CLI.UI;
using CLI.UI.ManageComment;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using InMemoryRepositories;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

Cliapp cliapp = new Cliapp(userRepository, commentRepository, postRepository);

await cliapp.StartAsync();

/*
The main point is 
•	Program.cs initially instantiates whatever needs to be created. Probably primarily repositories.
•	They are passed to the CliApp.
•	Then the CliApp is started. This call is await’ed. When you start using asynchronous programming, 
your entire app is quickly infiltrated with ”Async” and Tasks. 
In the app, eventually an async method on a repository is called, 
and async methods can only be called by other async methods. ”It’s turtles all the way down”, as they say.
*/