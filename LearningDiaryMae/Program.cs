using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Program
    {
        //declare dictionary and list + csv config outside main, needed by methods below
        static Dictionary<int, Topic> diaryDictionary = new Dictionary<int, Topic>();
        static List<Topic> diaryList = new List<Topic>();
        static CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            HasHeaderRecord = true,
            Comment = '#',
            AllowComments = true,
            Delimiter = ";",
        };

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

            //read from file to dictionary via list, if there is something to read
            if (File.Exists(path))
            {
                using (var streamReader = File.OpenText(path))
                {
                    using (var csvReader = new CsvReader(streamReader, csvConfig))
                    {
                        diaryList = File.ReadAllLines(path)
                            .Skip(1)
                            .Select(v => Topic.FromCsv(v))
                            .ToList();
                    };
                };

                foreach (var item in diaryList)
                {
                    diaryDictionary.Add(item.Id, item);
                }
            }

            //create new csv-file with headers replacing the old one
            string arrayString = String.Join(";", headerArray);
            File.WriteAllText(path, arrayString + "\n");

            //make id (determined by counter) start from last one instead of automatic 0
            counter = diaryDictionary.Count;

            string name = "your friendly Learning Diary";
            Console.WriteLine($"Welcome to {name}");

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
                            Topic newTopic = AddTopic(counter);
                            diaryDictionary.Add(counter, newTopic);
                            counter++;
                            break;

                        case 2: //adding a task, to be added at one point or another

                            break;

                        case 3: //printing a list of topics, from dictionary using method
                            PrintTopics();
                            break;

                        case 4: //finding a topic by id or title
                            var edit = ChooseIdOrTitle("find");
                            string printout = diaryDictionary[edit].ToStringPrint();
                            Console.WriteLine(printout);
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

                            if (choice == 1)
                                diaryDictionary[edit].Title = input;
                            else if (choice == 2)
                                diaryDictionary[edit].Description = input;
                            else if (choice == 3)
                                diaryDictionary[edit].Source = input;
                            else if (choice == 4)
                            {
                                diaryDictionary[edit].InProgress = false;
                                diaryDictionary[edit].CompletionDate = DateTime.Parse(input);
                                diaryDictionary[edit].TimeSpent = diaryDictionary[edit].CalculateTimeSpent();
                            }
                            else
                                Console.WriteLine("Did you choose a number between 1 and 4?");

                            diaryDictionary[edit].LastEditDate = DateTime.Now;
                            break;

                        case 6: // deleting by id or title, deletes from dictionary, creates temp dict to work out new id's/keys, reference to temp
                            edit = ChooseIdOrTitle("delete");
                            Console.WriteLine(diaryDictionary[edit].Title + " deleted.");
                            diaryDictionary.Remove(edit);
                            Dictionary<int, Topic> tempDictionary = new Dictionary<int, Topic>();

                            counter = 0;
                            foreach (var item in diaryDictionary)
                            {
                                tempDictionary[counter] = diaryDictionary[item.Key];
                                tempDictionary[counter].Id = counter;
                                counter++;
                            }
                            diaryDictionary.Clear();
                            diaryDictionary = tempDictionary;
                            break;

                        case 7: //exit the app, write dictionary to file
                            exit = true;
                            File.AppendAllLines(path, diaryDictionary.Select(add => add.Value.ToString()));
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
        public static Topic AddTopic(int counter)
        {
            Topic newTopic = new Topic(counter);

            Console.WriteLine("Title: ");
            newTopic.Title = Console.ReadLine();

            Console.WriteLine("Describe the area of study: ");
            newTopic.Description = Console.ReadLine();

            Console.WriteLine("How much time (hours) do you estimate you need for mastering the topic?");
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
            }
            newTopic.TimeSpent = newTopic.CalculateTimeSpent();
            newTopic.LastEditDate = DateTime.Now;

            return newTopic;
        }

        //prints topics from dictionary
        public static void PrintTopics()
        {
            {
                Console.WriteLine("A list of your study topics:\n******************************");

                foreach (var item in diaryDictionary)
                {
                    Console.WriteLine(item.Value.ToStringPrint());
                    Console.WriteLine("******************************");
                }
            }
        }

        //choosing topic by id or title, procedure = thing to be done to the topic
        public static int ChooseIdOrTitle(string procedure)
        {
            Console.WriteLine("Would you like to select the topic by 1) ID or 2) title?");
            int input = Convert.ToInt32(Console.ReadLine());
            int edit = 0;

            //finds the Key of the Topic to be edited
            if (input == 1)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                int idOption = Convert.ToInt32(Console.ReadLine());
                edit = idOption;
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which topic would you like to {procedure}?");
                string titleOption = Console.ReadLine();
                foreach (var value in diaryDictionary)
                {
                    if (titleOption.Equals(value.Value.Title, StringComparison.OrdinalIgnoreCase))
                        edit = value.Key;
                }
            }
            return edit;
        }
    }
}