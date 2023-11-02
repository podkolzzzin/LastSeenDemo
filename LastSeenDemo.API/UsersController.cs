using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LastSeenDemo.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("list")]
        public IActionResult GetUsersList()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }
    }

    public interface IUserService
    {
        List<UserDto> GetUsers();
    }

    public class UserService : IUserService
    {
        public List<UserDto> GetUsers()
        {
            // Mocked data
            return new List<UserDto>
            {
                new UserDto { Username = "Doug93", UserId = Guid.NewGuid(), FirstSeen = DateTime.UtcNow },
                // Add more users as needed
            };
        }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public DateTime FirstSeen { get; set; }
    }
}
