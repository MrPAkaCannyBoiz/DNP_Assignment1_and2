namespace WebAPI.Model.Dto.UserDto;

public class CreateUserDto
{
    public required string UserName { get; set; }
    public required string PassWord { get; set; }
}
