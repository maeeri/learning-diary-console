using System;

namespace LearningDiaryMae
{
    public class Task: IDiaryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime Deadline { get; set; }
        public bool Done { get; set; }

        //have to look into this a bit more, still
        public enum Priority
        {
            Urgent = 1,
            NotUrgent = 2,
            Optional = 3
        }

        public Task(int id, string title, string description, string notes,  bool done)
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
