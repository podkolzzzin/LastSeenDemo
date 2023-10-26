using System.Runtime.CompilerServices;
using Xunit;

namespace LastSeenDemo
{
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
}


