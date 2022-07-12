using System;

namespace LearningDiaryMae
{
    class TaskOriginal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime? Deadline { get; set; }
        public string Priority { get; set; }
        public bool? Done { get; set; }
        public int? Topic { get; set; }

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
