namespace LastSeenDemo;

public class GlobalMetrics
{
    public int DailyAverage { get; set; }
}
public class AllUsersTransformer
{
    private readonly IUserTransformer _transformer;
    private readonly IOnlineDetector _detector;
    public AllUsersTransformer(IUserTransformer transformer, IOnlineDetector detector) 
    {
        _transformer = transformer;
        _detector = detector; 
    }

    public void Transform(IEnumerable<User> allUsers, List<Guid> onlineUsers, Dictionary<Guid, List<UserTimeSpan>> result)
    {
        foreach (var user in allUsers)
        {
            if (!result.TryGetValue(user.UserId, out var userTimeSpans))
            {
                userTimeSpans = new List<UserTimeSpan>();
                result.Add(user.UserId, userTimeSpans);
            }
            var wasOnline = onlineUsers.Contains(user.UserId);
            _transformer.TransformSingleUser(user, wasOnline, userTimeSpans);

            if (!wasOnline && user.IsOnline)
            {
                onlineUsers.Add(user.UserId);
            }
            else if (!user.IsOnline)
            {
                onlineUsers.Remove(user.UserId);
            }
        }
    }
    
    /// <summary>
    /// Calculates the global metrics based on user time spans.
    /// </summary>
    /// <param name="userTimeSpans">A dictionary mapping user IDs (Guid) to lists of their time spans (UserTimeSpan).</param>
    /// <returns>A GlobalMetrics object containing computed metrics.</returns>
    public GlobalMetrics CalculateGlobalMetrics(Dictionary<Guid, List<UserTimeSpan>> userTimeSpans)
    {
        // Create a new instance of GlobalMetrics.
        var globalMetrics = new GlobalMetrics
        {
            // Calculate and set the daily average for all users using the provided user time spans.
            // The calculation is delegated to a method of the _detector object.
            DailyAverage = _detector.CalculateGlobalDailyAverageForAllUsers(userTimeSpans), 
        };

        // Return the calculated global metrics.
        return globalMetrics;
    }
}
