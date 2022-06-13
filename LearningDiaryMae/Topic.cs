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
    public class Topic: DiaryItem
    {
        //variables
        public double EstimatedTimeToMaster { get; set; }
        public string Source { get; set; }
        public DateTime StartLearningDate { get; set; }
        public bool InProgress { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime LastEditDate { get; set; }
        public double TimeSpent { get; set; }

        //method to calculate time spent on the topic
        public double CalculateTimeSpent()
        {
            TimeSpan timeSpent;
            double timeSpentDouble;
            if (InProgress == true)
            {
                timeSpent = DateTime.Now - StartLearningDate;
                timeSpentDouble = timeSpent.TotalDays;
            }

            else
            {
                timeSpent = CompletionDate - StartLearningDate;
                timeSpentDouble = timeSpent.TotalDays;
            }

            return timeSpentDouble;
        }

        //override the ToString-method in csv-compatible form
        public override string ToString() => $"{Id};{Title};{Description};{EstimatedTimeToMaster};{Source};{StartLearningDate};{CompletionDate};{TimeSpent};{LastEditDate};{InProgress}\n";

        //constructor with id
        public Topic(int id) => this.Id = id;

    }
}
