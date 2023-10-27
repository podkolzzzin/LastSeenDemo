namespace LastSeenDemo.E2E;
using Xunit;
using System.Net;
using System.Collections.Generic;

// run it on local and may the Force be with me

public class ReturnUserStatisticsE2E
{
    [Fact]
    public void E2EUserStatistics()
    {
        Assignment5 assignment5 = new Assignment5();
        var userlist = assignment5.GetUserList();
        Report report = new Report();
       // string[] report_args = new string[] { "dailyAverage", "weeklyAverage", "total", "min", "max"};
       List<string> report_args = new List<string>() { "dailyAverage", "weeklyAverage", "total", "min", "max"};
       List<Guid> user_ids = new List<Guid>();
        foreach (var single_user in userlist)
        {
            user_ids.Add(single_user.UserId);
        }

        var testable = report.ReturnDesiredInfo(report_args, user_ids);
        
        //Assert.Equal("", "");
        Assert.True(true);
        Assert.Equal("0", testable[new Guid("a69de642-dd03-ba60-2241-9952b57463ef")][0]);
        Assert.Equal("0", testable[new Guid("a69de642-dd03-ba60-2241-9952b57463ef")][1]);
        Assert.Equal("0", testable[new Guid("a69de642-dd03-ba60-2241-9952b57463ef")][2]);
        Assert.Equal("0", testable[new Guid("a69de642-dd03-ba60-2241-9952b57463ef")][3]);
        Assert.Equal("0", testable[new Guid("a69de642-dd03-ba60-2241-9952b57463ef")][4]);
    }
}
