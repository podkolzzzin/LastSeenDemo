using System.Net;

namespace LastSeenDemo;

public class Assignment2
{
     
}



public class Assignment2Features
{
    DateTime dtn = DateTime.Now;
    string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
    public void GetLastVisitors(string now)
    {
        Dictionary<string, string> users_dict = new Dictionary<string, string>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
            string[] s = json.Split('[');
            string[] subs = s[1].Split('{');

            foreach (var user in subs)
            {
                string[] user_info = user.Split(',');
                string user_str = "";
                string last_date= "";
                        
                foreach (var u in user_info)
                {
                    if (u.Contains("nickname"))
                    {
                        user_str = u;
                    }
                    if (u.Contains("lastSeenDate"))
                    {
                        last_date = u;
                    }
                }
                users_dict.Add(user_str, last_date);
            }

            foreach (var kvp in users_dict)
            {
                HumanizeTime(kvp.Key, kvp.Value, now);
            }
        }
    }
    
    public void GetLastVisitors2(string now)
    {
        Dictionary<string, string> users_dict = new Dictionary<string, string>();
        int limit = 0;
        int offset = 40;
        List<string> all_jsons = new List<string>();
        while (true)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString($"https://sef.podkolzin.consulting/api/users/lastSeen?offset={offset}&limit={limit}");
                all_jsons.Add(json);
                if (json.Length < limit)
                    break;
                
                offset += limit;
                
                string[] s = json.Split('[');
                string[] subs = s[1].Split('{');

                foreach (var user in subs)
                {
                    string[] user_info = user.Split(',');
                    string user_str = "";
                    string last_date= "";
                        
                    foreach (var u in user_info)
                    {
                        if (u.Contains("nickname"))
                        {
                            user_str = u;
                        }
                        if (u.Contains("lastSeenDate"))
                        {
                            last_date = u;
                        }
                    }
                    users_dict.Add(user_str, last_date);
                }

                foreach (var kvp in users_dict)
                {
                    HumanizeTime(kvp.Key, kvp.Value, now);
                }
                
            }
        }
        
    }
    
    
    
    public string HumanizeTimeOld(Dictionary<string, string> users_dict, string now)
    {
        int mounth_now = Int32.Parse(now.Substring(5, 2));
        int day_now = Int32.Parse(now.Substring(8, 2));
        int hour_now = Int32.Parse(now.Substring(11, 2));
        int minute_now = Int32.Parse(now.Substring(14, 2));
        int sec_now = Int32.Parse(now.Substring(17, 2));
        foreach (var kvp in users_dict) {
            var nickname = kvp.Key;
            if (nickname.Length > 0)
            {
                nickname = nickname.Remove(0, 11);
                Console.Write(nickname);
            }

            var time_online = kvp.Value;
            if (time_online.Length > 0 && (!time_online.Contains("null")))
            {
                time_online = time_online.Substring(0, time_online.Length-1);
                time_online = time_online.Remove(0, 16);
                
                int mounth_curr = Int32.Parse(time_online.Substring(5, 2));
                int day_curr = Int32.Parse(time_online.Substring(8, 2));
                int hour_curr = Int32.Parse(time_online.Substring(11, 2));
                int minute_curr = Int32.Parse(time_online.Substring(14, 2));
                int sec_curr = Int32.Parse(time_online.Substring(17, 2));

                if ((sec_now-sec_curr < 30) && minute_curr == minute_now && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online jsut now";
                }
                else if (minute_now-minute_curr <= 2 && hour_now == hour_curr && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online a minute ago";
                }
                else if (hour_now-hour_curr <= 1 && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online a few minutes ago";
                }
                else if (hour_now-hour_curr <= 2 && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online an hour ago";
                }
                else if (day_now == day_curr && mounth_curr == mounth_now)
                {
                    time_online = " was online today";
                }
                else if (day_now-day_curr <=2 && mounth_curr == mounth_now)
                {
                    time_online = " was online yesterday";
                }
                else if (day_now-day_curr <=8 && mounth_curr == mounth_now)
                {
                    time_online = " was online this week";
                } 
                else
                {
                    time_online = " was online long ago";
                }
            }
            else if(time_online.Contains("null"))
            {
                time_online = " is online";
            }
            Console.WriteLine(time_online);
        }

        return "";
    }
    
    public string HumanizeTime(string user_name, string user_time, string now)
    {
        int mounth_now = Int32.Parse(now.Substring(5, 2));
        int day_now = Int32.Parse(now.Substring(8, 2));
        int hour_now = Int32.Parse(now.Substring(11, 2));
        int minute_now = Int32.Parse(now.Substring(14, 2));
        int sec_now = Int32.Parse(now.Substring(17, 2));
         var nickname = user_name;
        if (nickname.Length > 0)
        {
            nickname = nickname.Remove(0, 11);
            Console.Write(nickname);
        }

        var time_online = user_time;
        if (time_online.Length > 0 && (!time_online.Contains("null")))
        {
            time_online = time_online.Substring(0, time_online.Length-1);
            time_online = time_online.Remove(0, 16);
            
            int mounth_curr = Int32.Parse(time_online.Substring(5, 2));
            int day_curr = Int32.Parse(time_online.Substring(8, 2));
            int hour_curr = Int32.Parse(time_online.Substring(11, 2));
            int minute_curr = Int32.Parse(time_online.Substring(14, 2));
            int sec_curr = Int32.Parse(time_online.Substring(17, 2));

            if ((sec_now-sec_curr < 30) && minute_curr == minute_now && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online jsut now";
            }
            else if (minute_now-minute_curr <= 2 && hour_now == hour_curr && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online a minute ago";
            }
            else if (hour_now-hour_curr < 1 && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online a few minutes ago";
            }
            else if (hour_now-hour_curr <= 2 && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online an hour ago";
            }
            else if (day_now == day_curr && mounth_curr == mounth_now)
            {
                return " was online today";
            }
            else if (day_now-day_curr <=2 && mounth_curr == mounth_now)
            {
                return " was online yesterday";
            }
            else if (day_now-day_curr <=8 && mounth_curr == mounth_now)
            {
                return " was online this week";
            } 
            else
            {
                return " was online long ago";
            }
        }
        else if (time_online.Contains("null"))
        {
            return " is online";
        }
        else
        {
            return " unavailable";
        }
    }
}


