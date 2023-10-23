using System;
using System.Collections.Generic;
using System.Linq;

namespace LastSeenDemo
{
    public class UserMinMax
    {
        private readonly OnlineDetector _onlineDetector;
        
        public UserMinMax(OnlineDetector onlineDetector)
        {
            _onlineDetector = onlineDetector;
        }

        public (double, double) CalculateMinMax(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to)
        {
            var listOnline = new List<double>();
            double minimum = double.MaxValue;
            double maximum = 0.0;

            while (from <= to)
            {
                double dailyOnlineTime = _onlineDetector.CalculateTotalTimeForUser(value, from, from.AddDays(1));
                listOnline.Add(dailyOnlineTime);

                if (dailyOnlineTime < minimum)
                {
                    minimum = dailyOnlineTime;
                }

                if (dailyOnlineTime > maximum)
                {
                    maximum = dailyOnlineTime;
                }

                from = from.AddDays(1);
            }

            return (minimum, maximum);
        }
    }
}
