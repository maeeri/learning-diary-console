using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Program
    {
        static Dictionary<int, Topic> diaryDictionary = new Dictionary<int, Topic>();
        //List<Topic> diaryList = new List<Topic>();

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

            if (!File.Exists(path))
            {
                //create csv-file with headers
                string arrayString = String.Join(";", headerArray);
                File.WriteAllText(path, arrayString + "\n");
            }

            //IEnumerable<string> diaryIEnumerable = File.ReadAllLines(path)
            //    .Skip(1)
            //    .Select(index => (IEnumerable<string>.FromCsv(index))
            //    .ToList();

            //using (TextReader diaryReader = new StreamReader(path))
            //{
            //    using (var csv = new CsvReader(diaryReader))
            //    {
            //        var records = csv.GetRecords<Topic>();
            //        diaryList = records.ToList();
            //    }
            //}
            //foreach(Topic entry in diaryIEnumerable)
            //{
            //    diaryDictionary.Add(entry.Id, entry);
            //    counter++;
            //}

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
                                      "6. Exit the app");
                    int answer = Convert.ToInt32(Console.ReadLine());

                    switch (answer)
                    {
                        case 1: //adding a topic to dictionary with method
                            Topic newTopic = AddTopic(counter);
                            diaryDictionary.Add(counter, newTopic);
                            //File.AppendAllLines(path, diaryIEnumerable);
                            counter++;
                            break;

                        case 2: //adding a task, to be added at one point or another

                            break;

                        case 3: //printing a list of topics
                            PrintTopics(path);
                            break;

                        case 4: //finding a topic by id or title
                            var edit = ChooseIdOrTitle("find");

                            string printout = $"Topic title: {diaryDictionary[edit].Title}\n" +
                                              $"Description: {diaryDictionary[edit].Description}\n" +
                                              $"Start date: {diaryDictionary[edit].StartLearningDate:d.M.yyyy}\n" +
                                              $"Last edit date: {diaryDictionary[edit].LastEditDate:d.M.yyyy}\n";
                            Console.WriteLine(printout);
                            break;

                        case 5: // editing by id or title
                            edit = ChooseIdOrTitle("edit");
                            break;

                        case 6: // deleting by id or title
                            edit = ChooseIdOrTitle("delete");
                            break;
                        case 7: //exit the app
                            Console.WriteLine("Exiting app.");
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Did you choose a number between 1 and 6?");
                            break;
                    }
                }
                catch (Exception e) //if all else fails, no crash
                {
                    Console.WriteLine("Oh dear, we encountered an error... Please try again.\n" + e);
                    continue;
                }

            } while (exit == false);

            //foreach (var entry in diaryDictionary)
            //{
            //    Console.WriteLine(entry.Key + "\t" + entry.Value.Title);
            //}
        }

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

        //prints topics from csv file
        public static void PrintTopics(string path)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Comment = '#',
                AllowComments = true,
                Delimiter = ";",
            };

            {
                Console.WriteLine("A list of your study topics:");

                using var streamReader = File.OpenText(path);
                using var csvReader = new CsvReader(streamReader, csvConfig);
                while (csvReader.Read())
                {
                    string title = csvReader.GetField(1);
                    string id = csvReader.GetField(0).ToString();
                    Console.Write("{0,3}", id);
                    Console.Write("\t");
                    Console.WriteLine(title);
                }
                Console.WriteLine("\n");
            }
        }

        public static int ChooseIdOrTitle(string placeholder)
        {
            Console.WriteLine("Would you like to select the topic by 1) ID or 2) title?");
            int input = Convert.ToInt32(Console.ReadLine());
            int edit = 0;

            //finds the Key of the Topic to be edited
            if (input == 1)
            {
                Console.WriteLine($"Which topic would you like to {placeholder}?");
                int idOption = Convert.ToInt32(Console.ReadLine());
                edit = idOption;
            }

            else if (input == 2)
            {
                Console.WriteLine($"Which topic would you like to {placeholder}?");
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