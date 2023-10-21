using System.Runtime.CompilerServices;
using Xunit;

namespace LastSeenDemo;

public class Assignment5Tests
{
    [Theory]
    [InlineData("0")]
    public void ReturnTotalTimeOnline(string expected)
    {
        Report report = new Report();
        Guid userId = Guid.NewGuid(); 
        //int result = features.ReturnTotalTimeOnline(userId, new List<UserTimeSpan>());
        var result = report.ReturnDesiredInfo(new List<string>() {"dailyAverage", "weeklyAverage", "total"}, new List<Guid>() {new Guid()});
        //Assert.True();
        List<string> specificList = result.Values.FirstOrDefault();

        bool containsZero = specificList != null && specificList.Count > 0 && specificList[0].Contains(expected);
        
        Assert.True(containsZero);
    }
}
