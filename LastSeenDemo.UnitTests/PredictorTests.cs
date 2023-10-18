using LastSeenDemo.UnitTests.Mocks;

namespace LastSeenDemo.UnitTests;

using System;
using System.Collections.Generic;
using Moq;
using Xunit;

public class PredictorTests
{
    [Fact]
    public void PredictUsersOnline_ReturnsAverageCountOnline()
    {
        // Arrange
        var allData = new Dictionary<Guid, List<UserTimeSpan>>()
        {
            [Guid.NewGuid()] = new() { new UserTimeSpan() { Login = MockDateTimeProvider.DefaultMockDate.AddDays(-14) } }
        };
        var offset = DateTimeOffset.Now;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Set up the detector's behavior
        detectorMock.Setup(detector => detector.CountOnline(It.IsAny<Dictionary<Guid, List<UserTimeSpan>>>(), It.IsAny<DateTimeOffset>()))
          .Returns(5); // Mock CountOnline to return 5

        // Act
        var result = predictor.PredictUsersOnline(allData, offset);

        // Assert
        Assert.Equal(5, result); // Since CountOnline always returns 5, the average should be 5
    }

    [Fact]
    public void PredictUsersOnline_WhenNoData_ReturnsZero()
    {
        // Arrange
        var allData = new Dictionary<Guid, List<UserTimeSpan>>()
        {
            [Guid.NewGuid()] = new() { new UserTimeSpan() { Login = MockDateTimeProvider.DefaultMockDate.AddDays(-14) } }
        };
        var offset = DateTimeOffset.Now;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Act
        var result = predictor.PredictUsersOnline(allData, offset);

        // Assert
        Assert.Equal(0, result); // Since there's no data, the result should be 0
    }

    [Fact]
    public void PredictUsersOnline_WithMultipleOffsets_CallsCountOnlineCorrectly()
    {
        // Arrange
        var allData = new Dictionary<Guid, List<UserTimeSpan>>()
        {
            [Guid.NewGuid()] = new() { new UserTimeSpan() { Login = MockDateTimeProvider.DefaultMockDate.AddDays(-7 * 8) } }
        };
        var offset = MockDateTimeProvider.DefaultMockDate;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Set up the detector's behavior
        detectorMock.Setup(detector => detector.CountOnline(It.IsAny<Dictionary<Guid, List<UserTimeSpan>>>(), It.IsAny<DateTimeOffset>()))
          .Returns(5); // Mock CountOnline to return 5

        // Act
        predictor.PredictUsersOnline(allData, offset);

        // Assert
        // Verify that CountOnline is called the correct number of times
        detectorMock.Verify(detector => detector.CountOnline(allData, It.IsAny<DateTimeOffset>()), Times.Exactly(8));
    }

    [Fact]
    public void PredictUserOnline_ReturnsCorrectOnlineRatio()
    {
        // Arrange
        var allData = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Now.AddMinutes(-30) },
      new UserTimeSpan { Login = DateTimeOffset.Now.AddMinutes(-60) },
      new UserTimeSpan { Login = DateTimeOffset.Now.AddMinutes(-90) }
      // Add more UserTimeSpans as needed
    };
        var offset = DateTimeOffset.Now;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Set up the detector's behavior
        detectorMock.Setup(detector => detector.Detect(It.IsAny<List<UserTimeSpan>>(), It.IsAny<DateTimeOffset>()))
          .Returns(true); // Mock Detect to always return true

        // Act
        var result = predictor.PredictUserOnline(allData, offset);

        // Assert
        Assert.Equal(1.0, result); // All data points are online, so the result should be 1.0
    }

    [Fact]
    public void PredictUserOnline_WhenNoData_ReturnsZero()
    {
        // Arrange
        var allData = new List<UserTimeSpan>();
        var offset = DateTimeOffset.Now;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Act
        var result = predictor.PredictUserOnline(allData, offset);

        // Assert
        Assert.Equal(0.0, result); // Since there's no data, the result should be 0.0
    }

    [Fact]
    public void PredictUserOnline_WithMultipleOffsets_CallsDetectCorrectly()
    {
        // Arrange
        var allData = new List<UserTimeSpan>
    {
      new UserTimeSpan { Login = DateTimeOffset.Now.AddMinutes(-30) }
      // Add more UserTimeSpans as needed
    };
        var offset = DateTimeOffset.Now;
        var detectorMock = new Mock<IOnlineDetector>();
        var predictor = new Predictor(detectorMock.Object);

        // Set up the detector's behavior
        detectorMock.Setup(detector => detector.Detect(It.IsAny<List<UserTimeSpan>>(), It.IsAny<DateTimeOffset>()))
          .Returns(true); // Mock Detect to always return true

        // Act
        predictor.PredictUserOnline(allData, offset);

        // Assert
        // Verify that Detect is called the correct number of times
        detectorMock.Verify(detector => detector.Detect(allData, It.IsAny<DateTimeOffset>()), Times.Exactly(1));
    }
}
