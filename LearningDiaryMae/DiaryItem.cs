using System;
using System.Collections.Generic;
using System.Text;


//would probably work better as interface?
namespace LearningDiaryMae
{
    public class DiaryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
