using System;
using System.Collections.Generic;

#nullable disable

namespace LearningDiaryMae.Models
{
    public partial class Note
    {
        public int Id { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public string Note5 { get; set; }
        public int? Task { get; set; }

        public virtual DiaryTask TaskNavigation { get; set; }
    }
}
