using System;
using System.Collections.Generic;

#nullable disable

namespace LearningDiaryMae.Models
{
    public partial class DiaryTopic
    {
        public DiaryTopic()
        {
            Tasks = new HashSet<DiaryTask>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? TimeToMaster { get; set; }
        public int? TimeSpent { get; set; }
        public string Source { get; set; }
        public DateTime? StartLearningDate { get; set; }
        public bool? InProgress { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        public virtual ICollection<DiaryTask> Tasks { get; set; }

        //method to calculate time spent on the topic
        public double CalculateTimeSpent()
        {
            TimeSpan timeSpent;
            double timeSpentDouble;
            if (InProgress == true)
            {
                timeSpent = (TimeSpan)(DateTime.Today - StartLearningDate);
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
    }
}
