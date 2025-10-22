using System.Runtime.CompilerServices;

namespace RepositoryContract.Dto.PostDto;

public class CreatePostDto
{
    public required string Title { get; set; } 
    public required string Body { get; set; } 
}
