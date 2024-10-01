using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    
    //Class Name : abstractTestClass
    //Purpose : This class is an abstract class that is used to define the parameters that are common with all the tests
    //          This class is used to define the parameters that are common with all the tests
    
    public abstract class abstractTestClass
    {
        //What do I need in the abstract class

        public int screenWidth { get; protected set; }
        public int screenHeight { get; protected set; }

        public int smallMugsize { get; protected set; }
        public int largeMugsize { get; protected set; }

        public int targetsPerCell { get; protected set; }
        public int distractorsPerCell { get; protected set; }

        public double smallImageSize { get; protected set; }

        public int numberOfLargeTargetPerCell { get; protected set; }
        public int numberOfLargeLeftGap { get; protected set; }
        public int numberOfLargeRightGap { get; protected set; }


        public int cellHeight { get; protected set; }
        public int cellWidth { get; protected set; }

        //The offset is used to set the header of t he frame to adjust for different 
        public int verticalOffset { get; protected set; }



        public List<mugObject> imageList { get; protected set; }
        public Point cells { get; protected set; }

        public abstract void drawHeader(Graphics g, DateTime endingTime);

        public int getMatrixLocation(int x, int y)
        {

            int row = y / cellHeight;
            int col = x / cellWidth;

            return row * this.cells.X + col;
        }

        public int getMatrixLocation(int ImageID)
        {
            Point imgCenter = this.imageList[ImageID - 1].imageCenter;
            return getMatrixLocation(imgCenter.X, imgCenter.Y);
        }

        public int numberOfHorizontalCells()
        {
            return this.cells.X;
        }

        public int numberOfVerticalCells()
        {
            return this.cells.Y;
        }

        public abstract int checkClick(Point p);
    }

    public enum imageTypes
    {
        DistractionLeft,
        DistractionRight,
        TargetLeft,
        TargetRight
    }

    public enum leftRightCenter
    {
        Left,
        Right,
        Center
    }
}
