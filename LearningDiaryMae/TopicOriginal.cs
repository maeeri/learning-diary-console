using System;

namespace LearningDiaryMae
{
    public class TopicOriginal
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
            string print = $"\t\tId: {Id}\n" +
                           $"\t\tTitle: {Title}\n" +
                           $"\t\tDescription: {Description}\n" +
                           $"\t\tStarted: {StartLearningDate:d.M.yyyy}\n" +
                           $"\t\tLast edited: {LastEditDate:d.M.yyyy h:m:s}\n";
            return print;
        }
    }
}
