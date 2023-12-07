namespace LastSeenDemo;

public interface IPredictor
{
    int PredictUsersOnline(Dictionary<Guid, List<UserTimeSpan>> allData, DateTimeOffset offset);

    double PredictUserOnline(List<UserTimeSpan> allData, DateTimeOffset offset);
}
