using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class Export_Map : abstractExportClass
    {
        public Export_Map(string patient_ID)
        {
            this.patient_ID = patient_ID;
        }

        public override void export(string folderLocation)
        {
            Bitmap bmp = new Bitmap(this.localExamObj.screenWidth, this.localExamObj.screenHeight);

            int SquareSize = 20;
            int halfSquare = SquareSize / 2;

            //Create a new graphics object
            Graphics g = Graphics.FromImage(bmp);

            //Set the backgroun to white
            g.Clear(Color.White);

            //Cancelletion test - Date
            string mapTitleStr = this.localExamObj.isPratice ? "Practice Test" : "Cancellation Test";
            g.DrawString(mapTitleStr + this.patient_ID, new Font("Arial", 16), Brushes.Black, new Point(10, 10));
            g.DrawString("Ending Time : " + this.total_TimeTaken, new Font("Arial", 12), Brushes.Black, new Point(10, 30));
            g.DrawString("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), new Font("Arial", 12), Brushes.Black, new Point(10, 50));

            foreach (mugObject img in this.localExamObj.imageList)
            {
                g.DrawImage(mugObject.getImageObject(img.imageType), img.imageCenter.X, img.imageCenter.Y, img.width, img.height);
                if (img.isClicked)
                {
                    //Draw a cross on the image
                    g.DrawLine(new Pen(Color.Black, 3), img.imageCenter.X, img.imageCenter.Y, img.imageCenter.X + img.width, img.imageCenter.Y + img.height);
                }
            }

            if (this.clickRows != null  && this.clickRows.Count > 0)
            {
                Point previousPoint = this.clickRows[0].normalizedLocation;

                Rectangle initialRectangle = new Rectangle(previousPoint.X - halfSquare, previousPoint.Y - halfSquare, SquareSize, SquareSize);

                for (int i = 1; i < this.clickRows.Count; i++)
                {
                    
                    Point currentPoint = this.clickRows[0].normalizedLocation;

                    //Draw the connection pine betyween the two points
                    g.DrawLine(new Pen(Color.Black, 4), previousPoint, currentPoint);

                    //Draw a dot in the middle of the image
                    g.FillRectangle(Brushes.Blue, currentPoint.X - halfSquare, currentPoint.Y - halfSquare, SquareSize, SquareSize);

                    previousPoint = currentPoint;
                }

                g.FillRectangle(Brushes.Red, initialRectangle);
            }

            if (this.localExamObj.isPratice)
            {
                int imgWidth = bmp.Width;
                int imgHeight = bmp.Height;

                int smallMugsize = (int)(0.04 * imgWidth);
                int largeMugsize = (int)(0.055 * imgWidth);

                Pen instructionPen = new Pen(Color.Black, 5);

                //Cross out the small left mug
                Point p1 = new Point((int)(0.38 * imgWidth), (int)(0.165 * imgWidth) + smallMugsize);
                Point p2 = new Point((int)(0.38 * imgWidth) + smallMugsize, (int)(0.165 * imgWidth));
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), p1.X, p2.Y, smallMugsize, smallMugsize);
                g.DrawLine(instructionPen, p1, p2);


                //Cross out the small right mug
                p1 = new Point((int)(0.44 * imgWidth), (int)(0.165 * imgWidth) + smallMugsize);
                p2 = new Point((int)(0.44 * imgWidth) + smallMugsize, (int)(0.165 * imgWidth));
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), p1.X, p2.Y, smallMugsize, smallMugsize);
                g.DrawLine(instructionPen, p1, p2);


                //Cross out the large left mug
                p1 = new Point((int)(0.37 * imgWidth), (int)(0.25 * imgWidth) + largeMugsize);
                p2 = new Point((int)(0.37 * imgWidth) + largeMugsize, (int)(0.25 * imgWidth));
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), p1.X, p2.Y, largeMugsize, largeMugsize);
                g.DrawLine(instructionPen, p1, p2);

                //Cross out the large right mug
                p1 = new Point((int)(0.435 * imgWidth), (int)(0.25 * imgWidth) + largeMugsize);
                p2 = new Point((int)(0.435 * imgWidth) + largeMugsize, (int)(0.25 * imgWidth));
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), p1.X, p2.Y, largeMugsize, largeMugsize);
                g.DrawLine(instructionPen, p1, p2);

                //Cross out label
                g.DrawString("Cross out the mugs",
                    new Font("Arial", (int)(0.02 * imgWidth)),
                    Brushes.Black,
                    new Point(0, (int)(0.1 * imgHeight)));

                //Small left Target Image
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), (int)(0.38 * imgWidth), (int)(0.165 * imgWidth), smallMugsize, smallMugsize);

                //Small right Target Image
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), (int)(0.44 * imgWidth), (int)(0.165 * imgWidth), smallMugsize, smallMugsize);

                //Large left Target Image
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), (int)(0.37 * imgWidth), (int)(0.25 * imgWidth), largeMugsize, largeMugsize);

                //Large right Target Image
                g.DrawImage(mugObject.getImageObject(imageTypes.TargetRight), (int)(0.435 * imgWidth), (int)(0.25 * imgWidth), largeMugsize, largeMugsize);

                //Small Left Distractor Image
                g.DrawImage(mugObject.getImageObject(imageTypes.DistractionLeft), (int)(0.38 * imgWidth), (int)(0.3 * imgWidth), smallMugsize, smallMugsize);

                //Small Right Distractor Image
                g.DrawImage(mugObject.getImageObject(imageTypes.DistractionRight), (int)(0.44 * imgWidth), (int)(0.3 * imgWidth), smallMugsize, smallMugsize);

                //Large Left Distractor Image
                g.DrawImage(mugObject.getImageObject(imageTypes.DistractionLeft), (int)(0.37 * imgWidth), (int)(0.35 * imgWidth), largeMugsize, largeMugsize);

                //Large Right Distractor Image
                g.DrawImage(mugObject.getImageObject(imageTypes.DistractionRight), (int)(0.435 * imgWidth), (int)(0.35 * imgWidth), largeMugsize, largeMugsize);
            }

            string outputFolder = this.getOutputPath(folderLocation);
            string filePath = this.getFileName(outputFolder, ".png");
            //string imageName = "test.png";

            Console.WriteLine("Saving the image");
            Console.WriteLine(filePath);

            //Save the image    
            bmp.Save(filePath);

        }

        
    }
}
