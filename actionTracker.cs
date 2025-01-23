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
                exportClass.leftSide_leftGap_distractorsCrossed = leftSide_leftGap_DistractorsCrossed;
                exportClass.leftSide_rightGap_distractors = Math.Round((double)leftSide_rightGap_DistractorsCrossed / leftSide_rightGap_Distractors, 4) * 100;
                exportClass.leftSide_rightGap_distractorsCrossed = leftSide_rightGap_DistractorsCrossed;

                exportClass.centerSide_leftGap_distractors = Math.Round((double)center_leftGap_DistractorsCrossed / center_leftGap_Distractors, 4) * 100;
                exportClass.centerSide_leftGap_distractorsCrossed = center_leftGap_DistractorsCrossed;
                exportClass.centerSide_rightGap_distractors = Math.Round((double)center_rightGap_DistractorsCrossed / center_rigthGap_Distractors, 4) * 100;
                exportClass.centerSide_rightGap_distractorsCrossed = center_rightGap_DistractorsCrossed;

                exportClass.rightSide_leftGap_distractors = Math.Round((double)rightSide_LeftGap_DistractorsCrossed / rightSide_LeftGap_Distractors, 4) * 100;
                exportClass.rightSide_leftGap_distractorsCrossed = rightSide_LeftGap_DistractorsCrossed;
                exportClass.rightSide_rightGap_distractors = Math.Round((double)rightSide_rightGap_DistractorsCrossed / rightSide_rightGap_Distractors, 4) * 100;
                exportClass.rightSide_rightGap_distractorsCrossed = rightSide_rightGap_DistractorsCrossed;

                exportClass.intersections = intersections;
                exportClass.intersectionRate = intersectionRate;

                exportClass.addClickData(clickRows);
                exportClass.localExamObj = this.localExamObject;
                exportClass.export();

                
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

