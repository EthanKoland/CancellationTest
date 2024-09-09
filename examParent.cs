using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using CancellationTest;
using OfficeOpenXml;

namespace CancellationTest
{
    internal class examParent : Form
    {
        private IContainer components;
        private MainMenu ImgMenu = null;
        private Graphics g = null;

        //A reference map to cacluate the interactions
        private int[,] bitMap = null;

        private int curPtindex = 0;
        private Pen GreenPen = null;
        private Pen BluePen = null;
        private Pen RedPen = null;
        private Pen CrossPen = null;
        private Point CurPt;
        private Point[] Cath2DPts = null;
        private Point[] CirPts = null;
        private int MaxPts = 0;

        private string patientName;

        public int adjustedScreenWidth { get; private set; }
        public int adjustedScreenHeight { get; private set; }

        //Inorder to standardize the screen size a variable is declared to adjust the size of the screen
        private double adjustSize = 1.0;

        //The none adjustsize of the screen
        private int screenwidth = 960;
        //Total screen size in 575
        private int screenheight = 540;
        private int verticalOffset = 0;

        //Vars that are declared in class init
        private int numberOfHorizontalGrids; // a variable that defines the number of grids in the horizontal direction
        private int numberOfVerticalGrids; // a variable that defines the number of grids in the vertical direction


        //Storage of the lines seperating the cells
        private int[] horizontalLines;
        private int[] verticalLines;

        //Refrences for the images
        private List<imageObject> images = new List<imageObject>();

        //Action tracker
        private actionTracker tracker;
        private abstractTestClass localExamObject;

        private DateTime endTime;

        private Label timeLabel;
        private Label currentTimeLabel;
        private Button helpButton;

        private Point centerScreen;
        private Point minBoundry;
        private Point maxBoundry;


        public examParent( abstractTestClass examObject, double adjustSize = 0.5,
            int seconds = 240, string patientName = "None")
        {

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.screenheight = Screen.PrimaryScreen.Bounds.Height;
            this.screenwidth = Screen.PrimaryScreen.Bounds.Width;

            //Assign the parameter values to the variables
            this.adjustSize = adjustSize;
            this.patientName = patientName;

            this.BackColor = Color.DarkGray;
            
            

            this.localExamObject = examObject;
            this.tracker = new actionTracker(this.localExamObject);


            this.endTime = DateTime.Now.AddSeconds(seconds);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (1000); // 10 seconds in milliseconds
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            //The timer 
            this.timeLabel = new Label();
            this.timeLabel.Text = "Time:";
            this.timeLabel.Location = new Point((int)(0.8*this.screenwidth), 5);
            this.timeLabel.Size = new Size((int)(0.05 * this.screenwidth ), 40);
            this.timeLabel.Font = new Font("Arial", 24);
            this.timeLabel.ForeColor = Color.DimGray;

            this.currentTimeLabel = new Label();
            this.currentTimeLabel.Text = "00:00";
            this.currentTimeLabel.Location = new Point((int)(0.9 * this.screenwidth), 5);
            this.currentTimeLabel.Size = new Size((int)(0.05 * this.screenwidth), 40);
            this.currentTimeLabel.Font = new Font("Arial", 24);
            this.currentTimeLabel.ForeColor = Color.DeepSkyBlue;

            this.helpButton = new Button();
            this.helpButton.Text = "?";
            this.helpButton.Location = new Point((int)(0.95 * this.screenwidth), 5);
            this.helpButton.Size = new Size(40, 40);
            this.helpButton.Font = new Font("Arial", 24);
            this.helpButton.ForeColor = Color.WhiteSmoke;
            this.helpButton.BackColor = Color.DeepSkyBlue;

            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.helpButton);
             
            this.KeyPreview = true;
            this.centerScreen = new Point(this.screenwidth / 2, this.screenheight / 2); 
            

            int frameX = (int)(examObject.screenWidth * this.adjustSize);
            int frameY = (int)(examObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX)/2, (this.screenheight - frameY)/2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);


            //Creating a new bitMap

            InitializeComponent();
            this.KeyDown += new KeyEventHandler(examParent_KeyDown);
            this.MouseClick += new MouseEventHandler(ImageViewer_MouseClick);
            this.DoubleBuffered = true;
            GreenPen = new Pen(Color.LightGreen, 1);
            BluePen = new Pen(Color.LightBlue, 1);
            RedPen = new Pen(Color.Red, 1);
            CrossPen = new Pen(Color.Black, (float)(this.screenheight * 0.01));
           
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeRemaining = endTime - DateTime.Now;

