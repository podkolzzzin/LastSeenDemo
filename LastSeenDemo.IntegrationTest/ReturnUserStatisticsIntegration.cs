using System.Runtime.CompilerServices;
using Xunit;
namespace LastSeenDemo;


public class ReturnUserStatisticsIntegration
{
    [Fact]
    public void ReturnUserStatistics()
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
            Assert.Contains("0", item);
         }
    }
}
