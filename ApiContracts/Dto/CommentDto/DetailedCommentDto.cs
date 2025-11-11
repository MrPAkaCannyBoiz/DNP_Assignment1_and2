using System;
using System.Collections.Generic;
using System.Text;

namespace ApiContracts.Dto.CommentDto;

public class DetailedCommentDto
{
    public required string Body { get; set; }
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
}
