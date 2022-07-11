using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearningDiaryMae.Models;
using static LearningDiaryMae.CommonMethods;

namespace LearningDiaryMae
{
    public static class DiaryTaskMethods
    {
        public static async Task<bool> TaskMenu()
        {
            bool taskMenu = true;
            {
                Console.WriteLine(" What do you want to do?\n" +
                                  "\t1) Add a task\n" +
                                  "\t2) Print a current list of tasks\n" +
                                  "\t3) Find a task by id or title\n" +
                                  "\t4) Edit a task\n" +
                                  "\t5) Delete a task\n" +
                                  "\t6) Exit the app");
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
                        DiaryTask editDiaryTask = await ChooseIdOrTitle("task", "find") as DiaryTask;
                        if (editDiaryTask != null) Console.WriteLine(editDiaryTask.ToStringPrint());
                        break;

                    case 4: //editing by id or title
                        Console.Clear();
                        EditTask();
                        break;

                    case 5: // deleting by id or title
                        Console.Clear();
                        editDiaryTask = await ChooseIdOrTitle("task", "delete") as DiaryTask;
                        await DeleteRow(editDiaryTask);
                        break;

                    case 6: //exit the app from task menu
                        Console.Clear();
                        taskMenu = false;
                        Console.WriteLine(" Exiting app.");
                        break;

                    default: //task menu default
                        Console.WriteLine(" Did you choose a number between 1 and 6?");
                        break;
                }
            }
            return taskMenu;
        }

        //print tasks from database
        public static async Task PrintTasks()
        {
            Console.WriteLine("\t\tA list of your tasks:\n\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            await using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                IQueryable<DiaryTask> read = newContext.Tasks.Select(task => task);
                foreach (var task in read)
                {
                    Console.WriteLine(task.ToStringPrint());
                    Console.WriteLine("\n\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
            }
        }

        //add task
        public static async Task AddTask()
        {
            Console.WriteLine(" Is the task linked to an existing study topic? Yes/no");
            bool link = ValidateYesOrNoInput(Console.ReadLine());
            int? topicId = null;

            if (link)
            {
                Console.WriteLine(" What is the ID of the topic the task is linked to?");
                topicId = ValidateIntInput(Console.ReadLine());
            }

            string taskTitle = GetStringInput("title");

            string taskDescription = GetStringInput("description");

            //doesn't work yet
            Console.WriteLine(" Do you want to add notes? Yes/no");
            bool validReply = ValidateYesOrNoInput(Console.ReadLine());
            string note = "";

            if (validReply)
            {
                note = GetStringInput("note");
            }
            
            DateTime deadline = GetDateTimeInput("deadline");

            Console.WriteLine(" How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
            int input3 = ValidateIntInput(Console.ReadLine());
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
                    Console.WriteLine("No priority set");
                    priority = null;
                    break;
            }

            Console.WriteLine(" Is the task done? yes/no");
            bool done = ValidateYesOrNoInput(Console.ReadLine());

            DiaryTask newDiaryTask = new DiaryTask()
            {
                Title = taskTitle,
                Description = taskDescription,
                Notes = note,
                Deadline = deadline,
                Priority = priority,
                Topic = topicId,
                Done = done
            };
            await using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Add(newDiaryTask);
                newContext.SaveChanges();
            }
        }

        //edit task
        public static async Task EditTask()
        {
            DiaryTask editDiaryTask = await ChooseIdOrTitle("task", "edit") as DiaryTask;
            Console.WriteLine(" Which field would you like to edit?\n" +
                              "\t1) DiaryTask title\n" +
                              "\t2) DiaryTask description\n" +
                              "\t3) Deadline(DD/MM/YYYY)\n" +
                              "\t4) DiaryTask status (done/not done)\n" +
                              "\t5) DiaryTask priority\n" +
                              "\t6) Add a note");
            int taskEditField = ValidateIntInput(Console.ReadLine());

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                switch (taskEditField)
                {
                    case 1:
                        editDiaryTask.Title = GetStringInput("new title");
                        break;

                    case 2:
                        editDiaryTask.Description = GetStringInput("new title");
                        break;

                    case 3:
                        editDiaryTask.Deadline = GetDateTimeInput("new deadline");
                        break;

                    case 4:
                        Console.WriteLine(" Is the task done? Yes/no");
                        editDiaryTask.Done = ValidateYesOrNoInput(Console.ReadLine());
                        break;

                    //set task priority
                    case 5:
                        Console.WriteLine(" How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
                        int priorityInput = ValidateIntInput(Console.ReadLine());
                        switch (priorityInput)
                        {
                            case 1:
                                editDiaryTask.Priority = "Urgent";
                                break;
                            case 2:
                                editDiaryTask.Priority = "Not urgent";
                                break;
                            case 3:
                                editDiaryTask.Priority = "Optional";
                                break;
                            default:
                                Console.WriteLine("No priority set.");
                                editDiaryTask.Priority = null;
                                break;
                        }
                        break;

                    //adding notes, doesn't work yet
                    case 6:
                        string newNote = GetStringInput("new note");
                        editDiaryTask.Notes += "\n" + newNote;
                        break;

                    default:
                        Console.WriteLine(" Did you choose a number between 1 and 4?");
                        break;
                }
                newContext.SaveChanges();
            }
        }

    }
}
