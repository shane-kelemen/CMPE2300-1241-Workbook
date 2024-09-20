using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Equals
{
    internal class Program
    {
        // List to hold all our Golfers
        static List<Golfer> golfers = new List<Golfer>();
        // Random object to be used for creating Golfer objects
        static Random rng = new Random();
        // List of first names for random Golfer generation
        static List<string> firsts = new List<string>() { "John", "Sally", "Henry", "Jane", "Dolly", "Frank" };
        // List of last names for random Golfer generation
        static List<string> lasts = new List<string>() { "Joe", "Doe", "Smith", "Ford", "Thompson", "Douglas" };
        
        static void Main(string[] args)
        {
            // Generate 100 Golfers with randomized information
            for (int i = 0; i < 100; ++i)
            {
                golfers.Add(new Golfer(firsts[rng.Next(firsts.Count)],
                                        lasts[rng.Next(lasts.Count)],
                                        rng.Next(-10, 51),
                                        rng.NextDouble() * 18 + 60,
                                        rng.NextDouble() * 100 + 120));
            }

            // Determine whether a golfer with a particular handicap exists in the 
            // collection of Golfers.
            int findHandicap = 15;
            Golfer test = new Golfer("Test", "Test", findHandicap, 43, 56);
            List<Golfer> foundGolfer = new List<Golfer>();
            // Note:  Contains uses Equals() as defined in the Golfer class to determine
            //        whether there is a match in the collection.
            if (golfers.Contains(test))
                Console.WriteLine("A matching golfer was found!");
            else
                Console.WriteLine("No matching golfers were found!");

            // The following calls the version of Equals that is defined in the string
            // data type.
            if (golfers[5].FirstName.Equals(golfers[10].FirstName))
            {
                // do stuff
            }

            // The following calls the version of Equals that is defined in the Golfer 
            // data type
            if (golfers[5].Equals(golfers[10]))
            {
                // do stuff
            }

            // Determine whether a golfer with matching first and last names exists in the 
            // collection of Golfers.
            int index = -1;
            Golfer sameName = new Golfer("John", "Doe", 34, 20, 12);
            // IndexOf() uses Equals in the Golfer class to compare the supplied Golfer
            // to each Golfer in the collection.  When the first match is found, the index
            // of the match is returned.
            if ((index = golfers.IndexOf(sameName)) != -1)
                Console.WriteLine($"A matching golfer was found at spot {index}!");
            else
                Console.WriteLine("No matching golfers were found!");

            Console.WriteLine();
            Console.ReadKey();
        }

        class Golfer
        {
            string _firstName;
            string _lastName;
            int _handicap;
            double _height;
            double _weight;

            public string FirstName
            {
                get { return _firstName; }
            }

            public string LastName
            {
                get { return _lastName; }
            }

            
            public int Handicap
            {
                get { return _handicap; }

                // If the supplied values is outside the defined range below, throw appropriate
                // expection message.
                private set
                {
                    if (value < -10)
                        throw new ArgumentException("That is not a possible handicap!");

                    if (value > 50)
                        throw new ArgumentException("You should quit golf!");

                    _handicap = value;
                }
            }

            public double Height { get; private set; }
            public double Weight { get; private set; }

            // Standard constructor.  Note that where Property sets are available,
            // the Property is used for assignment.  Best practice.
            // Another best practice is to put any sanitizing of input values into
            // manual properties to keep your constructors clean. 
            public Golfer(string f, string l, int h, double he, double w)
            {
                _firstName = f;
                _lastName = l;
                Handicap = h;
                Height = he;
                Weight = w;
            }

            public override bool Equals(object obj)
            {
                // The first thing that must be completed is to determine whether we are dealing 
                // with two objects that are they same type, similar to checking for the correct 
                // information being supplied to a thread.

                // If the 'is' operator returns true, the ! inverts the logic and statement is skipped.
                // At the same time, the input object is cast to a Golfer and its heap location placed
                // in arg.
                if (!(obj is Golfer arg)) return false;
                
                // Alternately, you may cast the object either using an explicit cast,
                // or the 'as' operator.  You will find these in legacy code created before the 
                // combination code above was introduced into the C# language.
                // Golfer arg = (Golfer)obj;
                // Golfer arg = obj as Golfer;

                // Uncomment the next line to have the Contains() test in main find a golfer in the set
                // using the handicap.
                // Golfers will be considered the same if they have the same handicap.
                // return Handicap.Equals(arg.Handicap);

                // Golfers will be considered Equal if they have the same first and last names
                return FirstName.Equals(arg.FirstName) && LastName.Equals(arg.LastName);
            }
        }
    }
}
