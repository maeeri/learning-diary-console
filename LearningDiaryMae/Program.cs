using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Net.Sockets;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LearningDiaryMae
{
    class Program
    {
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

            if (!File.Exists(path))
            {
                //create csv-file with headers
                string arrayString = String.Join(";", headerArray);
                File.WriteAllText(path, arrayString + "\n");
            }

            using var streamReader = File.OpenText(path);
            using var csvReader = new CsvReader(streamReader, csvConfig);

            string value = File.ReadAllLines(path)
                .Skip(1)
                .Select(v => Topic.FromCsv(v))
                .ToString(); 

            //while (csvReader.Read())
            //{
            //    for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
            //    {
            //        diaryList.Add(Topic.FromCsv());
            //        //Console.Write($"{value} ");
            //    }

            //    //Console.WriteLine();
            //}

            //IEnumerable<string> diaryIEnumerable = File.ReadAllLines(path)
            //    .Skip(1)
            //    .Select(index => IEnumerable<string>.FromCsv(index));

            //using (TextReader diaryReader = new StreamReader(path))
            //{
            //    using (var csv = new CsvReader(diaryReader))
            //    {
            //        var records = csv.GetRecords<Topic>();
            //        diaryList = records.ToList();
            //    }
            //}
            //foreach (Topic entry in diaryIEnumerable)
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
                            Topic newTopic = Topic.AddTopic(counter);
                            diaryDictionary.Add(counter, newTopic);
                            File.AppendAllText(path, newTopic.ToString());
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
                            Console.WriteLine("Which field would you like to edit?\n" +
                                              "1) Topic title" +
                                              "2) Topic description" +
                                              "3) Source" +
                                              "4) Date completed");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the edited field:");
                            string input = Console.ReadLine();

                            if (choice == 1)
                                diaryDictionary[edit].Title = input;
                            else if (choice == 2)
                                diaryDictionary[edit].Description = input;
                            else if (choice == 3)
                                diaryDictionary[edit].Source = input;
                            else if (choice == 4)
                            {
                                diaryDictionary[edit].CompletionDate = DateTime.Parse(input);
                                diaryDictionary[edit].InProgress = false;
                            }
                            else
                                Console.WriteLine("Did you choose a number between 1 and 4?");
                            break;

                        case 6: // deleting by id or title
                            edit = ChooseIdOrTitle("delete");
                            Console.WriteLine(diaryDictionary[edit].Title + " deleted.");
                            diaryDictionary.Remove(edit);
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
        }

        //prints topics from csv file
        public static void PrintTopics(string path)
        {
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