namespace LastSeenDemo.IntegrationTest;

public class ReportManagementTests
{
    [Fact]
    public void LoadReports_ShouldPopulateReportsList()
    {
        var reportManagement = new ReportManagement();

        Assert.NotNull(reportManagement.Reports);
    }

    [Fact]
    public void AddReport_ShouldIncreaseReportsListSize()
    {
        var reportManagement = new ReportManagement();
        var initialCount = reportManagement.Reports.Count;

        var newReport = new Report("TestReport", new List<Guid>(), new List<string>(), null, null);
        reportManagement.AddReport(newReport);

        Assert.Equal(initialCount + 1, reportManagement.Reports.Count);
    }
}


