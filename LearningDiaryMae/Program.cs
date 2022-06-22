using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using LearningDiaryMae.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearningDiaryMae
{
    class Program
    {

        static void Main(string[] args)
        {
            //declare universal variables
            bool exit = false;

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
                                      "4. Find a topic by id or title\n" +
                                      "5. Edit a topic\n" +
                                      "6. Delete a topic\n" +
                                      "7. Exit the app");
                    int answer = Convert.ToInt32(Console.ReadLine());

                    switch (answer)
                    {
                        case 1: //adding a topic to dictionary with method
                            Models.Topic newTopic = AddTopic();
                            using (LearningDiaryContext newContext = new LearningDiaryContext())
                            {
                                Models.Topic newTopic2 = new Models.Topic()
                                {
                                    Title = newTopic.Title,
                                    Description = newTopic.Description,
                                    Source = newTopic.Source,
                                    TimeToMaster = newTopic.TimeToMaster,
                                    StartLearningDate = newTopic.StartLearningDate,
                                    CompletionDate = (DateTime?)newTopic.CompletionDate,
                                    LastEditDate = newTopic.LastEditDate,
                                    InProgress = newTopic.InProgress
                                };
                                newContext.Topics.Add(newTopic2);
                                newContext.SaveChanges();
                            };

                            break;

                        case 2: //adding a task, work in progress
                            Console.WriteLine("Is the task linked to an existing study topic? Yes/no");
                            string reply = Console.ReadLine();
                            int? topicId;

                            if (reply.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("What is the ID of the topic the task is linked to?");
                                topicId = Convert.ToInt32(Console.ReadLine());
                            }

                            else if (reply.Equals("no", StringComparison.OrdinalIgnoreCase))
                            {
                                topicId = null;
                            }

                            Console.WriteLine("Task title:");
                            string taskTitle = Console.ReadLine();

                            Console.WriteLine("Task description: ");
                            string taskDescription = Console.ReadLine();

                            Console.WriteLine("Do you want to add notes? Yes/no");
                            reply = Console.ReadLine();
                            string note = "";

                            if (reply.Equals("yes", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Write your note:");
                                note = Console.ReadLine();
                            }

                            Console.WriteLine("What is the deadline? DD/MM/YYYY");
                            DateTime deadline = Convert.ToDateTime(Console.ReadLine());

                            Console.WriteLine("How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
                            int priority = Convert.ToInt32(Console.ReadLine());

                            break;

                        case 3: //printing a list of topics, from database using method
                            PrintTopics();
                            break;

                        case 4: //finding a topic by id or title
                            var edit = ChooseIdOrTitle("find");
                            Console.WriteLine(edit.ToStringPrint());
                            break;

                        case 5: // editing by id or title
                            edit = ChooseIdOrTitle("edit");
                            Console.WriteLine("Which field would you like to edit?\n" +
                                              "1) Topic title\n" +
                                              "2) Topic description\n" +
                                              "3) Source\n" +
                                              "4) Date completed (DD/MM/YYYY)");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the replacement:");
                            string input = Console.ReadLine();

                            using (LearningDiaryContext newContext = new LearningDiaryContext())
                            {
                                switch (choice)
                                {
                                    case 1:
                                        edit.Title = input;
                                        break;

                                    case 2:
                                        edit.Description = input;
                                        break;

                                    case 3:
                                        edit.Source = input;
                                        break;

                                    case 4:
                                        DateTime input2 = Convert.ToDateTime(input);
                                        edit.CompletionDate = input2;
                                        break;

                                    default:
                                        Console.WriteLine("Did you choose a number between 1 and 4?");
                                        break;
                                }
                                newContext.SaveChanges();
                            }
                            break;

                        case 6: // deleting by id or title
                            edit = ChooseIdOrTitle("delete");
                            using (LearningDiaryContext newContext = new LearningDiaryContext())
                            {
                                newContext.Remove(edit);
                                newContext.SaveChanges();
                            }
                            break;

                        case 7: //exit the app, write dictionary to database from list
                            exit = true;
                            Console.WriteLine("Exiting app.");
                            break;

                        default:
                            Console.WriteLine("Did you choose a number between 1 and 7?");
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

        //add a topic
        public static Models.Topic AddTopic()
        {
            Models.Topic newTopic = new Models.Topic();

                Console.WriteLine("Title: ");
                newTopic.Title = Console.ReadLine();

                Console.WriteLine("Describe the area of study: ");
                newTopic.Description = Console.ReadLine();

                Console.WriteLine("How much time (hours) do you estimate you need for mastering the topic?");
                newTopic.TimeToMaster = Convert.ToInt32(Console.ReadLine());

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
                }

                newTopic.TimeSpent = (int?)newTopic.CalculateTimeSpent();
                newTopic.LastEditDate = DateTime.Now;

                return newTopic;
        }

        //prints topics from dictionary
        public static void PrintTopics()
        {
            {
                Console.WriteLine("A list of your study topics:\n******************************");

                using (LearningDiaryContext newContext = new LearningDiaryContext())
                {
                    var read = newContext.Topics.Select(topic => topic);
                    foreach (var topic in read)
                    {
                        Console.WriteLine($"Id: {topic.Id}\n" +
                                          $"Title: {topic.Title}\n" +
                                          $"Description: {topic.Description}\n" +
                                          $"Started: {topic.StartLearningDate:d.M.yyyy}\n" +
                                          $"Last edited: {topic.LastEditDate:d.M.yyyy hh:m:s}\n");
                        Console.WriteLine("******************************");
                    }
                }
            }
        }

        //choosing topic by id or title, procedure = thing to be done to the topic
        public static Models.Topic ChooseIdOrTitle(string procedure)
        {
            Console.WriteLine("Would you like to select the topic by 1) ID or 2) title?");
            int input = Convert.ToInt32(Console.ReadLine());
            Models.Topic edit = new Models.Topic();

            //finds the topic to be modified
            if (input == 1)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                int idOption = Convert.ToInt32(Console.ReadLine());
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Topics.Where(x => x.Id == idOption).Select(x => x).FirstOrDefault();
                    edit = option;
                }
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                string titleOption = Console.ReadLine();
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Topics.Where(x=> x.Title == titleOption).Select(x => x).FirstOrDefault();
                    edit = option;
                }
            }
            return edit;
        }
    }
}