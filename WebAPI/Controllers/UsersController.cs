using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //TODO : Implement CRUD operations for User entity
    // get (both all and given id)
    [HttpGet]
    public ActionResult<IQueryable<User>> GetAllUsers() => Ok(_userRepository.GetManyAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userRepository.GetSingleAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    //add 
    [HttpPost]
    public async Task<ActionResult<User>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            VerifyUserNameIsAvailableAsync(request.UserName);
            User user = new()
            {
                UserName = request.UserName,
                Password = request.PassWord,
            };
            User createdUser = await _userRepository.AddAsync(user);
            UserDto dto = new()
            {
                Id = createdUser.Id,
                UserName = createdUser.UserName!
            };
            return Created($"/users/{dto.Id}", createdUser);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

    //update
    [HttpPut]
    public async Task<ActionResult<User>> UpdateUser([FromBody] UpdateUserDto request)
    {
        try
        {
            VerifyUserNameIsAvailableAsync(request.UserName);
            User user = new()
            {
                Id = request.UserId,
                UserName = request.UserName,
            };
            await _userRepository.UpdateAsync(user);
            UserDto dto = new()
            { Id = user.Id, UserName = user.UserName };
            return Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

    //Delete
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        try
        {
           await _userRepository.DeleteAsync(id);
           return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

  
    private void VerifyUserNameIsAvailableAsync(string username)
    {
        var users = _userRepository.GetManyAsync();
        if (users.Any(u => u.UserName!.Equals(username) && u.UserName != null))
        {
            throw new Exception($"Username: {username} is already exist");
        }
    }

}
