namespace LastSeenDemo;

public interface IDateTimeProvider
{
    public DateTimeOffset GetCurrentTime();
}

public class DateTimeProvider : IDateTimeProvider
{

    public DateTimeOffset GetCurrentTime()
    {
        return DateTimeOffset.UtcNow;
    }
}

public interface IUserTransformer
{
    void TransformSingleUser(User stateOfUserInCurrentTime, bool wasOnline, List<UserTimeSpan> userTimeSpans);
}

public class UserTransformer : IUserTransformer
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public UserTransformer(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public void TransformSingleUser(User stateOfUserInCurrentTime, bool wasOnline, List<UserTimeSpan> userTimeSpans)
    {
        if (wasOnline)
        {
            if (stateOfUserInCurrentTime.IsOnline)
            {
                userTimeSpans.Last().Logout = _dateTimeProvider.GetCurrentTime();
            }
            else
            {
                userTimeSpans.Last().Logout = stateOfUserInCurrentTime.LastSeenDate.Value;
            }
        }
        else
        {
            if (stateOfUserInCurrentTime.IsOnline)
            {
                userTimeSpans.Add(new UserTimeSpan() { Login = _dateTimeProvider.GetCurrentTime(), Logout = null });
            }
        }
    }
}
