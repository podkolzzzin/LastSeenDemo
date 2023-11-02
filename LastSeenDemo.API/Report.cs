using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Guid> Users { get; set; }
        public List<string> Metrics { get; set; }
        private readonly Worker _worker;
        private readonly OnlineDetector _detector;
        private UserMinMax _minMax;

        public Report(string reportName, List<Guid> users, List<string> metrics, Worker worker, OnlineDetector onlineDetector)
        {
            Name = reportName;
            Metrics = metrics;
            _worker = worker;
            _detector = onlineDetector;
            _minMax = new UserMinMax(_detector);
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
                        UserId = userId
                    };

                    foreach (var metric in Metrics)
                    {
                        switch (metric)
                        {
                            case "total":
                                userReport.Total = _detector.CalculateTotalTimeForUser(user);
                                break;
                            case "dailyAverage":
                                userReport.DailyAverage = _detector.CalculateDailyAverageForUser(user);
                                break;
                            case "weeklyAverage":
                                userReport.WeeklyAverage = _detector.CalculateWeeklyAverageForUser(user);
                                break;
                            case "min":
                                var (min1, max1) = _minMax.CalculateMinMax(user, from, to);
                                userReport.Min = min1;
                                break;
                            case "max":
                                var (min2, max2) = _minMax.CalculateMinMax(user, from, to);
                                userReport.Max = max2;
                                break;
                            default:
                                break;
                        }
                    }

                    report.Add(userReport);
                }
            }

            return report;
        }
    }
}
