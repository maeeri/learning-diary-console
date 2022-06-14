using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;
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
                timeSpent = DateTime.Today - StartLearningDate;
                timeSpentDouble = timeSpent.TotalDays;
                CompletionDate = new DateTime(0001, 1, 1);
            }

            else
            {
                timeSpent = CompletionDate - StartLearningDate;
                timeSpentDouble = timeSpent.TotalDays;
            }

            return timeSpentDouble;
        }

        public static Topic FromCsv(string csvLine)
        {
            string[] columns = csvLine.Split(';');
            Topic newTopic = new Topic();
            newTopic.Id = Convert.ToInt32(columns[0]);
            newTopic.Title = columns[1];
            newTopic.Description = columns[2];
            newTopic.EstimatedTimeToMaster = Convert.ToDouble(columns[3]);
            newTopic.Source = columns[4];
            newTopic.StartLearningDate = Convert.ToDateTime(columns[5]);
            newTopic.CompletionDate = Convert.ToDateTime(columns[7]);
            newTopic.TimeSpent = Convert.ToDouble(columns[8]);
            newTopic.LastEditDate = Convert.ToDateTime(columns[9]);
            newTopic.InProgress = Convert.ToBoolean(columns[10]);

            return newTopic;
        }

        //override the ToString-method in csv-compatible form
        public override string ToString() => $"{Id};{Title};{Description};{EstimatedTimeToMaster};{Source};{StartLearningDate};{CompletionDate};{TimeSpent};{LastEditDate};{InProgress}\n";

        //constructor with id
        public Topic(int id) => this.Id = id;

        //constructor without id
        public Topic() { }

    }
}
