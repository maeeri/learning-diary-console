using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearningDiaryMae.Models;
using static ClassLibraryDateMethods.Class1;
using static LearningDiaryMae.Methods;

namespace LearningDiaryMae
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //declare universal variables
            bool exit = false;

            //fancy title sequence
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

            //loop editing
            do
            {
                try
                {
                    //ask user whether to work with topics or tasks
                    Console.WriteLine("\nWhat do you want to do?\n" +
                                      "1. Work with topics\n" +
                                      "2. Work with tasks\n" +
                                      "3. Exit the app");
                    int mainMenuInput = ValidateIntInput(Console.ReadLine());

                    if (mainMenuInput == 1)
                    {
                        //ask what the user wants to do and go to switch
                        Console.WriteLine("What do you want to do?\n" +
                                          "1. Add a topic of study\n" +
                                          "2. Print a current list of topics\n" +
                                          "3. Find a topic by id or title\n" +
                                          "4. Edit a topic\n" +
                                          "5. Delete a topic\n" +
                                          "6. Print tasks related to a topic\n" +
                                          "7. Exit the app");
                        int topicMenuInput = ValidateIntInput(Console.ReadLine());


                        //what to do with topics
                        switch (topicMenuInput)
                        {
                            case 1: //adding a topic to database with method
                                Console.Clear();
                                await AddTopic();
                                break;

                            case 2: //printing a list of topics, from database using method
                                Console.Clear();
                                await PrintTopics();
                                break;

                            case 3: //finding a topic by id or title
                                Console.Clear();
                                DiaryTopic editDiaryTopic = ChooseIdOrTitle("topic", "find") as DiaryTopic;
                                if (editDiaryTopic != null) Console.WriteLine(editDiaryTopic.ToStringPrint());
                                break;

                            case 4: //editing by id or title
                                Console.Clear();
                                EditTopic();
                                break;

                            case 5: // deleting by id or title
                                Console.Clear();
                                editDiaryTopic = ChooseIdOrTitle("topic", "delete") as DiaryTopic;
                                await DeleteRow(editDiaryTopic);
                                break;

                            case 6: //print tasks related to a specific topic
                                Console.Clear();
                                await PrintTasksForTopic();
                                break;

                            case 7: //exit the app from topic menu
                                Console.Clear();
                                exit = true;
                                Console.WriteLine("Exiting app.");
                                break;

                            //topic menu default
                            default:
                                Console.WriteLine("Did you choose a number between 1 and 7?");
                                break;
                        }
                    }

                    //work with tasks
                    else if (mainMenuInput == 2)
                    {
                        Console.WriteLine("What do you want to do?\n" +
                                          "1. Add a task\n" +
                                          "2. Print a current list of tasks\n" +
                                          "3. Find a task by id or title\n" +
                                          "4. Edit a task\n" +
                                          "5. Delete a task\n" +
                                          "6. Exit the app");
                        int taskMenuChoice = ValidateIntInput(Console.ReadLine());

                        switch (taskMenuChoice)
                        {
                            case 1: //adding a task
                                Console.Clear();
                                await AddTask();
                                break;

                            case 2: //print tasks from database
                                Console.Clear();
                                await PrintTasks();
                                break;

                            case 3: //finding a task by id or title
                                Console.Clear();
                                DiaryTask editDiaryTask = ChooseIdOrTitle("task","find") as DiaryTask;
                                if (editDiaryTask != null) Console.WriteLine(editDiaryTask.ToStringPrint());
                                break;

                            case 4: //editing by id or title
                                Console.Clear();
                                EditTask();
                                break;

                            case 5: // deleting by id or title
                                Console.Clear();
                                editDiaryTask = ChooseIdOrTitle("task", "delete") as DiaryTask;
                                await DeleteRow(editDiaryTask);
                                break;

                            case 6: //exit the app from task menu
                                Console.Clear();
                                exit = true;
                                Console.WriteLine("Exiting app.");
                                break;

                            //task menu default
                            default:
                                Console.WriteLine("Did you choose a number between 1 and 6?");
                                break;
                        }
                    }

                    //exit the app from main menu
                    else if (mainMenuInput == 3)
                    {
                        Console.Clear();
                        exit = true;
                        Console.WriteLine("Exiting app.");
                        break;
                    }

                    //main menu default
                    else
                    {
                        Console.WriteLine("Did you choose a number between 1 and 3?");
                    }
                }
                catch (Exception e) //if all else fails, no crash
                {
                    Console.WriteLine("Oh dear, we encountered an error... Please try again.\n\n" + e);
                    continue;
                }
            } while (exit == false);
        }
    }
}