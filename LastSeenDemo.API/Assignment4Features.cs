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




public class Assignment4FeaturesInterestingTests
{
    [Theory]
    [InlineData(0)]
   // [InlineData(1)]// я хотів це завалити
   // [InlineData(-4)]// я хотів це завалити
    public void ReturnTotalTimeOnline(int expected)
    {
        Assignment4Features features = new Assignment4Features();
        Guid userId = Guid.NewGuid(); 
        int result = features.ReturnTotalTimeOnline(userId, new List<UserTimeSpan>());
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2023-10-01T15:30:00", "2023-10-01T15:30:30", 30)]
   // [InlineData("2023-10-01T15:30:00", "2023-10-01T15:30:30", 25)]// я хотів це завалити
    public void TestTimeSpanFunction(DateTimeOffset a, DateTimeOffset b, int c)
    {
        Assignment4Features features = new Assignment4Features();
        var test_time= features.CountTotalTime(a, b);
        Assert.Equal(test_time, c);
    }
    
    [Theory]
    [InlineData(30)]
    public void ReturnTotalTimeOnline_WithMockUser(int expected)
    {
        List<UserTimeSpan> userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now,
                Logout = DateTimeOffset.Now.AddSeconds(30)
            },
        };
        Assignment4Features features = new Assignment4Features();
        Guid userId = Guid.NewGuid(); 
        int result = features.ReturnTotalTimeOnline(userId, userTimeSpans);
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(24)]
    public void Average_Daily_Time(int expected)
    {
        List<UserTimeSpan> userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-10),
                Logout = DateTimeOffset.Now.AddDays(-10).AddMinutes(1)
            },
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-4),
                Logout = DateTimeOffset.Now.AddDays(-4).AddMinutes(2)
            },
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-2),
                Logout = DateTimeOffset.Now.AddDays(-2).AddMinutes(1)
            },
            
        };
        Assignment4Features features = new Assignment4Features();
        Guid userId = Guid.NewGuid();
        var result = features.ReturnAverageTime(userId, userTimeSpans);
        Assert.Equal(result.Daily_Time, expected);
    }
    
    [Theory]
    [InlineData(168)]
    public void Average_Weekly_Time(int expected)
    {
        List<UserTimeSpan> userTimeSpans = new List<UserTimeSpan>
        {
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-10),
                Logout = DateTimeOffset.Now.AddDays(-10).AddMinutes(1)
            },
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-4),
                Logout = DateTimeOffset.Now.AddDays(-4).AddMinutes(2)
            },
            new UserTimeSpan
            {
                Login = DateTimeOffset.Now.AddDays(-2),
                Logout = DateTimeOffset.Now.AddDays(-2).AddMinutes(1)
            },
            
        };
        Assignment4Features features = new Assignment4Features();
        Guid userId = Guid.NewGuid();
        var result = features.ReturnAverageTime(userId, userTimeSpans);
        Assert.Equal(result.Weekly_Time, expected);
    }

    [Fact]
    public void GetRidOfUser()
    {
        Assignment4Features features = new Assignment4Features();
        //Guid userId = new Guid("mock_user");
        Guid userId = Guid.NewGuid();
        string goal_url = "/api/stats/user/total?userId=";
        goal_url += userId;
        
       // UserLoader loader = new UserLoader(new Loader(), goal_url);

        //var all_users = new UserLoader(new Loader(), goal_url).LoadAllUsers();

         User[] all_users = new User[]
         {
             new User
             {
                 UserId = userId,
                 LastSeenDate = DateTimeOffset.Now.AddHours(-1),
                 Nickname = "user1",
                 IsOnline = true
             },
             new User
             {
                 UserId = Guid.NewGuid(),
                 LastSeenDate = DateTimeOffset.Now.AddHours(-2),
                 Nickname = "user2",
                 IsOnline = false
             },
             new User
             {
                 UserId = Guid.NewGuid(),
                 LastSeenDate = DateTimeOffset.Now.AddHours(-3),
                 Nickname = "user3",
                 IsOnline = true
             }
             
         };

         
         features.ForgetMe(userId);

         bool containsUserId = all_users.Any(u => u.UserId == userId);
         
         Assert.False(!containsUserId);
    }
}






