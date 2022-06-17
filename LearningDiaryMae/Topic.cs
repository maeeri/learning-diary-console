using System;
using System.Collections.Generic;

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

        //reads file to list
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
            newTopic.CompletionDate = Convert.ToDateTime(columns[6]);
            newTopic.TimeSpent = Convert.ToDouble(columns[7]);
            newTopic.LastEditDate = Convert.ToDateTime(columns[8]);
            newTopic.InProgress = Convert.ToBoolean(columns[9]);

            return newTopic;
        }

        //override the ToString-method in csv-compatible form
        public override string ToString() => $"{Id};{Title};{Description};{EstimatedTimeToMaster};{Source};{StartLearningDate};{CompletionDate};{TimeSpent};{LastEditDate};{InProgress}";

        //printing topics to console
        public string ToStringPrint()
        {
            string print = $"Id: {Id}\n" +
                           $"Title: {Title}\n" +
                           $"Description: {Description}\n" +
                           $"Started: {StartLearningDate:d.M.yyyy}\n" +
                           $"Last edited: {LastEditDate:d.M.yyyy h:m:s}\n";
            return print;
        }

        //constructor with id
        public Topic(int id) => this.Id = id;

        //constructor without id
        public Topic() { }
    }
}
