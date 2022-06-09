using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Topic
    {
        public Topic() { }
        public Topic(string Title, DateTime startLearningDate) { }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedTimeToMaster { get; set; }
        public string Source { get; set; }
        public DateTime StartLearningDate { get; set; }
        public bool InProgress { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime LastEditDate { get; set; }
        public double TimeSpent { get; set; }

        public double CalculateTimeSpent()
        {
            if (InProgress == true)
            {
                return Convert.ToDouble(DateTime.Now - StartLearningDate);
            }

            else
            {
                return Convert.ToDouble(CompletionDate - StartLearningDate);
            }
        }

        public string GetTitle() => Title;

        public void WriteToFile(string filePath, Topic topic)
        {
            {
                using var mem = new MemoryStream();
                using var writer = new StreamWriter(mem);
                using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
                csvWriter.NextRecord();
            }
        }

        public override string ToString()
        {
            return
                $"{Id};{Title};{Description};{EstimatedTimeToMaster};{Source};{StartLearningDate};{CompletionDate};{TimeSpent};{LastEditDate};{InProgress}";
        }
    }
}
