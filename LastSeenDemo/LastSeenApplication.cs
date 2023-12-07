namespace LastSeenDemo
{
    public interface ILastSeenApplication
    {
        List<string> Show(DateTimeOffset now);
    }

    public class LastSeenApplication : ILastSeenApplication
    {
        private readonly IUserLoader _userLoader; // Use IUserLoader interface

        public LastSeenApplication(IUserLoader userLoader)
        {
            _userLoader = userLoader;
        }

        public List<string> Show(DateTimeOffset now)
        {
            var users = _userLoader.LoadAllUsers();
            var format = new LastSeenFormatter();

            var result = new List<string>();
            foreach (var u in users)
            {
                result.Add($"{u.Nickname} {format.Format(now, u.LastSeenDate ?? now)}");
            }
            return result;
        }
    }
}
