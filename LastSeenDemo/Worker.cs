namespace LastSeenDemo;

public class Worker
{
    private readonly UserLoader _loader;
    private readonly AllUsersTransformer _transformer;
    private readonly List<Guid> _forgottenUsers = new();

    public Worker(UserLoader loader, AllUsersTransformer transformer)
    {
        _loader = loader;
        _transformer = transformer;
        Users = new Dictionary<Guid, List<UserTimeSpan>>();
    }
    /*
    public ReportData GenerateReportData(string reportName, DateTimeOffset from, DateTimeOffset to)
    {
        var reportData = new ReportData
        {
            ReportName = reportName,
            From = from,
            To = to,
            UserSummaries = new Dictionary<Guid, UserActivitySummary>()
        };

        foreach (var (userId, timeSpans) in Users)
        {
            var summary = new UserActivitySummary
            {
                TotalSessions = timeSpans.Count(ts => ts.Start >= from && ts.End <= to),
                TotalDuration = timeSpans.Where(ts => ts.Start >= from && ts.End <= to)
                    .Aggregate(TimeSpan.Zero, (sum, next) => sum + (next.End - next.Start))
            };
            
            reportData.UserSummaries[userId] = summary;
        }

        return reportData;
    }
    */

    public Dictionary<Guid, List<UserTimeSpan>> Users { get; set; }
    public List<Guid> OnlineUsers { get; } = new();

    public void LoadDataPeriodically()
    {
        while (true)
        {
            Console.WriteLine("Loading data");
            LoadDataIteration();
            Console.WriteLine("Data loaded");
            Thread.Sleep(5000);
        }
    }

    public void LoadDataIteration()
    {
        var allUsers = _loader.LoadAllUsers().ToList();
        allUsers.RemoveAll(x => _forgottenUsers.Contains(x.UserId));
        _transformer.Transform(allUsers, OnlineUsers, Users);
    }

    public void Forget(Guid userId)
    {
        _forgottenUsers.Add(userId);
        Users.Remove(userId);
        OnlineUsers.Remove(userId);
    }
}
