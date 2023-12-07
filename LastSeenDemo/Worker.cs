namespace LastSeenDemo;

public interface IWorker
{
    Dictionary<Guid, List<UserTimeSpan>> Users { get; }
    List<Guid> OnlineUsers { get; }

    void LoadDataPeriodically();
    void LoadDataIteration();
    void Forget(Guid userId);
}

public class Worker : IWorker
{
    private readonly IUserLoader _loader;
    private readonly IAllUsersTransformer _transformer;
    private readonly List<Guid> _forgottenUsers = new();

    public Worker(IUserLoader loader, IAllUsersTransformer transformer)
    {
        _loader = loader;
        _transformer = transformer;
        Users = new Dictionary<Guid, List<UserTimeSpan>>();
    }

    public Dictionary<Guid, List<UserTimeSpan>> Users { get; private set; }
    public List<Guid> OnlineUsers { get; private set; } = new();

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
