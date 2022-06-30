using System;
using System.Linq;
using System.Threading;
using LearningDiaryMae.Models;
using static ClassLibraryDateMethods.Class1;

namespace LearningDiaryMae
{
    public static class Methods
    {

        //add a topic
        public static void AddTopic()
        {
            Console.WriteLine("Title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Describe the area of study: ");
            string description = Console.ReadLine();

            Console.WriteLine("How much time (days) do you estimate you need for mastering the topic?");
            int timeToMaster = ValidateIntInput(Console.ReadLine());

            Console.WriteLine("Did you use a source? Yes/no");
            bool usedSource = ValidateYesOrNoInput(Console.ReadLine());

            string source = "";
            if (usedSource == true)
            {
                Console.WriteLine("Which source did you use?");
                source = Console.ReadLine();
            }

            Console.WriteLine("When did you start studying? DD/MM/YYYY");
            DateTime startLearningDate = ValidateDateTimeInput(Console.ReadLine());

            Console.WriteLine("Is your study finished? Yes/no");
            bool finished = ValidateYesOrNoInput(Console.ReadLine());
            bool inProgress = true;

            DateTime? completionDate = null;

            //if study is complete, ask for end date. If end date is in the future, ask whether study is complete again
            if (finished)
            {
                GiveCompletionDate(out inProgress, out completionDate);
            }

            DateTime lastEditDate = DateTime.Now;

            Topic newTopic = new Topic()
            {
                Title = title,
                Description = description,
                Source = source,
                TimeToMaster = timeToMaster,
                StartLearningDate = startLearningDate,
                CompletionDate = (DateTime?)completionDate,
                LastEditDate = lastEditDate,
                InProgress = inProgress
            };
            newTopic.TimeSpent = (int?)newTopic.CalculateTimeSpent();

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Topics.Add(newTopic);
                newContext.SaveChanges();
            };
        }

