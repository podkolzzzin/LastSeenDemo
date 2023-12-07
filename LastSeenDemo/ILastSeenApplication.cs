namespace LastSeenDemo;

public interface ILastSeenApplication
{
    List<string> Show(DateTimeOffset now);
}
