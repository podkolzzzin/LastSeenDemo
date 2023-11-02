using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace LastSeenDemo.Tests
{
    public class UsersListApiTests
    {
        private readonly HttpClient _client;

        public UsersListApiTests()
        {
            // Initialize HttpClient
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000") // Replace with your application's URL
            };
        }

        [Fact]
        public async Task GetUsersList_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/api/users/list";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetUsersList_ReturnsExpectedJsonStructure()
        {
            // Arrange
            var request = "/api/users/list";

            // Act
            var response = await _client.GetAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var usersList = JArray.Parse(responseString);

            // Assert
            Assert.All(usersList, item =>
            {
                item["userId"].Should().NotBeNull();
                item["username"].Should().NotBeNull();
                item["firstSeen"].Should().NotBeNull();
            });
        }

        [Fact]
        public async Task GetUsersList_ReturnsNotEmptyList()
        {
            // Arrange
            var request = "/api/users/list";

            // Act
            var response = await _client.GetAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var usersList = JArray.Parse(responseString);

            // Assert
            usersList.Should().NotBeEmpty();
        }
    }
}
