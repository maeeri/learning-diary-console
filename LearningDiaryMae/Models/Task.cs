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
        public int? Topic { get; set; }

        public virtual Topic TopicNavigation { get; set; }

        private string IsDone()
        {
            return (Done == true) ? "yes" : "no";
        }

        public string ToStringPrint()
        {
            string toPrint = ($"ID: {Id}\n" +
                              $"Title: {Title}\n" +
                              $"Description: {Description}\n" +
                              $"Notes: {Notes}\n" +
                              $"Deadline: {Deadline}\n" +
                              $"Done: {IsDone()}\n");

            return toPrint;
        }
    }
}