        //prints topics from database
        public static void PrintTopics()
        {
            {
                Console.WriteLine("A list of your study topics:\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                using (LearningDiaryContext newContext = new LearningDiaryContext())
                {
                    IQueryable<Topic> read = newContext.Topics.Select(topic => topic);
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
        public static Topic ChooseIdOrTitle(string procedure)
        {
            Console.WriteLine("Would you like to select the topic by 1) ID or 2) title?");
            int input = ValidateIntInput(Console.ReadLine());
            Topic edit = new Topic();

            //finds the topic to be modified
            if (input == 1)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                int idOption = ValidateIntInput(Console.ReadLine());
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    Topic option = connection.Topics.FirstOrDefault(x => x.Id == idOption);
                    edit = option;
                }
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                string titleOption = Console.ReadLine();
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    Topic option = connection.Topics.FirstOrDefault(x => x.Title == titleOption);
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
                    var read = newContext.Tasks.Select(task => task);
                    foreach (var task in read)
                    {
                        Console.WriteLine(task.ToStringPrint());
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    }
                }
            }
        }

        //choosing topic by id or title, procedure = thing to be done to the task
        public static Task ChooseTaskIdOrTitle(string procedure)
        {
            Console.WriteLine("Would you like to select the task by 1) ID or 2) title?");
            int input = ValidateIntInput(Console.ReadLine());
            Task edit = new Task();

            //finds the topic to be modified
            if (input == 1)
            {
                Console.WriteLine($"Which task would you like to {procedure}?");
                int idOption = ValidateIntInput(Console.ReadLine());
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Tasks.FirstOrDefault(x => x.Id == idOption);
                    edit = option;
                }
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which task would you like to {procedure}?");
                string titleOption = Console.ReadLine();
                using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    var option = connection.Tasks.FirstOrDefault(x => x.Title == titleOption);
                    edit = option;
                }
            }
            return edit;
        }

        public static void AddTask()
        {
            Console.WriteLine("Is the task linked to an existing study topic? Yes/no");
            string reply = Console.ReadLine();
            int? topicId = null;

            if (reply.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("What is the ID of the topic the task is linked to?");
                topicId = ValidateIntInput(Console.ReadLine());
            }

            Console.WriteLine("Task title:");
            string taskTitle = Console.ReadLine();

            Console.WriteLine("Task description: ");
            string taskDescription = Console.ReadLine();

            //doesn't work yet
            Console.WriteLine("Do you want to add notes? Yes/no");
            bool validReply = ValidateYesOrNoInput(Console.ReadLine());
            string note = "";

            if (validReply)
            {
                Console.WriteLine("Write your note:");
                note = Console.ReadLine();
            }

            Console.WriteLine("What is the deadline? DD/MM/YYYY");
            DateTime deadline = ValidateDateTimeInput(Console.ReadLine());

            Console.WriteLine("How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
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

            Console.WriteLine("Is the task done? yes/no");
            bool done = ValidateYesOrNoInput(Console.ReadLine());

            Task newTask = new Task()
            {
                Title = taskTitle,
                Description = taskDescription,
                Notes = note,
                Deadline = deadline,
                Priority = priority,
                Topic = topicId,
                Done = done
            };
            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Add(newTask);
                newContext.SaveChanges();
            }
        }

        //validates int input
        public static int ValidateIntInput(string input)
        {
            int newInput;
            while (true)
            {
                if (int.TryParse(input, out newInput))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please give a number:");
                    input = Console.ReadLine();
                    continue;
                }
            }
            return newInput;
        }

        //validates input for datetime
        public static DateTime ValidateDateTimeInput(string input)
        {
            DateTime newInput;
            while (true)
            {
                if (DateTime.TryParse(input, out newInput))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please give a date (DD/MM/YYYY):");
                    input = Console.ReadLine();
                    continue;
                }
            }
            return newInput;
        }

        //validates yes or no input
        public static bool ValidateYesOrNoInput(string input)
        {
            bool newInput;
            while (true)
            {
                if (input.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    newInput = true;
                else if (input.Equals("no", StringComparison.OrdinalIgnoreCase))
                    newInput = false;
                else
                {
                    Console.WriteLine("Did you write yes or no?");
                    input = Console.ReadLine();
                    continue;
                }
                break;
            }
            return newInput;
        }

        //gets completion date, makes sure it is in the past, returns whether study in progress and the completion date
        public static void GiveCompletionDate(out bool inProgress, out DateTime? completionDate)
        {
            completionDate = new DateTime?();
            inProgress = false;
            while (true)
            {
                try
                {
                    Console.WriteLine("When did you finish with the topic? DD/MM/YYYY");
                    completionDate = ValidateDateTimeInput(Console.ReadLine());
                    bool isFutureDate = FutureDate((DateTime)completionDate);

                    if (isFutureDate)
                    {
                        Console.WriteLine("The date you gave seems to be in the future. Is your study finished? Yes/no");
                        bool studyComplete = ValidateYesOrNoInput(Console.ReadLine());

                        if (studyComplete)
                        {
                            inProgress = false;
                            continue;
                        }
                        else
                        {
                            inProgress = true;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
                break;
            }
        }

        //print tasks related to a specific topic
        public static void PrintTasksForTopic()
        {
            Console.WriteLine("What is the id of the topic you want to get the tasks of?");
            int topicId = ValidateIntInput(Console.ReadLine());

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                int id = topicId;
                IQueryable<Task> tasks = newContext.Tasks.Where(task => task.Topic == id);
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                foreach (var item in tasks)
                {
                    Console.WriteLine(item.ToStringPrint());
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
            }
        }

        //edit topic
        public static void EditTopic()
        {
            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                Topic editTopic = ChooseIdOrTitle("edit");
                Console.WriteLine("Which field would you like to edit?\n" +
                                  "1) Topic title\n" +
                                  "2) Topic description\n" +
                                  "3) Source\n" +
                                  "4) Date completed (DD/MM/YYYY)");
                int topicFieldChoice = ValidateIntInput(Console.ReadLine());
                editTopic.LastEditDate = DateTime.Now;

                switch (topicFieldChoice)
                {
                    case 1:
                        Console.WriteLine("Enter the new title:");
                        editTopic.Title = Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("Enter the new description:");
                        editTopic.Description = Console.ReadLine();
                        break;

                    case 3:
                        Console.WriteLine("Enter the source:");
                        editTopic.Source = Console.ReadLine();
                        break;

                    case 4:
                        GiveCompletionDate(out bool inProgress, out DateTime? completionDate);
                        editTopic.InProgress = inProgress;
                        editTopic.CompletionDate = completionDate;
                        break;

                    default:
                        Console.WriteLine("Did you choose a number between 1 and 4?");
                        break;
                }
                newContext.SaveChanges();
            }
        }

        //edit task
        public static void EditTask()
        {
            Task editTask = ChooseTaskIdOrTitle("edit");
            Console.WriteLine("Which field would you like to edit?\n" +
                              "1) Task title\n" +
                              "2) Task description\n" +
                              "3) Deadline(DD/MM/YYYY)\n" +
                              "4) Task status (done/not done)\n" +
                              "5) Task priority\n" +
                              "6) Add a note");
            int taskEditField = ValidateIntInput(Console.ReadLine());

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                switch (taskEditField)
                {
                    case 1:
                        Console.WriteLine("Enter the new title:");
                        editTask.Title = Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("Enter the new description:");
                        editTask.Description = Console.ReadLine();
                        break;

                    case 3:
                        Console.WriteLine("Enter the new deadline:");
                        editTask.Deadline = ValidateDateTimeInput(Console.ReadLine());
                        break;

                    case 4:
                        Console.WriteLine("Is the task done? Yes/no");
                        editTask.Done = ValidateYesOrNoInput(Console.ReadLine());
                        break;

                    //set task priority
                    case 5:
                        Console.WriteLine("How urgent is the task? 1) Urgent, 2) Not urgent, 3) Optional");
                        int priorityInput = ValidateIntInput(Console.ReadLine());
                        switch (priorityInput)
                        {
                            case 1:
                                editTask.Priority = "Urgent";
                                break;
                            case 2:
                                editTask.Priority = "Not urgent";
                                break;
                            case 3:
                                editTask.Priority = "Optional";
                                break;
                            default:
                                Console.WriteLine("No priority set.");
                                editTask.Priority = null;
                                break;
                        }
                        break;

                    //adding notes, doesn't work yet
                    case 6:
                        Console.WriteLine("Enter the new note:");
                        string newNote = Console.ReadLine();
                        editTask.Notes += "\n" + newNote;
                        break;

                    default:
                        Console.WriteLine("Did you choose a number between 1 and 4?");
                        break;
                }
                newContext.SaveChanges();
            }
        }
    }
}
