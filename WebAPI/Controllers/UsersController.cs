using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Model.Dto.UserDto;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")] // means the route is /users according to the controller class name (exclude "Controller" part) 
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

    //update : both username and password (use put)
    //to prevent anyone from knowing the password of other users, we do not put password in http response body
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            VerifyUserNameIsAvailableAsync(request.UserName!);
            var existingUser = await _userRepository.GetSingleAsync(id);
            if (existingUser is null)
            {
                return NotFound();
            }
            User user = new()
            {
                Id = existingUser.Id,
                UserName = request.UserName,
                Password = request.PassWord,
            };
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body");
            }
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

    // update : only username (use patch)
    [HttpPatch("updateusername/{id}")]
    public async Task<ActionResult<User>> UpdateUserName(int id, [FromBody] UpdateUserNameDto request)
    {
        try
        {
            VerifyUserNameIsAvailableAsync(request.UserName!);
            var existingUser = await _userRepository.GetSingleAsync(id);
            if (existingUser is null)
            {
                return NotFound();
            }
            User user = new()
            {
                Id = existingUser.Id,
                UserName = request.UserName,
                Password = existingUser.Password,
            };
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body");
            }
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

    [HttpPatch("updatepass/{id}")]
    public async Task<ActionResult<User>> UpdateUserPassword(int id, [FromBody] UpdatePassWordDto request)
    {
        try
        {
            // TODO: verify old password is correct (not implement yet)
            var existingUser = await _userRepository.GetSingleAsync(id);
            if (existingUser is null)
            {
                return NotFound();
            }
            User user = new()
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Password = request.NewPassword,
            };
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body");
            }
            await _userRepository.UpdateAsync(user);
            UserDto dto = new()
            { Id = user.Id, UserName = user.UserName };
            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return BadRequest(ex.Message);
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
        if (users.Any(u => u.UserName!.Equals(username) && !string.IsNullOrWhiteSpace(u.UserName)))
        {
            throw new Exception($"Username: {username} is already exist");
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Username cannot be empty");
        }
    }

    private void VerifyOldPasswordIsCorrect(string oldPassword, string actualPassword)
    {
        if (!oldPassword.Equals(actualPassword))
        {
            throw new InvalidOperationException("Old password is not correct");
        }
    }

}
