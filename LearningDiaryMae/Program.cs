using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;
using LearningDiaryMae.Models;
using Microsoft.EntityFrameworkCore.Storage;
using ClassLibraryDateMethods;

namespace LearningDiaryMae
{
    class Program
    {

        static void Main(string[] args)
        {
            //declare universal variables
            bool exit = false;

            Console.Write(" ");
            for (int i = 0; i < 43; i++)
            {
                Console.Write("~");
                Thread.Sleep(10);
            }
            Console.Write("\n|  Welcome to Your Friendly Learning Diary  |\n ");
            for (int i = 0; i < 43; i++)
            {
                Console.Write("~");
                Thread.Sleep(10);
            }

            //loop topic editing
            do
            {
                try
                {
                    //ask user whether to work with topics or tasks
                    Console.WriteLine("\nWhat do you want to do?\n" +
                                      "1. Work with topics\n" +
                                      "2. Work with tasks\n" +
                                      "3. Exit the app");
                    int answer = Convert.ToInt32(Console.ReadLine());

                    if (answer == 1)
                    {
                        //ask what the user wants to do and go to switch
                        Console.WriteLine("What do you want to do?\n" +
                                          "1. Add a topic of study\n" +
                                          "2. Print a current list of topics\n" +
                                          "3. Find a topic by id or title\n" +
                                          "4. Edit a topic\n" +
                                          "5. Delete a topic\n" +
                                          "6. Get tasks related to a topic\n" +
                                          "7. Exit the app");
                        int answer2 = Convert.ToInt32(Console.ReadLine());

                        switch (answer2)
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
                                    newContext.SaveChangesAsync();
                                };
                                break;

                            case 2: //printing a list of topics, from database using method
                                PrintTopics();
                                break;

                            case 3: //finding a topic by id or title
                                var edit = ChooseIdOrTitle("find");
                                Console.WriteLine(edit.ToStringPrint());
                                break;

                            case 4: // editing by id or title
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
                                    newContext.SaveChangesAsync();
                                }
                                break;

                            case 5: // deleting by id or title
                                edit = ChooseIdOrTitle("delete");
                                using (LearningDiaryContext newContext = new LearningDiaryContext())
                                {
                                    newContext.Remove(edit);
                                    newContext.SaveChangesAsync();
                                }
                                break;

                            case 6:
                                Console.WriteLine("What is the id of the topic you want to get the tasks of?");
                                int answer3 = Convert.ToInt32(Console.ReadLine());

                                using (LearningDiaryContext newContext = new LearningDiaryContext())
                                {
                                    var something = newContext.Tasks.Where(task => task.Topic == answer3);
                                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                                    foreach (var item in something)
                                    {
                                        Console.WriteLine(item.ToStringPrint());
                                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                                    }
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

                    //work with tasks
                    else if (answer == 2)
                    {
                        Console.WriteLine("What do you want to do?\n" +
                                          "1. Add a task\n" +
                                          "2. Print a current list of tasks\n" +
                                          "3. Find a task by id or title\n" +
                                          "4. Edit a task\n" +
                                          "5. Delete a task\n" +
                                          "7. Exit the app");
                        int answer3 = Convert.ToInt32(Console.ReadLine());

                        switch (answer3)
                        {
                            case 1: //adding a task
                                Console.WriteLine("Is the task linked to an existing study topic? Yes/no");
                                string reply = Console.ReadLine();
                                int? topicId = null;

                                if (reply.Equals("yes", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine("What is the ID of the topic the task is linked to?");
                                    topicId = Convert.ToInt32(Console.ReadLine());
                                }

                                Console.WriteLine("Task title:");
                                string taskTitle = Console.ReadLine();

                                Console.WriteLine("Task description: ");
                                string taskDescription = Console.ReadLine();


                                //doesn't work yet
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
                                int input3 = Convert.ToInt32(Console.ReadLine());
                                string priority = "";

                                switch (input3)
                                {
                                    case 1:
                                        priority = "Urgent";
                                        break;
                                    case 2:
                                        priority = "Not urgent";
                                        break;
                                    case 3:
                                        priority = "Optional";
                                        break;
                                    default:
                                        Console.WriteLine("Did you choose a number between 1 and 3?");
                                        break;
                                }

                                Console.WriteLine("Is the task done? yes/no");
                                string input4 = Console.ReadLine().ToLower();
                                bool done;

                                input4.Equals(!string.IsNullOrEmpty("yes") ? done = true : done = false);

                                using (LearningDiaryContext newContext = new LearningDiaryContext())
                                {
                                    Models.Task newTask = new Models.Task()
                                    {
                                        Title = taskTitle,
                                        Description = taskDescription,
                                        Notes = note,
                                        Deadline = deadline,
                                        Priority = priority,
                                        Topic = topicId,
                                        Done = done
                                    };
                                    newContext.Add(newTask);
                                    newContext.SaveChangesAsync();
                                }
                                break;

                            case 2: //print tasks from database
                                PrintTasks();
                                break;

                            case 3: //finding a task by id or title
                                Models.Task edit2 = ChooseTaskIdOrTitle("find");
                                Console.WriteLine(edit2.ToStringPrint());
                                break;

                            case 4: // editing by id or title
                                edit2 = ChooseTaskIdOrTitle("edit");
                                Console.WriteLine("Which field would you like to edit?\n" +
                                                  "1) Task title\n" +
                                                  "2) Task description\n" +
                                                  "3) Deadline(DD/MM/YYYY)\n" +
                                                  "4) Task status (done/not done)\n" +
                                                  "5) Add a note");
                                int choice = Convert.ToInt32(Console.ReadLine());

                                using (LearningDiaryContext newContext = new LearningDiaryContext())
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                            Console.WriteLine("Enter the new title:");
                                            edit2.Title = Console.ReadLine();
                                            break;

                                        case 2:
                                            Console.WriteLine("Enter the new description:");
                                            edit2.Description = Console.ReadLine();
                                            break;

                                        case 3:
                                            Console.WriteLine("Enter the new deadline:");
                                            edit2.Deadline = Convert.ToDateTime(Console.ReadLine());
                                            break;

                                        case 4:
                                            Console.WriteLine("Do you want to mark the task as done? Yes/no");
                                            input4 = Console.ReadLine().ToLower();
                                            if (input4.Equals("yes"))
                                                edit2.Done = true;
                                            else if (input4.Equals("no"))
                                                edit2.Done = false;
                                            break;

                                        //doesn't work yet
                                        case 5:
                                            Console.WriteLine("Enter the new note:");
                                            string newNote = Console.ReadLine();
                                            edit2.Notes += "\n" + newNote;
                                            break;

                                        default:
                                            Console.WriteLine("Did you choose a number between 1 and 4?");
                                            break;
                                    }
                                    newContext.SaveChangesAsync();
                                }
                                break;

                            case 5: // deleting by id or title
                                edit2 = ChooseTaskIdOrTitle("delete");
                                using (LearningDiaryContext newContext = new LearningDiaryContext())
                                {
                                    newContext.Remove(edit2);
                                    newContext.SaveChangesAsync();
                                }
                                break;

                            case 6: //exit the app
                                exit = true;
                                Console.WriteLine("Exiting app.");
                                break;

                            default:
                                Console.WriteLine("Did you choose a number between 1 and 7?");
                                break;
                        }
                    }
                    else
                    {
                        exit = true;
                        Console.WriteLine("Exiting app.");
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

            //change!!!
            if (newTopic.InProgress == false)
            {
                Console.WriteLine("When did you finish with the topic? DD/MM/YYYY");
                newTopic.CompletionDate = Convert.ToDateTime(Console.ReadLine());
            }

            newTopic.TimeSpent = (int?)newTopic.CalculateTimeSpent();
            newTopic.LastEditDate = DateTime.Now;

            return newTopic;
        }

        //prints topics from database
        public static void PrintTopics()
        {
            {
                Console.WriteLine("A list of your study topics:\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                using (LearningDiaryContext newContext = new LearningDiaryContext())
                {
                    var read = newContext.Topics.Select(topic => topic);
                    foreach (var topic in read)
                    {
                        Console.WriteLine($"Id: {topic.Id}\n" +
                                          $"Title: {topic.Title}\n" +
                                          $"Description: {topic.Description}\n" +
                                          $"Start date: {topic.StartLearningDate: d.M.yyyy}\n" +
                                          $"Last edit date: {topic.LastEditDate: d.M.yyyy}");
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
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
                    var option = connection.Topics.Where(x => x.Title == titleOption).Select(x => x).FirstOrDefault();
                    edit = option;
                }
            }
            return edit;
        }

        //print tasks from database
        public static void PrintTasks()
        {
            {
                Console.WriteLine("A list of your tasks:\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                using (LearningDiaryContext newContext = new LearningDiaryContext())
                {
                    var read = newContext.Tasks.Select(topic => topic);
                    foreach (var task in read)
                    {
                        Console.WriteLine(task.ToStringPrint());
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                }
            }
        }

        //choosing topic by id or title, procedure = thing to be done to the task
        public static Models.Task ChooseTaskIdOrTitle(string procedure)
        {
            Console.WriteLine("Would you like to select the task by 1) ID or 2) title?");
            int input = Convert.ToInt32(Console.ReadLine());
            Models.Task edit = new Models.Task();

            //finds the topic to be modified
            if (input == 1)
            {
                Console.WriteLine($"Which task would you like to {procedure}?");
                int idOption = Convert.ToInt32(Console.ReadLine());
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Tasks.Where(x => x.Id == idOption).Select(x => x).FirstOrDefault();
                    edit = option;
                }
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which task would you like to {procedure}?");
                string titleOption = Console.ReadLine();
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Tasks.Where(x => x.Title == titleOption).Select(x => x).FirstOrDefault();
                    edit = option;
                }
            }
            return edit;
        }
    }
}