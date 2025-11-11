using System;
using System.Collections.Generic;
using System.Text;

namespace ApiContracts.Dto.PostDto;

public class PostDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }

    public required int UserId { get; set; }
}
