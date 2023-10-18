using LastSeenDemo.UnitTests.Mocks;

namespace LastSeenDemo.UnitTests;

public class UserTransformerTests
{
    [Fact]
    public void When_UserAppearedOnline_Should_AddUserToTheTimeSpansCollection()
    {
        var mockDateTimeProvider = new MockDateTimeProvider();
        var transformer = new UserTransformer(mockDateTimeProvider);
        var id = Guid.NewGuid();
        var userTimeSpans = new List<UserTimeSpan>();

        transformer.TransformSingleUser(new() { IsOnline = true, UserId = id }, false, userTimeSpans);

        Assert.Single(userTimeSpans);
        Assert.Equal(MockDateTimeProvider.DefaultMockDate, userTimeSpans.Single().Login);
        Assert.Null(userTimeSpans.Single().Logout);
    }

    [Fact]
    public void When_UserWasOnline_Should_ExtendInterval()
    {
        var mockDate = new DateTime(2000, 01, 31);
        var mockDateTimeProvider = new MockDateTimeProvider(mockDate);
        var transformer = new UserTransformer(mockDateTimeProvider);
        var id = Guid.NewGuid();
        var userTimeSpans = new List<UserTimeSpan>();
        userTimeSpans.Add(new UserTimeSpan() { Login = mockDate.AddDays(-1), Logout = mockDate.AddDays(-1) });

        transformer.TransformSingleUser(new() { IsOnline = true, UserId = id }, true, userTimeSpans);

        Assert.Single(userTimeSpans);
        Assert.Equal(mockDate.AddDays(-1), userTimeSpans.Single().Login);
        Assert.Equal(mockDate, userTimeSpans.Single().Logout);
    }

    [Fact]
    public void When_UserWasOnlineButNowNot_Should_UpdateLogoutTimeWithLastSeenProperty()
    {
        var mockDate = new DateTime(2000, 01, 31);
        var mockDateTimeProvider = new MockDateTimeProvider(mockDate);
        var transformer = new UserTransformer(mockDateTimeProvider);
        var id = Guid.NewGuid();
        var userTimeSpans = new List<UserTimeSpan>();
        userTimeSpans.Add(new UserTimeSpan() { Login = mockDate.AddDays(-3), Logout = mockDate.AddDays(-2) });

        transformer.TransformSingleUser(new() { IsOnline = false, LastSeenDate = mockDate.AddDays(-1), UserId = id }, true, userTimeSpans);

        Assert.Single(userTimeSpans);
        Assert.Equal(mockDate.AddDays(-3), userTimeSpans.Single().Login);
        Assert.Equal(mockDate.AddDays(-1), userTimeSpans.Single().Logout);
    }

    [Fact]
    public void When_UserWasNotOnlineAndStillNotOnline_Should_DoNothing()
    {
        var mockDate = new DateTime(2000, 01, 31);
        var mockDateTimeProvider = new MockDateTimeProvider(mockDate);
        var transformer = new UserTransformer(mockDateTimeProvider);
        var id = Guid.NewGuid();
        var userTimeSpans = new List<UserTimeSpan>();
        userTimeSpans.Add(new UserTimeSpan() { Login = mockDate.AddDays(-3), Logout = mockDate.AddDays(-2) });

        transformer.TransformSingleUser(new() { IsOnline = false, LastSeenDate = mockDate.AddDays(-1), UserId = id }, false, userTimeSpans);

        Assert.Single(userTimeSpans);
        Assert.Equal(mockDate.AddDays(-3), userTimeSpans.Single().Login);
        Assert.Equal(mockDate.AddDays(-2), userTimeSpans.Single().Logout);
    }
}
