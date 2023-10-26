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
        var result = report.ReturnDesiredInfo(new List<string>()
        {
            "dailyAverage",
            "weeklyAverage",
            "total",
            "min",
            "max"
        }, new List<Guid>());
        //change
         foreach (var item in result.Values)
         {
            Assert.Contains(expected, item);
         }
    }
}
