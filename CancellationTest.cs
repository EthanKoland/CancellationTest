using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using OfficeOpenXml;


namespace CancellationTest
{
    public abstract class testType
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


        
        public abstract void ReturnTargets();

        public List<image> imageList {  get; protected set; }
        public Point cells { get; protected set; }
    }

    public class randomTest : testType 
    { 
   
        private int[,] bitMap = null;
        public randomTest(int screenWidth = 1200, int screenHeight = 675)
        {
            //Horizontal then vertical
            cells = new Point(5, 2);

            imageList = new List<image>();

            this.cellWidth = 0;
            this.cellHeight = 0;

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.smallMugsize = 40;

            this.targetsPerCell = 5;
            this.distractorsPerCell = 5;

            this.numberOfLargeTargetPerCell = 1;
            this.numberOfLargeLeftGap = 1;
            this.numberOfLargeRightGap = 1;

            this.bitMap = new int[this.screenWidth, this.screenHeight];

            createImageList();
        }


        public override void ReturnTargets()
        {
            
        }

        private void createImageList()
        {
            
            this.cellWidth = this.screenWidth / this.cells.X;
            this.cellHeight = this.screenHeight / this.cells.Y;

            double midCell = (double) this.cells.X / 2;

            Random random = new Random(29);

            bool isLeft = true;
            for (int row = 0; row < this.cells.X; row++)
            {
                //iterate through the number of horizontal grids
                for (int col = 0; col < this.cells.Y; col++)
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



                        this.imageList.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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

                        this.imageList.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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

                        this.imageList.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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

        //The sizes fo the smaller and larger mugs
        private int largeMugsize = 40;
        private int smallMugsize;

        //Inorder to standardize the screen size a variable is declared to adjust the size of the screen
        private double adjustSize = 1.0;

        //The none adjustsize of the screen
        private int screenwidth = 1200;
        private int screenheight = 675;

        //Vars that are declared in class init
        private int numberOfHorizontalGrids; // a variable that defines the number of grids in the horizontal direction
        private int numberOfVerticalGrids; // a variable that defines the number of grids in the vertical direction
        private int targetsPerCell; // a variable that defines the number of targets in each cell
        private int distractorsPerCell;
        private double smallImageSize;
        private int numberOfLargeTargetPerCell;
        private int numberOfLargeLeftGap;
        private int numberOfLargeRightGap;

        //Storage of the lines seperating the cells
        private int[] horizontalLines;
        private int[] verticalLines;

        //Refrences for the images
        private List<image> images = new List<image>();

        //Action tracker
        private actionTracker tracker;


        public ImageViewer(double adjustSize = 1.0,
            int numberOfHorizontalGrids = 5, int numberOfVerticalGrids = 2, int targetsPerCell = 5, int distractorsPerCell = 5,
            double smallImageSize = 0.75, int numberOfLargeTargetPerCell = 1, int numberOfLargeLeftGap = 1, int numberOfLargeRightGap = 1)
        {
            //Assign the parameter values to the variables
            this.adjustSize = adjustSize;
            this.numberOfHorizontalGrids = numberOfHorizontalGrids;
            this.numberOfVerticalGrids = numberOfVerticalGrids;
            this.targetsPerCell = targetsPerCell;
            this.distractorsPerCell = distractorsPerCell;
            this.smallImageSize = smallImageSize;
            this.numberOfLargeTargetPerCell = numberOfLargeTargetPerCell;
            this.numberOfLargeLeftGap = numberOfLargeLeftGap;
            this.numberOfLargeRightGap = numberOfLargeRightGap;

            //Create the bitMap
            this.bitMap = new int[this.screenwidth, this.screenheight];


            //Calculate the large mug size
            this.smallMugsize = Convert.ToInt32(this.largeMugsize * this.smallImageSize);

            //Calculate the horizontal lines
            this.horizontalLines = new int[this.numberOfHorizontalGrids];
            int cellWidth = screenwidth / this.numberOfHorizontalGrids;

            for (int i = 0; i < this.numberOfHorizontalGrids; i++)
            {
                this.horizontalLines[i] = i * cellWidth;
            }

            //Calculate the vertical lines
            this.verticalLines = new int[this.numberOfVerticalGrids];
            int cellHeight = screenheight / this.numberOfVerticalGrids;
            for (int i = 0; i < this.numberOfVerticalGrids; i++)
            {
                this.verticalLines[i] = i * cellHeight;
            }

            //Determining the location of the images on the secreen
            //iterate through the number of vertical grids
            Random random = new Random(29);
            bool isLeft = true;
            double midCell = this.numberOfHorizontalGrids / 2.0;
            for (int row = 0; row < this.numberOfVerticalGrids; row++)
            {
                //iterate through the number of horizontal grids
                for (int col = 0; col < this.numberOfHorizontalGrids; col++)
                {
                    leftRightCenter side = col < midCell ? leftRightCenter.Left : col == midCell ? leftRightCenter.Center : leftRightCenter.Right;

                    int matrixLocation = row * this.numberOfHorizontalGrids + col;

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
                        int cellXMax = Math.Min(screenwidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenheight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);
                            if (!isOverlap(x, y, imgSize))
                            {
                                Console.WriteLine("Overlap");
                            }
                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.images.Count + 1;

                        Console.WriteLine("Image ID : " + nextImageID);



                        this.images.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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
                        int cellXMax = Math.Min(screenwidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenheight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);
                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.images.Count + 1;

                        this.images.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
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
                        int cellXMax = Math.Min(screenwidth - imgSize / 2, (col + 1) * cellWidth);

                        int cellYMin = Math.Max(imgSize / 2, row * cellHeight);
                        int cellYMax = Math.Min(screenheight - imgSize / 2, (row + 1) * cellHeight);

                        do
                        {
                            x = random.Next(cellXMin, cellXMax);
                            y = random.Next(cellYMin, cellYMax);

                        } while (isOverlap(x, y, imgSize));

                        int nextImageID = this.images.Count + 1;

                        this.images.Add(new image(nextImageID, type, new Point(x, y), imgSize, imgSize, side, matrixLocation));
                        updateBitMap(x - halfImgSize, x + halfImgSize, y - halfImgSize, y + halfImgSize, nextImageID);
                    }

                }
            }

            this.tracker = new actionTracker(this.images);
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(ImageViewer_KeyDown);
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

            CurPt.X = e.X;
            CurPt.Y = e.Y;




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
            else if (e.KeyCode == Keys.E) {
                Console.WriteLine("E Key Pressed");
                this.tracker.export("test.xlsx");
            }
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

            if (CurPt.X >= 0)
            {
                Draw2DPoint(dc, GreenPen, CurPt);

                Console.WriteLine("Mouse Clicked at : " + CurPt.X + " " + CurPt.Y);

                //Get the image that is clicked
                int clickedImageID = this.bitMap[CurPt.X, CurPt.Y];

                if (clickedImageID != 0)
                {
                    clickAction action = new clickAction();
                    action.clickPoint = new Point(CurPt.X, CurPt.Y);
                    action.ImageID = clickedImageID;
                    action.timeOfClick = DateTime.Now;

                    Console.WriteLine("Image Clicked : " + clickedImageID);
                    image clickedImage = this.images[clickedImageID - 1];
                    clickedImage.setClicked(dc);

                    action.isCrossed = clickedImage.isClicked;

                    this.tracker.addAction(action);
                }
            }

            //Draw the images in the list
            foreach (image img in this.images)
            {

                int halfWidth = img.width / 2;
                int halfHeight = img.height / 2;
                e.Graphics.DrawImage(image.getImageObject(img.imageType), img.imageCenter.X - halfWidth, img.imageCenter.Y - halfHeight, img.width, img.height);
                img.drawChecked(e.Graphics);
                e.Graphics.DrawRectangle(Pens.Black, img.imageCenter.X - halfWidth, img.imageCenter.Y - halfWidth, img.width, img.height);
            }


            //Draw the hozizontal lines
            for (i = 0; i < this.numberOfHorizontalGrids; i++)
            {
                dc.DrawLine(GreenPen, this.horizontalLines[i], 0, this.horizontalLines[i], screenheight);
            }

            //Draw the vertical lines
            for (i = 0; i < this.numberOfVerticalGrids; i++)
            {
                dc.DrawLine(RedPen, 0, this.verticalLines[i], screenwidth, this.verticalLines[i]);
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
            this.ClientSize = new System.Drawing.Size(screenwidth, screenheight);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ImageViewer";
            this.Text = "CancellationTest";
            this.ResumeLayout(false);

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


        [STAThread]
        static void Main()
        {
            Application.Run(new ImageViewer());
        }
    }

    public struct clickAction
    {
        public Point clickPoint;
        public int ImageID;
        public DateTime timeOfClick;
        public bool isCrossed;
    }

    public enum leftRightCenter
    {
        Left,
        Right,
        Center
    }
    public class image
    {
        public int imageID { get; private set; }
        public imageTypes imageType { get; private set; }
        public Point imageCenter { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public bool isClicked { get; private set; }

        public leftRightCenter side { get; private set; }

        public int matrixLocation { get; private set; }

        public image(int imageID, imageTypes imageType, Point imageCenter, int width, int height, leftRightCenter side, int matrixLocation)
        {
            this.imageID = imageID;
            this.imageType = imageType;
            this.imageCenter = imageCenter;
            this.width = width;
            this.height = height;
            this.isClicked = false;
            this.side = side;
            this.matrixLocation = matrixLocation;
        }

        public void setClicked(Graphics g)
        {
            //If the image is clicked then set the isClicked to true, but if it is already clicked then set it to false
            this.isClicked = !this.isClicked;
            Console.WriteLine("Image Clicked : " + this.imageID + "is clicked " + this.isClicked);

            this.drawChecked(g);

        }

        public void drawChecked(Graphics g)
        {
            //Draw the cross on the image
            if (this.isClicked)
            {
                g.DrawLine(Pens.Black, this.imageCenter.X - this.width / 2, this.imageCenter.Y - this.height / 2, this.imageCenter.X + this.width / 2, this.imageCenter.Y + this.height / 2);
                //g.DrawLine(Pens.Black, this.imageCenter.X - this.width / 2, this.imageCenter.Y + this.height / 2, this.imageCenter.X + this.width / 2, this.imageCenter.Y - this.height / 2);
            }
        }

        public Boolean isDistractor()
        {
            return this.imageType == imageTypes.DistractionLeft || this.imageType == imageTypes.DistractionRight;
        }

        static public Image getImageObject(imageTypes types)
        {
            if (types == imageTypes.DistractionLeft)
            {
                return Image.FromFile("left60.jpg");
            }
            else if (types == imageTypes.DistractionRight)
            {
                return Image.FromFile("right60.jpg");
            }
            else if (types == imageTypes.TargetLeft)
            {
                return Image.FromFile("leftB60.jpg");
            }
            else if (types == imageTypes.TargetRight)
            {
                return Image.FromFile("rightB60.jpg");
            }
            else
            {
                return null;
            }
        }

        static public string imageOrietation(imageTypes types)
        {
            if (types == imageTypes.DistractionLeft)
            {
                return "Left";
            }
            else if (types == imageTypes.DistractionRight)
            {
                return "Right";
            }
            else if (types == imageTypes.TargetLeft)
            {
                return "Left";
            }
            else if (types == imageTypes.TargetRight)
            {
                return "Right";
            }
            else
            {
                return "Unknown";
            }
        }
    }

    public enum imageTypes {
        DistractionLeft,
        DistractionRight,
        TargetLeft,
        TargetRight
    }

    class actionTracker
    {
        private List<clickAction> actions = new List<clickAction>();
        private List<image> images = new List<image>();

        private string patientID;

        private int numOfHorizontalCells;
        private DateTime startTime;

        private Double sizeRatio = 1.0;



        public actionTracker(List<image> images, string patientID = "Unknown", int numOfHorizontalCells = 5, double sizeRatio = 1.0)
        {
            this.actions = new List<clickAction>();
            this.images = images;
            this.patientID = patientID;
            this.numOfHorizontalCells = numOfHorizontalCells;
            this.startTime = DateTime.Now;
            this.sizeRatio = sizeRatio;
        }

        public void addAction(clickAction action) { this.actions.Add(action); }

        public void export(string filename)
        {

            DateTime endTime = DateTime.Now;

            int leftTargetsCrossed = 0;
            int rightTargetsCrossed = 0;
            int centerTargetsCrossed = 0;
            int numberOfTargets = 0;

            //Total number of tragets for each side
            int leftTargets = 0;
            int rightTargets = 0;
            int centerTargets = 0;

            int leftGapLeftDistractorsCrossed = 0;
            int leftGapRightDistractorsCrossed = 0;
            int leftGapCenterDistractorsCrossed = 0;
            int rightGapLeftDistractorsCrossed = 0;
            int rightGapRightDistractorsCrossed = 0;
            int rightGapCenterDistractorsCrossed = 0;

            int leftGapLeftDistractors = 0;
            int leftGapRightDistractors = 0;
            int leftGapCenterDistractors = 0;
            int rightGapLeftDistractors = 0;
            int rightGapRightDistractors = 0;
            int rightGapCenterDistractors = 0;

            int distractorsCrossed = 0;

            double totalTimeTaken = (endTime - this.startTime).TotalSeconds;
            double leftTimeTaken = 0;
            double rightTimeTaken = 0;

            int rightReclicks = 0;
            int leftReclicks = 0;
            int totalClicks = this.actions.Count;

            double searchSpeed = 0;
            double leftSearchSpeed = 0;
            double rightSearchSpeed = 0;

            DateTime previousTime = this.startTime;

            int reCancellations = 0;

            double[] intersectionResult = intersections(actions, 3);


            //Calculate the final results
            foreach (image img in this.images)
            {
                if (img.imageType == imageTypes.TargetLeft || img.imageType == imageTypes.TargetRight)
                {
                    numberOfTargets++;
                    if (img.side == leftRightCenter.Left)
                    {
                        leftTargets++;
                        leftTargetsCrossed += img.isClicked ? 1 : 0;
                    }
                    else if (img.side == leftRightCenter.Right)
                    {
                        rightTargets++;
                        rightTargetsCrossed += img.isClicked ? 1 : 0;
                    }
                    else
                    {
                        centerTargets++;
                        centerTargetsCrossed += img.isClicked ? 1 : 0;
                    }
                }
                else
                {
                    distractorsCrossed++;

                    if (img.side == leftRightCenter.Left)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            leftGapLeftDistractorsCrossed += img.isClicked ? 1 : 0;
                            leftGapCenterDistractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            leftGapRightDistractorsCrossed += img.isClicked ? 1 : 0;
                            leftGapRightDistractors += 1;
                        }
                    }
                    else if (img.side == leftRightCenter.Right)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            rightGapLeftDistractorsCrossed += img.isClicked ? 1 : 0;
                            rightGapLeftDistractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            rightGapRightDistractorsCrossed += img.isClicked ? 1 : 0;
                            rightGapRightDistractors += 1;
                        }
                    }
                    else
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            leftGapCenterDistractorsCrossed += img.isClicked ? 1 : 0;
                            leftGapCenterDistractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            rightGapCenterDistractorsCrossed += img.isClicked ? 1 : 0;
                            rightGapCenterDistractors += 1;
                        }
                    }

                }
            }

            DateTime previousSideTime = this.startTime;

            clickAction firstAction = this.actions[0];
            image firstImage = this.images[firstAction.ImageID - 1];
            leftRightCenter priviousSide = firstImage.side;

            foreach (clickAction action in this.actions)
            {
                image clickedImage = this.images[action.ImageID - 1];
                leftRightCenter side = clickedImage.side;

                //Check if the side has changed
                if (side != priviousSide)
                {
                    double timeTaken = (action.timeOfClick - previousSideTime).TotalSeconds;
                    if (priviousSide == leftRightCenter.Left)
                    {
                        leftTimeTaken += timeTaken;
                    }
                    else
                    {
                        rightTimeTaken += timeTaken;
                    }

                    previousSideTime = action.timeOfClick;
                    priviousSide = side;
                }

                //Calculate the speed of the search





            }

            //Merge the file name with the current directory
            filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), filename);

            Console.WriteLine("FileName " + filename);


            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(filename)))
            {

                Console.WriteLine("Exporting to Excel");

                //Check if the worksheet exists
                if (excel.Workbook.Worksheets["Sheet1"] != null)
                {
                    excel.Workbook.Worksheets.Delete("Sheet1");
                }

                excel.Workbook.Worksheets.Add("Sheet1");

                var worksheet = excel.Workbook.Worksheets["Sheet1"];

                worksheet.Cells[1, 1].Value = "Date:";
                worksheet.Cells[1, 2].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                worksheet.Cells[1, 3].Value = "Patient ID:";
                worksheet.Cells[1, 4].Value = this.patientID;

                //Some sort of informatino on the cellencaltion condition
                worksheet.Cells[2, 1].Value = "Unsure";

                //Session Resume
                worksheet.Cells[3, 1].Value = "Session Resume";

                worksheet.Cells[4, 1].Value = "Targets Cancelled";
                //Ration of the number of targets cancelled versus the total number of targets
                worksheet.Cells[4, 2].Value = "" + (leftTargetsCrossed + rightTargetsCrossed + centerTargetsCrossed) + "/" + numberOfTargets;

                worksheet.Cells[5, 1].Value = "Distractors Cancelled";
                //Ration of the number of distractors cancelled versus the total number of distractors
                worksheet.Cells[5, 2].Value = "" + distractorsCrossed + "/" + (this.images.Count - numberOfTargets);

                worksheet.Cells[6, 1].Value = "Re-cancellations";
                worksheet.Cells[6, 2].Value = reCancellations;

                worksheet.Cells[7, 1].Value = "Total Time Taken";
                worksheet.Cells[7, 2].Value = totalTimeTaken;

                worksheet.Cells[9, 1].Value = "Time Spent on the right side";
                worksheet.Cells[9, 2].Value = (Math.Round(rightTimeTaken / totalTimeTaken, 4) * 100) + "%";

                worksheet.Cells[10, 1].Value = "Time Spent on the left side";
                worksheet.Cells[10, 2].Value = (Math.Round(leftTimeTaken / totalTimeTaken, 4) * 100) + "%";

                worksheet.Cells[11, 1].Value = "Asymmetry Score";
                worksheet.Cells[11, 2].Value = Math.Round((rightTimeTaken - leftTimeTaken) / totalTimeTaken, 4);

                worksheet.Cells[13, 1].Value = "Search Speed";
                worksheet.Cells[13, 2].Value = searchSpeed;

                worksheet.Cells[14, 1].Value = "Left Search Speed";
                worksheet.Cells[14, 2].Value = leftSearchSpeed;

                worksheet.Cells[15, 1].Value = "Right Search Speed";
                worksheet.Cells[15, 2].Value = rightSearchSpeed;

                worksheet.Cells[17, 1].Value = "Reclicks on the right side";
                worksheet.Cells[17, 2].Value = rightReclicks;
                worksheet.Cells[17, 3].Value = "0.00% - not working";

                worksheet.Cells[18, 1].Value = "Reclicks on the left side";
                worksheet.Cells[18, 2].Value = leftReclicks;
                worksheet.Cells[18, 3].Value = "0.00% - not working";

                worksheet.Cells[20, 1].Value = "Accuracy per location";

                worksheet.Cells[21, 1].Value = "Left";
                worksheet.Cells[21, 2].Value = Math.Round((double)leftTargetsCrossed / leftTargets, 4) * 100 + "%";

                worksheet.Cells[22, 1].Value = "Center";
                worksheet.Cells[22, 2].Value = Math.Round((double)centerTargetsCrossed / centerTargets, 4) * 100 + "%";

                worksheet.Cells[23, 1].Value = "Right";
                worksheet.Cells[23, 2].Value = Math.Round((double)rightTargetsCrossed / rightTargets, 4) * 100 + "%";

                worksheet.Cells[25, 1].Value = "Distractors crossed in each screen region";
                worksheet.Cells[26, 1].Value = "Left Gap";

                worksheet.Cells[27, 1].Value = "Left";
                worksheet.Cells[27, 2].Value = Math.Round((double)leftGapLeftDistractorsCrossed / leftGapLeftDistractors, 4) * 100 + "%";

                worksheet.Cells[28, 1].Value = "Center";
                worksheet.Cells[28, 2].Value = Math.Round((double)leftGapCenterDistractorsCrossed / leftGapCenterDistractors, 4) * 100 + "%";

                worksheet.Cells[29, 1].Value = "Right";
                worksheet.Cells[29, 2].Value = Math.Round((double)leftGapRightDistractorsCrossed / leftGapRightDistractors, 4) * 100 + "%";

                worksheet.Cells[30, 1].Value = "Right Gap";

                worksheet.Cells[31, 1].Value = "Left";
                worksheet.Cells[31, 2].Value = Math.Round((double)rightGapLeftDistractorsCrossed / rightGapLeftDistractors, 4) * 100 + "%";

                worksheet.Cells[32, 1].Value = "Center";
                worksheet.Cells[32, 2].Value = Math.Round((double)rightGapCenterDistractorsCrossed / rightGapCenterDistractors, 4) * 100 + "%";

                worksheet.Cells[33, 1].Value = "Right";
                worksheet.Cells[33, 2].Value = Math.Round((double)rightGapRightDistractorsCrossed / rightGapRightDistractors, 4) * 100 + "%";

                worksheet.Cells[35, 1].Value = "Egocentric neglect Subscores";

                worksheet.Cells[37, 1].Value = "Total Targets Cancelled in right side of grid";
                worksheet.Cells[37, 2].Value = rightTargetsCrossed;

                worksheet.Cells[38, 1].Value = "Total Targets Cancelled in left side of grid";
                worksheet.Cells[38, 2].Value = leftTargetsCrossed;

                worksheet.Cells[39, 1].Value = "Egocentric neglect";
                worksheet.Cells[39, 2].Value = leftTargetsCrossed - rightTargetsCrossed;

                worksheet.Cells[41, 1].Value = "Allocentric neglect Subscores";

                worksheet.Cells[42, 1].Value = "Total number of left-gap distractors cancelled";
                worksheet.Cells[42, 2].Value = leftGapLeftDistractorsCrossed + leftGapCenterDistractorsCrossed + leftGapRightDistractorsCrossed;

                worksheet.Cells[43, 1].Value = "Total number of right-gap distractors cancelled";
                worksheet.Cells[43, 2].Value = rightGapLeftDistractorsCrossed + rightGapCenterDistractorsCrossed + rightGapRightDistractorsCrossed;

                worksheet.Cells[44, 1].Value = "Allocentric neglect";
                worksheet.Cells[44, 2].Value = (leftGapLeftDistractorsCrossed + leftGapCenterDistractorsCrossed + leftGapRightDistractorsCrossed) - (rightGapLeftDistractorsCrossed + rightGapCenterDistractorsCrossed + rightGapRightDistractorsCrossed);

                worksheet.Cells[46, 1].Value = "Intersections";

                worksheet.Cells[47, 1].Value = "Number of intersections";
                //worksheet.Cells[47,2].Value = transactionResult[0];
                worksheet.Cells[47, 2].Value = "Not working";

                worksheet.Cells[48, 1].Value = "Intersection Rate";
                //worksheet.Cells[48,2].Value = transactionResult[1];
                worksheet.Cells[48, 2].Value = "Not working";

                worksheet.Cells[50, 1].Value = "Session History";

                worksheet.Cells[51, 1].Value = "Success";
                worksheet.Cells[51, 2].Value = "Time of Cancellation";
                worksheet.Cells[51, 3].Value = "Location in Matrix";
                worksheet.Cells[51, 4].Value = "Side in Matrix";
                worksheet.Cells[51, 5].Value = "Orientatiom";
                worksheet.Cells[51, 6].Value = "Re-Cancelled";
                worksheet.Cells[51, 7].Value = "Pixel Location";
                worksheet.Cells[51, 8].Value = "Normalized Position";

                int row = 52;
                foreach (clickAction action in this.actions)
                {
                    image clickedImage = this.images[action.ImageID - 1];
                    worksheet.Cells[row, 1].Value = clickedImage.isClicked ? "Target Succesfully Cancelled" : "Distractor";
                    worksheet.Cells[row, 2].Value = timeSinceStart(action.timeOfClick);
                    worksheet.Cells[row, 3].Value = "1/10";
                    worksheet.Cells[row, 4].Value = clickedImage.side;
                    worksheet.Cells[row, 5].Value = image.imageOrietation(clickedImage.imageType);
                    worksheet.Cells[row, 6].Value = action.isCrossed ? "Yes" : "";
                    worksheet.Cells[row, 7].Value = action.clickPoint;
                    worksheet.Cells[row, 8].Value = (action.clickPoint.X * this.sizeRatio) + ", " + (action.clickPoint.Y * this.sizeRatio);

                    row++;
                }



                excel.Save();

            };
        }

        public string timeSinceStart(DateTime timeOfEvent)
        {
            return (timeOfEvent - this.startTime).ToString();
        }

        static double[] intersections(List<clickAction> actions, int numRevisit)
        {
            if (actions.Count < 4) return new double[] { 0, 0 };

            double result = 0;
            int nIntersect = calculateNIntersect(actions);
            int Ncancellation = actions.Count;

            result = (float)nIntersect / (Ncancellation - numRevisit);

            return new double[] { nIntersect, result };
        }

        static private int calculateNIntersect(List<clickAction> actions)
        {
            int nIntersect = 0;
            Vector2 intersection = new Vector2();
            bool segmentIntersect;

            Point imgPoint1i;
            Point imgPoint2i;
            Point imgPoint1j;
            Point imgPoint2j;

            for (int i = 0; i < actions.Count - 1 ; i++)
            {
                imgPoint1i = actions[i].clickPoint;
                imgPoint2i = actions[i + 1].clickPoint;

                for (int j = i + 2; j < actions.Count - 1 ; j++)
                {
                    imgPoint1j = actions[j].clickPoint;
                    imgPoint2j = actions[j + 1].clickPoint;

                    FindIntersection(imgPoint1i, imgPoint2i, imgPoint1j, imgPoint2j, out segmentIntersect, out intersection);

                    if (segmentIntersect)
                    {
                        nIntersect++;
                    }
                }
            }

            return nIntersect;

        }

        private static void FindIntersection(Point p1, Point p2, Point p3, Point p4, out bool segments_intersect, out Vector2 intersection)
        {

            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;

            if (float.IsInfinity(t1))
            {
                segments_intersect = false;
                intersection = new Vector2(float.NaN, float.NaN);
                return;
            }

            float t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            intersection = new Vector2(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            segments_intersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

        }

    } }