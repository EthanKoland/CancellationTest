using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class randomTest : abstractTestClass
    {

        //private int[,] bitMap = null;
        public randomTest(int screenWidth = 1000, int screenHeight = 600, int verticalOffset = 0)
        {
            //Horizontal then vertical
            cells = new Point(5, 2);

            imageList = new List<mugObject>();

            this.cellWidth = 0;
            this.cellHeight = 0;

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.verticalOffset = verticalOffset;

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


        /*public override void drawHeader(Graphics g, DateTime endingTime)
        {
            TimeSpan timeRemaining = endingTime - DateTime.Now;
            g.DrawString("Cancellation Test", new Font("Arial", 16), Brushes.Black, new Point(10, 10));
            g.DrawString("Ending Time : " + timeRemaining, new Font("Arial", 12), Brushes.Black, new Point(10, 30));
        }*/


        private void createImageList()
        {

            this.cellWidth = this.screenWidth / this.cells.X;
            this.cellHeight = this.screenHeight / this.cells.Y;

            double midCell = (double)this.cells.X / 2;

            Random random = new Random(29);

            bool isLeft = true;
            for (int row = 0; row < this.cells.Y; row++)
            {
                //iterate through the number of horizontal grids
                for (int col = 0; col < this.cells.X; col++)
                {
                    leftRightCenter side = col < midCell ? leftRightCenter.Left : col == midCell ? leftRightCenter.Center : leftRightCenter.Right;

                    int matrixLocation = row * this.cells.Y + col;

                    //create the target images
                    int largeRemaining = this.numberOfLargeTargetPerCell;
                    for (int t = 0; t < this.targetsPerCell; t++)
                    {
                        imageTypes type = isLeft ? imageTypes.TargetLeft : imageTypes.TargetRight;
                        isLeft = !isLeft;

                        int imgSize = largeRemaining > 0 ? this.largeMugsize : this.smallMugsize;
                        largeRemaining--;

                        int halfImgSize = imgSize / 2;

                        //initalize 
                        int x;
                        int y;

                        int cellXMin = Math.Max(imgSize / 2, col * cellWidth);
                        int cellXMax = Math.Min(screenWidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenHeight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);
                            if (!isOverlap(x, y, imgSize))
                            {
                                Console.WriteLine("Overlap");
                            }
                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.imageList.Count + 1;

                        Console.WriteLine("Image ID : " + nextImageID);



                        this.imageList.Add(new mugObject(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
                        updateBitMap(x - halfImgSize, x + halfImgSize, y - halfImgSize, y + halfImgSize, nextImageID);
                    }

                    //Create the left distractors
                    int largeLeftRemaining = this.numberOfLargeLeftGap;
                    for (int d = 0; d < this.distractorsPerCell; d++)
                    {
                        imageTypes type = isLeft ? imageTypes.DistractionLeft : imageTypes.DistractionRight;
                        isLeft = !isLeft;

                        int imgSize = largeLeftRemaining > 0 ? this.largeMugsize : this.smallMugsize;
                        largeLeftRemaining--;

                        int halfImgSize = imgSize / 2;

                        //initalize 
                        int x;
                        int y;

                        int cellXMin = Math.Max(imgSize / 2, col * cellWidth);
                        int cellXMax = Math.Min(screenWidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenHeight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);
                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.imageList.Count + 1;

                        this.imageList.Add(new mugObject(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
                        updateBitMap(x - halfImgSize, x + halfImgSize, y - halfImgSize, y + halfImgSize, nextImageID);
                    }

                    //Create the right distractors
                    int largeRightRemaining = this.numberOfLargeRightGap;
                    for (int d = 0; d < this.distractorsPerCell; d++)
                    {
                        imageTypes type = isLeft ? imageTypes.DistractionLeft : imageTypes.DistractionRight;
                        isLeft = !isLeft;

                        int imgSize = largeRightRemaining > 0 ? this.largeMugsize : this.smallMugsize;
                        largeRightRemaining--;

                        int halfImgSize = imgSize / 2;

                        //initalize 
                        int x;
                        int y;

                        int cellXMin = Math.Max(imgSize / 2, col * cellWidth);
                        int cellXMax = Math.Min(screenWidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenHeight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);

                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.imageList.Count + 1;

                        this.imageList.Add(new mugObject(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
                        updateBitMap(x - halfImgSize, x + halfImgSize, y - halfImgSize, y + halfImgSize, nextImageID);
                    }

                }
            }
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

        

    }
}
