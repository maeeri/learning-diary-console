using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Program
    {
        static void Main(string[] args)
        {
            //declare universal variables
            bool exit = false;
            int counter = 0;
            string path = @"C:\Users\Mae\source\repos\LearningDiaryMae\Diary.csv";
            string[] headerArray = new string[]
            {
                "Id", "Title", "Description", "Estimated time to master", "Source", "Date started", "Date completed",
                "Time spent", "Last edit", "In progress"
            };

            if (!File.Exists(path))
            {
                //create csv-file with headers
                string arrayString = String.Join(";", headerArray);
                File.WriteAllText(path, arrayString + "\n");
            }

            //loop topic editing
            do
            {
                try
                {
                    //ask what the user wants to do and go to switch
                    Console.WriteLine("What do you want to do?\n" +
                                      "1. Add a topic of study\n" +
                                      "2. Add a task\n" +
                                      "3. Print a current list of topics\n" +
                                      "4. Edit a topic\n" +
                                      "5. Delete a topic" +
                                      "6. Exit the app");
                    int answer = Convert.ToInt32(Console.ReadLine());

                    switch (answer)
                    {
                        case 1: //adding a topic
                            Topic newTopic = new Topic(counter);
                            counter++;

                            Console.WriteLine("Title: ");
                            newTopic.Title = Console.ReadLine();

                            Console.WriteLine("Describe the area of study: ");
                            newTopic.Description = Console.ReadLine();

                            Console.WriteLine("How much time (days) do you estimate you need for mastering the topic?");
                            newTopic.EstimatedTimeToMaster = Convert.ToDouble(Console.ReadLine());

                            Console.WriteLine("Did you use a source? Yes/no");
                            string input = Console.ReadLine();

                            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Which source did you use?");
                                newTopic.Source = Console.ReadLine();
                            }

                            Console.WriteLine("When did you start studying? DD/MM/YYYY");
                            newTopic.StartLearningDate = Convert.ToDateTime(Console.ReadLine());

                            Console.WriteLine("Is your study complete? Yes/no");
                            input = Console.ReadLine();

                            if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            {
                                newTopic.InProgress = false;
                            }

                            else if (input.Equals("no", StringComparison.OrdinalIgnoreCase))
                            {
                                newTopic.InProgress = true;
                            }

                            if (newTopic.InProgress == false)
                            {
                                Console.WriteLine("When did you finish with the topic? DD/MM/YYYY");
                                newTopic.CompletionDate = Convert.ToDateTime(Console.ReadLine());
                                newTopic.TimeSpent = newTopic.CalculateTimeSpent();
                            }

                            newTopic.LastEditDate = DateTime.Now;

                            File.AppendAllText(path, newTopic.ToString());
                            break;

                        case 2: //editing a task, to be added at one point or another
                            Console.WriteLine("Hey, something to look forward to!");
                            break;

                        case 3: //printing a list of topics
                            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                            {
                                HasHeaderRecord = false,
                                Comment = '#',
                                AllowComments = true,
                                Delimiter = ";",
                            };

                            {
                                Console.WriteLine("A list of your study topics:");

                                using var streamReader = File.OpenText(path);
                                using var csvReader = new CsvReader(streamReader, csvConfig);
                                while (csvReader.Read())
                                {
                                    string title = csvReader.GetField(1);
                                    string id = csvReader.GetField(0).ToString();
                                    Console.Write("{0,3}", id);
                                    Console.Write("\t");
                                    Console.WriteLine(title);
                                    //Console.WriteLine($"|{id}|\t|{title}|");
                                }
                                Console.WriteLine("\n");
                            }
                            break;

                        case 4: //editing a topic, to be added at one point or another
                            Console.WriteLine("Function to be added... I think.");
                            break;

                        case 5:

                            break;

                        case 6: //exit the app
                            Console.WriteLine("Exiting app.");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Did you choose a number between 1 and 6?");
                            break;
                    }
                }
                catch (Exception e) //if all else fails, no crash
                {
                    Console.WriteLine("Oh dear, we encountered an error... Please try again.\n" + e);
                    continue;
                }
            } while (exit == false);
        }
    }
}