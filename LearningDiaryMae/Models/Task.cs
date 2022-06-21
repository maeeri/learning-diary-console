using System;
using System.Collections.Generic;

#nullable disable

namespace LearningDiaryMae.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime? Deadline { get; set; }
        public string Priority { get; set; }
        public bool? Done { get; set; }
        public int? TopicId { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
