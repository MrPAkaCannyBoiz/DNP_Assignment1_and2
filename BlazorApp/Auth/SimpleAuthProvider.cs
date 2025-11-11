using ApiContracts.Dto.UserDto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorApp.Auth;

public class SimpleAuthProvider: AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentClaimsPrincipal;
    private readonly IJSRuntime _jSRuntime;
    private string? _primaryCacheUserJson; // primary cache for user json

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jSRuntime)
    {
        _httpClient = httpClient;
        _jSRuntime = jSRuntime;
    }

    public async Task LoginAsync(string userName, string password)
    {
        //1)	Call the web api with a LoginRequest, containing relevant information.
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login",
             new UserLoginDto() 
             {
                 UserName = userName,
                 Password = password
             });

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Login failed: {response.StatusCode}, {content}");
        }

        /*
        Notice the JSON options parameter, this is because the JSON uses camelCase, 
        but the UserDto properties are PascalCase. So, we must tell the serializer to ignore casing
        */
        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        string serializeUserData = JsonSerializer.Serialize(userDto);
        await _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serializeUserData);
        _primaryCacheUserJson = serializeUserData;

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userDto.UserName ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        ClaimsIdentity claimsIdentity = new(claims, "apiauth");
        _currentClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Notify the authentication state has changed, then Blazor will update the UI accordingly.
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));

    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = _primaryCacheUserJson;
        try
        {
            userAsJson = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            _primaryCacheUserJson = userAsJson; // update primary cache after getting from session storage
        }
        catch (InvalidOperationException) // when it's not logged in, return empty state
        {
            var emptyState = new AuthenticationState(new());
            return emptyState;
        }
        if (string.IsNullOrEmpty(userAsJson))
        {
            var emptyState = new AuthenticationState(new());
            return emptyState;
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userDto.UserName ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };
        ClaimsIdentity identity = new(claims, "apiauth");
        ClaimsPrincipal principal = new(identity);
        AuthenticationState state = new(principal ?? new());
        return state;
    }

    public async void LogoutAsync()
    {
        await _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        _primaryCacheUserJson = null;
        _currentClaimsPrincipal = new(); // clear the current user and set to an empty ClaimsPrincipal
        // notify Blazor
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }

}
