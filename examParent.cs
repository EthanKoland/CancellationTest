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
using System.Media;

namespace CancellationTest
{
    internal class examParent : Form
    {
        protected IContainer components;
        protected MainMenu ImgMenu = null;
        protected Graphics g = null;

        //A reference map to cacluate the interactions
        protected int[,] bitMap = null;

        protected int curPtindex = 0;
        protected Pen GreenPen = null;
        protected Pen BluePen = null;
        protected Pen RedPen = null;
        protected Pen CrossPen = null;
        protected Point CurPt;
        protected Point[] Cath2DPts = null;
        protected Point[] CirPts = null;
        protected int MaxPts = 0;

        protected string patientName;
        protected bool pauseTimer = false;
        protected TimeSpan pauseTime = new TimeSpan(0, 0, 0);


        //Inorder to standardize the screen size a variable is declared to adjust the size of the screen
        protected double adjustSize = 1.0;

        //The none adjustsize of the screen
        protected int screenwidth = 960;
        //Total screen size in 575
        protected int screenheight = 540;
        protected int verticalOffset = 0;

        //Vars that are declared in class init
        protected int numberOfHorizontalGrids; // a variable that defines the number of grids in the horizontal direction
        protected int numberOfVerticalGrids; // a variable that defines the number of grids in the vertical direction


        //Storage of the lines seperating the cells
        protected int[] horizontalLines;
        protected int[] verticalLines;

        //Timer tick time
        protected int tickTime = 1000;

        //Action tracker
        protected actionTracker tracker;
        protected abstractTestClass localExamObject;

        protected DateTime endTime;

        protected Label timeLabel;
        protected Label currentTimeLabel;
        protected Button helpButton;

        protected Point centerScreen;
        protected Point minBoundry;
        protected Point maxBoundry;

        //set of ints to determine wether an image has been clicked
        protected HashSet<int> clickedImages = new HashSet<int>();

        protected Double[] remainingCancelationTime;
        protected int crossOutTime;

        //Sound players for the popping sounds
        protected WMPLib.WindowsMediaPlayer pop2Player = new WMPLib.WindowsMediaPlayer();
        protected SoundPlayer poppingPlayer = new SoundPlayer(@"Sounds\popping.wav");

