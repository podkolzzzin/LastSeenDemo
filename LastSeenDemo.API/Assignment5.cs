namespace LastSeenDemo;
using System.Net;
using System.Collections.Generic;


using System;
public class Assignment5
{
    public List<User> GetUserList()
    {
        List<User> users_list = new List<User>();
        string json;
        using (WebClient wc = new WebClient())
        {
            json = wc.DownloadString("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
        }
        string[] json_no_square_brackets = json.Split('[');
        string[] separate_users_data = json_no_square_brackets[1].Split('{');

        foreach (var sep_datum in separate_users_data)
        {
            if (sep_datum != "")
            {
                string[] user_pieces = sep_datum.Split(',');
                List<string> temp_components = new List<string>();
                foreach (var user_piece in user_pieces)
                {
                    // user_piece = user_piece.Replace("\"", "");
                    if (user_piece != "")
                    {
                        string[] argument = user_piece.Split(':');
                        argument[1] = argument[1].Replace("\"", "");
                        temp_components.Add(argument[1]);
                    }
                }

                User user = new User();
                user.UserId = new Guid(temp_components[0]);
                user.Nickname = temp_components[1];
                if (temp_components[6] == "false")
                {
                    user.LastSeenDate = DateTimeOffset.Parse(temp_components[5]); 
                }
                else
                {
                    user.LastSeenDate = DateTimeOffset.Now;
                }
                users_list.Add(user);
            }

        }

        return users_list;
    }

}


public class Report
{
    //List<string> metrics = new List<string>() { "dailyAverage", "weeklyAverage", "total", "min", "max"};
    private Assignment4Features useful_stuff_returner = new Assignment4Features();

    public Dictionary<Guid, string[]> ReturnDesiredInfo_old_version_of_this_function(List<string> metrics, List<Guid> user_id_s)
    {
        List<string> output_piece = new List<string>();
        Dictionary<Guid, string[]> output = new Dictionary<Guid, string[]>();

        foreach (var user in user_id_s)
        {
            output.Add(user, new string[] {"", "", "", "", ""});
        }
        
        if (metrics.Contains("dailyAverage"))
        {
            foreach (var user_id in user_id_s)
            {
                var daily_time = useful_stuff_returner.ReturnAverageTime(user_id, new List<UserTimeSpan>()).Daily_Time;
                output[user_id][0] = daily_time.ToString();
            }
        }
        if (metrics.Contains("weeklyAverage"))
        {
            foreach (var user_id in user_id_s)
            {
                var weekly_time = useful_stuff_returner.ReturnAverageTime(user_id, new List<UserTimeSpan>()).Weekly_Time;
                output[user_id][1] = weekly_time.ToString();
            }
        }
        if (metrics.Contains("total"))
        {
            foreach (var user_id in user_id_s)
            {
                var total_time = useful_stuff_returner.ReturnTotalTimeOnline(user_id, new List<UserTimeSpan>());
                output[user_id][2] = total_time.ToString();
            }
        }
        if (metrics.Contains("min"))
        {
            List<int> mins = new List<int>();
            foreach (var user_id in user_id_s)
            {
                var new_minimal = ReturnMin(user_id);
                mins.Add(new_minimal);
            }
        }
        if (metrics.Contains("max"))
        {
            List<int> maxs = new List<int>();
            foreach (var user_id in user_id_s)
            {
                var new_maximum = ReturnMax(user_id);
               maxs.Add(new_maximum);
            }
        }

        return output;
    }

    public Dictionary<Guid, string[]> ReturnDesiredInfo(List<string> metrics, List<Guid> user_id_s)
    {
        List<string> output_piece = new List<string>();
        Dictionary<Guid, string[]> output = new Dictionary<Guid, string[]>();

        foreach (var user in user_id_s)
        {
            output.Add(user, new string[] {"", "", "", "", ""});
        }

        foreach (var user_id in user_id_s)
        {
            if (metrics.Contains("dailyAverage"))
            {
                var daily_time = useful_stuff_returner.ReturnAverageTime(user_id, new List<UserTimeSpan>()).Daily_Time;
                output[user_id][0] = daily_time.ToString();
            }

            if (metrics.Contains("weeklyAverage"))
            {
                var weekly_time = useful_stuff_returner.ReturnAverageTime(user_id, new List<UserTimeSpan>())
                    .Weekly_Time;
                output[user_id][1] = weekly_time.ToString();
            }

            if (metrics.Contains("total"))
            {
                var total_time = useful_stuff_returner.ReturnTotalTimeOnline(user_id, new List<UserTimeSpan>());
                output[user_id][2] = total_time.ToString();
            }

            if (metrics.Contains("min"))
            {
                var new_minimal = ReturnMin(user_id);
                output[user_id][3] = new_minimal.ToString();
            }

            if (metrics.Contains("max"))
            {
                var new_maximum = ReturnMax(user_id);
                output[user_id][4] = new_maximum.ToString();
            }
        }
        return output;
    }

    public int ReturnMin(Guid user_id)
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
            return min;
        }
        else
        {
            return 0;
        }
    }
    
    public int ReturnMax(Guid user_id)
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
            return max;
        }
        else
        {
            return 0;
        }
    }
    
    
}


