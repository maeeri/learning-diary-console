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
    }
}
