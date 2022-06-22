using System;
using System.Collections.Generic;

namespace LearningDiaryMae
{
    public class Topic
    {
        //variables
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedTimeToMaster { get; set; }
        public string Source { get; set; }
        public DateTime StartLearningDate { get; set; }
        public bool InProgress { get; set; }
        public DateTime? CompletionDate { get; set; }
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
                CompletionDate = null;
            }

            else
            {
                timeSpent = (TimeSpan)(CompletionDate - StartLearningDate);
                timeSpentDouble = timeSpent.TotalDays;
            }
            return timeSpentDouble;
        }

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

        //constructor without id
        public Topic() { }
    }
}
