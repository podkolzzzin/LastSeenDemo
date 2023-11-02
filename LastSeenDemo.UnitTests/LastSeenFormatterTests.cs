namespace LastSeenDemo.UnitTests;

/*
 * User Is Online -> Online
 * 15 seconds ago -> Just now
 * 40 seconds ago -> Less than a minute ago
 * 3 minutes ago -> Couple of minutes ago
 * 65 minutes ago -> Hour ago
 * 5h ago of the same Date -> Today
 * 5h ago but another Date -> Yesterday
 * 2 days ago -> less than a week
 * 30 days ago -> long time ago
 */

public class LastSeenFormatterTests
{
    private readonly DateTimeOffset
      _now = new(2000, 01, 01, 12, 00, 00, TimeSpan.Zero);

    [Fact]
    public void When_UserIsOnline_ShouldReturn_Online()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("Online", formatter.Format(_now, _now));
    }

    [Fact]
    public void When_UserWasOnline15SecondsAgo_ShouldReturn_JustNow()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("Just Now", formatter.Format(_now, _now - TimeSpan.FromSeconds(15)));
    }

    [Fact]
    public void When_UserWasOnline40SecondsAgo_ShouldReturn_LessThanAMinuteAgo()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("Less than a minute ago", formatter.Format(_now, _now - TimeSpan.FromSeconds(40)));
    }

    [Fact]
    public void When_UserWasOnline3MinutesAgo_ShouldReturn_CoupleOfMinutesAgo()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("Couple of minutes ago", formatter.Format(_now, _now - TimeSpan.FromMinutes(3)));
    }

    [Fact]
    public void When_UserWasOnline65MinutesAgo_ShouldReturn_HourAgo()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("an hour ago", formatter.Format(_now, _now - TimeSpan.FromMinutes(65)));
    }

    [Fact]
    public void When_UserWasOnline5HoursAgoOfTheSameDate_ShouldReturn_Today()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("today", formatter.Format(_now, _now - TimeSpan.FromHours(5)));
    }

    [Fact]
    public void When_UserWasOnline13HoursAgoButAnotherDate_ShouldReturn_Yesterday()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("yesterday", formatter.Format(_now, _now - TimeSpan.FromHours(13)));
    }

    [Fact]
    public void When_UserWasOnline2DaysAgo_ShouldReturn_LessThanAWeek()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("this week", formatter.Format(_now, _now - TimeSpan.FromDays(2)));
    }

    [Fact]
    public void When_UserWasOnline30DaysAgo_ShouldReturn_LongTimeAgo()
    {
        var formatter = new LastSeenFormatter();
        Assert.Equal("long time ago", formatter.Format(_now, _now - TimeSpan.FromDays(30)));
    }
}
