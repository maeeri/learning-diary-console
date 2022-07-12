

#nullable disable

using System;
using System.Collections.Generic;
using CsvHelper;

namespace LearningDiaryMae.Models
{
    public partial class DiaryTask
    {
        public DiaryTask()
        {
            NotesNavigation = new HashSet<Note>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Notes { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Priority { get; set; }
        public bool? Done { get; set; }
        public int? Topic { get; set; }

        public enum PriorityEnum
        {
            First = 1,
            Second = 2,
            Optional = 3
        }

        public virtual DiaryTopic TopicNavigation { get; set; }
        public virtual ICollection<Note> NotesNavigation { get; set; }
    }
}
