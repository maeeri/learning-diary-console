using System;
using System.Threading.Tasks;
using static LearningDiaryMae.CommonMethods;

namespace LearningDiaryMae
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool continueLoop = true;
            TitleSequence();

            //loop editing
            while(continueLoop)
            {
                try
                {
                    continueLoop = await MainMenu(true);
                }
                catch (Exception e) //if all else fails, no crash
                {
                    Console.WriteLine("Oh dear, we encountered an error... Please try again.\n\n" + e);
                }
            }
        }
    }
}