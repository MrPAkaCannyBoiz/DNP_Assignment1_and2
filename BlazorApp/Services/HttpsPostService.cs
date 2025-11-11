using ApiContracts.Dto.PostDto;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace BlazorApp.Services;

public class HttpsPostService : IPostService
{
    private readonly HttpClient? _httpClient;

    public HttpsPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }   
    public async Task<CreatePostDto> CreatePostAsync(int userId, CreatePostDto request)
    {
        HttpResponseMessage response = 
            await _httpClient!.PostAsJsonAsync($"posts/users/{userId}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
             throw new Exception($"Error creating post for user id: {userId}: " +
                 $"{response.StatusCode}, {responseContent}");
        }
        CreatePostDto createdPostDto = JsonSerializer.Deserialize<CreatePostDto>
            (responseContent, makeNameCaseInsensitive())!;
        return createdPostDto;
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await _httpClient!.DeleteAsync($"posts/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error deleting post id: {id}: " +
                $"{response.StatusCode}, {responseContent}");
        }
    }

    public async Task<IQueryable<PostDto>> GetAllPostsAsync()
    {
        HttpResponseMessage response = await _httpClient!.GetAsync("posts");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting posts: {response.StatusCode}, {responseContent}");
        }
        List<PostDto> postsDto = JsonSerializer.Deserialize<List<PostDto>>
            (responseContent, makeNameCaseInsensitive())!;
        return postsDto.AsQueryable();
    }

    public async Task<PostDto> GetPostByIdAsync(int id)
    {
        HttpResponseMessage response = await _httpClient!.GetAsync($"posts/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting post by id: {id}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        PostDto postDto = JsonSerializer.Deserialize<PostDto>
            (responseContent, makeNameCaseInsensitive())!;
        return postDto;
    }

    public async Task<UpdatePostDto> UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage response =  
            _httpClient!.PutAsJsonAsync($"posts/{id}", request).Result;
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error updating post id: {id}: " +
                $"{response.StatusCode}, {responseContent}");
        }
        UpdatePostDto updatedPostDto = JsonSerializer.Deserialize<UpdatePostDto>
            (responseContent, makeNameCaseInsensitive())!;
        return updatedPostDto;
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
