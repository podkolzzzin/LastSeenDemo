using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LastSeenDemo.Tests
{
    public class UsersApiIntegrationTests
    {
        private readonly HttpClient _httpClient;

        public UsersApiIntegrationTests()
        {
            // Replace with the actual base address where your application is running.
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        }

        [Fact]
        public async Task GetUsersList_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/users/list");

            // Assert
            response.EnsureSuccessStatusCode(); // This will throw if the status code is not 2xx.
        }
    }
}
