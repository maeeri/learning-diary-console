using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearningDiaryMae.Models;
using static LearningDiaryMae.DiaryTopicMethods;
using static LearningDiaryMae.DiaryTaskMethods;


namespace LearningDiaryMae
{
    public static class CommonMethods
    {
        //main menu
        public static async Task<bool> MainMenu(bool continueLoop)
        {
            bool mainMenu = continueLoop;

            if (!mainMenu)
                return mainMenu;

            //ask user whether to work with topics or tasks
            Console.WriteLine("\n What do you want to do?\n" +
                              "\t1) Work with topics\n" +
                              "\t2) Work with tasks\n" +
                              "\t3) Exit the app");
            int mainMenuInput = ValidateIntInput(Console.ReadLine());
            Console.WriteLine("\n\n");

            switch (mainMenuInput)
            {
                case 1:
                    mainMenu = await TopicMenu();
                    PressKey();
                    break;
                case 2:
                    mainMenu = await TaskMenu();
                    PressKey();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine(" Exiting app.");
                    mainMenu = false;
                    break;
                default:
                    Console.WriteLine(" Did you write a number between 1 and 3?");
                    mainMenu = true;
                    break;
            }
            return mainMenu;
        }


        public static void TitleSequence()
        {
            //fancy title sequence
            Console.Write("\t\t   ");
            for (int i = 0; i < 64; i++)
            {
                Console.Write("~");
                Thread.Sleep(10);
            }
            Console.Write("\n" + asciiart + "\n\t\t   ");
            for (int i = 0; i < 64; i++)
            {
                Console.Write("~");
                Thread.Sleep(10);
            }
        }

        //choosing topic or task by id or title, target = topic or task, procedure = thing to be done to the topic
        public static async Task<Object> ChooseIdOrTitle(string target, string procedure)
        {
            while (true)
            {
                Console.WriteLine($" What is the title or Id of the {target} you want to {procedure}");
                string input = Console.ReadLine();
                await using (LearningDiaryContext connection = new LearningDiaryContext())
                {
                    DiaryTask optionDiaryTask;
                    DiaryTopic optionDiaryTopic;
                    if (int.TryParse(input, out int idOption))
                    {
                        optionDiaryTopic = connection.Topics.FirstOrDefault(x => x.Id == idOption);
                        optionDiaryTask = connection.Tasks.FirstOrDefault(x => x.Id == idOption);
                    }
                    
                    else
                    {
                        optionDiaryTopic = connection.Topics.FirstOrDefault(x => x.Title == input);
                        optionDiaryTask = connection.Tasks.FirstOrDefault(x => x.Title == input);
                    }

                    Object? edit;
                    if (target == "topic")
                        edit = optionDiaryTopic;
                    else
                        edit = optionDiaryTask;

                    if (edit != null)
                        return edit;
                    else
                        Console.WriteLine($" The id or title of the {target} you gave doesn't seem to exist. Please try again.");
                }
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
                    Console.WriteLine(" Please give a number:");
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
                    Console.WriteLine(" Please give a date (DD/MM/YYYY):");
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
                    Console.WriteLine(" Did you write yes or no?");
                    input = Console.ReadLine();
                    continue;
                }
                break;
            }
            return newInput;
        }

        //remove a topic or task from database
        public static async Task DeleteRow(Object toBeDeleted)
        {
            using (LearningDiaryContext newContext = new LearningDiaryContext())
            {
                newContext.Remove(toBeDeleted);
                await newContext.SaveChangesAsync();
            }
        }

        //asks for input and returns it in string form
        public static string GetStringInput(string item)
        {
            Console.WriteLine($" Please give the {item}: ");
            string response = Console.ReadLine();
            return response;
        }

        //asks for input and returns it in string form
        public static DateTime GetDateTimeInput(string item)
        {
            Console.WriteLine($" Please give the {item} (DD/MM/YYYY)");
            DateTime response = ValidateDateTimeInput(Console.ReadLine());
            return response;
        }

        //to help with async operations
        public static void PressKey()
        {
            Console.WriteLine(" Press enter to continue");
            Console.ReadKey();
        }

        public static string asciiart = @"                     ,    _, _   ,_  ,  , ___,,  ,  _,      ,_   ___,_   ,_    ,  ,
                     |   /_,'|\  |_) |\ |' |  |\ | / _      | \,' | '|\  |_)   \_/ 
                    '|__'\_  |-\'| \ |'\| _|_,|'\|'\_|`    _|_/  _|_,|-\'| \  , /` 
                       '   ` '  `'  `'  `'    '  `  _|    '     '    '  `'  `(_/   
                                                   '                               ";
    }
}
