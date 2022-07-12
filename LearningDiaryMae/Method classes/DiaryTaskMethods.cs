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
                Console.Clear();
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
                        if (editDiaryTask != null) TaskToPrint(editDiaryTask);
                        break;

                    case 4: //editing by id or title
                        Console.Clear();
                        EditTask();
                        break;

                    case 5: // deleting by id or title
                        Console.Clear();
                        editDiaryTask = await ChooseIdOrTitle("task", "delete") as DiaryTask;
                        await DeleteRow(editDiaryTask);
                        Console.WriteLine($" Task {editDiaryTask.Id}: {editDiaryTask.Title} deleted.");
                        break;

                    case 6: //exit the app from task menu
                        Console.Clear();
                        taskMenu = false;
                        Console.WriteLine(" Exiting app.");
                        break;

                    default: //task menu default
                        AlertInvalidChoice(6);
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
                    TaskToPrint(task);
                    Console.WriteLine("\n\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
            }
        }

        //add task
        public static async Task AddTask()
        {
            DiaryTask newDiaryTask = new DiaryTask();
            Console.WriteLine(" Is the task linked to an existing study topic? Yes/no");
            bool link = ValidateYesOrNoInput(Console.ReadLine());
            int? topicId = null;

            if (link)
            {
                Console.WriteLine(" What is the ID of the topic the task is linked to?");
                newDiaryTask.Topic = ValidateIntInput(Console.ReadLine());
            }

            newDiaryTask.Title = GetStringInput("title");
            newDiaryTask.Description = GetStringInput("description");
            newDiaryTask.Deadline = GetDateTimeInput("deadline");
            newDiaryTask.Priority = SetPriority(newDiaryTask);

            Console.WriteLine(" Is the task done? yes/no");
            newDiaryTask.Done = ValidateYesOrNoInput(Console.ReadLine());

            await using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Tasks.Add(newDiaryTask);
                newContext.SaveChanges();
            }

            //adding a note to new task. has to open a new context after saving task to db
            //do not try to combine or will clash with task id!!!
            Console.WriteLine(" Do you want to add notes? Yes/no");
            bool validReply = ValidateYesOrNoInput(Console.ReadLine());
            Note note = new Note();

            if (validReply)
            {
                await AddNewNote(note, newDiaryTask);
            }
        }

        //edit task
        public static async Task EditTask()
        {
            DiaryTask editDiaryTask = await ChooseIdOrTitle("task", "edit") as DiaryTask;
            Note newNote = new Note();

            Console.WriteLine(" Which field would you like to edit?\n" +
                              "\t1) DiaryTask title\n" +
                              "\t2) DiaryTask description\n" +
                              "\t3) Deadline(DD/MM/YYYY)\n" +
                              "\t4) DiaryTask status (done/not done)\n" +
                              "\t5) DiaryTask priority\n" +
                              "\t6) Add a note");
            int taskEditField = ValidateIntInput(Console.ReadLine());

            switch (taskEditField)
            {
                case 1:
                    editDiaryTask.Title = GetStringInput("new title");
                    break;

                case 2:
                    editDiaryTask.Description = GetStringInput("new description");
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
                    editDiaryTask.Priority = SetPriority(editDiaryTask);
                    break;

                //adding notes
                case 6:
                    string addNote = GetStringInput("new note");
                    newNote.Note1 = addNote;
                    newNote.Task = editDiaryTask.Id;
                    break;

                default:
                    AlertInvalidChoice(6);
                    break;
            }

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                if (newNote.Note1 != null)
                    newContext.Notes.Add(newNote);

                newContext.Update(editDiaryTask);
                newContext.SaveChanges();
            }
        }

        //helper method to print yes if task is done and no if it is not
        private static string IsDone(DiaryTask task)
        {
            return (task.Done == true) ? "yes" : "no";
        }

        //print task to console
        public static void TaskToPrint(DiaryTask task)
        {
            string toPrint = ($"\t\tID: {task.Id}\n" +
                              $"\t\tTitle: {task.Title}\n" +
                              $"\t\tDescription: {task.Description}\n" +
                              $"\t\tDeadline: {task.Deadline: d.M.yyyy}\n" +
                              $"\t\tDone: {IsDone(task)}\n" +
                              $"\t\tPriority: {(DiaryTask.PriorityEnum)task.Priority}\n\n" +
                              $"\t\tNotes:\n");
            Console.WriteLine(toPrint);
            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                var read = newContext.Notes.Where(note => note.Task == task.Id);
                foreach (var item in read)
                {
                    Console.WriteLine($"\t\t-- {item.Note1}");
                }
            }
        }

        //sets task priority
        public static int? SetPriority(DiaryTask task)
        {
            int? priority;
            Console.WriteLine(" How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
            int priorityInput = ValidateIntInput(Console.ReadLine());
            if (priorityInput == 1 || priorityInput == 2 || priorityInput == 3)
            {
                priority = priorityInput;
            }
            else
            {
                Console.WriteLine("No priority set");
                priority = null;
            }
            return priority;
        }

        public static async Task AddNewNote(Note note, DiaryTask task)
        {
            note.Note1 = GetStringInput("note");
            note.Task = task.Id;
            using (LearningDiaryContext otherContext = new LearningDiaryContext())
            {
                otherContext.Notes.Add(note);
                otherContext.SaveChanges();
            }
        }
    }
}
