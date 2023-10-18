namespace LastSeenDemo.UnitTests;

public class UserLoaderTests
{
    private class LoaderMock : ILoader
    {
        private readonly Page[] _pages;
        public List<string> Urls { get; } = new();

        public LoaderMock(Page[] pages)
        {
            _pages = pages;
        }

        public Page Load(string url)
        {
            var result = _pages[Urls.Count];
            Urls.Add(url);
            return result;
        }
    }

    [Fact]
    public void Should_ReturnEmptyArray_When_NoData()
    {
        // Arrange
        var mock = new LoaderMock(new[] { new Page() { Total = 0, Data = Array.Empty<User>() } });
        var userLoader = new UserLoader(mock, "url");

        // Act
        var result = userLoader.LoadAllUsers();

        // Assert
        Assert.Empty(result);
        Assert.Single(mock.Urls);
        Assert.Equal("url?offset=0", mock.Urls[0]);
    }

    [Fact]
    public void Should_ReturnAllResultsAsArray_When_2Pages()
    {
        // Arrange
        var mock = new LoaderMock(new[]
        {
      new Page() { Total = 3, Data = new[] { new User() { Nickname = "u1" } } },
      new Page()
      {
        Total = 3, Data = new[]
        {
          new User() { Nickname = "u2" },
          new User() { Nickname = "u3" }
        }
      },
      new Page() { Total = 3, Data = Array.Empty<User>()}
    });
        var userLoader = new UserLoader(mock, "url");

        // Act
        var result = userLoader.LoadAllUsers();

        // Assert
        Assert.Collection(result,
          u => Assert.Equal("u1", u.Nickname),
          u => Assert.Equal("u2", u.Nickname),
          u => Assert.Equal("u3", u.Nickname)
        );
        Assert.Equal(3, mock.Urls.Count);
        Assert.Equal("url?offset=0", mock.Urls[0]);
        Assert.Equal("url?offset=1", mock.Urls[1]);
        Assert.Equal("url?offset=3", mock.Urls[2]);
    }
}
