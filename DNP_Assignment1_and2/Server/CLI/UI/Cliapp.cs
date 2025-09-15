using System;
using CLI.UI.ManageComment;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using InMemoryRepositories;

namespace CLI.UI;

public class Cliapp
{
    public IUserRepository? UserInMemoryRepository { get; set; }
    public ICommentRepository? CommentInMemoryRepository { get; set; }
    public IPostRepository? PostInMemoryRepository { get; set; }

    private CreateUserView CreateUserView;
    private ListUserCommand ListUserCommand;

    private CreatePostView CreatePostView;
    private ListPostsCommand ListPostsCommand;
    private ShowOnePostCommand ShowOnePostCommand;

    private CreateCommentView CreateCommentView;
    private ShowOneCommentCommand ShowOneCommentCommand;

    private bool MainMenuActive = true;
    private bool SubMenuActive = false;

    public Cliapp(IUserRepository UserInMemoryRepository, ICommentRepository CommentInMemoryRepository
        , IPostRepository PostInMemoryRepository)
    {
        this.UserInMemoryRepository = UserInMemoryRepository;
        this.CommentInMemoryRepository = CommentInMemoryRepository;
        this.PostInMemoryRepository = PostInMemoryRepository;

        this.CreateUserView = new(UserInMemoryRepository);
        this.ListUserCommand = new(UserInMemoryRepository);

        this.CreatePostView = new(PostInMemoryRepository);
        this.ListPostsCommand = new(PostInMemoryRepository);
        this.ShowOnePostCommand = new(PostInMemoryRepository, CommentInMemoryRepository);

        this.CreateCommentView = new(CommentInMemoryRepository);
        this.ShowOneCommentCommand = new(CommentInMemoryRepository);
    }

    public async Task StartAsync()
    {
        while (MainMenuActive)
        {
            System.Console.WriteLine("\nForum Console Admin version");
            System.Console.WriteLine("Press 1 to enter post view");
            System.Console.WriteLine("Press 2 to enter user view");
            System.Console.WriteLine("Press 3 to enter comment view");
            System.Console.WriteLine("Press 0 to Quit the App");

            var input = Console.ReadLine();
            if (input == "0")
            {
                break;
            }
            else
                switch (input)
                {
                    case "1":
                        MainMenuActive = false;
                        SubMenuActive = true;
                        await GoToPostView();
                        break;
                    case "2":
                        MainMenuActive = false;
                        SubMenuActive = true;
                        await GoToUserView();
                        break;
                    case "3":
                        MainMenuActive = false;
                        SubMenuActive = true;
                        await GoToCommentView();
                        break;
                }
        }
    }

    private async Task GoToPostView()
    {
        while (SubMenuActive)
        {
            System.Console.WriteLine("""

                            _Post_Manager_View
                            - Press 1 to Create new Post
                            - Press 2 to List all Posts
                            - Press 3 to Show one Post by ID
                            - Press 0 to Go back to Main menu
                            """);
            string? input = Console.ReadLine();
            if (input is "0")
            {
                MainMenuActive = true;
                SubMenuActive = false;
                break;
            }
            else
                switch (input)
                {
                    case "1":
                        await CreatePostView.CreatePost();
                        break;
                    case "2":
                        await ListPostsCommand.ListPosts();
                        break;
                    case "3":
                        await ShowOnePostCommand.ShowOnePostById();
                        break;
                }
        }
    }

    private async Task GoToUserView()
    {       
        while (SubMenuActive)
        {
        System.Console.WriteLine("""

                            _User_Manager_View
                            - Press 1 to Create new User
                            - Press 2 to List all Users
                            - Press 0 to Go back to Main menu
                            """);
                            string? input = Console.ReadLine();
                            if (input is "0")
                            {
                                MainMenuActive = true;
                                SubMenuActive = false;
                                break;
                            }
           
                            else
                                switch (input)
                                {
                                    case "1":
                                        await CreateUserView.CreateUser();
                                        break;
                                    case "2":
                                        await ListUserCommand.ListUsers();
                                        break;  
                }
        }
    }

    private async Task GoToCommentView()
    {
        while (SubMenuActive)
        {
            System.Console.WriteLine("""
                            _Comment_Manager_View
                            - Press 1 to Create new Comment
                            - Press 3 to Show one Comment by ID
                            - Press 0 to Go back to Main menu
                            """);
            string? input = Console.ReadLine();
            if (input is "0")
            {
                MainMenuActive = true;
                SubMenuActive = false;
                break;
            }
            else
                switch (input)
                {
                    case "1":
                        await CreateCommentView.CreateComment();
                        break;
                    case "3":
                        await ShowOneCommentCommand.ShowOneCommentById();
                        break;
                }
        }
    }

}
