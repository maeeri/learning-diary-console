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
    }
}
