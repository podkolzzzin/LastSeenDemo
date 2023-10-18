namespace LastSeenDemo;

public interface IOnlineDetector
{
    bool Detect(List<UserTimeSpan> data, DateTimeOffset date);
    DateTimeOffset? GetClosestOnlineTime(List<UserTimeSpan> data, DateTimeOffset date);
    int CountOnline(Dictionary<Guid, List<UserTimeSpan>> users, DateTimeOffset date);
    double CalculateTotalTimeForUser(List<UserTimeSpan> value);
    double CalculateTotalTimeForUser(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to);
    double CalculateDailyAverageForUser(List<UserTimeSpan> user);
    double CalculateWeeklyAverageForUser(List<UserTimeSpan> user);
}
