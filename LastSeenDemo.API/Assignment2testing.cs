using Xunit;

namespace LastSeenDemo;


public class ToBeTestedCoverTests2ElectricBoogaloo
{
    [Fact]
    public void JustNowScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-22T06:45:36.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online jsut now", few_sec_ago);          // 20 sec difference
    }
    [Fact]
    public void AMinAgoScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-22T06:44:46.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online a minute ago", few_sec_ago);          //  1 min 10 sec difference
    }
    [Fact]
    public void FewMinAgoScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-22T06:25:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online a few minutes ago", few_sec_ago);          // 20 min difference
    }
    [Fact]
    public void AnHourAgoScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-22T05:43:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online an hour ago", few_sec_ago);          // 1 hour 2 min difference
    }
    [Fact]
    public void AnTodayScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-22T01:23:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online today", few_sec_ago);          // 5 hours 12 min difference
    }
    [Fact]
    public void YesterdayScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-09-21T05:45:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online yesterday", few_sec_ago);          // 25 hours difference
    }

    [Fact]
    public void ThisWeekScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean",
            "abcdefghijklmnop2023-09-18T06:45:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online this week", few_sec_ago); // 4 days difference
    }
    [Fact]
    public void LongAgoScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var few_sec_ago = program.HumanizeTime("31545374634mr Bean", "abcdefghijklmnop2023-02-22T06:45:56.5192279+00:00", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" was online long ago", few_sec_ago);          // 2 months difference
    }
    [Fact]
    public void OnlineScenario()
    {
        Assignment2Features program = new Assignment2Features();
        var when_last_online = program.HumanizeTime("31545374634mr Bean", "null", "2023-09-22T06:45:56.5192279+00:00");
        Assert.Equal(" is online", when_last_online);          //
    }
}
