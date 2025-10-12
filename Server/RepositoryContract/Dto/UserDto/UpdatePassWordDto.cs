namespace WebAPI.Model.Dto.UserDto;

public class UpdatePassWordDto
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
