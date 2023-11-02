using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using System.Collections.Generic;
using LastSeenDemo.API.Controllers;
using System;

namespace LastSeenDemo.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public void GetUsersList_ReturnsAListOfUsers()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            mockService.Setup(service => service.GetUsers()).Returns(GetTestUsers());
            var controller = new UsersController(mockService.Object);

            // Act
            var result = controller.GetUsersList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, users.Count); // Assuming GetTestUsers() returns 2 users
        }

        private List<UserDto> GetTestUsers()
        {
            return new List<UserDto>
            {
                new UserDto { Username = "User1", UserId = Guid.NewGuid(), FirstSeen = DateTime.UtcNow },
                new UserDto { Username = "User2", UserId = Guid.NewGuid(), FirstSeen = DateTime.UtcNow }
            };
        }
    }

    // Mock IUserService implementation
    public interface IUserService
    {
        List<UserDto> GetUsers();
    }

    // User Data Transfer Object (DTO)
    public class UserDto
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public DateTime FirstSeen { get; set; }
    }
}
