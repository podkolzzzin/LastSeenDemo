using LastSeenDemo.IntegrationTest.Mocks;

namespace LastSeenDemo.IntegrationTest;

public class WorkerTests
{
    [Fact]
    public void When_ForgetIsCalled_UserShouldBeRemovedFromData()
    {
        var id = Guid.NewGuid();
        var page1 = new Page()
        {
            Data = new User[]
          {
        new() { UserId = id, IsOnline = true }
          },
            Total = 1,
        };
        var page2 = new Page()
        {
            Data = new User[0],
            Total = 0
        };
        var mock = new LoaderMock(new[]
        {
      page1,
      page2,
      page1,
      page2
    });
        var worker = new Worker(new UserLoader(mock, "any"),
          new AllUsersTransformer(
            new UserTransformer(
              new DateTimeProvider())));

        worker.LoadDataIteration();

        // Assert#1
        Assert.Single(worker.OnlineUsers, id);
        Assert.Single(worker.Users);
        Assert.Single(worker.Users[id]); // Ensure that user was added

        // Act
        worker.Forget(id);

        // Assert#2
        Assert.False(worker.Users.ContainsKey(id)); // Ensure that user was removed
        Assert.Empty(worker.OnlineUsers);

        worker.LoadDataIteration(); // Simulate next iteration
        Assert.False(worker.Users.ContainsKey(id));
        Assert.Empty(worker.OnlineUsers);
    }
}
