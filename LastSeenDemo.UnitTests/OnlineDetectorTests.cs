using LastSeenDemo.UnitTests.Mocks;

namespace LastSeenDemo.UnitTests;

using System;
using System.Collections.Generic;
using Xunit;

public class OnlineDetectorTests
{
    private readonly OnlineDetector _onlineDetector = new(new MockDateTimeProvider());

    [Fact]
    public void When_UserIsOnline_Should_ReturnTrue()
    {
        // Arrange
        var data = new List<UserTimeSpan>
    {
      new() { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new() { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = null },
      new() { Login = DateTimeOffset.Parse("2022-01-01 16:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 18:00:00") }
    };
        var date = DateTimeOffset.Parse("2022-01-01 15:00:00");

        // Act
        var result = _onlineDetector.Detect(data, date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void When_UserIsOffline_Should_ReturnFalse()
    {
        // Arrange
        var data = new List<UserTimeSpan>
    {
      new() { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new() { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 16:00:00") },
      new() { Login = DateTimeOffset.Parse("2022-01-01 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 20:00:00") }
    };
        var date = DateTimeOffset.Parse("2022-01-01 13:00:00");

        // Act
        var result = _onlineDetector.Detect(data, date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void When_NoDataProvided_Should_ReturnFalse()
    {
        // Arrange
        var data = new List<UserTimeSpan>();
        var date = DateTimeOffset.Parse("2022-01-01 15:00:00");

        // Act
        var result = _onlineDetector.Detect(data, date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void When_UserIsOnlineAtSpecifiedDate_Should_ReturnLoginTime()
    {
        // Arrange
        var data = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = null },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 16:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 18:00:00") }
    };
        var date = DateTimeOffset.Parse("2022-01-01 15:00:00");

        // Act
        var closestTime = _onlineDetector.GetClosestOnlineTime(data, date);

        // Assert
        Assert.Equal(DateTimeOffset.Parse("2022-01-01 14:00:00"), closestTime);
    }

    [Fact]
    public void When_UserIsOfflineBeforeSpecifiedDate_Should_ReturnClosestLoginTime()
    {
        // Arrange
        var data = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 16:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 20:00:00") }
    };
        var date = DateTimeOffset.Parse("2022-01-01 11:00:00");

        // Act
        var closestTime = _onlineDetector.GetClosestOnlineTime(data, date);

        // Assert
        Assert.Equal(DateTimeOffset.Parse("2022-01-01 10:00:00"), closestTime);
    }

    [Fact]
    public void When_UserIsOfflineAfterSpecifiedDate_Should_ReturnClosestLoginTime()
    {
        // Arrange
        var data = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 16:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 20:00:00") }
    };
        var date = DateTimeOffset.Parse("2022-01-01 17:00:00");

        // Act
        var closestTime = _onlineDetector.GetClosestOnlineTime(data, date);

        // Assert
        Assert.Equal(DateTimeOffset.Parse("2022-01-01 18:00:00"), closestTime);
    }

    [Fact]
    public void When_NoDataProvided_Should_ReturnNull()
    {
        // Arrange
        var data = new List<UserTimeSpan>();
        var date = DateTimeOffset.Parse("2022-01-01 15:00:00");

        // Act
        var closestTime = _onlineDetector.GetClosestOnlineTime(data, date);

        // Assert
        Assert.Null(closestTime);
    }

    [Fact]
    public void CountOnline_Should_ReturnCorrectCount()
    {
        // Arrange
        var users = new Dictionary<Guid, List<UserTimeSpan>>
        {
            [Guid.NewGuid()] = new List<UserTimeSpan>
      {
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = null },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 16:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 18:00:00") }
      },
            [Guid.NewGuid()] = new List<UserTimeSpan>
      {
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 09:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 11:00:00") },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 13:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 15:00:00") },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 17:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 19:00:00") }
      },
            [Guid.NewGuid()] = new List<UserTimeSpan>
      {
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 08:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 10:00:00") },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 12:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 14:00:00") },
        new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 16:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 18:00:00") }
      }
        };
        var date = DateTimeOffset.Parse("2022-01-01 15:00:00");

        // Act
        var count = _onlineDetector.CountOnline(users, date);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void CalculateTotalTimeForUser_Should_ReturnCorrectTotalTime()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 12:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 16:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 18:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 20:00:00") }
    };

        // Act
        var totalTime = _onlineDetector.CalculateTotalTimeForUser(userTimeSpans);

        // Assert
        Assert.Equal(21600, totalTime); // 6 hours * 60 minutes * 60 seconds = 21600 seconds
    }

    [Fact]
    public void CalculateTotalTimeForUser_Should_ReturnZero_When_NoUserTimeSpansProvided()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>();

        // Act
        var totalTime = _onlineDetector.CalculateTotalTimeForUser(userTimeSpans);

        // Assert
        Assert.Equal(0, totalTime);
    }

    [Fact]
    public void CalculateTotalTimeForUser_Should_ReturnCorrectTotalTime_When_UserIsStillOnline()
    {
        // Arrange
        var userTimeSpans = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 10:00:00"), Logout = null },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 14:00:00"), Logout = DateTimeOffset.Parse("2022-01-01 16:00:00") },
      new UserTimeSpan { Login = DateTimeOffset.Parse("2022-01-01 18:00:00"), Logout = null }
    };

        // Act
        var totalTime = _onlineDetector.CalculateTotalTimeForUser(userTimeSpans);

        // Assert
        var expectedTotalTime = (MockDateTimeProvider.DefaultMockDate - DateTimeOffset.Parse("2022-01-01 10:00:00")).TotalSeconds +
                                (DateTimeOffset.Parse("2022-01-01 16:00:00") - DateTimeOffset.Parse("2022-01-01 14:00:00")).TotalSeconds +
                                (MockDateTimeProvider.DefaultMockDate - DateTimeOffset.Parse("2022-01-01 18:00:00")).TotalSeconds;
        Assert.Equal(expectedTotalTime, totalTime);
    }
}
