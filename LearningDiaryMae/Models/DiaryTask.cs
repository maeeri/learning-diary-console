using System;

#nullable disable

namespace LearningDiaryMae.Models
{
    public partial class DiaryTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime? Deadline { get; set; }
        public string Priority { get; set; }
        public bool? Done { get; set; }
        public int? Topic { get; set; }

        public virtual DiaryTopic DiaryTopicNavigation { get; set; }

        private string IsDone()
        {
            return (Done == true) ? "yes" : "no";
        }

        public string ToStringPrint()
        {
            string toPrint = ($"\t\tID: {Id}\n" +
                              $"\t\tTitle: {Title}\n" +
                              $"\t\tDescription: {Description}\n" +
                              $"\t\tNotes: {Notes}\n" +
                              $"\t\tDeadline: {Deadline}\n" +
                              $"\t\tDone: {IsDone()}\n");

            return toPrint;
        }
    }


}