        public examParent( abstractTestClass examObject, double adjustSize = 0.5,
            int seconds = 240, string patientName = "None", int crossOutTime = -1) 
        {
            
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.pop2Player.URL =  "Sounds\\pop2.mp3";

            this.screenheight = Screen.PrimaryScreen.Bounds.Height;
            this.screenwidth = Screen.PrimaryScreen.Bounds.Width;
            this.crossOutTime = crossOutTime;
            //this.screenwidth = this.Width;
            //this.screenheight = this.Height;

            //Assign the parameter values to the variables
            this.adjustSize = adjustSize * ((double) this.screenwidth/1920.0);
            
            //List of size the number elements of the mugs list in exam object offset by one to account for IDS starting at 1
            this.remainingCancelationTime = new double[examObject.imageList.Count + 1];


            this.patientName = patientName;

            this.BackColor = Color.DarkGray;
            
            

            this.localExamObject = examObject;
            bool invisableCancelation = crossOutTime == -1 ? false : true;
            this.tracker = new actionTracker(this.localExamObject, invisableCancelation, patientID: patientName);


            this.endTime = DateTime.Now.AddSeconds(seconds);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (this.tickTime); 
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            //The timer 
            this.timeLabel = new Label();
            this.timeLabel.Text = "Time:";
            this.timeLabel.Location = new Point((int)(0.8*this.screenwidth), 5);
            this.timeLabel.Size = new Size((int)(0.05 * this.screenwidth ), (int)(0.035 * this.screenheight));
            this.timeLabel.Font = new Font("Arial", (int)(0.022 * this.screenheight));
            this.timeLabel.ForeColor = Color.DimGray;

            this.currentTimeLabel = new Label();
            this.currentTimeLabel.Text = "00:00";
            this.currentTimeLabel.Location = new Point((int)(0.9 * this.screenwidth), 5);
            this.currentTimeLabel.Size = new Size((int)(0.05 * this.screenwidth), (int)(0.03 * this.screenheight));
            this.currentTimeLabel.Font = new Font("Arial", (int)(0.022 * this.screenheight));
            this.currentTimeLabel.ForeColor = Color.DeepSkyBlue;

            this.helpButton = new Button();
            this.helpButton.Text = "?";
            this.helpButton.Location = new Point((int)(0.95 * this.screenwidth), 5);
            this.helpButton.Size = new Size((int)(0.035 * this.screenheight), (int)(0.035 * this.screenheight));
            this.helpButton.Font = new Font("Arial", (int)(0.022 * this.screenheight));
            this.helpButton.ForeColor = Color.WhiteSmoke;
            this.helpButton.BackColor = Color.DeepSkyBlue;
            this.helpButton.Click += new EventHandler(helpButton_Click);

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

        protected void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeRemaining = endTime - DateTime.Now;

            if (this.pauseTimer)
            {
                return;
            }

            //If the time is less than 0 then stop the timer
            if (timeRemaining.TotalSeconds < 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();
                this.currentTimeLabel.Text = "0:00";

                tracker.endTime = DateTime.Now;
                string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                //Open the export screen
                exportScreen exportScreenObj = new exportScreen(this.patientName, this.tracker, this.adjustSize);
                exportScreenObj.ShowDialog();

                System.Windows.Forms.Application.Exit();

                return;
            }

            int totalSeconds = (int)Math.Round(timeRemaining.TotalSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string modifedSeconds = seconds < 10 ? "0" + seconds : "" + seconds;

            this.currentTimeLabel.Text = "" + minutes + ":" + modifedSeconds;

            bool refreshScreen = false;

            //Update the remaining time in the remainingCancelationTime array
            if (this.crossOutTime > 0)
            {
                for (int i = 1; i < this.remainingCancelationTime.Length; i++)
                {
                    double newTime = Math.Max(0, this.remainingCancelationTime[i] - (tickTime / 1000.0));
                    this.remainingCancelationTime[i] = newTime;
                    refreshScreen = newTime == 0 ? true : refreshScreen;
                }
            }
            if(refreshScreen)
            {
                this.Refresh();
            }
        }

        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            this.CurPt = new Point((int) ((e.X - this.minBoundry.X) / (1 * adjustSize)),
                                   (int)((e.Y - this.minBoundry.Y) / (1 * adjustSize)));

            //Figure out if the click point is within the boundry
            if (e.X < this.minBoundry.X || e.X > this.maxBoundry.X || e.Y < this.minBoundry.Y || e.Y > this.maxBoundry.Y)
            {
                return;
            }   

        

            // Check if the ooint correlates to any of the images
            int imageID = this.localExamObject.checkClick(this.CurPt);
            Console.WriteLine("Image ID : " + imageID);



            if (imageID != 0)
           {

                Random rand = new Random();
                int nextint = rand.Next(2);

                if (rand.Next(2) == 1)
                {
                    //Play the popping sound
                    this.pop2Player.controls.play();
                }
                else
                {
                    //Play the other popping sound
                    this.poppingPlayer.Play();
                }
                
                int halfWidth = this.localExamObject.imageList[imageID - 1].width / 2;
                int halfHeight = this.localExamObject.imageList[imageID - 1].height / 2;

                Point imageCenter = this.localExamObject.imageList[imageID - 1].imageCenter;

                clickAction action = new clickAction();
                action.clickPoint = new Point(this.CurPt.X, this.CurPt.Y);
                action.ImageID = imageID;
                action.timeOfClick = DateTime.Now;
                action.leftOrRightSide = this.localExamObject.imageList[imageID - 1].imageCenter.X < this.screenwidth / 2 ? LeftRight.Left : LeftRight.Right;

                bool timeRemaining = this.remainingCancelationTime[imageID] <= 0;

                Console.WriteLine("Image Clicked : " + imageID);
                mugObject clickedImage = this.localExamObject.imageList[imageID - 1];
                clickedImage.setClicked(0, adjustSize, timeRemaining);

                //Set the image crossed to state to update the action tracker to keep track of user actions
                action.isCrossed = clickedImage.isClicked;

                //Update the remaining time in the remainingCancelationTime array
                this.remainingCancelationTime[imageID] = this.crossOutTime == -1 ? 1 : this.crossOutTime;

                Console.WriteLine("Image clicked State" + clickedImage.isClicked);

                this.tracker.addAction(action);

                
           }

                
            Console.WriteLine("Image ID : " + imageID);

            Console.WriteLine("ImageCenter for index 1 : " + this.localExamObject.imageList[0].imageCenter.X + " " + this.localExamObject.imageList[0].imageCenter.Y);



            this.Refresh();

        }

