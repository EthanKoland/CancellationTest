using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace CancellationTest
{
    internal class Export_Txt : abstractExportClass
    {
        public Export_Txt(string patient_ID)
        {
            this.patient_ID = patient_ID;
        }

        public override void export(string folderLocation)
        {


            string folderPath = this.getOutputPath(folderLocation);
            string fileName = this.getFileName(folderPath, ".txt");

            //Export the data to an excel file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(folderPath, fileName))) { 
            
                outputFile.WriteLine("Patient ID:" + this.patient_ID);
                outputFile.WriteLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                outputFile.WriteLine(this.invisableCancelation ? "Invisible cancellation condition enabled" : "Invisible cancellation condition disabled");
                outputFile.WriteLine();
                
                outputFile.WriteLine("Session Resume");
                //                                           |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("Variable               |" + CenterString("Scores", 16) + "|    Mean (SD)   |      Min       |      Max       | 5th Percentile | 95th Percentile|");
                string temp_Str = (left_TargetCrossed + right_TargetCrossed + center_TargetCrossed) + "/" + total_Targets;
                outputFile.WriteLine("Targets Cancelled:     |" + CenterString(temp_Str, 16) + "|       49       |       39       |       50       |       46       |       50       |");
                temp_Str = total_DistractorsCrossed + "/" + total_Distractors;
                outputFile.WriteLine("Distractors Cancelled: |" + CenterString(temp_Str, 16) + "|       0        |       0        |       4        |       0        |       1        |");
                temp_Str = "0" + Math.Round(Math.Pow((left_TargetCrossed + right_TargetCrossed + center_TargetCrossed), 2) / (total_Targets * total_TimeTaken),2);
                outputFile.WriteLine("Quality Score:         |" + CenterString(temp_Str, 16) + "|    .43 (.12)   |      .13       |      .75       |      .26       |      .64       |");
                temp_Str = recancelations.ToString();
                outputFile.WriteLine("Re-cancellations:      |" + CenterString(temp_Str, 16) + "|       0        |       0        |       1        |       0        |       0        |");
                temp_Str = total_TimeTaken.ToString();
                outputFile.WriteLine("Total Time Taken       |" + CenterString(temp_Str, 16) + "| 119.07 (36.85) |       66       |      287       |       73       |      187       |");
                outputFile.WriteLine();

                temp_Str = Math.Round(total_SearchSpeed, 2).ToString();
                outputFile.WriteLine("Search Speed:          |" + CenterString(temp_Str, 16) + "| 140.86 (27.79) |      71.19     |     231.20     |      96.29     |     182.68     |");
                temp_Str = Math.Round(left_SearchSpeed, 2).ToString();
                outputFile.WriteLine("Left Search Speed:     |" + CenterString(temp_Str, 16) + "| 126.54 (27.95) |      64.98     |     209.24     |      86.12     |     171.02     |");
                temp_Str = Math.Round(right_SearchSpeed, 2).ToString();
                outputFile.WriteLine("Right Search Speed:    |" + CenterString(temp_Str, 16) + "| 135.49 (29.33) |      65.28     |     182.69     |      86.12     |     180.23     |");
                outputFile.WriteLine();

                temp_Str = Math.Round((right_TimeTaken / total_TimeTaken), 4).ToString() + "%";
                outputFile.WriteLine("Time Spent on the      |" + CenterString(temp_Str, 16) + "|  45.88 (6.05)  |      28.72     |      28.72     |      36.54     |      56.29     |");
                outputFile.WriteLine("right side:            |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = Math.Round((left_TimeTaken / total_TimeTaken), 4).ToString() + "%";
                outputFile.WriteLine("Time Spent on the      |" + CenterString(temp_Str, 16) + "|  54.12 (6.05)  |      35.21     |      71.28     |      43.71     |      63.46     |");
                outputFile.WriteLine("left  side:            |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = Math.Round((right_TimeTaken - left_TimeTaken) / total_TimeTaken, 4).ToString();
                outputFile.WriteLine("Asymmetry Score:       |" + CenterString(temp_Str, 16) + "| -8.24 (12.11)  |     -42.57     |      29.37     |     -26.37     |      12.58     |");
                outputFile.WriteLine();

                outputFile.WriteLine("Egocentric Neglect Subscores");
                temp_Str = left_TargetCrossed.ToString();
                outputFile.WriteLine("Total Targets Cancelled|" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("in left side of grid   |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = right_TargetCrossed.ToString();
                outputFile.WriteLine("Total Targets Cancelled|" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("in right side of grid  |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = (right_TargetCrossed - left_TargetCrossed).ToString();
                outputFile.WriteLine("Egocentric Neglect:    |" + CenterString(temp_Str, 16) + "|       0        |       -3       |        5       |       -2       |        2       |");
                outputFile.WriteLine();

                outputFile.WriteLine("Allocentric Neglect Subscores");
                temp_Str = CenterString((this.leftGap_DistractorsCrossed).ToString(), 16);
                outputFile.WriteLine("Total num of left-gap  |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("distractors cancelled  |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = CenterString((this.rightGap_DistractorsCrossed).ToString(), 16);
                outputFile.WriteLine("Total num of right-gap |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("distractors cancelled  |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = CenterString((this.rightGap_DistractorsCrossed - this.leftGap_DistractorsCrossed).ToString(), 16);
                outputFile.WriteLine("Allocentric Neglect:   |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");

                temp_Str = leftGap_DistractorsCrossed.ToString();
                outputFile.WriteLine("Total Num of Left gap  |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("distractors cancelled  |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = rightGap_DistractorsCrossed.ToString();
                outputFile.WriteLine("Total Num of Right gap |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine("distractors cancelled  |" + CenterString("  ", 16) +     "|                |                |                |                |                |");
                temp_Str = (right_TargetCrossed - left_TargetCrossed).ToString();
                outputFile.WriteLine("Allocentric Neglect:   |" + CenterString(temp_Str, 16) + "|       0        |       -2       |        1       |        0       |        1       |");
                outputFile.WriteLine();

                outputFile.WriteLine("Intersections");

                outputFile.WriteLine("Num of intersections   |" + CenterString(temp_Str, 16) + "|       3        |        0       |        43      |        0       |        20      |");
                temp_Str = intersectionRate.ToString();
                outputFile.WriteLine("Intersection Rate      |" + CenterString(temp_Str, 16) + "|                |                |                |                |                |");
                outputFile.WriteLine();

                outputFile.WriteLine("Reclicks on the left Side: " + left_Reclicks);
                outputFile.WriteLine("Reclicks on the right Side: " + right_Reclicks);
                outputFile.WriteLine();

                outputFile.WriteLine("Accuracy per location");
                outputFile.WriteLine("Left Side: " + left_Accuracy);
                outputFile.WriteLine("Center Side: " + center_Accuracy);
                outputFile.WriteLine("Right Side: " + right_Accuracy);
                outputFile.WriteLine();

                outputFile.WriteLine("Distractors crossed in each screen regions");
                outputFile.WriteLine("Left Gap");
                outputFile.WriteLine("Left Side: " + leftSide_leftGap_distractorsCrossed);
                outputFile.WriteLine("Center Side: " + centerSide_leftGap_distractorsCrossed);
                outputFile.WriteLine("Right Side: " + rightSide_leftGap_distractorsCrossed);
                outputFile.WriteLine("Right Gap");
                outputFile.WriteLine("Left Side: " + leftSide_rightGap_distractorsCrossed);
                outputFile.WriteLine("Center Side: " + centerSide_rightGap_distractorsCrossed);
                outputFile.WriteLine("Right Side: " + rightSide_rightGap_distractorsCrossed);
                outputFile.WriteLine();

                

                

                outputFile.WriteLine("Normative data by age groups (by decade)");
                outputFile.WriteLine("      |       | 18-29 | 30-39 | 40-49 | 50-59 | 60-69 | 70-79 | 80-94 |");
                outputFile.WriteLine("      |       |n = 36 |n = 26 |n = 20 |n = 38 |n = 32 |n = 19 | n = 8 |");
                outputFile.WriteLine("------|-------|-------|-------|-------|-------|-------|-------|-------|");
                outputFile.WriteLine("      |mean   |106.47 |103.65 | 111.75| 123.05| 127.38| 128.89| 168.75|");
                outputFile.WriteLine("Search|SD     | 36.53 | 27.86 | 29.08 | 33.32 | 32.39 | 34.05 | 63.19 |");
                outputFile.WriteLine("time  |min    |  66   |  66   |  71   |  79   |  76   |  78   |  111  |");
                outputFile.WriteLine("in sec|max    |  214  |  176  |  184  |  207  |  187  |  235  |  287  |");
                outputFile.WriteLine("      |5th    | 66.85 | 67.05 | 71.10 | 80.90 | 79.90 | 78.00 | 111.00|");
                outputFile.WriteLine("      |95th   | 206.35| 175.30| 182.55| 194.65| 184.40|   -   |   -   |");
                outputFile.WriteLine("------|-------|-------|-------|-------|-------|-------|-------|-------|");
                outputFile.WriteLine("      |mean   |  0.43 |  0.50 |  0.43 |  0.41 |  0.40 |  0.37 |  0.30 |");
                outputFile.WriteLine("Qual  |SD     |  0.13 |  0.12 |  0.11 |  0.09 |  0.10 |  0.07 |  0.10 |");
                outputFile.WriteLine("of    |min    |  0.22 |  0.26 |  0.26 |  0.24 |  0.27 |  0.20 |  0.12 |");
                outputFile.WriteLine("search|max    |  0.75 |  0.73 |  0.65 |  0.61 |  0.66 |  0.51 |  0.40 |");
                outputFile.WriteLine("      |5th    |  0.24 |  0.26 |  0.26 |  0.24 |  0.27 |  0.20 |  0.13 |");
                outputFile.WriteLine("      |95th   |  0.72 |  0.72 |  0.65 |  0.58 |  0.60 |   -   |   -   |");
                outputFile.WriteLine("------|-------|-------|-------|-------|-------|-------|-------|-------|");
                outputFile.WriteLine("      |mean   | 152.31| 151.66| 148.84| 140.71| 127.27| 133.90| 105.91|");
                outputFile.WriteLine("Search|SD     | 25.93 | 28.65 | 21.93 | 26.39 | 24.71 | 25.67 | 17.05 |");
                outputFile.WriteLine("Speed |min    | 88.82 | 71.19 | 103.78| 90.47 | 84.94 | 96.77 | 82.93 |");
                outputFile.WriteLine("in sec|max    | 231.20| 198.97| 182.68| 224.58| 175.57| 192.72| 132.72|");
                outputFile.WriteLine("      |5th    | 102.75| 79.69 | 104.19| 96.49 | 91.29 | 96.77 | 82.93 |");
                outputFile.WriteLine("      |95th   | 194.93| 193.20| 182.45| 209.77| 172.52|   -   |   -   |");
                outputFile.WriteLine("------|-------|-------|-------|-------|-------|-------|-------|-------|");
                outputFile.WriteLine("");

                outputFile.WriteLine("Session History");
                outputFile.WriteLine("    Success    |  Time of Cancelation  |  Location in Matrix  |  Matrix Side  |  Orientation  |  Recancelation  |  Pixel Location  |  Normalized Position  ");
                
                outputFile.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------");
                //Write the click d   Target Success-
                //                    fully Cancelled
                //                       Distractor  |
                foreach (clickRow cRow in this.clickRows)
                {
                    bool writeSecondLine = false;
                    string line = "";

                    if(cRow.success.Equals("Target Succesfully Cancelled"))
                    {
                        writeSecondLine = true;
                        line += "Target Success-|";

                    }
                    else
                    {
                        line += "   Distractor  |";
                    }

                    line += CenterString(cRow.time, 23) + "|";
                    line += CenterString(cRow.matrixLocation, 22) + "|";
                    line += CenterString(cRow.matrixSide, 15) + "|";
                    line += CenterString(cRow.orientation, 15) + "|";
                    line += CenterString(cRow.reCancel, 17) + "|";
                    line += CenterString(cRow.PixelLocation, 18) + "|";
                    line += CenterString(cRow.normalizedLocation.ToString(), 21);

                    outputFile.WriteLine(line);
                    if (writeSecondLine)
                    {
                        outputFile.WriteLine("fully Cancelled|                       |                      |               |               |                 |                  |                       ");
                    }
                    outputFile.WriteLine();

                }


                outputFile.Close();
            };


        }

        public static string CenterString(string input, int totalLength)
        {
           
            int padding = totalLength - input.Length;
            int padLeft = padding / 2 + input.Length;
            return input.PadLeft(padLeft).PadRight(totalLength);
        }
    }
}