            //If the time is less than 0 then stop the timer
            if (timeRemaining.TotalSeconds < 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                this.currentTimeLabel.Text = "0:00";

                return;
            }

            int totalSeconds = (int)Math.Round(timeRemaining.TotalSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string modifedSeconds = seconds < 10 ? "0" + seconds : "" + seconds;

            this.currentTimeLabel.Text = "" + minutes + ":" + modifedSeconds;
        }

        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {


            //Figure out if the click point is within the boundry
            if(e.X < this.minBoundry.X || e.X > this.maxBoundry.X || e.Y < this.minBoundry.Y || e.Y > this.maxBoundry.Y)
            {
                return;
            }   

            this.CurPt = adjustedClickPoint(new Point(e.X, e.Y));


            // Check if the ooint correlates to any of the images
            int imageID = this.localExamObject.checkClick(this.CurPt);
            Console.WriteLine("Image ID : " + imageID);



            if (imageID != 0)
           {

                int halfWidth = this.localExamObject.imageList[imageID - 1].width / 2;
                int halfHeight = this.localExamObject.imageList[imageID - 1].height / 2;

                Point imageCenter = this.localExamObject.imageList[imageID - 1].imageCenter;

                clickAction action = new clickAction();
                action.clickPoint = new Point(imageCenter.X + halfWidth, imageCenter.Y + halfHeight);
                action.ImageID = imageID;
                action.timeOfClick = DateTime.Now;

                Console.WriteLine("Image Clicked : " + imageID);
                imageObject clickedImage = this.localExamObject.imageList[imageID - 1];
                clickedImage.setClicked(0, adjustSize);

                action.isCrossed = clickedImage.isClicked;

                Console.WriteLine("Image cliscked State" + clickedImage.isClicked);

                this.tracker.addAction(action);
           }

                
            Console.WriteLine("Image ID : " + imageID);

            Console.WriteLine("ImageCenter for index 1 : " + this.localExamObject.imageList[0].imageCenter.X + " " + this.localExamObject.imageList[0].imageCenter.Y);



            this.Refresh();

        }

