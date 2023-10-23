using LastSeenDemo.IntegrationTest.Mocks;

namespace LastSeenDemo.IntegrationTest;

public class LastSeenApplicationTests
{
    private readonly DateTimeOffset _now = new(2000, 01, 01, 00, 00, 00, TimeSpan.Zero);

    [Fact]
    public void When_DataIsPresented_AllUsersLastSeenDate_Should_BeFormattedWell()
    {
        // Arrange
        var mock = new LoaderMock(new[]
        {
      new Page()
      {
        Total = 3, Data = new[]
        {
          new User()
          {
            Nickname = "u1",
            IsOnline = true,
          }
        }
      },
      new Page()
      {
        Total = 3, Data = new[]
        {
          new User()
          {
            Nickname = "u2",
            LastSeenDate = _now - TimeSpan.FromMinutes(5)
          },
          new User()
          {
            Nickname = "u3",
            LastSeenDate = _now - TimeSpan.FromHours(5)
          }
        }
      },
      new Page() { Total = 3, Data = Array.Empty<User>() }
    });

        var allUsersLoader = new UserLoader(mock, "any");
        var application = new LastSeenApplication(allUsersLoader);

        // Act
        var result = application.Show(_now);

        // Assert
        Assert.Collection(result,
          s => Assert.Equal("u1 Online", s),
          s => Assert.Equal("u2 Couple of minutes ago", s),
          s => Assert.Equal("u3 yesterday", s)
        );
    }
}
