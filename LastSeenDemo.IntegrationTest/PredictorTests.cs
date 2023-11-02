namespace LastSeenDemo.IntegrationTest;

public class PredictorTests
{
    [Fact]
    public void When_PredictUserOnlineWasOnlineAtThisTime_ShouldReturn100PercentChance()
    {
        var detector = new OnlineDetector(new DateTimeProvider());
        var predictor = new Predictor(detector);

        var userTimeSpans = new List<UserTimeSpan>()
    {
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-10), Logout = DateTimeOffset.UtcNow.AddMinutes(-5) },
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-4), Logout = DateTimeOffset.UtcNow.AddMinutes(-2) },
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-1), Logout = null },
    };

        var chance = predictor.PredictUserOnline(userTimeSpans, DateTimeOffset.UtcNow.AddMinutes(-3));
        Assert.Equal(1.0, chance);
    }

    [Fact]
    public void When_PredictUserOnlineWasOnlineAtThisTime_ShouldReturn50PercentChance()
    {
        var detector = new OnlineDetector(new DateTimeProvider());
        var predictor = new Predictor(detector);

        var userTimeSpans = new List<UserTimeSpan>()
    {
      new() { Login = DateTimeOffset.UtcNow.AddDays(-10), Logout = DateTimeOffset.UtcNow.AddDays(-9) },
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-10), Logout = DateTimeOffset.UtcNow.AddMinutes(-5) },
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-4), Logout = DateTimeOffset.UtcNow.AddMinutes(-2) },
      new() { Login = DateTimeOffset.UtcNow.AddMinutes(-1), Logout = null },
    };

        var chance = predictor.PredictUserOnline(userTimeSpans, DateTimeOffset.UtcNow.AddMinutes(-3));
        Assert.Equal(0.5, chance);
    }
}
