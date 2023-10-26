using System;
using System.Collections.Generic;

namespace LastSeenDemo
{
    public class ReportRequest
    {
        public List<string> Metrics { get; set; }
        public List<Guid> Users { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
    }

    public class ReportItem
    {
        public Guid UserId { get; set; }
        public double Total { get; set; }
        public double DailyAverage { get; set; }
        public double WeeklyAverage { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }

    public class Report
    {
        public string Name { get; set; }
        public static List<Guid> Users { get; set; }
        public List<string> Metrics { get; set; }
        private readonly Worker _worker;
        private readonly OnlineDetector _detector;
        private readonly UserMinMaxCalculator _minMax;

        public Report(string reportName, List<Guid> users, List<string> metrics, Worker worker, OnlineDetector onlineDetector)
        {
            Name = reportName;
            Metrics = metrics;
            _worker = worker;
            _detector = onlineDetector;
            _minMax = new UserMinMaxCalculator(_detector);
            Users = users;
        }

        public List<ReportItem> CreateReport(DateTimeOffset from, DateTimeOffset to)
        {
            var report = new List<ReportItem>();

            foreach (var userId in Users)
            {
                if (_worker.Users.TryGetValue(userId, out var user))
                {
                    var userReport = new ReportItem
                    {
                        UserId = userId,
                        Total = Metrics.Contains("total") ? _detector.CalculateTotalTimeForUser(user) : 0,
                        DailyAverage = Metrics.Contains("dailyAverage") ? _detector.CalculateDailyAverageForUser(user) : 0,
                        WeeklyAverage = Metrics.Contains("weeklyAverage") ? _detector.CalculateWeeklyAverageForUser(user) : 0,
                        Min = Metrics.Contains("min") ? _minMax.CalculateMinMax(user, from, to).Item1 : 0,
                        Max = Metrics.Contains("max") ? _minMax.CalculateMinMax(user, from, to).Item2 : 0
                    };

                    report.Add(userReport);
                }
            }

            return report;
        }
    }
}

