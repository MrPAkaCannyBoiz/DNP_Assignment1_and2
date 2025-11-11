namespace BlazorApp.Services;
using ApiContracts.Dto.PostDto;
public interface IPostService
{
    public Task<IQueryable<PostDto>> GetAllPostsAsync();
    public Task<PostDto> GetPostByIdAsync(int id);
    public Task<CreatePostDto> CreatePostAsync(int userId, CreatePostDto request);
    public Task<UpdatePostDto> UpdatePostAsync(int id, UpdatePostDto request);
    public Task DeletePostAsync(int id);

}
