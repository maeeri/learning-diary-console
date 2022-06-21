using System;
using System.Collections.Generic;

#nullable disable

namespace LearningDiaryMae.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public int? TimeToMaster { get; set; }
        public int? TimeSpent { get; set; }
        public string SourceOfStudy { get; set; }
        public DateTime? StartLearningDate { get; set; }
        public bool? InProgress { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