        void examParent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //pnlwhichImg.Text = " image " + curimg;
                this.Refresh();
            }
            else if (e.KeyCode == Keys.E)
            {
                TimeSpan timeRemaining = endTime - DateTime.Now;
                Console.WriteLine("E Key Pressed");
                string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                this.tracker.export(filename);
                this.tracker.outPutImage(this.patientName, timeRemaining);
            }
            else if (e.KeyCode == Keys.Oemplus)
            {
                increaseAdjustSize();
                Console.WriteLine("Adjust Size : " + this.adjustSize);
            }
            else if (e.KeyCode == Keys.OemMinus)
            {
                decreaseAdjustSize();
                Console.WriteLine("Adjust Size : " + this.adjustSize);
            }
            else if(e.KeyCode == Keys.H)
            {
                Console.WriteLine("Help Button Clicked");
            }
            else if(e.KeyCode == Keys.Q)
            {
                System.Windows.Forms.Application.Exit();

            }
            Console.WriteLine("Key Pressed" + e.KeyCode);
            this.Refresh();
        }


        //draw customized mouse pointer
        private Cursor crossCursor(Pen pen, int x, int y)
        {
            var pic = new Bitmap(x, y);
            Graphics gr = Graphics.FromImage(pic);

            var pathX = new GraphicsPath();
            var pathY = new GraphicsPath();
            pathX.AddLine(0, y / 2, x, y / 2);
            pathY.AddLine(x / 2, 0, x / 2, y);
            gr.DrawPath(pen, pathX);
            gr.DrawPath(pen, pathY);
            gr.DrawArc(pen, 2, 2, x - 5, y - 5, 0, 360);

            Console.WriteLine("X : " + x + " Y : " + y);

            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        }

        private void bitMapValidation(Graphics g)
        {
            for (int i = 0; i < this.screenwidth; i++)
            {
                for (int j = 0; j < this.screenheight; j++)
                {
                    Point tempPoi = new Point(i, j);
                    Point adjustedPoint = adjustedClickPoint(tempPoi);
                    int imageID = this.localExamObject.checkClick(adjustedPoint);
                    if (imageID != 0)
                    {
                        Console.WriteLine("Image ID : " + imageID);
                        g.DrawRectangle(Pens.Black, i, j, 1, 1);
                    }

                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //re-adjust the size of cursor
            //Cursor = crossCursor(Pens.WhiteSmoke, Brushes.WhiteSmoke, Bounds.Width, Bounds.Height);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 255), 5);
            Cursor = crossCursor(blackPen, 50, 50);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int i;
            base.OnPaint(e);
            Graphics dc = e.Graphics;

            //localExamObject.drawHeader(dc, endTime);

            //bitMapValidation(dc);


            //Draw the images in the list
            foreach (imageObject img in localExamObject.imageList)
            {
                
                int halfWidth = (int)Math.Round(img.width * adjustSize) / 2;
                int halfHeight = (int)Math.Round(img.height * adjustSize) / 2;

                int adjustedHeight = (int)Math.Round(img.height * adjustSize);
                int adjustedWidth = (int)Math.Round(img.width * adjustSize);

                int adjustedX = (int)Math.Round(img.imageCenter.X * adjustSize + this.minBoundry.X);
                int adjustedY = (int)Math.Round(img.imageCenter.Y * adjustSize + this.minBoundry.Y);

                e.Graphics.DrawImage(imageObject.getImageObject(img.imageType), adjustedX - halfWidth, adjustedY - halfHeight, adjustedWidth, adjustedHeight);
                
                if(img.isClicked)
                {
                    int x1 = adjustedX - adjustedWidth/2;

                    int y1 = adjustedY - adjustedHeight / 2;


                    int x2 = x1 + adjustedWidth;

                    int y2 = y1 + adjustedHeight;

                    e.Graphics.DrawLine(this.CrossPen, x1, y1, x2, y2);
                    
                }


                e.Graphics.DrawRectangle(Pens.Black, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
            }



            

            Console.WriteLine("Min Boundry : " + this.minBoundry.X + " " + this.minBoundry.Y);
            Console.WriteLine("Max Boundry : " + this.maxBoundry.X + " " + this.maxBoundry.Y);

            dc.DrawRectangle(RedPen, this.minBoundry.X, this.minBoundry.Y, this.maxBoundry.X - this.minBoundry.X, this.maxBoundry.Y - this.minBoundry.Y);
            dc.DrawRectangle(RedPen, 900, 500, 120, 80);
        }

        private void Draw2DPoint(Graphics dc, Pen pen, Point pt)
        {
            dc.DrawLine(pen, pt.X - 7, pt.Y, pt.X + 7, pt.Y);
            dc.DrawLine(pen, pt.X, pt.Y - 7, pt.X, pt.Y + 7);
        }


        private void increaseAdjustSize()
        {
            this.adjustSize += 0.01;

            //int adjusX = (int)Math.Round(screenwidth * 0.01);
            //int adjusY = (int)Math.Round(screenheight * 0.01);

            //this.minBoundry = new Point(Math.Max(this.minBoundry.X - adjusX, 0), Math.Max(this.minBoundry.Y - adjusY, 0));
            //this.maxBoundry = new Point(Math.Min(this.maxBoundry.X + adjusX, screenwidth), Math.Min(this.maxBoundry.Y + adjusY, screenheight));

            int frameX = (int)(localExamObject.screenWidth * this.adjustSize);
            int frameY = (int)(localExamObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX) / 2, (this.screenheight - frameY) / 2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            this.CrossPen = new Pen(Color.Black, (float)(frameY * 0.01));
    
        }

        private void decreaseAdjustSize()
        {
            this.adjustSize -= 0.01;

            int frameX = (int)(localExamObject.screenWidth * this.adjustSize);
            int frameY = (int)(localExamObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX) / 2, (this.screenheight - frameY) / 2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            this.CrossPen = new Pen(Color.Black, (float)(frameY * 0.01));
        }

        private Point adjustedClickPoint(Point clickPoint)
        {
            
            Point newPoint = new Point((int)Math.Round(clickPoint.X / adjustSize) - this.minBoundry.X,
                                        (int)Math.Round(clickPoint.Y / adjustSize) - this.minBoundry.Y);
            Console.WriteLine("Adjusted Point : " + newPoint.X + " " + newPoint.Y);
            
            return newPoint;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImageViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(11, 27);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(screenwidth, screenheight + verticalOffset);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImageViewer";
            this.Text = "CancellationTest";
            this.ResumeLayout(false);

        }
    }

    public enum AvailableExams
    {
        RandomPlacement,
        Assessment,
        Pratice_1,
        Pratice_2
    }
}
