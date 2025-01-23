using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    internal class Export_CSV : abstractExportClass
    {
        public Export_CSV(string patient_ID)
        {
            this.patient_ID = patient_ID;
        }

        public override void export(string folderLocation)
        {
            string folderPath = this.getOutputPath(folderLocation);
            string fileName = this.getFileName(folderPath, ".csv");

            //Export the data to an excel file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(folderPath, fileName)))
            {

                outputFile.WriteLine("Patient ID," + this.patient_ID);
                outputFile.WriteLine("Date," + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                outputFile.WriteLine(this.invisableCancelation ? "Invisible cancellation condition enabled" : "Invisible cancellation condition disabled");
                outputFile.WriteLine();

                outputFile.WriteLine("Session Resume");
                outputFile.WriteLine("Targets Cancelled," + (left_TargetCrossed + right_TargetCrossed + center_TargetCrossed) + "/" + total_Targets);
                outputFile.WriteLine("Distractors Cancelled," + total_DistractorsCrossed + "/" + total_Distractors);
                outputFile.WriteLine("Re-cancellations," + recancelations);
                outputFile.WriteLine("Total Time Taken," + this.total_TimeTaken);
                outputFile.WriteLine("Quality Score," + Math.Pow((left_TargetCrossed + right_TargetCrossed + center_TargetCrossed), 2) / (total_Targets * total_TimeTaken));
                outputFile.WriteLine();

                outputFile.WriteLine("Time Spent on the right side," + (Math.Round(right_TimeTaken / total_TimeTaken, 4) * 100) + "%");
                outputFile.WriteLine("Time Spent on the left side," + (Math.Round(left_TimeTaken / total_TimeTaken, 4) * 100) + "%");
                outputFile.WriteLine("Asymmetry Score," + Math.Round((right_TimeTaken - left_TimeTaken) / total_TimeTaken, 4));
                outputFile.WriteLine();

                outputFile.WriteLine("Search Speed," + total_SearchSpeed);
                outputFile.WriteLine("Left Search Speed," + left_SearchSpeed);
                outputFile.WriteLine("Right Search Speed," + right_SearchSpeed);
                outputFile.WriteLine();

                outputFile.WriteLine("Reclicks on the left Side," + left_Reclicks);
                outputFile.WriteLine("Reclicks on the right Side," + right_Reclicks);
                outputFile.WriteLine();

                outputFile.WriteLine("Accuracy per location");
                outputFile.WriteLine("Left Side," + left_Accuracy);
                outputFile.WriteLine("Center Side," + center_Accuracy);
                outputFile.WriteLine("Right Side," + right_Accuracy);
                outputFile.WriteLine();

                outputFile.WriteLine("Distractors crossed in each screen regions");
                outputFile.WriteLine("Left Gap");
                outputFile.WriteLine("Left Side," + leftSide_leftGap_distractors);
                outputFile.WriteLine("Center Side," + centerSide_leftGap_distractors);
                outputFile.WriteLine("Right Side," + rightSide_leftGap_distractors);
                outputFile.WriteLine("Right Gap");
                outputFile.WriteLine("Left Side," + leftSide_rightGap_distractors);
                outputFile.WriteLine("Center Side," + centerSide_rightGap_distractors);
                outputFile.WriteLine("Right Side," + rightSide_rightGap_distractors);
                outputFile.WriteLine();

                outputFile.WriteLine("Egocentric Neglect Subscores");
                outputFile.WriteLine("Total Targets Cancelled in the Left side of grid," + left_TargetCrossed);
                outputFile.WriteLine("Total Targets Cancelled in the Right side of grid," + right_TargetCrossed);
                outputFile.WriteLine("Egocentric Neglect," + (this.right_TargetCrossed - this.left_TargetCrossed));
                outputFile.WriteLine();

                outputFile.WriteLine("Allocentric Neglect Subscores");
                outputFile.WriteLine("Total number of left-gap distractors cancelled," + this.leftGap_DistractorsCrossed);
                outputFile.WriteLine("Total number of right-gap distractors cancelled," + this.rightGap_DistractorsCrossed);
                outputFile.WriteLine("Allocentric Neglect," + (this.rightGap_DistractorsCrossed - this.leftGap_DistractorsCrossed));
                outputFile.WriteLine();

                outputFile.WriteLine("Intersections");
                outputFile.WriteLine("Number of intersections," + intersections);
                outputFile.WriteLine("Intersection Rate," + intersectionRate);
                outputFile.WriteLine();

                outputFile.WriteLine("Session History");
                outputFile.WriteLine("Success,Time of Cancelation,Location in Matrix,Matrix Side,Orientation,Recancelation,Pixel Location,Normalized Position");

                foreach (clickRow cRow in this.clickRows)
                {
                    bool writeSecondLine = false;
                    string line = "";

                    line += cRow.success + ",";
                    line += cRow.time + ",";
                    line += cRow.matrixLocation + ",";
                    line += cRow.matrixSide + ",";
                    line += cRow.orientation + ",";
                    line += cRow.reCancel + ",";
                    line += cRow.PixelLocation + ",";
                    line += cRow.normalizedLocation.ToString();

                    outputFile.WriteLine(line);
                    if (writeSecondLine)
                    {
                        outputFile.WriteLine("fully Cancelled|                           |                          |                   |                     |                      |                           ");
                    }
                    //outputFile.WriteLine();

                }


                outputFile.Close();
            };
        }
    }
}
