using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class assesmentExam:  abstractTestClass
    {
        private int[,] bitMap = null;
        private int nextImageID = 1;
        public assesmentExam()
        {
            //Horizontal then vertical
            cells = new Point(5, 2);

            imageList = new List<mugObject>();

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
            addImage(85, 135, 54, imageTypes.DistractionLeft, 1);
            addImage(200, 120, 68, imageTypes.DistractionRight, 1);
            addImage(80, 260, 54, imageTypes.TargetLeft, 1);
            addImage(183, 255, 68, imageTypes.TargetRight, 1);
            addImage(95, 360, 68, imageTypes.DistractionRight, 1);
            addImage(80, 474, 68, imageTypes.DistractionLeft, 1);
            addImage(133, 585, 68, imageTypes.TargetLeft, 2);
            addImage(112, 753, 54, imageTypes.TargetRight, 2);
            addImage(95, 859, 54, imageTypes.DistractionRight, 2);
            addImage(85, 966, 68, imageTypes.DistractionLeft, 2);
            addImage(200, 385, 68, imageTypes.DistractionLeft, 1);
            addImage(193, 485, 68, imageTypes.TargetLeft, 1);
            addImage(200, 619, 54, imageTypes.DistractionRight, 2);
            addImage(210, 730, 68, imageTypes.DistractionRight, 2);
            addImage(220, 866, 68, imageTypes.DistractionLeft, 2);
            addImage(200, 966, 68, imageTypes.DistractionRight, 2);
            addImage(304, 193, 68, imageTypes.DistractionLeft, 1);
            addImage(281, 283, 54, imageTypes.DistractionLeft, 1);
            addImage(318, 385, 54, imageTypes.DistractionRight, 1);
            addImage(298, 503, 54, imageTypes.TargetLeft, 1);
            addImage(298, 653, 68, imageTypes.TargetRight, 2);
            addImage(318, 811, 54, imageTypes.DistractionLeft, 2);
            addImage(368, 911, 68, imageTypes.TargetLeft, 2);
            addImage(304, 1018, 54, imageTypes.DistractionLeft, 2);
            addImage(416, 1001, 68, imageTypes.DistractionRight, 2);
            addImage(403, 128, 54, imageTypes.TargetRight, 1);
            addImage(403, 255, 54, imageTypes.DistractionRight, 1);
            addImage(416, 495, 54, imageTypes.DistractionRight, 1);
            addImage(410, 619, 54, imageTypes.DistractionLeft, 2);
            addImage(403, 730, 54, imageTypes.TargetLeft, 2);
            addImage(508, 128, 54, imageTypes.DistractionRight, 3);
            addImage(503, 235, 68, imageTypes.DistractionLeft, 3);
            addImage(501, 340, 68, imageTypes.TargetLeft, 3);
            addImage(543, 440, 54, imageTypes.TargetRight, 3);
            addImage(543, 540, 68, imageTypes.DistractionLeft, 4);
            addImage(578, 618, 54, imageTypes.TargetRight, 4);
            addImage(514, 711, 54, imageTypes.DistractionLeft, 4);
            addImage(501, 804, 54, imageTypes.DistractionLeft, 4);
            addImage(514, 901, 54, imageTypes.TargetLeft, 4);
            addImage(514, 1011, 54, imageTypes.DistractionRight, 4);
            addImage(602, 128, 68, imageTypes.TargetRight, 3);
            addImage(602, 228, 68, imageTypes.DistractionRight, 3);
            addImage(581, 335, 68, imageTypes.DistractionLeft, 3);
            addImage(641, 440, 68, imageTypes.DistractionRight, 3);
            addImage(676, 523, 54, imageTypes.DistractionLeft, 3);
            addImage(700, 618, 68, imageTypes.TargetLeft, 4);
            addImage(651, 711, 68, imageTypes.DistractionLeft, 4);
            addImage(613, 830, 68, imageTypes.TargetRight, 4);
            addImage(613, 966, 54, imageTypes.DistractionRight, 4);
            addImage(683, 171, 54, imageTypes.TargetLeft, 3);
            addImage(679, 271, 54, imageTypes.DistractionLeft, 3);
            addImage(739, 435, 68, imageTypes.TargetLeft, 3);
            addImage(787, 506, 68, imageTypes.DistractionRight, 3);
            addImage(787, 681, 54, imageTypes.DistractionRight, 4);
            addImage(749, 781, 68, imageTypes.DistractionRight, 4);
            addImage(722, 881, 54, imageTypes.TargetLeft, 4);
            addImage(700, 1001, 68, imageTypes.DistractionLeft, 4);
            addImage(781, 124, 54, imageTypes.DistractionLeft, 3);
            addImage(775, 255, 54, imageTypes.DistractionRight, 3);
            addImage(775, 335, 54, imageTypes.DistractionLeft, 5);
            addImage(879, 140, 68, imageTypes.TargetLeft, 5);
            addImage(905, 240, 54, imageTypes.DistractionRight, 5);
            addImage(857, 340, 54, imageTypes.TargetLeft, 5);
            addImage(867, 423, 68, imageTypes.DistractionRight, 5);
            addImage(885, 506, 54, imageTypes.TargetLeft, 5);
            addImage(847, 603, 54, imageTypes.TargetRight, 6);
            addImage(879, 675, 54, imageTypes.DistractionLeft, 6);
            addImage(847, 766, 54, imageTypes.TargetLeft, 6);
            addImage(820, 875, 54, imageTypes.DistractionLeft, 6);
            addImage(787, 981, 68, imageTypes.DistractionRight, 4);
            addImage(992, 135, 54, imageTypes.TargetRight, 5);
            addImage(992, 260, 68, imageTypes.DistractionLeft, 5);
            addImage(943, 340, 54, imageTypes.DistractionRight, 5);
            addImage(965, 413, 68, imageTypes.DistractionLeft, 5);
            addImage(965, 495, 68, imageTypes.DistractionLeft, 5);
            addImage(965, 603, 68, imageTypes.DistractionRight, 6);
            addImage(954, 703, 54, imageTypes.DistractionRight, 6);
            addImage(905, 830, 54, imageTypes.DistractionRight, 6);
            addImage(935, 981, 54, imageTypes.TargetLeft, 6);
            addImage(1090, 155, 54, imageTypes.DistractionLeft, 5);
            addImage(1090, 240, 68, imageTypes.DistractionRight, 5);
            addImage(1051, 395, 54, imageTypes.DistractionRight, 5);
            addImage(1063, 495, 68, imageTypes.TargetRight, 5);
            addImage(1075, 595, 68, imageTypes.DistractionLeft, 6);
            addImage(1034, 675, 68, imageTypes.TargetLeft, 6);
            addImage(1090, 746, 68, imageTypes.DistractionRight, 6);
            addImage(1003, 775, 54, imageTypes.DistractionRight, 6);
            addImage(977, 858, 68, imageTypes.DistractionLeft, 6);
            addImage(1063, 846, 68, imageTypes.TargetRight, 6);
            addImage(1051, 981, 68, imageTypes.DistractionLeft, 6);
            addImage(1192, 155, 68, imageTypes.TargetLeft, 7);
            addImage(1149, 313, 68, imageTypes.DistractionRight, 7);
            addImage(1206, 255, 54, imageTypes.DistractionRight, 7);
            addImage(1149, 395, 68, imageTypes.TargetLeft, 7);
            addImage(1234, 355, 68, imageTypes.DistractionRight, 7);
            addImage(1161, 503, 68, imageTypes.DistractionLeft, 7);
            addImage(1161, 575, 68, imageTypes.TargetRight, 8);
            addImage(1161, 666, 54, imageTypes.DistractionRight, 8);
            addImage(1206, 746, 68, imageTypes.DistractionRight, 8);
            addImage(1149, 881, 68, imageTypes.TargetLeft, 8);
            addImage(1149, 981, 68, imageTypes.DistractionLeft, 8);
            addImage(1247, 820, 68, imageTypes.TargetLeft, 8);
            addImage(1257, 975, 68, imageTypes.DistractionLeft, 8);
            addImage(1355, 990, 54, imageTypes.DistractionLeft, 8);
            addImage(1355, 881, 54, imageTypes.TargetRight, 8);
            addImage(1487, 1011, 54, imageTypes.DistractionRight, 10);
            addImage(1487, 911, 68, imageTypes.TargetRight, 10);
            addImage(1585, 981, 68, imageTypes.DistractionRight, 10);
            addImage(1683, 995, 54, imageTypes.DistractionLeft, 10);
            addImage(1768, 958, 68, imageTypes.DistractionRight, 10);
            addImage(1603, 891, 54, imageTypes.DistractionLeft, 10);
            addImage(1706, 866, 68, imageTypes.DistractionLeft, 10);
            addImage(1543, 804, 54, imageTypes.DistractionLeft, 10);
            addImage(1716, 766, 54, imageTypes.DistractionRight, 10);
            addImage(1402, 791, 54, imageTypes.DistractionLeft, 8);
            addImage(1318, 766, 54, imageTypes.DistractionRight, 8);
            addImage(1260, 675, 68, imageTypes.DistractionRight, 8);
            addImage(1290, 611, 54, imageTypes.TargetLeft, 8);
            addImage(1321, 528, 68, imageTypes.DistractionLeft, 7);
            addImage(1272, 440, 68, imageTypes.TargetRight, 7);
            addImage(1318, 355, 54, imageTypes.DistractionRight, 7);
            addImage(1304, 240, 68, imageTypes.DistractionRight, 7);
            addImage(1290, 140, 54, imageTypes.TargetRight, 7);
            addImage(1388, 160, 54, imageTypes.DistractionLeft, 7);
            addImage(1388, 298, 54, imageTypes.TargetLeft, 7);
            addImage(1417, 398, 54, imageTypes.DistractionLeft, 7);
            addImage(1419, 498, 54, imageTypes.DistractionLeft, 7);
            addImage(1419, 595, 54, imageTypes.DistractionLeft, 8);
            addImage(1380, 681, 68, imageTypes.DistractionRight, 8);
            addImage(1465, 720, 54, imageTypes.TargetRight, 10);
            addImage(1595, 730, 68, imageTypes.TargetLeft, 10);
            addImage(1693, 666, 68, imageTypes.DistractionLeft, 10);
            addImage(1585, 646, 54, imageTypes.TargetLeft, 10);
            addImage(1563, 566, 68, imageTypes.DistractionRight, 10);
            addImage(1741, 575, 68, imageTypes.TargetLeft, 10);
            addImage(1741, 485, 68, imageTypes.TargetLeft, 9);
            addImage(1608, 485, 54, imageTypes.TargetRight, 9);
            addImage(1505, 476, 54, imageTypes.DistractionLeft, 9);
            addImage(1778, 403, 68, imageTypes.DistractionLeft, 9);
            addImage(1595, 395, 54, imageTypes.DistractionLeft, 9);
            addImage(1510, 360, 54, imageTypes.DistractionRight, 9);
            addImage(1656, 364, 68, imageTypes.DistractionRight, 9);
            addImage(1754, 318, 68, imageTypes.TargetRight, 9);
            addImage(1735, 238, 68, imageTypes.DistractionLeft, 9);
            addImage(1628, 255, 54, imageTypes.TargetLeft, 9);
            addImage(1517, 264, 68, imageTypes.DistractionLeft, 9);
            addImage(1746, 164, 54, imageTypes.DistractionRight, 9);
            addImage(1665, 156, 68, imageTypes.DistractionRight, 9);
            addImage(1576, 149, 54, imageTypes.DistractionRight, 9);
            addImage(1487, 140, 54, imageTypes.TargetLeft, 9);





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

            this.imageList.Add(new mugObject(this.nextImageID, imageType, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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
