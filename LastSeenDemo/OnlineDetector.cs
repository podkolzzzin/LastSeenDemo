using LastSeenDemo;

public interface IOnlineDetector
{
    bool Detect(List<UserTimeSpan> data, DateTimeOffset date);
    DateTimeOffset? GetClosestOnlineTime(List<UserTimeSpan> data, DateTimeOffset date);
    int CountOnline(Dictionary<Guid, List<UserTimeSpan>> users, DateTimeOffset date);
    double CalculateTotalTimeForUser(List<UserTimeSpan> value);
    double CalculateTotalTimeForUser(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to);
    double CalculateDailyAverageForUser(List<UserTimeSpan> user);
    double CalculateWeeklyAverageForUser(List<UserTimeSpan> user);
    int CalculateGlobalDailyAverageForAllUsers(Dictionary<Guid, List<UserTimeSpan>> userTimeSpans);
}

public class OnlineDetector : IOnlineDetector
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public OnlineDetector(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public bool Detect(List<UserTimeSpan> data, DateTimeOffset date)
    {
        foreach (var interval in data)
        {
            if (interval.Login <= date && (interval.Logout == null || interval.Logout >= date))
            {
                return true;
            }
        }
        return false;
    }

    public DateTimeOffset? GetClosestOnlineTime(List<UserTimeSpan> data, DateTimeOffset date)
    {
        DateTimeOffset? closestTime = null;
        TimeSpan closestDifference = TimeSpan.MaxValue;

        foreach (var interval in data)
        {
            if (interval.Login <= date && (interval.Logout == null || interval.Logout >= date))
            {
                var difference = date - interval.Login;
                if (difference < closestDifference)
                {
                    closestDifference = difference;
                    closestTime = interval.Login;
                }
            }
            else if (interval.Login > date && (closestTime == null || interval.Login - date < closestDifference))
            {
                closestDifference = interval.Login - date;
                closestTime = interval.Login;
            }
        }

        return closestTime;
    }

    public int CountOnline(Dictionary<Guid, List<UserTimeSpan>> users, DateTimeOffset date)
    {
        return users.Values.Count(u => Detect(u, date));
    }

    public double CalculateTotalTimeForUser(List<UserTimeSpan> value)
    {
        double totalTime = 0;
        foreach (var onlineInterval in value)
        {
            var login = onlineInterval.Login;
            // If user was online during last check let's assume he is still online
            var logout = onlineInterval.Logout ?? _dateTimeProvider.GetCurrentTime();

            totalTime += (logout - login).TotalSeconds;
        }
        return totalTime;
    }

    public double CalculateTotalTimeForUser(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to)
    {
        double totalTime = 0;
        foreach (var onlineInterval in value)
        {
            var login = onlineInterval.Login;
            // If user was online during last check let's assume he is still online
            var logout = onlineInterval.Logout ?? _dateTimeProvider.GetCurrentTime();

            if (login < from)
            {
                login = from;
            }
            if (logout < from)
            {
                logout = from;
            }

            if (login > to)
            {
                login = to;
            }
            if (logout > to)
            {
                logout = to;
            }

            totalTime += (logout - login).TotalSeconds;
        }
        return totalTime;
    }

    public double CalculateDailyAverageForUser(List<UserTimeSpan> user)
    {
        return CalculateAverageForUser(user, TimeSpan.FromDays(1));
    }

    public double CalculateWeeklyAverageForUser(List<UserTimeSpan> user)
    {
        return CalculateAverageForUser(user, TimeSpan.FromDays(7));
    }

    private double CalculateAverageForUser(List<UserTimeSpan> user, TimeSpan stepInterval)
    {
        var firstOnline = user.Min(x => x.Login);
        var dayStart = _dateTimeProvider.GetCurrentTime().Date;
        var dayEnd = dayStart + stepInterval;

        int totalDays = 0;
        double totalTime = 0;
        while (dayEnd > firstOnline)
        {
            totalDays++;

            totalTime += CalculateTotalTimeForUser(user, dayStart, dayEnd);

            dayEnd = dayEnd - stepInterval;
            dayStart = dayStart - stepInterval;
        }

        return totalTime / totalDays;
    }
    
    /// <summary>
    /// Calculates the global daily average time for all users.
    /// </summary>
    /// <param name="userTimeSpans">Dictionary containing user IDs and their corresponding lists of time spans.</param>
    /// <returns>The global daily average time as an integer.</returns>
    public int CalculateGlobalDailyAverageForAllUsers(Dictionary<Guid, List<UserTimeSpan>> userTimeSpans)
    {
        // Initialize a variable to accumulate the total daily average across all users.
        double totalDailyAverage = 0;

        // Iterate over each user's time spans in the dictionary.
        foreach (var user in userTimeSpans.Values)
        {
            // Add the calculated daily average for each user to the total.
            totalDailyAverage += CalculateDailyAverageForUser(user);
        }

        // Calculate the global daily average by rounding the total daily average.
        // This converts it to an integer for a consistent and simplified data type.
        int globalDailyAverage = (int)Math.Round(totalDailyAverage);

        return globalDailyAverage;
    }
}

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
