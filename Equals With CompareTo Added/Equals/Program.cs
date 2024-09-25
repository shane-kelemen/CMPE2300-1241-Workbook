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

            // Use the static enumeration value in the class to indicate how we wish 
            // to sort the collection
            Golfer.SortBy = SortType.Name;
            golfers.Sort();                 // Sort the collection.  Note: this sorts 
                                            // the collection in place, it does not create
                                            // a new collection.
            // Display appropriate information for our collection of golfers to veify the 
            // sort is functioning as expected.
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.FirstName} {g.LastName}");
            Console.WriteLine();

            Golfer.SortBy = SortType.DescendingHandicap;    // Change sorting criteria
            golfers.Sort();                                 // Resort collection
            // Display the collection again to verify new sorting criteria
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Handicap} - {g.FirstName} {g.LastName}");
            Console.WriteLine();

            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// Enumeration used for indicating the sorting criteria choices.  Used in CompareTo()
        /// of the Golfer class.
        /// </summary>
        public enum SortType { Name, DescendingHandicap, Weight, Height }

        /// <summary>
        /// The golfer class is encapsulating some common characteristics describing golfers
        /// </summary>
        class Golfer : IComparable
        {
            string _firstName;      // Golfer first name
            string _lastName;       // Golfer last name
            int _handicap;          // Golfer handicap - average strokes over par in last 10 games
            double _height;         // Golfer height in centimetres
            double _weight;         // Golfer weight in kilograms

            /// <summary>
            /// Provide the user with the golfer's first name.
            /// </summary>
            public string FirstName
            {
                get { return _firstName; }

                // set not provided so user may not provide new golfer first name
            }

            /// <summary>
            /// Provide the user with the golfer's last name
            /// </summary>
            public string LastName
            {
                get { return _lastName; }

                // set not provided so user may not provide new golfer last name
            }

            /// <summary>
            /// Provide the user with the golfer's Handicap.
            /// </summary>
            public int Handicap
            {
                get { return _handicap; }

                // If the supplied values is outside the defined range below, throw appropriate
                // expection message.
                // Making set private provides the error handling we require in the property as 
                // is best practice, but it is hidden from users of the class
                private set
                {
                    if (value < -10)
                        throw new ArgumentException("That is not a possible handicap!");

                    if (value > 50)
                        throw new ArgumentException("You should quit golf!");

                    _handicap = value;
                }
            }

            /// <summary>
            /// Automatic property allowing full access to the user for golfer height
            /// </summary>
            public double Height { get; private set; }

            /// <summary>
            /// Automatic property allowing full access to the user for golfer weight
            /// </summary>
            public double Weight { get; private set; }

            /// <summary>
            /// Automatic property allowing full access to the user for changing sort criteria
            /// </summary>
            public static SortType SortBy { get; set; }


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

            /// <summary>
            /// Overriden equals to change the equality criteria from "identity semantics", meaning 
            /// checking the addresses of the two objects, to "value semantics", meaning using 
            /// criteria defined by the coder based on values contained in the class objects.
            /// </summary>
            /// <param name="obj">The object to be tested for equality</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                // The first thing that must be completed is to determine whether we are dealing 
                // with two objects that are they same type, similar to checking for the correct 
                // information being supplied to a thread.

                // If the 'is' operator returns true, the ! inverts the logic and statement is skipped.
                // At the same time, the input object is cast to a Golfer and its heap location placed
                // in arg.
                if (!(obj is Golfer arg)) 
                    return false;       // If the objects are not the same type, obviously they are not equal
                
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
                // Note:  It is best practice to use the underlying Equals of the object types being
                //        compared if it exists.
                return FirstName.Equals(arg.FirstName) && LastName.Equals(arg.LastName);
            }

            /// <summary>
            /// CompareTo() required to satisfy inclusion of the IComparable interface.  It is 
            /// used externally by many methods, such as Sort(), when two objects must be compared
            /// to determine whether one or the other is "smaller", "larger", or "equal" in a 
            /// single operation.  Standard implementation is to cause ascending sorting, but this may be 
            /// changed.
            /// </summary>
            /// <param name="obj">The object to be used for comparison</param>
            /// <returns>int: -'ve (invoking object smaller), 0 (objects equal), +'ve (invoking object larger)</returns>
            /// <exception cref="ArgumentException">If a different type than the invoking type is provided</exception>
            public int CompareTo(object obj)
            {
                // If the 'is' operator returns true, the ! inverts the logic and statement is skipped.
                // At the same time, the input object is cast to a Golfer and its heap location placed
                // in arg. 
                if (!(obj is Golfer arg))
                    // If the objects are not the same type, we cannot compare them.  Always throw an
                    // exception in this situation.
                    throw new ArgumentException("An incorrect type was supplied!");

                // Many ways to do the following, but the following is straight-forward when many
                // different types of sorts may be implemented/
                int result = 0;

                // Choose sort type by value stored in enumeration variable.  
                switch(SortBy)
                {
                    // Example of two-layer sort, in this case first name then last name, or
                    // otherwise stated, last name WITHIN first name
                    case SortType.Name:
                        result = FirstName.CompareTo(arg.FirstName);  // level one sort

                        if (result.Equals(0))  // level two sort only needed if level one is equal
                            result = LastName.CompareTo(arg.LastName);
                        break;

                    // Example of a single layer sort, but not respecting the standard way CompareTo()
                    // is expected to sort collection items.  Have a very good reason for doing this.
                    // Note:  Reversing the sort merely requires switching which object invokes the
                    // underlying CompareTo()
                    // Note:  It is best practice to use the underlying CompareTo() for the internal types
                    //        being compared if it exists. Other methods of calculating the return value
                    //        may be used if it does not.
                    case SortType.DescendingHandicap:
                        result = arg.Handicap.CompareTo(Handicap);
                        break;
                }

                // Default expected functionality... 
                // return -'ve, 0, or +'ve value to the user to indicate smaller, equal, and larger respectively.
                return result;
            }
        }
    }
}
