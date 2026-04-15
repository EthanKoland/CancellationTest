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

        private List<Point> intersectionPoints = new List<Point>();

        public DateTime endTime { get; set; }

        public actionTracker(abstractTestClass examObject, bool invisableCancelation, string patientID = "Unknown", int numOfHorizontalCells = 5, double sizeRatio = 1.0)
        {
            //Var creation and initialization
            this.actions = new List<clickAction>();
            this.localExamObject = examObject;
            this.patientID = patientID;
            this.numOfHorizontalCells = numOfHorizontalCells;
            this.sizeRatio = sizeRatio;
            this.outputPath = AppDomain.CurrentDomain.BaseDirectory;
            this.reclicks = new Dictionary<int, int>();
            this.invisableCancelation = invisableCancelation;
            this.startTime = DateTime.Now;
        }

        //Because actions is a private variable, we need to create a method to add actions to the list. This simply controlls the action being added to the list
        public void addAction(clickAction action) { 
            this.actions.Add(action);
            this.reclicks[action.ImageID] = this.reclicks.ContainsKey(action.ImageID) ? this.reclicks[action.ImageID] + 1 : 1;
        }

        //This is the main export function that creates the excel file from the actions
        public void export( List<abstractExportClass> exportClasses, string baseFolderLocation = null)
        {

            //DateTime endTime = DateTime.Now;

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

            //double totalTimeTaken = (this.actions[this.actions.Count() - 1].timeOfClick - this.actions[0].timeOfClick).TotalSeconds;
            //this.startTime = this.actions[0].timeOfClick;
            //this.endTime = this.actions[this.actions.Count - 1].timeOfClick;
            double totalTimeTaken = (endTime - startTime).TotalSeconds;
            this.endTime = this.actions.Count > 0 ?  this.actions[this.actions.Count - 1].timeOfClick: this.endTime;
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
                        leftTargetsCrossed += img.hasBeenClicked ? 1 : 0;
                    }
                    else if (img.side == leftRightCenter.Right)
                    {
                        rightTargets++;
                        rightTargetsCrossed += img.hasBeenClicked ? 1 : 0;
                    }
                    else
                    {
                        centerTargets++;
                        centerTargetsCrossed += img.hasBeenClicked ? 1 : 0;
                    }

                    if (img.hasBeenClicked && img.imageType == imageTypes.TargetLeft)
                    {
                        leftAllocentric++;
                    }
                    else if (img.hasBeenClicked && img.imageType == imageTypes.TargetRight)
                    {
                        rightAllocentric++;
                    }
                }
                //If the image is not a target, then it is a distractor
                else
                {
                    //Incriment the number of distractors
                    if (img.hasBeenClicked)
                    {
                        distractorsCrossed++;
                    }

                    if (img.side == leftRightCenter.Left)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            leftSide_leftGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            leftSide_leftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            leftSide_rightGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            leftSide_rightGap_Distractors += 1;
                        }
                    }
                    else if (img.side == leftRightCenter.Right)
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            rightSide_LeftGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            rightSide_LeftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            rightSide_rightGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            rightSide_rightGap_Distractors += 1;
                        }
                    }
                    else
                    {
                        if (img.imageType == imageTypes.DistractionLeft)
                        {
                            center_leftGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            center_leftGap_Distractors += 1;
                        }
                        else if (img.imageType == imageTypes.DistractionRight)
                        {
                            center_rightGap_DistractorsCrossed += img.hasBeenClicked ? 1 : 0;
                            center_rigthGap_Distractors += 1;
                        }
                    }

                }
                //Calculating Reclicks
                
            }

            //Starting point for the caluclations on the time taken on each side of the screen
            
            Point previousPoint = this.actions.Count > 0 ? this.actions[0].clickPoint : new Point(0, 0);

            clickAction firstAction = this.actions[0];
            mugObject firstImage = this.localExamObject.imageList[firstAction.ImageID - 1];
            int maxX = this.localExamObject.screenWidth;
            int maxY = this.localExamObject.screenHeight;

            LeftRight previousSide = firstAction.leftOrRightSide;
            DateTime previousSideTime = firstAction.timeOfClick;

            DateTime previousTime = this.startTime;
            if (previousSide == LeftRight.Left)
            {
                leftTimeTaken = (previousSideTime - previousTime).TotalSeconds;
            }
            else
            {
                rightTimeTaken = (previousSideTime - previousTime).TotalSeconds;
            }

            int rightClicks = this.actions[0].leftOrRightSide == LeftRight.Right ? 1 : 0;
            int leftClicks = this.actions[0].leftOrRightSide == LeftRight.Left ? 1 : 0;

            int rightClickDivide = rightClicks;
            int leftClickDivide = leftClicks;
            
            List<double> list_searchspeeds = new List<double>();
            List<double> list_leftsearchspeeds = new List<double>();
            List<double> list_rightsearchspeeds = new List<double>();

            //Loop through each of the click actions
            for (int i = 1; i < this.actions.Count; i++)
            {
                clickAction action = this.actions[i];


                //Retrieve the image that was clicked
                mugObject clickedImage = this.localExamObject.imageList[action.ImageID - 1];


                double localX = (double)action.clickPoint.X / maxX;
                double timeTaken = (action.timeOfClick - previousSideTime).TotalSeconds;
                double distance = Math.Sqrt(Math.Pow(previousPoint.X - action.clickPoint.X, 2) + Math.Pow(previousPoint.Y - action.clickPoint.Y, 2));
                double local_searchSpeed = distance / timeTaken;
                list_searchspeeds.Add(local_searchSpeed);

                LeftRight side = action.leftOrRightSide;

                leftClicks += action.leftOrRightSide == LeftRight.Left ? 1 : 0;
                rightClicks += action.leftOrRightSide == LeftRight.Right ? 1 : 0;


                //Check if the side has changed - If it isn't then the patient has switched sides on the screen so need to determine attributes of the side
                if (side != previousSide)
                {
                    
                    

                    double xGreater = (double) Math.Max(previousPoint.X, action.clickPoint.X);
                    double xLesser = (double) Math.Min(previousPoint.X, action.clickPoint.X);

                    double t = Math.Abs((previousPoint.X - action.clickPoint.X));
                    double t1 = (double)maxX / 2.0;

                    double modifierRight = (xGreater - t1) / t;
                    double modifierLeft = (t1 - xLesser) / t;
                    rightTimeTaken += timeTaken * modifierRight;
                    leftTimeTaken += timeTaken * modifierLeft;
                    distanceRight += distance * modifierRight;
                    distanceLeft += distance * modifierLeft;


                    previousSide = side;

                    searchSpeed += local_searchSpeed;
                    leftSearchSpeed += local_searchSpeed;
                    rightSearchSpeed += local_searchSpeed;

                    list_leftsearchspeeds.Add(local_searchSpeed);
                    list_rightsearchspeeds.Add(local_searchSpeed);


                }
                else
                {
                    if (side == LeftRight.Left)
                    {
                        Console.WriteLine("Left Side Clicked", (action.timeOfClick - previousSideTime).TotalSeconds);
                        leftTimeTaken += timeTaken;
                        distanceLeft += distance;
                        list_leftsearchspeeds.Add(local_searchSpeed);
                    }
                    else if (side == LeftRight.Right)
                    {
                        rightTimeTaken += timeTaken;
                        distanceRight +=distance;
                        list_rightsearchspeeds.Add(local_searchSpeed);
                    }
                }

                previousSideTime = action.timeOfClick;
                previousPoint = action.clickPoint;



                //Calculate the speed of the search

                if (action.reClick)
                {
                    leftReclicks += action.leftOrRightSide == LeftRight.Left ? 1 : 0;
                    rightReclicks += action.leftOrRightSide == LeftRight.Right ? 1 : 0;
                    reCancellations++;
                }

            }

            rightSearchSpeed = list_rightsearchspeeds.Count() > 0 ? list_rightsearchspeeds.Sum(x => x) / list_rightsearchspeeds.Count() : 0;
            leftSearchSpeed = list_leftsearchspeeds.Count() > 0 ? list_leftsearchspeeds.Sum(x => x) / list_leftsearchspeeds.Count() :0;
            searchSpeed = list_searchspeeds.Count() > 0 ? list_searchspeeds.Sum(x => x) / list_searchspeeds.Count() : 0;

            clickAction lastClickAction = this.actions[this.actions.Count - 1];
            if (lastClickAction.leftOrRightSide == LeftRight.Left)
            {
                leftTimeTaken += (endTime - lastClickAction.timeOfClick).TotalSeconds;
            }
            else
            {
                rightTimeTaken += (endTime - lastClickAction.timeOfClick).TotalSeconds;
            }

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
                newRow.reCancel = action.reClick ? "Yes" : "";
                newRow.PixelLocation = action.clickPoint.ToString();
                newRow.normalizedLocation = new Point((int)(action.clickPoint.X), (int)(action.clickPoint.Y));
                newRow.mugImageCenter = clickedImage.imageCenter;

                clickRows.Add(newRow);
            }

            double centerOfCancellation = calculateCenterOfCancellation();

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

                exportClass.centerOfCancellation = centerOfCancellation;

                exportClass.intersectionPoints = intersectionPoints;

                exportClass.leftSearchDistance = distanceLeft;
                exportClass.rightSearchDistance = distanceRight;
                exportClass.startTime = this.startTime;
                exportClass.endTime = this.endTime;

                exportClass.addClickData(clickRows);
                exportClass.localExamObj = this.localExamObject;
                exportClass.export();

                
            }
        }
        public string timeSinceStart(DateTime timeOfEvent)
        {
            // Calculate the time difference
            TimeSpan totalTime = timeOfEvent - this.startTime;

            // Extract minutes, seconds, and the first digit of milliseconds
            int minutes = totalTime.Minutes;
            int seconds = totalTime.Seconds;
            int firstDigitMilliseconds = totalTime.Milliseconds / 100; // Get the first digit of milliseconds

            // Format the string as "mm:ss.f"
            return $"{minutes:D2}:{seconds:D2}.{firstDigitMilliseconds}";
        }

        //Seperate function that determines the number of intersections between the paths of clicks
        private int intersectionCount()
        {
            this.intersectionPoints.Clear();

            // Need at least 4 clicks to have two non-adjacent segments
            if (this.actions.Count < 4)
            {
                return 0;
            }

            int count = 0;

            // Segment i: actions[i] -> actions[i+1]
            for (int i = 0; i < this.actions.Count - 1; i++)
            {
                Point a1 = this.actions[i].clickPoint;
                Point a2 = this.actions[i + 1].clickPoint;

                // Segment j: actions[j] -> actions[j+1]
                // Start at i + 2 so segments are non-adjacent and do not share endpoints.
                for (int j = i + 2; j < this.actions.Count - 1; j++)
                {
                    Point b1 = this.actions[j].clickPoint;
                    Point b2 = this.actions[j + 1].clickPoint;

                    // Also skip wrap-around case where first and last segments share endpoint
                    if (i == 0 && j + 1 == this.actions.Count - 1)
                    {
                        continue;
                    }

                    Point intersection;
                    if (TryGetSegmentIntersection(a1, a2, b1, b2, out intersection))
                    {
                        count++;
                        this.intersectionPoints.Add(intersection);
                    }
                }
            }

            return count;
        }

        private static bool TryGetSegmentIntersection(Point p1, Point p2, Point p3, Point p4, out Point intersection)
        {
            intersection = new Point(0, 0);

            // Parametric line intersection
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;
            double x3 = p3.X;
            double y3 = p3.Y;
            double x4 = p4.X;
            double y4 = p4.Y;

            double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            // Parallel or collinear -> no single crossing point
            if (Math.Abs(denom) < 1e-10)
            {
                return false;
            }

            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
            double u = ((x1 - x3) * (y1 - y2) - (y1 - y3) * (x1 - x2)) / denom;

            // Strictly inside both segments (exclude touching at endpoints)
            if (t <= 0.0 || t >= 1.0 || u <= 0.0 || u >= 1.0)
            {
                return false;
            }

            double ix = x1 + t * (x2 - x1);
            double iy = y1 + t * (y2 - y1);
            intersection = new Point((int)Math.Round(ix), (int)Math.Round(iy));
            return true;
        }

        private double calculateCenterOfCancellation()
        {
            int localScreenWidth = this.localExamObject.screenWidth;

            int targetsCancelled = 0;

            double horizontalSum = 0;
            //Loop through each of the mugs
            foreach (mugObject img in this.localExamObject.imageList)
            {
                //Check if the image is a target
                if (img.imageType == imageTypes.TargetLeft || img.imageType == imageTypes.TargetRight)
                {
                    //Check if the image is clicked
                    if (img.isClicked)
                    {
                        //Incriment the click
                        targetsCancelled++;

                        int x = img.imageCenter.X - (localScreenWidth/2);

                        //x -= localScreenWidth;

                        horizontalSum += (double) x/ ((double) localScreenWidth/2);

                        //horizontalSum += (double)x;

                    }
                }
            }

            return targetsCancelled == 0 ? 0 : horizontalSum / targetsCancelled;
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
        public LeftRight leftOrRightSide; //True is for the left side
        public bool reClick;
    }

    public enum LeftRight
    {
        Left,
        Right
    }
}

