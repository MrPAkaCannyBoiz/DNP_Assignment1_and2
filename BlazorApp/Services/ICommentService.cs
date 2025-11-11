namespace BlazorApp.Services;
using ApiContracts.Dto.CommentDto;
public interface ICommentService
{
    public Task<IQueryable<DetailedCommentDto>> GetManyCommentsByPostIdAsync(int postId);
    public Task<DetailedCommentDto?> GetSingleCommentByIdAsync(int postId, int commentId);
    public Task<DetailedCommentDto> AddCommentAsync(int postId, int userId, DetailedCommentDto request);
    public Task<CommentDto> UpdateCommentAsync(int postId, int commentId, CommentDto request);
    public Task DeleteCommentAsync(int postId, int commentId);
}
