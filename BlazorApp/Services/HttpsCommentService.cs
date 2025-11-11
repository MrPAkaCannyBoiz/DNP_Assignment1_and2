using ApiContracts.Dto.CommentDto;
using System.Text.Json;

namespace BlazorApp.Services;

public class HttpsCommentService : ICommentService
{
    private readonly HttpClient? _httpClient;

    public HttpsCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DetailedCommentDto> AddCommentAsync(int postId, int userId, DetailedCommentDto request)
    {
        HttpResponseMessage response =
            await _httpClient!.PostAsJsonAsync($"comments/posts/{postId}/users/{userId}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error adding comment for post id: {postId}, user id: {userId}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        DetailedCommentDto createdCommentDto = JsonSerializer.Deserialize<DetailedCommentDto>
            (responseContent, makeNameCaseInsensitive())!;
        return createdCommentDto;
    }

    public async Task DeleteCommentAsync(int postId, int commentId)
    {
        HttpResponseMessage response = 
            await _httpClient!.DeleteAsync($"posts/{postId}/comments/{commentId}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error deleting comment id: {commentId}: " +
                $"{response.StatusCode}, {responseContent}");
        }
    }

    public async Task<IQueryable<DetailedCommentDto>> GetManyCommentsByPostIdAsync(int postId)
    {
        HttpResponseMessage response = 
           await _httpClient!.GetAsync($"posts/{postId}/comments");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting comments for post id: {postId}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        List<DetailedCommentDto> commentsDto = JsonSerializer.Deserialize<List<DetailedCommentDto>>
            (responseContent, makeNameCaseInsensitive())!;
        return commentsDto.AsQueryable();
    }

    public async Task<DetailedCommentDto?> GetSingleCommentByIdAsync(int postId, int commentId)
    {
        HttpResponseMessage response =
            await _httpClient!.GetAsync($"posts/{postId}/comments/{commentId}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting comment id: {commentId} for post id: {postId}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        DetailedCommentDto? commentDto = JsonSerializer.Deserialize<DetailedCommentDto?>
            (responseContent, makeNameCaseInsensitive());
        return commentDto;
    }

    public async Task<CommentDto> UpdateCommentAsync(int postId, int commentId, CommentDto request)
    {
        HttpResponseMessage response =
            await _httpClient!.PutAsJsonAsync($"posts/{postId}/comments/{commentId}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error updating comment id: {commentId} for post id: {postId}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        CommentDto updatedCommentDto = JsonSerializer.Deserialize<CommentDto>
            (responseContent, makeNameCaseInsensitive())!;
        return updatedCommentDto;
    }

    private JsonSerializerOptions makeNameCaseInsensitive()
    {
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return jsonSerializerOptions;
    }
}
