using System;
using System.Collections.Generic;

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

            // Display appropriate information for our collection of golfers to verify the 
            // sort is functioning as expected.
            Console.WriteLine("Sort by ascending first name then ascending last name :");
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.FirstName} {g.LastName}");
            Console.WriteLine();

            Golfer.SortBy = SortType.DescendingHandicap;    // Change sorting criteria
            golfers.Sort();                                 // Resort collection
            // Display the collection again to verify new sorting criteria
            Console.WriteLine("Sort by descending handicap :");
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Handicap} - {g.FirstName} {g.LastName}");
            Console.WriteLine();


            // The following section is using different Comparison compliant static methods within the
            // class to determine the sorting type.
            Console.WriteLine("Sort by as ending first name then ascending last name:");
            golfers.Sort(Golfer.SortByHeight);
            Console.WriteLine("Sort by ascending height :");
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Height} - {g.FirstName} {g.LastName}");
            Console.WriteLine();

            Console.WriteLine("Sort by descending height :");
            golfers.Sort(Golfer.SortByDescendingHeight);
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Height} - {g.FirstName} {g.LastName}");
            Console.WriteLine();

            // Use the Comparer class defined after the Golfer class to sort the golfers.  This type
            // of class is not often needed with the inclusion of lambda expressions (to be explored
            // in the collections section of the course), unless the same lambda expression is needed
            // on multiple occasions.
            GolferComparer golferComp = new GolferComparer();   // Create comparer class instance
            golfers.Sort(golferComp);                           // Provide comparer to sort collection
            Console.WriteLine("Sort by ascending handicap :");
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Handicap} - {g.FirstName} {g.LastName}");
            Console.WriteLine();


            // The following section is a prelude to using a comparison compliant lambda expression
            // to provide the sorting criteria of the collection elements.
            golfers.Sort((left, right) => left.Weight.CompareTo(right.Weight));
            Console.WriteLine("Sort by ascending weight :");
            foreach (Golfer g in golfers)
                Console.WriteLine($"{g.Weight} - {g.FirstName} {g.LastName}");
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
            // double _height;      // Golfer height in centimetres - decioded to replace below with
                                    //    automatic property.
            // double _weight;      // Golfer weight in centimetres - decioded to replace below with
                                    //    automatic property.

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

                // set made private so user may not provide new golfer last name, but the property 
                // may still be used internally for proper leveraging.
                private set
                {
                    // Ensure the first letter is upper case and all the rest of the letters are
                    // lower case.
                    _lastName = "";
                    for (int index = 0; index < value.Length; ++index)
                        _lastName += index == 0 ? char.ToUpper(value[0]) : char.ToLower(value[index]);
                    
                }
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
            /// Automatic property allowing access to the user for golfer height
            /// </summary>
            public double Height { get; private set; } // private set does not allow user to change value

            /// <summary>
            /// Automatic property allowing full access to the user for golfer weight
            /// </summary>
            public double Weight { get; set; }

            /// <summary>
            /// Automatic property allowing full access to the user for changing sort criteria
            /// </summary>
            public static SortType SortBy { get; set; }  // This will default to the first defined
                                                         // value if it is not specified elsewhere.
                                                         // Not a good idea to initialize in instance
                                                         // constructor as it will reset every time
                                                         // a new instance is created.

            /// <summary>
            /// Best place to intitialize static variables / properties.
            /// </summary>
            static Golfer()
            {
                SortBy = SortType.Name;
            }

            /// <summary>
            /// Standard constructor.  Note that where Property sets are available,
            /// the Property is used for assignment.  Best practice.
            /// Another best practice is to put any sanitizing of input values into
            /// manual properties to keep your constructors clean.
            /// </summary>
            /// <param name="f">First name of the golfer</param>
            /// <param name="l">Last name of the golfer</param>
            /// <param name="h">Handicap name of the golfer</param>
            /// <param name="he">Height name of the golfer</param>
            /// <param name="w">Weight name of the golfer</param>
            public Golfer(string f, string l, int h, double he, double w)
            {
                _firstName = f;
                LastName = l;
                Handicap = h;
                Height = he;
                Weight = w;
            }

            /// <summary>
            /// The default constructor sets standard values to the instance data members (fields).
            /// To continue the best practice of leveraging, note how the default constructor
            /// calls the explicit constructor through the "this" keyword.
            /// </summary>
            public Golfer() : this("DefaultFirst", "DefaultLast", 48, 72, 180) { }

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

            /// <summary>
            /// A comparison compliant method sorting golfers by ascending height.
            /// </summary>
            /// <param name="left">Element that comes first in the collection</param>
            /// <param name="right">Element that comes second in the collection</param>
            /// <returns></returns>
            public static int SortByHeight(Golfer left, Golfer right)
            {
                if (left == null && right == null)  // If both are null, consider them equal
                    return 0;
                if (left == null)  // Consider first element in collection smaller if only one null
                    return -1;
                if (right == null) // Consider second element in collection smaller if only one null
                    return 1;      

                // Use CompareTo() of double type to determine if golfers equal or which is "smaller"
                return left.Height.CompareTo(right.Height);
            }

            /// <summary>
            /// A comparison compliant method sorting golfers by descendiong height.
            /// </summary>
            /// <param name="left">Element that comes first in the collection</param>
            /// <param name="right">Element that comes second in the collection</param>
            /// <returns></returns>
            public static int SortByDescendingHeight(Golfer left, Golfer right)
            {
                // The following block of code is the same as ascending sort but reversed
                //if (left == null && right == null)  // If both are null, consider them equal
                //    return 0;
                //if (left == null)  // Consider first element in collection larger if only one null
                //    return 1;
                //if (right == null) // Consider second element in collection larger if only one null
                //    return -1;

                // Use CompareTo() of double type to determine if golfers equal or which is "larger"
                // Note the reversal of which element is used to invoke the CompareTo()
                //return right.Height.CompareTo(left.Height);


                // The following eliminates the need for the code above by leveraging the already 
                // existing SortByHeight.  Multiplying by -1 effectively reverses the order of the sort.
                //return SortByHeight(left, right) * -1;


                // The following also eliminates the need for the explicit sort code through leveraging,
                // but is slightly mopre elegant as it eliminates the need for the multiplier. 
                return SortByHeight(right, left);
            }
        }


        /// <summary>
        /// Comparer class example for expending functionality of class where source code is not 
        /// available.
        /// </summary>
        class GolferComparer : IComparer<Golfer>
        {
            /// <summary>
            /// Default constructor... somewhat obviously
            /// </summary>
            public GolferComparer()
            {
                // Just like any class, you may initialize any needed fields here.
                // Examples could include functionality similar to the enumeration type
                // used in the CompareTo() method of the Golfer class.
            }

            /// <summary>
            /// Method required to satisfy IComparer interface
            /// </summary>
            /// <param name="left">Element that comes first in the collection</param>
            /// <param name="right">Element that comes second in the collection</param>
            /// <returns></returns>
            public int Compare(Golfer left, Golfer right)
            {
                // If leveraging of existing class code is not possible, use the same pattern as
                // with static comparison compliant delegate methods.

                // If possible, choose to leverage off of existing sorting criteria of the exisiting 
                // class.  In this case we will reverse the existing DescendingHandicap of CompareTo()
                Golfer.SortBy = SortType.DescendingHandicap;  // Set the compare type via static 
                                                              // enumeration field in Golfer class.
                                                              // Other possibilities can be used...
                // Reverse the sort to be ascending handicap.
                return right.CompareTo(left);
            }
        }
    }
}
