using System.Runtime.CompilerServices;

namespace ApiContracts.Dto.PostDto;

public class CreatePostDto
{
    public required string Title { get; set; } 
    public required string Body { get; set; } 
}
