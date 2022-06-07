using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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

        public double TimeSpent()
        {
            return Convert.ToDouble(CompletionDate-StartLearningDate);
        }

        public string GetTitle()
        {
            return Title;
        }
    }
}
