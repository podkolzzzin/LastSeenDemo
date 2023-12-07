namespace LastSeenDemo;

public interface ILastSeenFormatter
{
    string Format(DateTimeOffset now, DateTimeOffset lastSeen);
}


public class LastSeenFormatter : ILastSeenFormatter
{
    public string Format(DateTimeOffset now, DateTimeOffset lastSeen)
    {
        var span = now - lastSeen;
        if (span == TimeSpan.Zero)
        {
            return "Online";
        }
        else if (span < TimeSpan.FromSeconds(30))
        {
            return "Just Now";
        }
        else if (span < TimeSpan.FromMinutes(1))
        {
            return "Less than a minute ago";
        }
        else if (span < TimeSpan.FromMinutes(60))
        {
            return "Couple of minutes ago";
        }
        else if (span < TimeSpan.FromMinutes(120))
        {
            return "an hour ago";
        }
        else if (now.Date == lastSeen.Date)
        {
            return "today";
        }
        else if (now.Date - lastSeen.Date == TimeSpan.FromDays(1))
        {
            return "yesterday";
        }
        else if (span < TimeSpan.FromDays(7))
        {
            return "this week";
        }
        else
        {
            return "long time ago";
        }
    }
}
