namespace WebAPI.Model;

public class UpdateUserDto
{
    //in user's pov, u need 2 thing to update, ID & username
    public required int UserId { get; set; }
    public required string UserName { get; set; }
}
