using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearningDiaryMae.Models;
using ClassLibraryDateMethods;
using static LearningDiaryMae.CommonMethods;

namespace LearningDiaryMae
{
    public static class DiaryTopicMethods
    {
        public static async Task<bool> TopicMenu()
        {
            bool topicMenu = true;

            Console.Clear();
            //ask what the user wants to do and go to switch
            Console.WriteLine(" What do you want to do?\n" +
                              "\t1) Add a topic of study\n" +
                              "\t2) Print a current list of topics\n" +
                              "\t3) Find a topic by id or title\n" +
                              "\t4) Edit a topic\n" +
                              "\t5) Delete a topic\n" +
                              "\t6) Print tasks related to a topic\n" +
                              "\t7) Exit the app");
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
                    DiaryTopic editDiaryTopic = await ChooseIdOrTitle("topic", "find") as DiaryTopic;
                    if (editDiaryTopic != null) Console.WriteLine(TopicToPrint(editDiaryTopic));
                    break;

                case 4: //editing by id or title
                    Console.Clear();
                    await EditTopic();
                    break;

                case 5: // deleting by id or title
                    Console.Clear();
                    editDiaryTopic = await ChooseIdOrTitle("topic", "delete") as DiaryTopic;
                    DeleteRow(editDiaryTopic);
                    Console.WriteLine($" Topic {editDiaryTopic.Id}: {editDiaryTopic.Title} deleted");
                    break;

                case 6: //print tasks related to a specific topic
                    Console.Clear();
                    await PrintTasksForTopic();
                    break;

                case 7: //exit the app from topic menu
                    Console.Clear();
                    topicMenu = false;
                    Console.WriteLine(" Exiting app.");
                    break;

                //topic menu default
                default:
                    AlertInvalidChoice(7);
                    break;
            }

            return topicMenu;
        }

        //add a topic
        public static async Task AddTopic()
        {
            string title = GetStringInput("title");

            string description = GetStringInput("description");

            Console.WriteLine(" How much time (days) do you estimate you need for mastering the topic?");
            int timeToMaster = ValidateIntInput(Console.ReadLine());

            Console.WriteLine(" Did/will you use a source? Yes/no");
            bool usedSource = ValidateYesOrNoInput(Console.ReadLine());

            string source = "";
            if (usedSource == true)
            {
                source = GetStringInput("source");
            }

            DateTime startLearningDate = GetDateTimeInput("starting date");

            Console.WriteLine(" Is your study finished? Yes/no");
            bool finished = ValidateYesOrNoInput(Console.ReadLine());

            bool inProgress = true;
            DateTime? completionDate = null;

            //if study is complete, ask for end date. If end date is in the future, ask whether study is complete again
            if (finished)
            {
                GiveCompletionDate(out inProgress, out completionDate);
            }

            DateTime lastEditDate = DateTime.Now;

            DiaryTopic newDiaryTopic = new DiaryTopic()
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
            newDiaryTopic.TimeSpent = (int?)CalculateTimeSpent(newDiaryTopic);

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Topics.Add(newDiaryTopic);
                await newContext.SaveChangesAsync();
            }
        }

        //prints topics from database
        public static async Task PrintTopics()
        {
            Console.WriteLine("\t\tA list of your study topics:\n\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            await using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                IQueryable<DiaryTopic> read = newContext.Topics.Select(topic => topic);
                foreach (var topic in read)
                {
                    Console.WriteLine(TopicToPrint(topic));
                    Console.WriteLine("\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("\n\n");
        }

        //gets completion date, makes sure it is in the past, returns whether study in progress and the completion date
        private static void GiveCompletionDate(out bool inProgress, out DateTime? completionDate)
        {
            completionDate = new DateTime?();
            inProgress = false;
            while (true)
            {
                try
                {
                    completionDate = GetDateTimeInput("completion date");
                    bool isFutureDate = Class1.FutureDate((DateTime)completionDate);

                    if (isFutureDate)
                    {
                        Console.WriteLine(" The date you gave seems to be in the future. Is your study finished? Yes/no");
                        bool studyComplete = ValidateYesOrNoInput(Console.ReadLine());

                        if (!studyComplete)
                        {
                            inProgress = true;
                            completionDate = null;
                            break;
                        }

                        inProgress = false;
                        continue;
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
        public static async Task PrintTasksForTopic()
        {
            Console.WriteLine(" What is the id of the topic you want to get the tasks of?");
            int topicId = ValidateIntInput(Console.ReadLine());

            await using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                int id = topicId;
                IQueryable<DiaryTask> tasks = newContext.Tasks.Where(task => task.Topic == id);
                Console.WriteLine("\n\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                foreach (var item in tasks)
                {
                    DiaryTaskMethods.TaskToPrint(item);
                    Console.WriteLine("\t\t~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                }
            }
        }

        //edit topic
        public static async Task EditTopic()
        {
            DiaryTopic editDiaryTopic = await ChooseIdOrTitle("topic", "edit") as DiaryTopic;
            Console.WriteLine(" Which field would you like to edit?\n" +
                              "\t1) DiaryTopic title\n" +
                              "\t2) DiaryTopic description\n" +
                              "\t3) Source\n" +
                              "\t4) Date completed (DD/MM/YYYY)");
            int topicFieldChoice = ValidateIntInput(Console.ReadLine());
            editDiaryTopic.LastEditDate = DateTime.Now;

            switch (topicFieldChoice)
            {
                case 1:
                    editDiaryTopic.Title = GetStringInput("title");
                    break;

                case 2:
                    editDiaryTopic.Description = GetStringInput("description");
                    break;

                case 3:
                    editDiaryTopic.Source = GetStringInput("source");
                    break;

                case 4:
                    GiveCompletionDate(out bool inProgress, out DateTime? completionDate);
                    editDiaryTopic.InProgress = inProgress;
                    editDiaryTopic.CompletionDate = completionDate;
                    break;

                default:
                    AlertInvalidChoice(4);
                    break;
            }

            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Update(editDiaryTopic);
                await newContext.SaveChangesAsync();
            }
        }

        //method to calculate time spent on the topic
        public static double CalculateTimeSpent(DiaryTopic topic)
        {
            TimeSpan timeSpent;
            double timeSpentDouble;
            if (topic.InProgress == true)
            {
                timeSpent = (TimeSpan)(DateTime.Today - topic.StartLearningDate);
                timeSpentDouble = timeSpent.TotalDays;
                topic.CompletionDate = null;
            }

            else
            {
                timeSpent = (TimeSpan)(topic.CompletionDate - topic.StartLearningDate);
                timeSpentDouble = timeSpent.TotalDays;
            }
            return timeSpentDouble;
        }

        //printing topics to console
        public static string TopicToPrint(DiaryTopic topic)
        {
            string print = $"\t\tId: {topic.Id}\n" +
                           $"\t\tTitle: {topic.Title}\n" +
                           $"\t\tDescription: {topic.Description}\n" +
                           $"\t\tStarted: {topic.StartLearningDate:d.M.yyyy}\n" +
                           $"\t\tLast edited: {topic.LastEditDate:d.M.yyyy h:m:s}\n";
            return print;
        }
    }
}