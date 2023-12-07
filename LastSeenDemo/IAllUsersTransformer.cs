namespace LastSeenDemo;

public interface IAllUsersTransformer
{
    void Transform(IEnumerable<User> allUsers, List<Guid> onlineUsers, Dictionary<Guid, List<UserTimeSpan>> result);
}

