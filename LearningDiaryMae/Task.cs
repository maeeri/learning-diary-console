using System;

namespace LearningDiaryMae
{
    public class Task: DiaryItem
    {
        public string Notes { get; set; }
        public DateTime Deadline { get; set; }
        public bool Done { get; set; }

        //have to look into this a bit more
        public enum Priority
        {
            Urgent = 1,
            NotUrgent = 2,
            Optional = 3
        }

        public Task(string title, bool done)
        {
            Title = title;
            Done = done;
        }

        public void AddNotes()
        {
            Console.WriteLine("Add your note: ");
            Notes = Console.ReadLine();
        }


    }
}
