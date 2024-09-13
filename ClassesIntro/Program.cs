using System.Drawing;

namespace ClassesIntro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sphere myBall = new Sphere();
            //myBall.X = 34;  // This line must be commented out if you choose to use the private
                              // modifier on the set of the X property in the Sphere class, or if
                              // you eliminate the set entirely.

            // These lines are just for displaying the class data using its methods / properties
            // You will need to adjust the calls to use the different features in the class below.
            Console.WriteLine($"x : {myBall.X} - y : {myBall.Y} - Rad : {myBall.GetRadius()} - Color : {myBall.GetColour()}");
            Console.WriteLine("Volume : " + myBall.Volume.ToString("F3"));

            List<int> numbers = new List<int>();
            for (int i = 0; i < 20; ++i)
                numbers.Add(i * i);

            myBall.MyInts = numbers;

            Console.WriteLine("Sum of internal List<int> : " + myBall.Sum);
        }
    }


    // Generally, the organization of a class is as follows:
    // Data Members
    // Properties
    // Constructors
    // Other Methods
    // This list will be expanded as we add new features into our classes
    class Sphere
    {
        // Data Members - These must be commented even when it seems trivial
        
        private int _x;             // location xCoordinate 
        //private int _y;           // location yCoordinate - commented out when we replaced it
                                    //                        with an automatic property.
        private double _radius;     // radius of the sphere
        private Color _color;       // colour of the sphere
        private string _name;       // added the class after the demo to show modification
                                    //      of data leaving the class via the get of the Name property.
                                    //      In this case there would be no way for the user to retrieve 
                                    //      the raw value of _name.
        private int _sum;           // added the class after the demo to demonstrate more complex 
                                    //      objects accepted as "value" in the set. Stores sum of 
                                    //      List<int> input via property MyInts
        List<int> _values;          // added the class after the demo to demonstrate binding a more
                                    //      complex object (reference type) to a class data member.
                                    //      Set a breakpoint somewhere in Main so that you may examine the 
                                    //      class object and witness the internally bound List<int>

        // Properties - Look at these as if you are a user of the class.  The get is the user 
        //              retrieving information from the class.  The set is the user providing
        //              information to the class for processing / storage.

        // This property was added the class after the demo to show 
        // the retrievable value of a data member restricted to a 
        // particular format.
        public string Name
        {
            get
            {
                return _name.ToUpper();
            }
        } 

        // This property allows access to the unaltered value of the xCoordinate,
        // but restricts the legal values of new assigned values to the xCoordinate.
        // An alternate philosophy might be to correct incorrect input.  There are
        // convincing arguments on both sides to be had.  Be able to back up your choice.
        public int X
        {
            get { return _x; }
            set
            {
                if (value < 0)
                    throw new Exception("The x coordinate must be a positive integer!");

                _x = value;
            }
        }

        // This property was added the class after the original demo to show more complex
        // data types and operations occurring within the property.  Not that two data members
        // were affected, and also that "value" may be interated through as it is representative
        // of the List<int> supplied from outside the class.
        public List<int> MyInts
        {
            set
            {
                foreach (int num in value)
                    _sum += num;

                _values = value;
            }
        }

        // Allows us to check that the sum was indeed set using the property above.
        public int Sum
        {
            get { return _sum; }
        }

        //public int Y
        //{
        //    get { return _y; }
        //    set { _y = value; }
        //}

        // This automatic property replaced the manual property above.
        // This is a reasonable design decision when the manual property
        // is doing nothing except straight retrieval and assignment.
        public int Y { get; private set; }

        // This property was added to show that properties are really
        // just special methods, and that they do not need to be directly
        // tied to a single data member for assignment.
        public double Volume
        {
            get { return 4 * Math.PI * Math.Pow(_radius, 3) / 3; }
        }


        // Constructors
        // This first constructor is the most complex in this class.
        // It is an excellent candidate for leveraging by other, less
        // fully defined constructors.
        public Sphere(Color c, int x = 100, int y = 100, double r = 6.0)
        {
            X = x;          // Note that when a property exists for assignment (set),
                            // it is best practice to put any limiting code in the property
                            // and then use the property inside the class for consistency,
                            // and to make sure the limiting code need only reside in one place.
                            // This is a type of code leveraging.
            Y = y;
            _radius = r;
            _color = c;

            _name = "Jason";
        }

        // This is a default constructor, meaning it takes no arguments.
        // If no constructors for the class are defined at all, then the
        // default constructor is provided for free, and will cause all
        // the data members to be initialized to their "false equivalents".
        // Thus, bools = false, numbers = 0, references = null, etc.
        public Sphere() : this(Color.Aqua, 5, 10) // The use of "this" allows one constructor 
                                                  // to leverage another so that initializing
                                                  // code need only be written in one location
        {
            Y = 500;  // After the object has been built and initialized via leveraging,
                      // the body of the constructor that was doing the leveraging will 
                      // still be executed.
        }


        // The following methods are the old way of accessing class data members,
        // basically the version of "get/set" that is needed in languages that do
        // not have Properties the way C# does.  They are called accessors and mutators.
        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return Y;
        }

        public double GetRadius()
        {
            return _radius;
        }

        public Color GetColour()
        {
            return _color;
        }


        // The following mutator method asks the class instance to change a data member value
        public void SetX(int x)
        {
            if (x < 0)
                throw new Exception("The x coordinate must be a positive integer!");

            _x = x;
        }
    }
}
