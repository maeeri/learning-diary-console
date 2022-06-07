using System;
using System.Collections.Generic;

namespace LearningDiaryMae
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Topic> diary = new List<Topic>();
            bool stopLoop = false;

            while (stopLoop == false)
            {
                Console.WriteLine("Do you want to add an entry? Yes/no");
                string answer = Console.ReadLine();

                if (answer.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    stopLoop = true;
                    break;
                }

                Topic newTopic = new Topic();

                Console.WriteLine("What is the title of your study?");
                newTopic.Title = Console.ReadLine();

                Console.WriteLine("Describe the area of study: ");
                newTopic.Description = Console.ReadLine();

                Console.WriteLine("How much time do you need for studying the topic?");
                newTopic.EstimatedTimeToMaster = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Did you use a source? Yes/no");
                answer = Console.ReadLine();

                if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Which source did you use?");
                    newTopic.Source = Console.ReadLine();
                }

                Console.WriteLine("When did you start studying? YYYY/MM/DD");
                newTopic.StartLearningDate = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("Is your study complete? Yes/no");
                answer = Console.ReadLine();

                if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    newTopic.InProgress = false;
                }

                else
                {
                    newTopic.InProgress = true;
                }

                if (newTopic.InProgress == false)
                {
                    Console.WriteLine("When did you finish with the topic? YYYY/MM/DD");
                    newTopic.CompletionDate = Convert.ToDateTime(Console.ReadLine());
                }

                diary.Add(newTopic);
            }

            for (int i=0; i < diary.Count; i++)
            {

            }
        }
    }
}
