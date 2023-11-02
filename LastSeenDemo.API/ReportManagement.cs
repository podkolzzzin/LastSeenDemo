using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LastSeenDemo
{
    public class ReportManagement
    {
        private List<Report> reports;
        private string reportsFilePath;

        public List<Report> Reports
        {
            get { return reports; }
            set { reports = value; }
        }

        public ReportManagement()
        {
            reports = new List<Report>();
            reportsFilePath = "reports.json";
            LoadReports();
        }

        public void AddReport(Report report)
        {
            reports.Add(report);
        }

        private void LoadReports()
        {
            if (File.Exists(reportsFilePath))
            {
                string json = File.ReadAllText(reportsFilePath);
                reports = JsonSerializer.Deserialize<List<Report>>(json);
            }
        }
    }
}
