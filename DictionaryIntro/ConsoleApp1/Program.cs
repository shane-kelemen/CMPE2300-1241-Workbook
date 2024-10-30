using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Going to be generating lots of random values for collection population
            Random rng = new Random();

            // Need some fist and last names to randomly generate names, but not so many that duplicates
            // are impossible.  We need to demonstrate a crash on the Add operation.
            string[] first = new string[5] { "Tom", "Ben", "John", "Jerry", "Hank" };
            string[] last = new string[5] { "Smith", "Jones", "Weight", "Ferguson", "Carpenter" };

            // Need a place to put the Golfer class instances before Dictionary operations
            List<Golfer> myGolfers = new List<Golfer>();
            
            // Populate the list with 20 Golfers
            for(int i = 0; i < 20; ++i)
            {
                myGolfers.Add(new Golfer(first[rng.Next(5)], last[rng.Next(5)], rng.Next(35)));
                
            }

            // Display the unordered and unfiltered set of Golfers 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers in Order of Creation");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Golfer golfer in myGolfers)
                Console.WriteLine(golfer.ToString());
            Console.WriteLine();

            // Need a collection that will allow us to easily categorize our Golfers
            Dictionary<int, List<Golfer>> catGolfers = new Dictionary<int, List<Golfer>>();

            // Iterate through the Golfers and add each Golfer to the categorized collection
            // organized by equal handicaps
            foreach (Golfer golfer in myGolfers)
            {
                if (!catGolfers.ContainsKey(golfer.Handicap))  // Comment out this line and run the code to see 
                                                               // a crash when a duplicate Key is added to the Dictionary
                    catGolfers.Add(golfer.Handicap, new List<Golfer>());
                // The following line could be used instead of the Add() attempt above, and if you eliminate
                // the check the program will not crash, but you will continuously create a new List<Golfer>
                // for each match, and thus will only ever have one Golfer per handicap score.  Lost information
                // is a big no-no.
                // catGolfers[golfer.Handicap] = new List<Golfer>();  // Assigns a new List to the Dictionary index

                catGolfers[golfer.Handicap].Add(golfer);  // Notice that the indexed Dictionary value may be treated
                                                          // just like the List<Golfer> it is.
            }

            // The following displays all the golfers grouped by handicap, 
            // but not in the order of the handicaps
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers Grouped By Handicap - Handicaps not in Order");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (List<Golfer> golfers in catGolfers.Values)
                foreach (Golfer golfer in golfers)
                    Console.WriteLine(golfer);
            Console.WriteLine();

            // We can create a temporary list of the Keys for sorting, then 
            // run through the sorted list and display the groups again, this time
            // ordered by handicap if the we index the dictionary with the sorted Keys
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers Grouped By Handicap - Handicaps in Order After Sorting Keys Only");
            Console.ForegroundColor = ConsoleColor.Gray;
            List<int> handicaps = catGolfers.Keys.ToList();
            handicaps.Sort();
            foreach(int handicap in handicaps)
                foreach (Golfer golfer in catGolfers[handicap])
                        Console.WriteLine(golfer);
            Console.WriteLine();

            // The following will accomplish the same as the above by using the OrderBy
            // extension method of the dictionary
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers Grouped By Handicap - Handicaps in Order After Sorting KeyValuePairs");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (KeyValuePair<int, List<Golfer>> golfers in catGolfers.OrderBy(x => x.Key))
                foreach (Golfer golfer in golfers.Value)
                    Console.WriteLine(golfer);
            Console.WriteLine();

            // The following will filter the Golfers, keeping only those with even handicaps in a temporary 
            // collection.  Not that it has been completed twice, once creating a new Dictionary, and once 
            // creating a List<KeyValuePair> from the original Dictionary.  Try them both out.
            // In both cases, the filtered items are removed from the original Dictionary.
            // Remove the .ToDictionary() or .ToList() to witness how a crash occurs if you do not create a
            // temporary collection.  This occurs as a result of deferred execution changing the collection
            // as it is being iterated.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers Grouped By Handicap - Filtered to Display Only Even Handicaps");
            Console.ForegroundColor = ConsoleColor.Gray;
            //foreach (KeyValuePair<int, List<Golfer>> kvp in catGolfers.Where(x => x.Key % 2 == 0)
            //                                                            .ToDictionary(x => x.Key, x => x.Value))
            foreach (KeyValuePair<int, List<Golfer>> kvp in catGolfers.Where(x => x.Key % 2 == 0).ToList())
                {
                foreach (Golfer golfer in kvp.Value)
                {
                    Console.WriteLine(golfer);
                }
                catGolfers.Remove(kvp.Key);
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("20 Golfers Grouped By Handicap - Leftover Odd Handicaps after Filtering Above");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (List<Golfer> golfers in catGolfers.Values)
                foreach (Golfer golfer in golfers)
                    Console.WriteLine(golfer);
            Console.WriteLine();

            Console.Read();  // Pause to show results
        }
    }


    // Simple class to use with the Dictionary operations above.
    // 3 properties for texture and filters
    // 1 constructor for easy of adding to collections
    // To string override for display
    class Golfer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Handicap { get; set; }

        public Golfer(string first,string last, int hand)
        {
            FirstName = first;
            LastName = last;
            Handicap = hand;
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName} - {Handicap}";
        }
    }
}
