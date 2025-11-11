namespace BlazorApp.Services;

using ApiContracts.Dto.UserDto;

public interface IUserService
{
    public Task<CreateUserDto> AddAsync(CreateUserDto request); // you expect return type to be UserDto
    public Task UpdateUserAsync(int id, UpdateUserDto request); // you expect return type to be nothing
    public Task UpdateUserNameAsync(int id, UpdateUserNameDto request);
    public Task UpdatePasswordAsync(int id, UpdatePassWordDto request);
    public Task DeleteAsync(int id);
    public Task<UserDto?> GetSingleAsync(int id);
    public Task<IQueryable<UserDto>> GetManyAsync();
}
