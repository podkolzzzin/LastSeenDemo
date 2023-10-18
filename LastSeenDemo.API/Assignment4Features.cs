using System.Runtime.CompilerServices;
using Xunit;

namespace LastSeenDemo;


public class Assignment4Features
{
    public int ReturnTotalTimeOnline(Guid user_id, List<UserTimeSpan> timeSpans)
    {
        Loader loader = new Loader();
        string goal_url = "/api/stats/user/total?userId=USERID";
        
       // Worker worker = new Worker(new UserLoader(loader, goal_url), new AllUsersTransformer(new UserTransformer(new DateTimeProvider())));
       int total_time_online_in_seconds = 0; 
       foreach (var single_timespan in timeSpans)
       {
           if (single_timespan.Logout == null)
           {
               total_time_online_in_seconds += CountTotalTime(single_timespan.Login.UtcDateTime, DateTime.Now);
           }
           else
           {
               var actual_datetime = single_timespan.Logout.Value;
               total_time_online_in_seconds += CountTotalTime(single_timespan.Login.UtcDateTime, actual_datetime);
           }
       }
       
        return total_time_online_in_seconds;
    }

    public WeeklyTime_and_DailyTime ReturnAverageTime(Guid user_id, List<UserTimeSpan> timeSpans)
    {// ну і як це робити? Через while для кожної сесії, у яких день однаковий, а тоді з кроком 7?
        // А!
        // total time ділю на кількість днів з першого входу
        
        int total_time_online_in_seconds = 0; 
        foreach (var single_timespan in timeSpans)
        {
            if (single_timespan.Logout == null)
            {
                total_time_online_in_seconds += CountTotalTime(single_timespan.Login.UtcDateTime, DateTime.Now);
            }
            else
            {
                var actual_datetime = single_timespan.Logout.Value;
                total_time_online_in_seconds += CountTotalTime(single_timespan.Login.UtcDateTime, actual_datetime);
            }
        }

        var first_login = timeSpans[0].Login;
        var now = DateTimeOffset.Now;
        
        TimeSpan difference = now - first_login;

        int days_since_first_login = difference.Days;
      //  int weeks_since_first_login = days_since_first_login / 7;

        int avg_daily_time = total_time_online_in_seconds / days_since_first_login;
        int avg_weekly_time = avg_daily_time * 7;

        return new WeeklyTime_and_DailyTime( avg_weekly_time, avg_daily_time);
    }

    public void ForgetMe(Guid user_Id)
    {
        Loader loader = new Loader();
        string goal_url = "/api/stats/user/total?userId=";
        goal_url += user_Id;
        
        Worker worker = new Worker(new UserLoader(loader, goal_url), new AllUsersTransformer(new UserTransformer(new DateTimeProvider())));
        worker.Forget(user_Id);
    }
    
    
    public int CountTotalTime(DateTimeOffset date1, DateTimeOffset date2)
    {
        int result_seconds = 0;
        TimeSpan difference = date2 - date1;
        result_seconds = (int)difference.TotalSeconds;
        return result_seconds;
    }
}


public struct WeeklyTime_and_DailyTime
{
    public int Weekly_Time;
    public int Daily_Time;

    public WeeklyTime_and_DailyTime(int weekly_time, int daily_time)
    {
        Weekly_Time = weekly_time;
        Daily_Time = daily_time;
    }
    public WeeklyTime_and_DailyTime(DateTime weekly_time, DateTime daily_time)
    {
        Weekly_Time = (int)(weekly_time - DateTime.MinValue).TotalSeconds;
        Daily_Time = (int)(daily_time - DateTime.MinValue).TotalSeconds;
    }
}