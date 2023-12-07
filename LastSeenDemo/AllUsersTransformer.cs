namespace LastSeenDemo;

public class AllUsersTransformer : IAllUsersTransformer
{
    private readonly IUserTransformer _transformer;
    public AllUsersTransformer(IUserTransformer transformer)
    {
        _transformer = transformer;
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
}