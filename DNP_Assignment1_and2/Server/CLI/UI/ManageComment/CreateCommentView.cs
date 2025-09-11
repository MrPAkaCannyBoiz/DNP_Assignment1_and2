using System;

namespace CLI.UI.ManageComment;

public class CreateCommentView
{
    private readonly ICommentRepository CommentRepo;
    public CreateCommentView(ICommentRepository CommnetRepo)
    {
        CommentRepo = CommnetRepo;
    }
}
