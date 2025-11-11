using ApiContracts.Dto.UserDto;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class HttpsUserService : IUserService
{
    private readonly HttpClient? _httpClient;

    public HttpsUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<CreateUserDto> AddAsync(CreateUserDto request)
    {
        //PostAsJsonAsync() will serialize the request object to json and set the content type header to application/json
        // your post endpoint is "users", therefore the first parameter(url) is "users"
        // therefore you don't need to serialize the request object first
        HttpResponseMessage response = await _httpClient!.PostAsJsonAsync("users", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
             throw new Exception($"Error adding user: {response.StatusCode}, {responseContent}");
        }
        CreateUserDto createdUserDto = JsonSerializer.Deserialize<CreateUserDto>
            (responseContent, makeNameCaseInsensitive())!;
        return createdUserDto;

    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await _httpClient!.DeleteAsync($"users/{id}")!;
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error deleting user: {response.StatusCode}, {responseContent}");
        }
    }

    public async Task<IQueryable<UserDto>> GetManyAsync()
    {
        HttpResponseMessage response = await _httpClient!.GetAsync("users");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting users: {response.StatusCode}, {responseContent}");
        }
        List<UserDto> usersDto = JsonSerializer.Deserialize<List<UserDto>>
            (responseContent, makeNameCaseInsensitive())!;
        return usersDto.AsQueryable();
    }

    public async Task<UserDto?> GetSingleAsync(int id)
    {
        HttpResponseMessage response = await _httpClient!.GetAsync($"users/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting user id: {id}: {response.StatusCode}, {responseContent}");
        }
        UserDto? userDto = JsonSerializer.Deserialize<UserDto>
            (responseContent, makeNameCaseInsensitive())!;
        return userDto;
    }

    public async Task UpdatePasswordAsync(int id, UpdatePassWordDto request)
    {
        HttpResponseMessage response = await _httpClient!
            .PatchAsJsonAsync($"users/updatepass/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error updating user password id: {id}: {response.StatusCode}, {responseContent}");
        }
        UpdatePassWordDto updatePassWordDto = JsonSerializer.Deserialize<UpdatePassWordDto>
           (responseContent, makeNameCaseInsensitive())!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage response = await _httpClient!
            .PutAsJsonAsync($"users/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error updating user id: {id}: {response.StatusCode}, {responseContent}");
        }
        UpdateUserDto updateUserDto = JsonSerializer.Deserialize<UpdateUserDto>
            (responseContent, makeNameCaseInsensitive())!;
    }

    public async Task UpdateUserNameAsync(int id, UpdateUserNameDto request)
    {
        HttpResponseMessage response = await _httpClient!
            .PatchAsJsonAsync($"users/updateusername/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error updating user id: {id}: {response.StatusCode}, {responseContent}");
        }
        UpdateUserNameDto updateUserNameDto = JsonSerializer.Deserialize<UpdateUserNameDto>
            (responseContent, makeNameCaseInsensitive())!;
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
