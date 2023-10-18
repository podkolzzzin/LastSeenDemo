namespace LastSeenDemo.UnitTests;

public class StubUserTransformer : IUserTransformer
{

    public void TransformSingleUser(User stateOfUserInCurrentTime, bool wasOnline, List<UserTimeSpan> userTimeSpans)
    {
    }
}

public class AllUsersTransformerTests
{
    [Fact]
    public void WhenNewUserSignedUp_Should_BeAddedToTheDictionary()
    {
        var allUsersTransformer = new AllUsersTransformer(new StubUserTransformer());
        var id = Guid.NewGuid();
        var users = new[]
        {
      new User() { UserId = id, IsOnline = false }
    };
        var onlineUsers = new List<Guid>();
        var result = new Dictionary<Guid, List<UserTimeSpan>>();

        allUsersTransformer.Transform(users, onlineUsers, result);

        Assert.Single(result);
        Assert.Equal(id, result.Keys.Single());
    }

    [Fact]
    public void WhenUserWasNotOnlineButJoined_Should_BeAddedToTheOnlineUsers()
    {
        var allUsersTransformer = new AllUsersTransformer(new StubUserTransformer());
        var id = Guid.NewGuid();
        var users = new[]
        {
      new User() { UserId = id, IsOnline = true }
    };
        var onlineUsers = new List<Guid>();
        var result = new Dictionary<Guid, List<UserTimeSpan>>()
        {
            [id] = new() { new UserTimeSpan() }
        };

        allUsersTransformer.Transform(users, onlineUsers, result);

        Assert.Single(onlineUsers);
        Assert.Equal(id, onlineUsers.Single());
    }
}
