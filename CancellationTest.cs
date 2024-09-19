using System;
using System.Collections.Generic;
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

    public class ImageViewer : Form
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
        private Point CurPt;
        private int Mode = 0;
        private Point[] Cath2DPts = null;
        private Point[] CirPts = null;
        private int MaxPts = 0;

        public int adjustedScreenWidth { get; private set; }
        public int adjustedScreenHeight { get; private set; }

        //Inorder to standardize the screen size a variable is declared to adjust the size of the screen
        private double adjustSize = 1.0;

        //The none adjustsize of the screen
        private int screenwidth = 1920;
        //Total screen size in 575
        private int screenheight = 1035;
        private int verticalOffset = 45;

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


        public ImageViewer(double adjustSize = 1.0,
            int numberOfHorizontalGrids = 5, int numberOfVerticalGrids = 2, int seconds = 240)
        {
            //Assign the parameter values to the variables
            this.adjustSize = adjustSize;
            this.numberOfHorizontalGrids = numberOfHorizontalGrids;
            this.numberOfVerticalGrids = numberOfVerticalGrids;

            this.localExamObject = new randomTest(this.screenwidth, this.screenheight, this.verticalOffset);

            this.adjustedScreenHeight = (int)Math.Round(this.screenheight * adjustSize);
            this.adjustedScreenWidth = (int)Math.Round(this.screenwidth * adjustSize);

            this.bitMap = new int[this.adjustedScreenWidth, this.adjustedScreenHeight];

            this.endTime = DateTime.Now.AddSeconds(seconds);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (1000); // 10 seconds in milliseconds
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            //The timer 
            this.timeLabel = new Label();
            this.timeLabel.Text = "Time:";
            this.timeLabel.Location = new Point(760, 5);
            this.timeLabel.Size = new Size(100, 40);
            this.timeLabel.Font = new Font("Arial", 24);
            this.timeLabel.ForeColor = Color.DimGray;

            this.currentTimeLabel = new Label();
            this.currentTimeLabel.Text = "00:00";
            this.currentTimeLabel.Location = new Point(860, 5);
            this.currentTimeLabel.Size = new Size(100, 40);
            this.currentTimeLabel.Font = new Font("Arial", 24);
            this.currentTimeLabel.ForeColor = Color.DeepSkyBlue;

            this.helpButton = new Button();
            this.helpButton.Text = "?";
            this.helpButton.Location = new Point(980, 5);
            this.helpButton.Size = new Size(40, 40);
            this.helpButton.Font = new Font("Arial", 24);
            this.helpButton.ForeColor = Color.WhiteSmoke;
            this.helpButton.BackColor = Color.DeepSkyBlue;
           
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.helpButton);

            foreach (imageObject img in localExamObject.imageList)
            {
                int halfWidth = (int)Math.Round(img.width * adjustSize) / 2;
                int halfHeight = (int)Math.Round(img.height * adjustSize) / 2;

                int adjustedHeight = (int)Math.Round(img.height * adjustSize);
                int adjustedWidth = (int)Math.Round(img.width * adjustSize);

                int adjustedX = (int)Math.Round(img.imageCenter.X * adjustSize);
                int adjustedY = (int)Math.Round(img.imageCenter.Y * adjustSize);

                for (int i = adjustedX - halfWidth; i < adjustedX + halfWidth; i++)
                {
                    for (int j = adjustedY - halfHeight; j < adjustedY + halfHeight; j++)
                    {
                        this.bitMap[i, j] = img.imageID;
                    }
                }

            }


            //Calculate the horizontal lines
            this.horizontalLines = new int[localExamObject.numberOfHorizontalCells()];

            for (int i = 0; i < localExamObject.numberOfHorizontalCells(); i++)
            {
                this.horizontalLines[i] = i * localExamObject.cellWidth;
            }

            //Calculate the vertical lines
            this.verticalLines = new int[localExamObject.numberOfVerticalCells()];

            for (int i = 0; i < localExamObject.numberOfVerticalCells(); i++)
            {
                this.verticalLines[i] = i * localExamObject.cellHeight;
            }

            //Creating a new bitMap




            this.tracker = new actionTracker(this.localExamObject);
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            //this.KeyDown += new KeyEventHandler(ImageViewer_KeyDown);
            this.MouseClick += new MouseEventHandler(ImageViewer_MouseClick);
            this.DoubleBuffered = true;
            GreenPen = new Pen(Color.LightGreen, 1);
            BluePen = new Pen(Color.LightBlue, 1);
            RedPen = new Pen(Color.Red, 1);
            MaxPts = 100;
            CurPt = new Point(-100, -100);
            Cath2DPts = new Point[MaxPts];
            CirPts = new Point[5];
            for (int i = 0; i < MaxPts; i++) Cath2DPts[i].X = -100;
            for (int i = 0; i < 5; i++) CirPts[i].X = -100;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Key Pressed");
            e.Handled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeRemaining = endTime - DateTime.Now;

            //If the time is less than 0 then stop the timer
            if (timeRemaining.TotalSeconds < 0)
            {
                ((System.Windows.Forms.Timer)sender).Stop();

                return;
            }

            int totalSeconds =  (int) Math.Round(timeRemaining.TotalSeconds);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string modifedSeconds = seconds < 10 ? "0" + seconds : "" + seconds;
            
            this.currentTimeLabel.Text =  "" + minutes + ":" + modifedSeconds;
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
        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            double radius;

            CurPt.X = (int)(e.X * this.adjustSize);
            CurPt.Y = (int)(e.Y * this.adjustSize);






            if (Mode == 0)
            {
                //CathTips[curimg].X = e.X;
                //CathTips[curimg].Y = e.Y;
            }
            if (Mode == 1)
            {
                Console.WriteLine("Mode 1");
                Cath2DPts[curPtindex].X = e.X;
                Cath2DPts[curPtindex].Y = e.Y;
                if (curPtindex < MaxPts) curPtindex++;
            }
            if (Mode == 2)
            {
                Console.WriteLine("Mode 2");
                CirPts[curPtindex].X = e.X;
                CirPts[curPtindex].Y = e.Y;
                if (curPtindex == 1)
                {
                    CirPts[2].X = (CirPts[0].X + CirPts[1].X) / 2;
                    CirPts[2].Y = (CirPts[0].Y + CirPts[1].Y) / 2;
                    radius = Math.Sqrt((CirPts[0].X - CirPts[2].X) * (CirPts[0].X - CirPts[2].X) + (CirPts[0].Y - CirPts[2].Y) * (CirPts[0].Y - CirPts[2].Y));
                    CirPts[3].X = CirPts[2].X - Convert.ToInt32(radius);
                    CirPts[3].Y = CirPts[2].Y - Convert.ToInt32(radius);
                    CirPts[4].X = Convert.ToInt32(radius);
                    CirPts[4].Y = Convert.ToInt32(radius);
                }
                if (curPtindex < 1) curPtindex++;
            }
            this.Refresh();

        }

        void ImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                //pnlwhichImg.Text = " image " + curimg;
                this.Refresh();
            }
            else if (e.KeyCode == Keys.E)
            {
                Console.WriteLine("E Key Pressed");
                this.tracker.export("test.xlsx");
            }
            Console.Write(e.KeyCode);
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("Dispose");
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //draw customized mouse pointer
        private Cursor crossCursor(Pen pen, Brush brush, int x, int y)
        {
            var pic = new Bitmap(x, y);
            Graphics gr = Graphics.FromImage(pic);

            var pathX = new GraphicsPath();
            var pathY = new GraphicsPath();
            pathX.AddLine(0, y / 2, x, y / 2);
            pathY.AddLine(x / 2, 0, x / 2, y);
            gr.DrawPath(pen, pathX);
            gr.DrawPath(pen, pathY);
            gr.DrawArc(pen, 2, 2, x-5, y-5, 0, 360);

            Console.WriteLine("X : " + x + " Y : " + y);

            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
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
            Cursor = crossCursor(blackPen, Brushes.WhiteSmoke, 100, 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int i;
            base.OnPaint(e);
            Graphics dc = e.Graphics;

            //localExamObject.drawHeader(dc, endTime);

            if (CurPt.X >= 0 && CurPt.Y >= localExamObject.verticalOffset)
            {
                Point offsetPoint = new Point(CurPt.X, CurPt.Y - localExamObject.verticalOffset);

                //Draw2DPoint(dc, GreenPen, offsetPoint);

                Console.WriteLine("Mouse Clicked at : " + offsetPoint.X + " " + offsetPoint.Y);

                //Get the image that is clicked
                int clickedImageID = this.bitMap[offsetPoint.X, offsetPoint.Y];

                if (clickedImageID != 0)
                {
                    clickAction action = new clickAction();
                    action.clickPoint = new Point(offsetPoint.X, offsetPoint.Y);
                    action.ImageID = clickedImageID;
                    action.timeOfClick = DateTime.Now;

                    Console.WriteLine("Image Clicked : " + clickedImageID);
                    imageObject clickedImage = this.localExamObject.imageList[clickedImageID - 1];
                    clickedImage.setClicked(localExamObject.verticalOffset, adjustSize);

                    action.isCrossed = clickedImage.isClicked;

                    this.tracker.addAction(action);
                }
            }

            //Draw the images in the list
            foreach (imageObject img in localExamObject.imageList)
            {

                int halfWidth = (int)Math.Round(img.width * adjustSize) / 2;
                int halfHeight = (int)Math.Round(img.height * adjustSize) / 2;

                int adjustedHeight = (int)Math.Round(img.height * adjustSize);
                int adjustedWidth = (int)Math.Round(img.width * adjustSize);

                int adjustedX = (int)Math.Round(img.imageCenter.X * adjustSize);
                int adjustedY = (int)Math.Round((img.imageCenter.Y + localExamObject.verticalOffset) * adjustSize);

                e.Graphics.DrawImage(imageObject.getImageObject(img.imageType), adjustedX - halfWidth, adjustedY - halfHeight, adjustedWidth, adjustedHeight);

                if (img.isClicked)
                {
                    int x1 = adjustedX;

                    int y1 = adjustedY;


                    int x2 = x1 + adjustedWidth;

                    int y2 = y1 + adjustedHeight;

                    g.DrawLine(Pens.Black, x1, y1, x2, y2);

                }

                e.Graphics.DrawRectangle(Pens.Black, adjustedX - halfWidth, adjustedY - halfWidth, adjustedWidth, adjustedHeight);
            }

            dc.DrawLine(GreenPen, 65, 0, 65, 756);
            dc.DrawLine(GreenPen, 0, 69, 1024, 69);


            //Draw the hozizontal lines
            for (i = 0; i < this.numberOfHorizontalGrids; i++)
            {
                int adjustedX = (int)Math.Round(this.horizontalLines[i] * adjustSize);
                int adjustedScreenHeight = (int)Math.Round(screenheight * adjustSize);
                int adjustedY = (int)Math.Round(localExamObject.verticalOffset * adjustSize);
                dc.DrawLine(GreenPen, adjustedX, adjustedY, adjustedX, adjustedY + adjustedScreenHeight);
                //dc.DrawLine(GreenPen, this.horizontalLines[i], 0, this.horizontalLines[i], screenheight);
            }

            //Draw the vertical lines
            for (i = 0; i < this.numberOfVerticalGrids; i++)
            {
                int adjustedY = (int)Math.Round((this.verticalLines[i] + localExamObject.verticalOffset) * adjustSize);
                int adjustedScreenWidth = (int)Math.Round(screenwidth * adjustSize);

                dc.DrawLine(GreenPen, 0, adjustedY, adjustedScreenWidth, adjustedY);
                //dc.DrawLine(RedPen, 0, this.verticalLines[i], screenwidth, this.verticalLines[i]);
            }
        }

        private void Draw2DPoint(Graphics dc, Pen pen, Point pt)
        {
            dc.DrawLine(pen, pt.X - 7, pt.Y, pt.X + 7, pt.Y);
            dc.DrawLine(pen, pt.X, pt.Y - 7, pt.X, pt.Y + 7);
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

        public int getMatrixLocation(int x, int y)
        {
            int cellWidth = screenwidth / this.numberOfHorizontalGrids;
            int cellHeight = screenheight / this.numberOfVerticalGrids;

            int row = y / cellHeight;
            int col = x / cellWidth;

            return row * this.numberOfHorizontalGrids + col;
        }

        public int getMatrixLocation(int ImageID)
        {
            Point imgCenter = this.images[ImageID - 1].imageCenter;
            return getMatrixLocation(imgCenter.X, imgCenter.Y);
        }

        public static float GetWindowsDisplayScale()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                // Get the DPI for the X and Y axis
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;

                // Assuming the scale is uniform, you can use either dpiX or dpiY
                // The default DPI is 96, so the scale factor is dpi / 96
                float scaleFactor = dpiX / 96.0f;

                return scaleFactor;
            }
        }


        [STAThread]
        static void Main()
        {
            abstractTestClass test = new  assesmentExam();
            abstractTestClass p1 = new praticeExam1();
            abstractTestClass p2 = new praticeExam2();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new examParent(test, 1.0));
            //Application.Run(new examParent(p1, 1.0));
            //Application.Run(new praticeParent(p2,1.0));
            //Application.Run(new screenSizeAdjustment());
            Application.Run(new Menu());
            //Application.Run(new IntermediateScreen());


            //Testing DPIUtil
            Console.WriteLine(DPIUtil.IsSupportingDpiPerMonitor);
            Console.WriteLine(GetWindowsDisplayScale());

            float scale = DisplayScaleHelper.GetWindowsDisplayScale();
            Console.WriteLine($"Windows display scale factor: {scale}");


            //Get the current screen scale

        }
    }

    

    



    
}