public class ReworkedProgram
{
    DateTime dtn = DateTime.Now;
    string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
    public void GetLastVisitors(string now)
    {
        Dictionary<string, string> users_dict = new Dictionary<string, string>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
            string[] s = json.Split('[');
            string[] subs = s[1].Split('{');

            foreach (var user in subs)
            {
                string[] user_info = user.Split(',');
                string user_str = "";
                string last_date= "";
                        
                foreach (var u in user_info)
                {
                    if (u.Contains("nickname"))
                    {
                        user_str = u;
                    }
                    if (u.Contains("lastSeenDate"))
                    {
                        last_date = u;
                    }
                }
                users_dict.Add(user_str, last_date);
            }

            foreach (var kvp in users_dict)
            {
                HumanizeTime(kvp.Key, kvp.Value, now);
            }
        }
    }
    public List<User> GetLastVisitorsStr(string now)
    {
        List<User> user_list = new List<User>();
        Dictionary<string, string> users_dict = new Dictionary<string, string>();
        using (WebClient wc = new WebClient())
        {
            var json = wc.DownloadString("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
            string[] s = json.Split('[');
            string[] subs = s[1].Split('{');

            foreach (var user in subs)
            {
                string[] user_info = user.Split(',');
                string user_str = "";
                string last_date= "";
                        
                foreach (var u in user_info)
                {
                    if (u.Contains("nickname"))
                    {
                        user_str = u;
                    }
                    if (u.Contains("lastSeenDate"))
                    {
                        last_date = u;
                    }
                }
                users_dict.Add(user_str, last_date);
            }

            foreach (var kvp in users_dict)
            {
                HumanizeTime(kvp.Key, kvp.Value, now);
            }
        }

        return user_list;
    }
    
    public string HumanizeTimeOld(Dictionary<string, string> users_dict, string now)
    {
        int mounth_now = Int32.Parse(now.Substring(5, 2));
        int day_now = Int32.Parse(now.Substring(8, 2));
        int hour_now = Int32.Parse(now.Substring(11, 2));
        int minute_now = Int32.Parse(now.Substring(14, 2));
        int sec_now = Int32.Parse(now.Substring(17, 2));
        foreach (var kvp in users_dict) {
            var nickname = kvp.Key;
            if (nickname.Length > 0)
            {
                nickname = nickname.Remove(0, 11);
                Console.Write(nickname);
            }

            var time_online = kvp.Value;
            if (time_online.Length > 0 && (!time_online.Contains("null")))
            {
                time_online = time_online.Substring(0, time_online.Length-1);
                time_online = time_online.Remove(0, 16);
                
                int mounth_curr = Int32.Parse(time_online.Substring(5, 2));
                int day_curr = Int32.Parse(time_online.Substring(8, 2));
                int hour_curr = Int32.Parse(time_online.Substring(11, 2));
                int minute_curr = Int32.Parse(time_online.Substring(14, 2));
                int sec_curr = Int32.Parse(time_online.Substring(17, 2));

                if ((sec_now-sec_curr < 30) && minute_curr == minute_now && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online jsut now";
                }
                else if (minute_now-minute_curr <= 2 && hour_now == hour_curr && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online a minute ago";
                }
                else if (hour_now-hour_curr <= 1 && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online a few minutes ago";
                }
                else if (hour_now-hour_curr <= 2 && day_curr == day_now && mounth_curr == mounth_now)
                {
                    time_online = " was online an hour ago";
                }
                else if (day_now == day_curr && mounth_curr == mounth_now)
                {
                    time_online = " was online today";
                }
                else if (day_now-day_curr <=2 && mounth_curr == mounth_now)
                {
                    time_online = " was online yesterday";
                }
                else if (day_now-day_curr <=8 && mounth_curr == mounth_now)
                {
                    time_online = " was online this week";
                } 
                else
                {
                    time_online = " was online long ago";
                }
            }
            else if(time_online.Contains("null"))
            {
                time_online = " is online";
            }
            Console.WriteLine(time_online);
        }

        return "";
    }
    
    public string HumanizeTime(string user_name, string user_time, string now)
    {
        int mounth_now = Int32.Parse(now.Substring(5, 2));
        int day_now = Int32.Parse(now.Substring(8, 2));
        int hour_now = Int32.Parse(now.Substring(11, 2));
        int minute_now = Int32.Parse(now.Substring(14, 2));
        int sec_now = Int32.Parse(now.Substring(17, 2));
         var nickname = user_name;
        if (nickname.Length > 0)
        {
            nickname = nickname.Remove(0, 11);
            Console.Write(nickname);
        }

        var time_online = user_time;
        if (time_online.Length > 0 && (!time_online.Contains("null")))
        {
            time_online = time_online.Substring(0, time_online.Length-1);
            time_online = time_online.Remove(0, 16);
            
            int mounth_curr = Int32.Parse(time_online.Substring(5, 2));
            int day_curr = Int32.Parse(time_online.Substring(8, 2));
            int hour_curr = Int32.Parse(time_online.Substring(11, 2));
            int minute_curr = Int32.Parse(time_online.Substring(14, 2));
            int sec_curr = Int32.Parse(time_online.Substring(17, 2));

            if ((sec_now-sec_curr < 30) && minute_curr == minute_now && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online jsut now";
            }
            else if (minute_now-minute_curr <= 2 && hour_now == hour_curr && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online a minute ago";
            }
            else if (hour_now-hour_curr < 1 && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online a few minutes ago";
            }
            else if (hour_now-hour_curr <= 2 && day_curr == day_now && mounth_curr == mounth_now)
            {
                return " was online an hour ago";
            }
            else if (day_now == day_curr && mounth_curr == mounth_now)
            {
                return " was online today";
            }
            else if (day_now-day_curr <=2 && mounth_curr == mounth_now)
            {
                return " was online yesterday";
            }
            else if (day_now-day_curr <=8 && mounth_curr == mounth_now)
            {
                return " was online this week";
            } 
            else
            {
                return " was online long ago";
            }
        }
        else if (time_online.Contains("null"))
        {
            return " is online";
        }
        else
        {
            return " unavailable";
        }
    }
}
