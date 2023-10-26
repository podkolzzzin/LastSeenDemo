using System;
using System.Collections.Generic;
using System.Linq;

namespace LastSeenDemo
{
    public class UserMinMaxCalculator
    {
        private readonly OnlineDetector _onlineDetector;

        public UserMinMaxCalculator(OnlineDetector onlineDetector)
        {
            _onlineDetector = onlineDetector;
        }

        public (double, double) CalculateMinMax(List<UserTimeSpan> value, DateTimeOffset from, DateTimeOffset to)
        {
            var listOnline = new List<double>();

            double minimum = double.MaxValue;
            double maximum = double.MinValue;

            while (from <= to)
            {
                double dailyOnlineTime = _onlineDetector.CalculateTotalTimeForUser(value, from, from.AddDays(1));
                listOnline.Add(dailyOnlineTime);

                minimum = Math.Min(minimum, dailyOnlineTime);
                maximum = Math.Max(maximum, dailyOnlineTime);

                from = from.AddDays(1);
            }

            return (minimum, maximum);
        }
    }
}
