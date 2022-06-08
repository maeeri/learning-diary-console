﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Program
    {
        static void Main(string[] args)
        {
            //declare variables
            bool exit = false;
            int counter = 0;
            string path = @"C:\Users\Mae\source\repos\LearningDiaryMae\Diary.csv";

            //create csv-file
            if (!File.Exists(path))
            {
                using StreamWriter diary = File.CreateText(path);
            }

            //loop topic editing
            do
            {
                try
                {
                    Console.WriteLine("What do you want to do?\n" +
                                      "1. Add a topic of study\n" +
                                      "2. Add a task\n" +
                                      "3. Print a current list of topics\n" +
                                      "4. Edit a topic\n" +
                                      "5. Exit the app");
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

                            Console.WriteLine("How much time do you estimate you need for mastering the topic?");
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

                            newTopic.LastEditDate = DateTime.Now;

                            StreamWriter diary = File.AppendText(path);
                            diary.WriteLine(newTopic);

                            break;

                        case 2:

                            break;

                        case 3:
                            //var[] diaryArray = File.ReadAllLines(path);
                            //foreach (var item in diaryArray)
                            //{
                            //    Console.WriteLine("Title: " + item.GetTitle() + ". Id: " + item.Id);
                            //}

                            break;

                        case 4:
                            Console.WriteLine("Function to be added... I think.");
                            break;

                        case 5:
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