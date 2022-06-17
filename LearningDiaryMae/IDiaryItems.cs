using System;
using System.Collections.Generic;
using System.Text;

namespace LearningDiaryMae
{
    interface IDiaryItem
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
    }
}
