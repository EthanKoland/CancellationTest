using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    
    //Class Name : abstractTestClass
    //Purpose : This class is an abstract class that is used to define the parameters that are common with all the test. 
    //          By using abstract classes the amount of reused code increases
    
    public abstract class abstractTestClass
    {
        //What do I need in the abstract class

        public int screenWidth { get; protected set; }
        public int screenHeight { get; protected set; }

        public int smallMugsize { get; protected set; }
        public int largeMugsize { get; protected set; }

        public int targetsPerCell { get; protected set; }
        public int distractorsPerCell { get; protected set; }


        public int numberOfLargeTargetPerCell { get; protected set; }
        public int numberOfLargeLeftGap { get; protected set; }
        public int numberOfLargeRightGap { get; protected set; }


        public int cellHeight { get; protected set; }
        public int cellWidth { get; protected set; }

        //The offset is used to set the header of t he frame to adjust for different 
        public int verticalOffset { get; protected set; }

        public bool isPratice { get; protected set; } = true;



        public List<mugObject> imageList { get; protected set; }
        public Point cells { get; protected set; }

        



        public abstract int checkClick(Point p);
    }

    //A enum is used to classify each of the images that are used in the test. Th enum is used to make the code more readable
    public enum imageTypes
    {
        DistractionLeft,
        DistractionRight,
        TargetLeft,
        TargetRight
    }

    //A enum is used to classify the location of the text on the screen. The enum is used to make the code more readable
    public enum leftRightCenter
    {
        Left,
        Right,
        Center
    }
}
