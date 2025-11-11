using ApiContracts.Dto.UserDto;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public ActionResult LoginWithUserName([FromBody] UserLoginDto request)
    {
        try
        {
            var allUsers = _userRepository.GetManyAsync();
            var user = allUsers.FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            UserDto userDto = new()
            { 
                Id = user.Id, 
                UserName = user.UserName 
            };
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }


}
