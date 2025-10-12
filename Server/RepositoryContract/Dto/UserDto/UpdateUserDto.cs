namespace WebAPI.Model.Dto.UserDto;

public class UpdateUserDto
{
    //in user's pov, u can update at most 2 things, password & username
    public string? UserName { get; set; } = "";
    public string? PassWord { get; set; } = "";
}
