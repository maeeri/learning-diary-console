using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

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
            Urgent,
            NotUrgent,
            Optional
        }

        public Task(string title, bool done, int priority)
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
