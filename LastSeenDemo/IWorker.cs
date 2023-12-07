namespace LastSeenDemo;

public interface IWorker
{
    Dictionary<Guid, List<UserTimeSpan>> Users { get; }
    List<Guid> OnlineUsers { get; }

    void LoadDataPeriodically();
    void LoadDataIteration();
    void Forget(Guid userId);
}
