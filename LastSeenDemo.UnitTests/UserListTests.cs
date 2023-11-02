using LastSeenDemo;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LastSeenDemo.UnitTests
{
    public class UserEndpointTests
    {
        [Fact]
        public void GetUserList_ReturnsCorrectFormat()
        {
            // Arrange
            var mockUserLoader = new Mock<UserLoader>();
            var mockAllUsersTransformer = new Mock<AllUsersTransformer>();
            
            var testUsers = new Dictionary<Guid, List<UserTimeSpan>>
            {
                {
                    Guid.NewGuid(), new List<UserTimeSpan>
                    {
                        new UserTimeSpan { Login = new DateTimeOffset(new DateTime(2023, 1, 1)) },
                    }
                },
                {
                    Guid.NewGuid(), new List<UserTimeSpan>
                    {
                        new UserTimeSpan { Login = new DateTimeOffset(new DateTime(2023, 1, 2)) },
                    }
                }
            };

            var worker = new Worker(mockUserLoader.Object, mockAllUsersTransformer.Object);
            worker.Users = testUsers; // Setting the Users property

            // Act
            var usersList = worker.Users.Select(userEntry => new
            {
                username = userEntry.Key.ToString(),
                userId = userEntry.Key,
                firstSeen = userEntry.Value.OrderBy(u => u.Login).FirstOrDefault()?.Login.ToString("o") // ISO 8601 format
            }).ToList();

            // Assert
            Assert.Equal(testUsers.Count, usersList.Count);
            foreach (var user in usersList)
            {
                Assert.True(testUsers.ContainsKey(user.userId));
                Assert.NotNull(user.firstSeen); // Ensuring that firstSeen is not null
                // Additional assertions can be added as necessary
            }
        }
    }
}
