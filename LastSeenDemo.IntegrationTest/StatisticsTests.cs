using LastSeenDemo.E2E;

namespace LastSeenDemo;

public class StatisticsTests
{
    [Fact]
    public void OverallShouldReturnResult()
    {
        var result = HttpHelper.Get("/api/report/statistics");
        Assert.NotNull(result);
    }
}
