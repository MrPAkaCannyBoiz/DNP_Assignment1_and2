using System;
using System.Collections.Generic;
using System.Text;

namespace ApiContracts.Dto.UserDto;

public class UserLoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
