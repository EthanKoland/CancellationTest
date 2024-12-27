using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    //Action Tracker is used to track the actions of each of exams. This was broken into a seperate class to seperate the front end from the backend of the program. The main goal is to output the data to an excel file
    public class actionTracker
    {
        private List<clickAction> actions = new List<clickAction>();
        private abstractTestClass localExamObject;

        private string patientID;

        private int numOfHorizontalCells;
        private DateTime startTime;

        private Double sizeRatio = 1.0;

        private String outputPath;

        private Dictionary<int, int> reclicks;

        private bool invisableCancelation;

        public DateTime endTime { get; set; }

        public actionTracker(abstractTestClass examObject, bool invisableCancelation, string patientID = "Unknown", int numOfHorizontalCells = 5, double sizeRatio = 1.0)
        {
            //Var creation and initialization
            this.actions = new List<clickAction>();
            this.localExamObject = examObject;
            this.patientID = patientID;
            this.numOfHorizontalCells = numOfHorizontalCells;
            this.startTime = DateTime.Now;
            this.sizeRatio = sizeRatio;
            this.outputPath = AppDomain.CurrentDomain.BaseDirectory;
            this.reclicks = new Dictionary<int, int>();
            this.invisableCancelation = invisableCancelation;
        }

        //Because actions is a private variable, we need to create a method to add actions to the list. This simply controlls the action being added to the list
        public void addAction(clickAction action) { 
            this.actions.Add(action);
            this.reclicks[action.ImageID] = this.reclicks.ContainsKey(action.ImageID) ? this.reclicks[action.ImageID] + 1 : 1;
        }

        //This is the main export function that creates the excel file from the actions
        public void export( List<abstractExportClass> exportClasses, string baseFolderLocation = null)
        {

            DateTime endTime = DateTime.Now;

            //Variables to store the results 
            int leftTargetsCrossed = 0;
            int rightTargetsCrossed = 0;
            int centerTargetsCrossed = 0;
            int numberOfTargets = 0;

            //Total number of tragets for each side
            int leftTargets = 0;
            int rightTargets = 0;
            int centerTargets = 0;

            int leftSide_leftGap_DistractorsCrossed = 0;
            int leftSide_rightGap_DistractorsCrossed = 0;
            int center_leftGap_DistractorsCrossed = 0;
            int rightSide_LeftGap_DistractorsCrossed = 0;
            int rightSide_rightGap_DistractorsCrossed = 0;
            int center_rightGap_DistractorsCrossed = 0;

            int leftSide_leftGap_Distractors = 0;
            int leftSide_rightGap_Distractors = 0;
            int center_leftGap_Distractors = 0;
            int rightSide_LeftGap_Distractors = 0;
            int rightSide_rightGap_Distractors = 0;
            int center_rigthGap_Distractors = 0;

            int distractorsCrossed = 0;

            double totalTimeTaken = (endTime - this.startTime).TotalSeconds;
            double leftTimeTaken = 0;
            double rightTimeTaken = 0;

            int rightReclicks = 0;
            int leftReclicks = 0;
            int totalClicks = this.actions.Count;

            int leftAllocentric = 0;
            int rightAllocentric = 0;

            double searchSpeed = 0;
            double leftSearchSpeed = 0;
            double rightSearchSpeed = 0;
            double distanceLeft = 0;
            double distanceRight = 0;

            DateTime previousTime = this.startTime;

            int reCancellations = 0;

            int intersections = intersectionCount();

            List<clickRow> clickRows = new List<clickRow>();

            //Protect against no action being taken and causing our or range errors
            if (this.actions.Count == 0)
            {
                return;
            }


            //Loop through each of the mugs to tally the mugs clicked
            foreach (mugObject img in localExamObject.imageList)
            {
                //Check if the image is a target or a distractor
                if (img.imageType == imageTypes.TargetLeft || img.imageType == imageTypes.TargetRight)
                {
                    //Incriment the number of targets
                    numberOfTargets++;

                    //If statement to sperate and tally the correct tally based on location
                    if (img.side == leftRightCenter.Left)
                    {
                        leftTargets++;
                        //Terinary operator to check if the image was clicked
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

                    if (img.isClicked && img.imageType == imageTypes.TargetLeft)
                    {
                        leftAllocentric++;
                    }
                    else if (img.isClicked && img.imageType == imageTypes.TargetRight)
                    {
                        rightAllocentric++;
                    }
                }
                //If the image is not a target, then it is a distractor
                else
                {
                    //Incriment the number of distractors
                    if (img.isClicked)
                    {
                        distractorsCrossed++;
                    }

                    if (img.side == leftRightCenter.Left)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            leftSide_leftGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            leftSide_leftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            leftSide_rightGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            leftSide_rightGap_Distractors += 1;
                        }
                    }
                    else if (img.side == leftRightCenter.Right)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            rightSide_LeftGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            rightSide_LeftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            rightSide_rightGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            rightSide_rightGap_Distractors += 1;
                        }
                    }
                    else
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            center_leftGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            center_leftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            center_rightGap_DistractorsCrossed += img.isClicked ? 1 : 0;
                            center_rigthGap_Distractors += 1;
                        }
                    }

                }
                //Calculating Reclicks
                if (this.reclicks.ContainsKey(img.imageID))
                {

                    int imgX = img.imageCenter.X;

                    if (0.5 > imgX / this.localExamObject.screenWidth)
                    {
                        leftReclicks += (this.reclicks[img.imageID] - 1);
                    }
                    else
                    {
                        rightReclicks += (this.reclicks[img.imageID] - 1);
                    }
                }
            }

            //Starting point for the caluclations on the time taken on each side of the screen
            DateTime previousSideTime = this.startTime;
            Point previousPoint = this.actions.Count > 0 ? this.actions[0].clickPoint : new Point(0, 0);

            clickAction firstAction = this.actions[0];
            mugObject firstImage = this.localExamObject.imageList[firstAction.ImageID - 1];
            int maxX = this.localExamObject.screenWidth;
            int maxY = this.localExamObject.screenHeight;

            leftRightCenter priviousSide = firstAction.clickPoint.X / maxX < 0.5 ? leftRightCenter.Left : leftRightCenter.Right;


            //Loop through each of the click actions
            for (int i = 1; i < this.actions.Count; i++)
            {
                clickAction action = this.actions[i];


                //Retrieve the image that was clicked
                mugObject clickedImage = this.localExamObject.imageList[action.ImageID - 1];


                double localX = (double)action.clickPoint.X / maxX;

                leftRightCenter side = localX < 0.5 ? leftRightCenter.Left : leftRightCenter.Right;


                //Check if the side has changed - If it isn't then the patient has switched sides on the screen so need to determine attributes of the side
                if (side != priviousSide)
                {
                    double timeTaken = (action.timeOfClick - previousSideTime).TotalSeconds;
                    double distance = Math.Sqrt(Math.Pow(previousPoint.X - action.clickPoint.X, 2) + Math.Pow(previousPoint.Y - action.clickPoint.Y, 2));

                    int xGreater = Math.Max(previousPoint.X, action.clickPoint.X);
                    int xLesser = Math.Min(previousPoint.X, action.clickPoint.X);

                    double modifierRight = (xGreater - maxX / 2) / distance;
                    double modifierLeft = (maxX / 2 - xLesser) / distance;
                    rightTimeTaken += timeTaken * modifierRight;
                    leftTimeTaken += timeTaken * modifierLeft;
                    distanceRight += distance * modifierRight;
                    distanceLeft += distance * modifierLeft;


                    priviousSide = side;


                }
                else
                {
                    if (side == leftRightCenter.Left)
                    {
                        leftTimeTaken += (action.timeOfClick - previousSideTime).TotalSeconds;
                        distanceLeft += Math.Sqrt(Math.Pow(previousPoint.X - action.clickPoint.X, 2) + Math.Pow(previousPoint.Y - action.clickPoint.Y, 2));

                    }
                    else if (side == leftRightCenter.Right)
                    {
                        rightTimeTaken += (action.timeOfClick - previousSideTime).TotalSeconds;
                        distanceRight += Math.Sqrt(Math.Pow(previousPoint.X - action.clickPoint.X, 2) + Math.Pow(previousPoint.Y - action.clickPoint.Y, 2));

                    }
                }

                previousSideTime = action.timeOfClick;
                previousPoint = action.clickPoint;

                if (!action.isCrossed)
                {
                    reCancellations++;
                }

                //Calculate the speed of the search





            }

            searchSpeed = (distanceLeft + distanceRight) / (leftTimeTaken + rightTimeTaken);

            leftSearchSpeed = distanceLeft / leftTimeTaken;
            rightSearchSpeed = distanceRight / rightTimeTaken;

            double intersectionRate = (double)intersections / (double)(totalClicks - leftReclicks - rightReclicks);

            foreach (clickAction action in this.actions)
            {
                mugObject clickedImage = this.localExamObject.imageList[action.ImageID - 1];
                //worksheet.Cells[row, 1].Value = clickedImage.isClicked ? "Target Succesfully Cancelled" : "Distractor";

                clickRow newRow = new clickRow();

                if (clickedImage.imageType == imageTypes.TargetLeft || clickedImage.imageType == imageTypes.TargetRight)
                {
                    newRow.success = "Target Succesfully Cancelled";
                }
                else
                {
                    newRow.success = "Distractor";
                }
                newRow.time = timeSinceStart(action.timeOfClick);
                newRow.matrixLocation = clickedImage.matrixLocation.ToString();
                newRow.matrixSide = clickedImage.side == leftRightCenter.Left ? "Left" : clickedImage.side == leftRightCenter.Right ? "Right" : "Center";
                newRow.orientation = mugObject.imageOrietation(clickedImage.imageType);
                newRow.reCancel = action.isCrossed ? "" : "Yes";
                newRow.PixelLocation = action.clickPoint.ToString();
                newRow.normalizedLocation = new Point((int)(action.clickPoint.X * this.sizeRatio), (int)(action.clickPoint.Y * this.sizeRatio));

                clickRows.Add(newRow);
            }


            foreach (abstractExportClass exportClass in exportClasses)
            {

                exportClass.invisableCancelation = this.invisableCancelation;

                //Add the variables to the export class
                exportClass.left_TargetCrossed = leftTargetsCrossed;
                exportClass.right_TargetCrossed = rightTargetsCrossed;
                exportClass.center_TargetCrossed = centerTargetsCrossed;
                exportClass.total_Targets = numberOfTargets;

                exportClass.leftGap_DistractorsCrossed = leftSide_leftGap_DistractorsCrossed + center_leftGap_DistractorsCrossed + rightSide_LeftGap_DistractorsCrossed;
                exportClass.rightGap_DistractorsCrossed = leftSide_rightGap_DistractorsCrossed + center_rightGap_DistractorsCrossed + rightSide_rightGap_DistractorsCrossed;
                exportClass.total_DistractorsCrossed = distractorsCrossed;
                exportClass.total_Distractors = this.localExamObject.imageList.Count - numberOfTargets;

                exportClass.recancelations = reCancellations;

                exportClass.right_TimeTaken = rightTimeTaken;
                exportClass.left_TimeTaken = leftTimeTaken;
                exportClass.total_TimeTaken = totalTimeTaken;

                exportClass.right_SearchSpeed = rightSearchSpeed;
                exportClass.left_SearchSpeed = leftSearchSpeed;
                exportClass.total_SearchSpeed = searchSpeed;

                exportClass.right_Reclicks = rightReclicks;
                exportClass.left_Reclicks = leftReclicks;

                exportClass.left_Accuracy = (double)leftTargetsCrossed / leftTargets;
                exportClass.center_Accuracy = (double)centerTargetsCrossed / centerTargets;
                exportClass.right_Accuracy = (double)rightTargetsCrossed / rightTargets;

                exportClass.leftSide_leftGap_distractors = Math.Round((double)leftSide_leftGap_DistractorsCrossed / leftSide_leftGap_Distractors, 4) * 100;
                exportClass.leftSide_rightGap_distractors = Math.Round((double)leftSide_rightGap_DistractorsCrossed / leftSide_rightGap_Distractors, 4) * 100;

                exportClass.centerSide_leftGap_distractors = Math.Round((double)center_leftGap_DistractorsCrossed / center_leftGap_Distractors, 4) * 100;
                exportClass.centerSide_rightGap_distractors = Math.Round((double)center_rightGap_DistractorsCrossed / center_rigthGap_Distractors, 4) * 100;

                exportClass.rightSide_leftGap_distractors = Math.Round((double)rightSide_LeftGap_DistractorsCrossed / rightSide_LeftGap_Distractors, 4) * 100;
                exportClass.rightSide_rightGap_distractors = Math.Round((double)rightSide_rightGap_DistractorsCrossed / rightSide_rightGap_Distractors, 4) * 100;

                exportClass.intersections = intersections;
                exportClass.intersectionRate = intersectionRate;

                exportClass.addClickData(clickRows);
                exportClass.localExamObj = this.localExamObject;
                exportClass.export();

                /*//Update the output path to refect the current time. Also insures that the excel and photo have the same name and location
                this.updateOutputPath();



                filename = Path.Combine(this.outputPath, filename);

                Console.WriteLine("FileName " + filename);

                //Creating the excel file
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
                    worksheet.Cells[5, 2].Value = "" + distractorsCrossed + "/" + (this.localExamObject.imageList.Count - numberOfTargets);

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

                    worksheet.Cells[18, 1].Value = "Reclicks on the left side";
                    worksheet.Cells[18, 2].Value = leftReclicks;

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
                    worksheet.Cells[27, 2].Value = Math.Round((double)leftSide_leftGap_DistractorsCrossed / leftSide_leftGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[28, 1].Value = "Center";
                    worksheet.Cells[28, 2].Value = Math.Round((double)center_leftGap_DistractorsCrossed / center_leftGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[29, 1].Value = "Right";
                    worksheet.Cells[29, 2].Value = Math.Round((double)rightSide_LeftGap_DistractorsCrossed / rightSide_LeftGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[30, 1].Value = "Right Gap";

                    worksheet.Cells[31, 1].Value = "Left";
                    worksheet.Cells[31, 2].Value = Math.Round((double)leftSide_rightGap_DistractorsCrossed / leftSide_rightGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[32, 1].Value = "Center";
                    worksheet.Cells[32, 2].Value = Math.Round((double)center_rightGap_DistractorsCrossed / center_rigthGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[33, 1].Value = "Right";
                    worksheet.Cells[33, 2].Value = Math.Round((double)rightSide_rightGap_DistractorsCrossed / rightSide_rightGap_Distractors, 4) * 100 + "%";

                    worksheet.Cells[35, 1].Value = "Egocentric neglect Subscores";

                    worksheet.Cells[37, 1].Value = "Total Targets Cancelled in right side of grid";
                    worksheet.Cells[37, 2].Value = rightTargetsCrossed;

                    worksheet.Cells[38, 1].Value = "Total Targets Cancelled in left side of grid";
                    worksheet.Cells[38, 2].Value = leftTargetsCrossed;

                    worksheet.Cells[39, 1].Value = "Egocentric neglect";
                    worksheet.Cells[39, 2].Value = rightTargetsCrossed - leftTargetsCrossed;

                    worksheet.Cells[41, 1].Value = "Allocentric neglect Subscores";

                    worksheet.Cells[42, 1].Value = "Total number of left-gap distractors cancelled";
                    worksheet.Cells[42, 2].Value = leftSide_leftGap_DistractorsCrossed + center_leftGap_DistractorsCrossed + rightSide_LeftGap_DistractorsCrossed;

                    worksheet.Cells[43, 1].Value = "Total number of right-gap distractors cancelled";
                    worksheet.Cells[43, 2].Value = leftSide_rightGap_DistractorsCrossed + center_rightGap_DistractorsCrossed + rightSide_rightGap_DistractorsCrossed;

                    worksheet.Cells[44, 1].Value = "Allocentric neglect";
                    worksheet.Cells[44, 2].Value =  (leftSide_rightGap_DistractorsCrossed + center_rightGap_DistractorsCrossed + rightSide_rightGap_DistractorsCrossed) - (leftSide_leftGap_DistractorsCrossed + center_leftGap_DistractorsCrossed + rightSide_LeftGap_DistractorsCrossed);

                    worksheet.Cells[46, 1].Value = "Intersections";

                    worksheet.Cells[47, 1].Value = "Number of intersections";
                    worksheet.Cells[47, 2].Value = intersections;

                    worksheet.Cells[48, 1].Value = "Intersection Rate";
                    worksheet.Cells[48, 2].Value = intersectionRate;

                    worksheet.Cells[50, 1].Value = "Session History";

                    worksheet.Cells[51, 1].Value = "Success";
                    worksheet.Cells[51, 2].Value = "Time of Cancellation";
                    worksheet.Cells[51, 3].Value = "Location in Matrix";
                    worksheet.Cells[51, 4].Value = "Side in Matrix";
                    worksheet.Cells[51, 5].Value = "Orientation";
                    worksheet.Cells[51, 6].Value = "Re-Cancelled";
                    worksheet.Cells[51, 7].Value = "Pixel Location";
                    worksheet.Cells[51, 8].Value = "Normalized Position";

                    //Adding each action to the excel file
                    int row = 52;
                    foreach (clickAction action in this.actions)
                    {
                        mugObject clickedImage = this.localExamObject.imageList[action.ImageID - 1];
                        //worksheet.Cells[row, 1].Value = clickedImage.isClicked ? "Target Succesfully Cancelled" : "Distractor";
                        if(clickedImage.imageType == imageTypes.TargetLeft || clickedImage.imageType == imageTypes.TargetRight)
                        {
                            worksheet.Cells[row, 1].Value = "Target Succesfully Cancelled";
                        }
                        else
                        {
                            worksheet.Cells[row, 1].Value = "Distractor";
                        }
                        worksheet.Cells[row, 2].Value = timeSinceStart(action.timeOfClick);
                        worksheet.Cells[row, 3].Value = clickedImage.matrixLocation;
                        worksheet.Cells[row, 4].Value = clickedImage.side;
                        worksheet.Cells[row, 5].Value = mugObject.imageOrietation(clickedImage.imageType);
                        worksheet.Cells[row, 6].Value = action.isCrossed ? "" : "Yes";
                        worksheet.Cells[row, 7].Value = action.clickPoint;
                        worksheet.Cells[row, 8].Value = (action.clickPoint.X * this.sizeRatio) + ", " + (action.clickPoint.Y * this.sizeRatio);

                        row++;
                    }



                    excel.Save();

                };
            }

            //Creating the supporting image for the excel file and test
            public void outPutImage(string PatientName, TimeSpan timeTaken, int pratice = 0)
            {
                //Create a new bitmap - Blank image to write on
                Bitmap bmp = new Bitmap(this.localExamObject.screenWidth, this.localExamObject.screenHeight);

                int SquareSize = 20;
                int halfSquare = SquareSize / 2;

                //Create a new graphics object
                Graphics g = Graphics.FromImage(bmp);

                //Set the backgroun to white
                g.Clear(Color.White);

                //Cancelletion test - Date
                g.DrawString("Cancellation Test : " + PatientName, new Font("Arial", 16), Brushes.Black, new Point(10, 10));
                g.DrawString("Ending Time : " + timeTaken, new Font("Arial", 12), Brushes.Black, new Point(10, 30));
                g.DrawString("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), new Font("Arial", 12), Brushes.Black, new Point(10, 50));

                //Draw the images
                foreach (mugObject img in this.localExamObject.imageList)
                {
                    g.DrawImage(mugObject.getImageObject(img.imageType), img.imageCenter.X, img.imageCenter.Y, img.width, img.height);
                    if(img.isClicked)
                    {
                        //Draw a cross on the image
                        g.DrawLine(new Pen(Color.Black, 3), img.imageCenter.X, img.imageCenter.Y, img.imageCenter.X + img.width, img.imageCenter.Y + img.height);
                    }
                }

                if (this.actions.Count > 0)
                {
                    //Draw the path of the user
                    Point previousPoint = this.actions[0].clickPoint;



                    //Draw a red square centered at the center of the image to enote the start
                    Rectangle initialRectangle = new Rectangle(previousPoint.X - halfSquare, previousPoint.Y - halfSquare, SquareSize, SquareSize);


                    for(int i = 1; i < this.actions.Count; i++)
                    {
                        clickAction action = this.actions[i];

                        //Get the center of the image
                        int clickImageID = action.ImageID;
                        Point currentPoint = action.clickPoint;

                        //Draw the connection pine betyween the two points
                        g.DrawLine(new Pen(Color.Black, 4), previousPoint, currentPoint);

                        //Draw a dot in the middle of the image
                        g.FillRectangle(Brushes.Blue, currentPoint.X - halfSquare, currentPoint.Y - halfSquare, SquareSize, SquareSize);

                        previousPoint = currentPoint;
                    }

                    g.FillRectangle(Brushes.Red, initialRectangle);

                }

                //If the exam type is pratice then include title mugs
                if (pratice > 0)
                {
                    int imgWidth = bmp.Width;
                    int imgHeight = bmp.Height;

                    int smallMugsize = (int)(0.04 *  imgWidth);
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
                    g.DrawImage(mugObject.getImageObject(imageTypes.TargetLeft), p1.X, p2.Y,largeMugsize, largeMugsize);
                    g.DrawLine(instructionPen, p1, p2);

                    //Cross out the large right mug
                    p1 = new Point((int)(0.435 * imgWidth), (int)(0.25 * imgWidth) + largeMugsize);
                    p2 = new Point((int)(0.435 * imgWidth) +  largeMugsize, (int)(0.25 * imgWidth));
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

                string imageName = pratice > 0 ? "Pratice" + pratice + "_" : "CancellationTest_";
                imageName += PatientName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
                //string imageName = "test.png";

                Console.WriteLine("Saving the image");
                Console.WriteLine(Path.Combine(this.outputPath, imageName));

                //Save the image
                bmp.Save(Path.Combine(this.outputPath, imageName));
            */
            }
        }
        public string timeSinceStart(DateTime timeOfEvent)
        {
            return (timeOfEvent - this.startTime).ToString();
        }

        //Seperate function that determines the number of intersections between the paths of clicks
        private int intersectionCount()
        {
            int count = 0;

            //If the count is less then 4, then there are not enough points to create an intersection
            if(this.actions.Count < 4)
            {
                return 0;
            }

            for(int i = 0; i < this.actions.Count - 2; i++)
            {
                Point point1i = this.actions[i].clickPoint;
                Point point2i = this.actions[i + 1].clickPoint;

                for(int j = i + 1; j < this.actions.Count - 1; j++)
                {
                    Point point1j = this.actions[j].clickPoint;
                    Point point2j = this.actions[j + 1].clickPoint;

                    //Retrieve the X and Y values for each of the points - This is done to make the calculations easier and more readable
                    int x1i = point1i.X;
                    int y1i = point1i.Y;

                    int x2i = point2i.X;
                    int y2i = point2i.Y;

                    int x1j = point1j.X;
                    int y1j = point1j.Y;

                    int x2j = point2j.X;
                    int y2j = point2j.Y;

                    //Slope of the lines
                    double m1 = (double)(y2i - y1i) / (double)(x2i - x1i);
                    double m2 = (double)(y2j - y1j) / (double)(x2j - x1j);

                    double b1 = y1i - m1 * x1i;
                    double b2 = y1j - m2 * x1j;

                    //find the intersectinos between the two lines
                    double x = (b2 - b1) / (m1 - m2);
                    double y = m1 * x + b1;

                    //Check if the point is with the two X values for each line
                    Boolean condition1A = (x1i < x) && (x < x2i) || (x2i < x) && (x < x1i);
                    Boolean condition1B = (x1j < x) && (x < x2j) || (x2j < x) && (x < x2i);
                    Boolean condition1 = condition1A && condition1B;

                    Boolean condition2A = (y1i < y) && (y < y2i) || (y2i < y) && (y < y1i);
                    Boolean condition2B = (y1j < y) && (y < y2j) || (y2j < y) && (y < y1j);
                    Boolean condition2 = condition2A && condition2B;
                    
                    
                    count += condition1 && condition2 ? 1 : 0;

                }
            }
            

            return count;




        }

        private void updateOutputPath()
        {
            string localPath = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(Path.Combine(localPath, "Output")))
            {
                Directory.CreateDirectory(Path.Combine(localPath, "Output"));
            }
            localPath = Path.Combine(localPath, "Output");

            if (!Directory.Exists(Path.Combine(localPath, this.patientID)))
            {
                Directory.CreateDirectory(Path.Combine(localPath, this.patientID));
            }
            localPath = Path.Combine(localPath, this.patientID);

            if (!Directory.Exists(Path.Combine(localPath, DateTime.Now.ToString("yyyy-MM-dd"))))
            {
                Directory.CreateDirectory(Path.Combine(localPath, DateTime.Now.ToString("yyyy-MM-dd")));
            }
            this.outputPath = Path.Combine(localPath, DateTime.Now.ToString("yyyy-MM-dd"));
        }




    }

    //A Simple struct that is used to group together data from the click action
    public struct clickAction
    {
        public Point clickPoint;
        public int ImageID;
        public DateTime timeOfClick;
        public bool isCrossed;
    }
}

