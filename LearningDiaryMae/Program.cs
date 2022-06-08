using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace LearningDiaryMae
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            int counter = 0;


            do
            {
                try
                {
                    Console.WriteLine("What do you want to do?\n" +
                                      "1. Add a topic of study\n" +
                                      "2. Print a current list of topics\n" +
                                      "3. Edit a topic\n" +
                                      "4. Exit the app");
                    int answer = Convert.ToInt32(Console.ReadLine());

                    switch (answer)
                    {
                        case 1:
                            Topic newTopic = new Topic();
                            newTopic.Id = counter;
                            counter++;

                            Console.WriteLine("Title: ");
                            newTopic.Title = Console.ReadLine();

                            Console.WriteLine("Describe the area of study: ");
                            newTopic.Description = Console.ReadLine();

                            Console.WriteLine("How much time do you need for studying the topic?");
                            newTopic.EstimatedTimeToMaster = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Did you use a source? Yes/no");
                            string input = Console.ReadLine();

                            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Which source did you use?");
                                newTopic.Source = Console.ReadLine();
                            }

                            Console.WriteLine("When did you start studying? YYYY/MM/DD");
                            newTopic.StartLearningDate = Convert.ToDateTime(Console.ReadLine());

                            Console.WriteLine("Is your study complete? Yes/no");
                            input = Console.ReadLine();

                            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
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

                            
                            break;

                        case 2:
                            foreach (Topic item in diary)
                            {
                                Console.WriteLine("Title: " + item.GetTitle() + ". Id: " + item.Id);
                            }

                            break;

                        case 3:
                            Console.WriteLine("Function to be added...");
                            break;

                        case 4:
                            Console.WriteLine("Exiting app.");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Did you choose a number between 1 and 4?");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Did you choose a number between 1 and 4?");
                    continue;
                }
            } while (exit == false);
        }
    }
}