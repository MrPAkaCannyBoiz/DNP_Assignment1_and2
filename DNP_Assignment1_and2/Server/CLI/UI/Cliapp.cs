using System;
using CLI.UI.ManageComment;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;

namespace CLI.UI;

public class Cliapp
{
    public IUserRepository? UserInMemoryRepository { get; set; }
    public ICommentRepository? CommentInMemoryRepository { get; set; }
    public IPostRepository? PostInMemoryRepository { get; set; }

    private readonly CreateUserView _createUserView;
    private readonly ListUserCommand _listUserCommand;
    private readonly RemoveUserCommand _removeUserCommand;
    private readonly ShowOneUserCommand _showOneUserCommand;
    private readonly UpdateUserCommand _updateUserCommand;


    private readonly CreatePostView _createPostView;
    private readonly ListPostsCommand _listPostsCommand;
    private readonly ShowOnePostCommand _showOnePostCommand;
    private readonly RemovePostCommand _removePostCommand;
    private readonly UpdatePostCommand _updatePostCommand;

    private readonly CreateCommentView _createCommentView;
    private readonly ShowOneCommentCommand _showOneCommentCommand;
    private readonly ShowAllCommentsInOnePostCommand _showAllCommentsInOnePostCommand;


    private bool MainMenuActive = true;
    private bool SubMenuActive = false;

    public Cliapp(IUserRepository UserInMemoryRepository, ICommentRepository CommentInMemoryRepository
        , IPostRepository PostInMemoryRepository)
    {
        this.UserInMemoryRepository = UserInMemoryRepository;
        this.CommentInMemoryRepository = CommentInMemoryRepository;
        this.PostInMemoryRepository = PostInMemoryRepository;

        this._createUserView = new(UserInMemoryRepository);
        this._listUserCommand = new(UserInMemoryRepository);
        this._removeUserCommand = new(UserInMemoryRepository);
        this._showOneUserCommand = new(UserInMemoryRepository);
        this. _updateUserCommand = new(UserInMemoryRepository);

        this._createPostView = new(PostInMemoryRepository);
        this._listPostsCommand = new(PostInMemoryRepository);
        this._showOnePostCommand = new(PostInMemoryRepository, CommentInMemoryRepository);
        this._removePostCommand = new(PostInMemoryRepository);
        this._updatePostCommand = new(PostInMemoryRepository);


        this._createCommentView = new(CommentInMemoryRepository);
        this._showOneCommentCommand = new(CommentInMemoryRepository);
        this._showAllCommentsInOnePostCommand = new(CommentInMemoryRepository, PostInMemoryRepository);
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
                            - Press 4 to Remove a Post by ID
                            - Press 5 to Update a Post by ID
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
                        await _createPostView.CreatePost();
                        break;
                    case "2":
                        await _listPostsCommand.ListPosts();
                        break;
                    case "3":
                        await _showOnePostCommand.ShowOnePostById();
                        break;
                    case "4":
                        await _removePostCommand.Execute();
                        break;
                    case "5":
                        await _updatePostCommand.Execute();
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
                            - Press 3 to Show one User by ID
                            - Press 4 to Remove a User by ID
                            - Press 5 to Update a User by ID 
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
                                        await _createUserView.CreateUser();
                                        break;
                                    case "2":
                                        await _listUserCommand.ListUsers();
                                        break;
                                    case "3":
                                        await _showOneUserCommand.Execute();
                                        break;
                                    case "4":
                                        await _removeUserCommand.Execute();
                                        break;
                                    case "5":
                                        await _updateUserCommand.Execute();
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
                        await _createCommentView.CreateComment();
                        break;
                    case "3":
                        await _showOneCommentCommand.ShowOneCommentById();
                        break;
                }
        }
    }

}
