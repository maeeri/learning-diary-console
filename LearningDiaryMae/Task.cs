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

        public enum Priority
        {
            Urgent,
            NotUrgent,
            Optional
        }


    }
}
