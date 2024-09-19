using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class praticeExam2:  abstractTestClass
    {
        private int[,] bitMap = null;
        private int nextImageID = 1;
        public praticeExam2()
        {
            //Horizontal then vertical
            cells = new Point(5, 2);

            imageList = new List<imageObject>();

            this.cellWidth = 0;
            this.cellHeight = 0;

            this.screenWidth = 1920;
            this.screenHeight = 1080;
            this.verticalOffset = 200;

            this.smallMugsize = 28;
            this.largeMugsize = 36;

            this.targetsPerCell = 5;
            this.distractorsPerCell = 5;

            this.numberOfLargeTargetPerCell = 1;
            this.numberOfLargeLeftGap = 1;
            this.numberOfLargeRightGap = 1;

            this.bitMap = new int[this.screenWidth, this.screenHeight];

            createImageList();
        }


        public override void drawHeader(Graphics g, DateTime endingTime)
        {
            TimeSpan timeRemaining = endingTime - DateTime.Now;
            g.DrawString("Cancellation Test", new Font("Arial", 16), Brushes.Black, new Point(10, 10));
            g.DrawString("Ending Time : " + timeRemaining, new Font("Arial", 12), Brushes.Black, new Point(10, 30));
        }


        private void createImageList()
        {

            this.cellWidth = this.screenWidth / this.cells.X;
            this.cellHeight = this.screenHeight / this.cells.Y;

            double midCell = (double)this.cells.X / 2;



            int halfSmallMugSize = this.smallMugsize / 2;
            int halfLargeMugSize = this.largeMugsize / 2;


            //addImage()
            addImage(389, 691, 68, imageTypes.TargetLeft, 1);
            addImage(505, 591, 68, imageTypes.TargetRight, 1);
            addImage(691, 661, 54, imageTypes.DistractionRight, 1);
            addImage(604, 776, 68, imageTypes.DistractionLeft, 1);
            addImage(470, 811, 54, imageTypes.DistractionLeft, 2);
            addImage(368, 958, 68, imageTypes.DistractionLeft, 2);
            addImage(563, 961, 54, imageTypes.TargetRight, 2);
            addImage(626, 936, 54, imageTypes.TargetRight, 2);
            addImage(894, 535, 68, imageTypes.TargetRight, 3);
            addImage(1019, 658, 54, imageTypes.TargetLeft, 3);
            addImage(1083, 657, 54, imageTypes.DistractionLeft, 3);
            addImage(940, 790, 68, imageTypes.DistractionRight, 4);
            addImage(924, 960, 68, imageTypes.TargetRight, 4);
            addImage(1108, 965, 54, imageTypes.TargetLeft, 4);
            addImage(1090, 829, 68, imageTypes.DistractionLeft, 4);
            addImage(1167, 490, 54, imageTypes.DistractionRight, 3);
            addImage(1426, 491, 54, imageTypes.DistractionRight, 5);
            addImage(1605, 498, 54, imageTypes.TargetRight, 5);
            addImage(1682, 639, 68, imageTypes.DistractionLeft, 5);
            addImage(1419, 691, 68, imageTypes.TargetLeft, 5);
            addImage(1595, 780, 54, imageTypes.DistractionLeft, 6);
            addImage(1411, 861, 54, imageTypes.DistractionLeft, 6);
            addImage(1579, 933, 68, imageTypes.TargetRight, 6);
            addImage(1346, 978, 68, imageTypes.TargetRight, 6);





        }

        private void addImage(int x, int y, int imgSize, imageTypes imageType, int matrixLocation)
        {
            leftRightCenter side;
            if (matrixLocation % 5 < 3)
            {
                side = leftRightCenter.Left;
            }
            else if (matrixLocation % 5 == 3)
            {
                side = leftRightCenter.Center;
            }
            else
            {
                side = leftRightCenter.Right;
            }

            this.imageList.Add(new imageObject(this.nextImageID, imageType, new Point(x, y), imgSize, imgSize, side, matrixLocation));
            updateBitMap(x - imgSize / 2, x + imgSize / 2, y - imgSize / 2, y + imgSize / 2, this.nextImageID);
            this.nextImageID++;
        }
        private bool isOverlap(int x, int y, int size)
        {
            for (int i = x - size / 2; i < x + size / 2; i++)
            {
                for (int j = y - size / 2; j < y + size / 2; j++)
                {
                    if (this.bitMap[i, j] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void updateBitMap(int xLower, int xHigher, int yLower, int yHigher, int imageID)
        {
            for (int i = xLower; i < xHigher; i++)
            {
                for (int j = yLower; j < yHigher; j++)
                {
                    this.bitMap[i, j] = imageID;
                }
            }
        }

        override public int checkClick(Point p)
        {
            return this.bitMap[p.X, p.Y];
        }


    }
}
