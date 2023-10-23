namespace LastSeenDemo;

using System;
public class Assignment5
{
    
}


public class Report
{
    //List<string> metrics = new List<string>() { "dailyAverage", "weeklyAverage", "total", "min", "max"};
    private Assignment4Features useful_stuff_returner = new Assignment4Features();

    public Dictionary<Guid, string[]> ReturnDesiredInfo(List<string> metrics, List<Guid> user_id_s)
    {
        List<string> output_piece = new List<string>();
        Dictionary<Guid, string[]> output = new Dictionary<Guid, string[]>();

        foreach (var user in user_id_s)
        {
            output.Add(user, new string[] {"", "", "", "", ""});
        }
        
        if (metrics.Contains("dailyAverage"))
        {
            foreach (var user in user_id_s)
            {
                var daily_time = useful_stuff_returner.ReturnAverageTime(user, new List<UserTimeSpan>()).Daily_Time;
                output[user][0] = daily_time.ToString();
            }
        }
        if (metrics.Contains("weeklyAverage"))
        {
            foreach (var user in user_id_s)
            {
                var weekly_time = useful_stuff_returner.ReturnAverageTime(user, new List<UserTimeSpan>()).Weekly_Time;
                output[user][1] = weekly_time.ToString();
            }
        }
        if (metrics.Contains("total"))
        {
            foreach (var user in user_id_s)
            {
                var total_time = useful_stuff_returner.ReturnTotalTimeOnline(user, new List<UserTimeSpan>());
                output[user][2] = total_time.ToString();
            }
        }
        if (metrics.Contains("min"))
        {
            List<int> mins = new List<int>();
            foreach (var user_id in user_id_s)
            {
                var now = DateTimeOffset.Now;
                //var min = useful_stuff_returner.ReturnTotalTimeOnline(user_id, new List<UserTimeSpan>());
                int min = 0;
                Loader loader = new Loader();
                Worker worker = new Worker(new UserLoader(loader, ""), new AllUsersTransformer(new UserTransformer(new DateTimeProvider())));
                if (worker.Users.ContainsKey(user_id))
                {
                    var uts = worker.Users[user_id];
                    
                    foreach (var ts in uts)
                    {
                        var tslogin = ts.Login;
                        var tslogout = ts.Logout;

                        if (tslogout.HasValue)
                        {
                            var difference = useful_stuff_returner.CountTotalTime(tslogout.Value, tslogin);
                            if (difference>min)
                            {
                                min = difference;
                            }
                        }
                    }
                    mins.Add(min);
                }
            }
        }
        if (metrics.Contains("max"))
        {
            List<int> maxs = new List<int>();
            foreach (var user_id in user_id_s)
            {
                var now = DateTimeOffset.Now;
                //var min = useful_stuff_returner.ReturnTotalTimeOnline(user_id, new List<UserTimeSpan>());
                int max = 0;
                Loader loader = new Loader();
                Worker worker = new Worker(new UserLoader(loader, ""), new AllUsersTransformer(new UserTransformer(new DateTimeProvider())));
                if (worker.Users.ContainsKey(user_id))
                {
                    var uts = worker.Users[user_id];
                    
                    foreach (var ts in uts)
                    {
                        var tslogin = ts.Login;
                        var tslogout = ts.Logout;

                        if (tslogout.HasValue)
                        {
                            var difference = useful_stuff_returner.CountTotalTime(tslogout.Value, tslogin);
                            if (difference>max)
                            {
                                max = difference;
                            }
                        }
                    }
                    maxs.Add(max);
                }
            }
        }

        return output;
    }


}
