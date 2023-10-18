namespace LastSeenDemo;

public class Predictor
{
    private readonly IOnlineDetector _detector;
    public Predictor(IOnlineDetector detector)
    {
        _detector = detector;
    }

    public int PredictUsersOnline(Dictionary<Guid, List<UserTimeSpan>> allData, DateTimeOffset offset)
    {
        var minDate = allData.SelectMany(x => x.Value).Min(x => x.Login);

        var counts = new List<int>();
        while (offset > minDate)
        {
            var countOnline = _detector.CountOnline(allData, offset);
            counts.Add(countOnline);
            offset = offset.AddDays(-7);
        }
        return (int)Math.Round(counts.Average());
    }

    public double PredictUserOnline(List<UserTimeSpan> allData, DateTimeOffset offset)
    {
        if (allData.Count == 0)
        {
            return 0;
        }
        var firstLogin = allData.Min(x => x.Login);
        int totalCount = 0,
            wasOnlineCount = 0;
        while (offset > firstLogin)
        {
            var wasOnline = _detector.Detect(allData, offset);
            if (wasOnline)
            {
                wasOnlineCount++;
            }
            totalCount++;
            offset = offset.AddDays(-7);
        }

        if (totalCount == 0)
        {
            return 0;
        }
        return (double)wasOnlineCount / totalCount;
    }
}
