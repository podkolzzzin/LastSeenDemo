using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LastSeenDemo
{
    public class ReportManagment
    {
        private List<Report> reports = new List<Report>();
        private string reportsFilePath = "reports.json";

        public List<Report> Reports
        {
            get { return reports; }
            set { reports = value; }
        }

        public ReportManagment()
        {
            LoadReports();
        }

        public void AddReport(Report report)
        {
            reports.Add(report);
            SaveReports();
        }

        private void LoadReports()
        {
            if (File.Exists(reportsFilePath))
            {
                var json = File.ReadAllText(reportsFilePath);
                reports = JsonSerializer.Deserialize<List<Report>>(json);
            }
        }

        private void SaveReports()
        {
            var json = JsonSerializer.Serialize(reports);
            File.WriteAllText(reportsFilePath, json);
        }
    }
}