        void examParent_MouseEvent(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Event");

            //The pos
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

            }
            else if (e.KeyCode == Keys.H)
            {
                Console.WriteLine("Help Button Clicked");
            }
            else if (e.KeyCode == Keys.Q || e.KeyCode == Keys.Escape)
            {
                this.pauseTimer = true;
                TimeSpan timeRemaining = endTime - DateTime.Now;
                tracker.endTime = DateTime.Now;
                Console.WriteLine("E Key Pressed");
                string filename = "CancellationTest_" + this.patientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                //Open the export screen
                exportScreen exportScreenObj = new exportScreen(this.patientName, this.tracker, this.adjustSize);
                exportScreenObj.ShowDialog();

                System.Windows.Forms.Application.Exit();

            }
            else if (e.KeyCode == Keys.D1)
            {
                Console.WriteLine("1 Key Pressed -> Creating a Map");
                tracker.endTime = DateTime.Now;
                Export_Map map = new Export_Map(this.patientName);
                
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D2)
            {
                Console.WriteLine("2 Key Pressed -> Creating a XLSX");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_XLSX(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D3)
            {
                Console.WriteLine("2 Key Pressed -> Creating a Txt");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_Txt(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);

                this.tracker.export(list);
            }
            else if (e.KeyCode == Keys.D4)
            {
                Console.WriteLine("2 Key Pressed -> Creating a CSV");
                tracker.endTime = DateTime.Now;
                abstractExportClass map = new Export_CSV(this.patientName);
                List<abstractExportClass> list = new List<abstractExportClass>();
                list.Add(map);
                this.tracker.export(list);
            }

            Console.WriteLine("Key Pressed" + e.KeyCode);
            this.Refresh();
        }


        //draw customized mouse pointer
        protected Cursor crossCursor(Pen pen, int x, int y)
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

        protected void bitMapValidation(Graphics g)
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
            
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            Graphics dc = e.Graphics;
            dc.PageUnit = GraphicsUnit.Pixel;

            //localExamObject.drawHeader(dc, endTime);

            //bitMapValidation(dc);
            
            


            //Draw the images in the list
            foreach (mugObject img in localExamObject.imageList)
            {
                
                int halfWidth = (int)Math.Round(img.width * adjustSize) / 2;
                int halfHeight = (int)Math.Round(img.height * adjustSize) / 2;

                int adjustedHeight = (int)Math.Round(img.height * adjustSize);
                int adjustedWidth = (int)Math.Round(img.width * adjustSize);

                int adjustedX = (int)Math.Round(img.imageCenter.X * adjustSize + this.minBoundry.X);
                int adjustedY = (int)Math.Round(img.imageCenter.Y * adjustSize + this.minBoundry.Y);

                e.Graphics.DrawImage(mugObject.getImageObject(img.imageType), adjustedX - halfWidth, adjustedY - halfHeight, adjustedWidth, adjustedHeight);
                

                //if(clickedImages.Contains(img.imageID))
                if(this.remainingCancelationTime[img.imageID] > 0)
                {
                    int x1 = adjustedX - adjustedWidth/2;

                    int y1 = adjustedY - adjustedHeight / 2;


                    int x2 = x1 + adjustedWidth;

                    int y2 = y1 + adjustedHeight;

                    e.Graphics.DrawLine(this.CrossPen, x1, y2, x2, y1);
                    
                }

                //IF img.side is left draw a blue rectange
                /*if (img.matrixLocation == 2 )
                {
                    e.Graphics.DrawRectangle(BluePen, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
                }
                else if ( img.matrixLocation == 3)
                {
                    e.Graphics.DrawRectangle(GreenPen, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
                }
                else if (img.matrixLocation == 7)
                {
                    e.Graphics.DrawRectangle(RedPen, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
                }*/

                //e.Graphics.DrawRectangle(Pens.Black, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
            }

           

        }

        protected void Draw2DPoint(Graphics dc, Pen pen, Point pt)
        {
            dc.DrawLine(pen, pt.X - 7, pt.Y, pt.X + 7, pt.Y);
            dc.DrawLine(pen, pt.X, pt.Y - 7, pt.X, pt.Y + 7);
        }


        protected void increaseAdjustSize()
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

        protected void decreaseAdjustSize()
        {
            this.adjustSize -= 0.01;

            int frameX = (int)(localExamObject.screenWidth * this.adjustSize);
            int frameY = (int)(localExamObject.screenHeight * this.adjustSize);

            this.minBoundry = new Point((this.screenwidth - frameX) / 2, (this.screenheight - frameY) / 2);
            this.maxBoundry = new Point((this.screenwidth + frameX) / 2, (this.screenheight + frameY) / 2);

            this.CrossPen = new Pen(Color.Black, (float)(frameY * 0.01));
        }

        protected Point adjustedClickPoint(Point clickPoint)
        {
            
            Point newPoint = new Point((int)Math.Round(clickPoint.X / adjustSize) - this.minBoundry.X,
                                        (int)Math.Round(clickPoint.Y / adjustSize) - this.minBoundry.Y);
            Console.WriteLine("Adjusted Point : " + newPoint.X + " " + newPoint.Y);
            
            return newPoint;
        }

        protected void InitializeComponent()
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

        protected void helpButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Help Button Clicked");

            //Pop up the help menu
            helpScreen help = new helpScreen();
            help.ShowDialog();

        }
    }

    public enum AvailableExams
    {
        RandomPlacement,
        Assessment,
        Practice_1,
        Practice_2
    }
}
