using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class Export_XLSX : abstractExportClass
    {


        public Export_XLSX(string patient_ID)
        {
            this.patient_ID = patient_ID;
        }



        public override void export(string folderLocation)
        {


            string folderPath = this.getOutputPath(folderLocation);
            string fileName = this.getFileName(folderPath, ".xlsx");

            //Export the data to an excel file
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(new FileInfo(fileName)))
            {

                Console.WriteLine("Exporting to Excel");

                //Check if the worksheet exists
                if (excel.Workbook.Worksheets["Session_Resume"] != null)
                {
                    excel.Workbook.Worksheets.Delete("Session_Resume");
                }

                excel.Workbook.Worksheets.Add("Session_Resume");

                var worksheet = excel.Workbook.Worksheets["Session_Resume"];

                int row = 1;

                worksheet.Cells[row, 1].Value = "Date:";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                worksheet.Cells[row, 3].Value = "Patient ID:";
                worksheet.Cells[row, 3].Style.Font.Bold = true;
                worksheet.Cells[row, 4].Value = this.patient_ID;

                //Some sort of informatino on the cellencaltion condition
                row++;
                worksheet.Cells[row, 1].Value = this.invisableCancelation ? "Invisible cancellation condition enabled" : "Invisible cancellation condition disabled";
                worksheet.Cells[row, 1, row, 1 + 5].Merge = true;

                row++;
                worksheet.Cells[row, 1].Value = "5th and 95th percentiles are shown here to be used as preliminary cutoffs to determine impairment.";
                worksheet.Cells[row, 1, row, 1 + 5].Merge = true;


                //Session Resume
                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Session Resume";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.UnderLine = true;
                

                row++;
                worksheet.Cells[row, 1].Value = "Variable";
                worksheet.Cells[row, 2].Value = "Scores";
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Style.Font.UnderLine = true;
                worksheet.Cells[row, 3].Value = "Mean Score (SD)";
                worksheet.Cells[row, 3].Style.Font.Bold = true;
                worksheet.Cells[row, 3].Style.Font.UnderLine = true;
                worksheet.Cells[row, 4].Value = "Min Score";
                worksheet.Cells[row, 4].Style.Font.Bold = true;
                worksheet.Cells[row, 4].Style.Font.UnderLine = true;
                worksheet.Cells[row, 5].Value = "Max Score";
                worksheet.Cells[row, 5].Style.Font.Bold = true;
                worksheet.Cells[row, 5].Style.Font.UnderLine = true;
                worksheet.Cells[row, 6].Value = "5th Percentile";
                worksheet.Cells[row, 6].Style.Font.Bold = true;
                worksheet.Cells[row, 6].Style.Font.UnderLine = true;
                worksheet.Cells[row, 7].Value = "95th Percentile";
                worksheet.Cells[row, 7].Style.Font.Bold = true;
                worksheet.Cells[row, 7].Style.Font.UnderLine = true;


                row++;
                worksheet.Cells[row, 1].Value = "Targets Cancelled";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = "" + (left_TargetCrossed + right_TargetCrossed + center_TargetCrossed) + "/" + total_Targets;
                worksheet.Cells[row, 3].Value = "49";
                worksheet.Cells[row, 4].Value = "39";
                worksheet.Cells[row, 5].Value = "50";
                worksheet.Cells[row, 6].Value = "46";
                worksheet.Cells[row, 7].Value = "50";

                row++;
                worksheet.Cells[row, 1].Value = "Distractors Cancelled";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = "" + total_DistractorsCrossed + "/" + total_Distractors;
                worksheet.Cells[row, 3].Value = "0"; //Mean score
                worksheet.Cells[row, 4].Value = "0"; //Min score
                worksheet.Cells[row, 5].Value = "4"; //Max score
                worksheet.Cells[row, 6].Value = "0"; //5th percentile
                worksheet.Cells[row, 7].Value = "1"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Quality Score";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = Math.Pow((left_TargetCrossed + right_TargetCrossed + center_TargetCrossed), 2) / (total_Targets * total_TimeTaken);
                worksheet.Cells[row, 3].Value = "0.43 (.12)"; //Mean score
                worksheet.Cells[row, 4].Value = "0.13"; //Min score
                worksheet.Cells[row, 5].Value = "0.75"; //Max score
                worksheet.Cells[row, 6].Value = "0.26"; //5th percentile
                worksheet.Cells[row, 7].Value = "0.64"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Re-cancellations";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = recancelations;
                worksheet.Cells[row, 3].Value = "0"; //Mean score
                worksheet.Cells[row, 4].Value = "0"; //Min score
                worksheet.Cells[row, 5].Value = "1"; //Max score
                worksheet.Cells[row, 6].Value = "0"; //5th percentile
                worksheet.Cells[row, 7].Value = "0"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Total Time Taken";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = total_TimeTaken;
                worksheet.Cells[row, 3].Value = "119.07 (36.85)"; //Mean score
                worksheet.Cells[row, 4].Value = "66"; //Min score
                worksheet.Cells[row, 5].Value = "287"; //Max score
                worksheet.Cells[row, 6].Value = "73"; //5th percentile
                worksheet.Cells[row, 7].Value = "187"; //95th percentile


                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Time Spent on the right side";
                worksheet.Cells[row, 2].Value = (Math.Round(right_TimeTaken / total_TimeTaken, 4) * 100) + "%";
                worksheet.Cells[row, 3].Value = "45.88 (6.05)"; //Mean score
                worksheet.Cells[row, 4].Value = "28.72"; //Min score
                worksheet.Cells[row, 5].Value = "64.69"; //Max score
                worksheet.Cells[row, 6].Value = "36.54"; //5th percentile
                worksheet.Cells[row, 7].Value = "56.29"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Time Spent on the left side";
                worksheet.Cells[row, 2].Value = (Math.Round(left_TimeTaken / total_TimeTaken, 4) * 100) + "%";
                worksheet.Cells[row, 3].Value = "54.12 (6.05)"; //Mean score
                worksheet.Cells[row, 4].Value = "35.21"; //Min score
                worksheet.Cells[row, 5].Value = "71.28"; //Max score
                worksheet.Cells[row, 6].Value = "43.71"; //5th percentile
                worksheet.Cells[row, 7].Value = "63.46"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Asymmetry Score";
                worksheet.Cells[row, 2].Value = Math.Round((right_TimeTaken - left_TimeTaken) / total_TimeTaken, 4);
                worksheet.Cells[row, 3].Value = "-8.24 (12.11)"; //Mean score
                worksheet.Cells[row, 4].Value = "-42.57"; //Min score
                worksheet.Cells[row, 5].Value = "29.37"; //Max score
                worksheet.Cells[row, 6].Value = "-26.92"; //5th percentile
                worksheet.Cells[row, 7].Value = "12.58"; //95th percentile

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Search Speed";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = total_SearchSpeed;
                worksheet.Cells[row, 3].Value = "140.86 (27.79)"; //Mean score
                worksheet.Cells[row, 4].Value = "71.19"; //Min score
                worksheet.Cells[row, 5].Value = "231.20"; //Max score
                worksheet.Cells[row, 6].Value = "96.29"; //5th percentile
                worksheet.Cells[row, 7].Value = "182.68"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Left Search Speed";
                worksheet.Cells[row, 2].Value = left_SearchSpeed;
                worksheet.Cells[row, 3].Value = "135.49 (29.33)"; //Mean score
                worksheet.Cells[row, 4].Value = "65.28"; //Min score
                worksheet.Cells[row, 5].Value = "209.24"; //Max score
                worksheet.Cells[row, 6].Value = "86.12"; //5th percentile
                worksheet.Cells[row, 7].Value = "180.23"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Right Search Speed";
                worksheet.Cells[row, 2].Value = right_SearchSpeed;
                worksheet.Cells[row, 3].Value = "126.54 (27.95)"; //Mean score
                worksheet.Cells[row, 4].Value = "64.98"; //Min score
                worksheet.Cells[row, 5].Value = "182.69"; //Max score
                worksheet.Cells[row, 6].Value = "78.97"; //5th percentile
                worksheet.Cells[row, 7].Value = "171.02"; //95th percentile

                

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Egocentric neglect Subscores";
                worksheet.Cells[row, 1].Style.Font.Bold = true;


                row++;
                worksheet.Cells[row, 1].Value = "Total Targets Cancelled in left side of grid";
                worksheet.Cells[row, 2].Value = this.left_TargetCrossed;

                row++;
                worksheet.Cells[row, 1].Value = "Total Targets Cancelled in right side of grid";
                worksheet.Cells[row, 2].Value = this.right_TargetCrossed;

                row++;
                worksheet.Cells[row, 1].Value = "Egocentric neglect";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = this.right_TargetCrossed - this.left_TargetCrossed;
                worksheet.Cells[row, 3].Value = "0"; //Mean score
                worksheet.Cells[row, 4].Value = "-3"; //Min score
                worksheet.Cells[row, 5].Value = "5"; //Max score
                worksheet.Cells[row, 6].Value = "-2"; //5th percentile
                worksheet.Cells[row, 7].Value = "2"; //95th percentile

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Allocentric neglect Subscores";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Total number of left-gap distractors cancelled";
                worksheet.Cells[row, 2].Value = this.leftGap_DistractorsCrossed;

                row++;
                worksheet.Cells[row, 1].Value = "Total number of right-gap distractors cancelled";
                worksheet.Cells[row, 2].Value = this.rightGap_DistractorsCrossed;

                row++;
                worksheet.Cells[row, 1].Value = "Allocentric neglect";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 2].Value = this.rightGap_DistractorsCrossed - this.leftGap_DistractorsCrossed;
                worksheet.Cells[row, 3].Value = "0"; //Mean score
                worksheet.Cells[row, 4].Value = "-2"; //Min score
                worksheet.Cells[row, 5].Value = "1"; //Max score
                worksheet.Cells[row, 6].Value = "0"; //5th percentile
                worksheet.Cells[row, 7].Value = "1"; //95th percentile

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Reclicks on the left side";
                worksheet.Cells[row, 2].Value = left_Reclicks;

                row++;
                worksheet.Cells[row, 1].Value = "Reclicks on the right side";
                worksheet.Cells[row, 2].Value = right_Reclicks;

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Accuracy per location";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                worksheet.Cells[row, 1].Value = "Left";
                worksheet.Cells[row, 2].Value = left_Accuracy;

                row++;
                worksheet.Cells[row, 1].Value = "Center";
                worksheet.Cells[row, 2].Value = center_Accuracy;

                row++;
                worksheet.Cells[row, 1].Value = "Right";
                worksheet.Cells[row, 2].Value = right_Accuracy;

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Distractors crossed in each screen region";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Left Gap";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++; 
                worksheet.Cells[row, 1].Value = "Left";
                worksheet.Cells[row, 2].Value = leftSide_leftGap_distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Center";
                worksheet.Cells[row, 2].Value = centerSide_leftGap_distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Right";
                worksheet.Cells[row, 2].Value = rightSide_leftGap_distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Right Gap";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Left";
                worksheet.Cells[row, 2].Value = leftSide_rightGap_distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Center";
                worksheet.Cells[row, 2].Value = centerSide_rightGap_distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Right";
                worksheet.Cells[row, 2].Value = rightSide_rightGap_distractors;

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Intersections";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Number of intersections";
                worksheet.Cells[row, 2].Value = intersections;
                worksheet.Cells[row, 3].Value = "3"; //Mean score
                worksheet.Cells[row, 4].Value = "0"; //Min score
                worksheet.Cells[row, 5].Value = "43"; //Max score
                worksheet.Cells[row, 6].Value = "0"; //5th percentile
                worksheet.Cells[row, 7].Value = "20"; //95th percentile

                row++;
                worksheet.Cells[row, 1].Value = "Intersection Rate";
                worksheet.Cells[row, 2].Value = intersectionRate;

                int row_Checkpoint = row;

                //row = this.addNormativeAgeTable(worksheet, row);
                //row = this.addSessionHistory(worksheet, row);

                if (excel.Workbook.Worksheets["Session_History"] != null)
                {
                    excel.Workbook.Worksheets.Delete("Session_History");
                }

                excel.Workbook.Worksheets.Add("Session_History");

                var Session_History = excel.Workbook.Worksheets["Session_History"];
                int _ = this.addSessionHistory(Session_History, 1);

                if (excel.Workbook.Worksheets["Normative_Age_Table"] != null)
                {
                    excel.Workbook.Worksheets.Delete("Normative_Age_Table");
                }

                excel.Workbook.Worksheets.Add("Normative_Age_Table");

                var Normative_Age_Table = excel.Workbook.Worksheets["Normative_Age_Table"];
                _ = this.addNormativeAgeTable(Normative_Age_Table, 1);


                excel.Save();

            }


        }

        private int addSessionHistory(ExcelWorksheet worksheet, int row)
        {
            row++;
            worksheet.Cells[row, 1].Value = "Session History";
            worksheet.Cells[row, 1].Style.Font.Bold = true;

            row++;
            worksheet.Cells[row, 1].Value = "Success";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 2].Value = "Time of Cancellation";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 3].Value = "Location in Matrix";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 4].Value = "Side in Matrix";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 5].Value = "Orientation";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 6].Value = "Re-Cancelled";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 7].Value = "Pixel Location";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;
            worksheet.Cells[row, 8].Value = "Normalized Position";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.UnderLine = true;

            //Adding each action to the excel file
            row++;
            foreach (clickRow cRow in this.clickRows)
            {
                worksheet.Cells[row, 1].Value = cRow.success;
                worksheet.Cells[row, 2].Value = cRow.time;
                worksheet.Cells[row, 3].Value = cRow.matrixLocation;
                worksheet.Cells[row, 4].Value = cRow.matrixSide;
                worksheet.Cells[row, 5].Value = cRow.orientation;
                worksheet.Cells[row, 6].Value = cRow.reCancel;
                worksheet.Cells[row, 7].Value = cRow.PixelLocation;
                worksheet.Cells[row, 8].Value = cRow.normalizedLocation;


                row++;
            }

            return row;
        }

        private int addNormativeAgeTable(ExcelWorksheet worksheet, int row)
        {
            row++;
            row++;
            worksheet.Cells[row, 1].Value = "Normative data by age groups (by decade).";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row, 1 + 5].Merge = true;

            row++;
            worksheet.Cells[row, 1].Value = "5th and 95th percentiles are shown here to be used as preliminary cutoffs to determine impairment.";
            worksheet.Cells[row, 1, row, 1 + 5].Merge = true;

            row++;
            worksheet.Cells[row, 3].Value = "18-29";
            worksheet.Cells[row, 3].Style.Font.Bold = true;
            worksheet.Cells[row, 4].Value = "30-39";
            worksheet.Cells[row, 4].Style.Font.Bold = true;
            worksheet.Cells[row, 5].Value = "40-49";
            worksheet.Cells[row, 5].Style.Font.Bold = true;
            worksheet.Cells[row, 6].Value = "50-59";
            worksheet.Cells[row, 6].Style.Font.Bold = true;
            worksheet.Cells[row, 7].Value = "60-69";
            worksheet.Cells[row, 7].Style.Font.Bold = true;
            worksheet.Cells[row, 8].Value = "70-79";
            worksheet.Cells[row, 8].Style.Font.Bold = true;
            worksheet.Cells[row, 9].Value = "80-94";
            worksheet.Cells[row, 9].Style.Font.Bold = true;

            row++;
            worksheet.Cells[row, 3].Value = "(n = 36)";
            worksheet.Cells[row, 3].Style.Font.Bold = true;
            worksheet.Cells[row, 4].Value = "(n = 26)";
            worksheet.Cells[row, 4].Style.Font.Bold = true;
            worksheet.Cells[row, 5].Value = "(n = 20)";
            worksheet.Cells[row, 5].Style.Font.Bold = true;
            worksheet.Cells[row, 6].Value = "(n = 38)";
            worksheet.Cells[row, 6].Style.Font.Bold = true;
            worksheet.Cells[row, 7].Value = "(n = 32)";
            worksheet.Cells[row, 7].Style.Font.Bold = true;
            worksheet.Cells[row, 8].Value = "(n = 19)";
            worksheet.Cells[row, 8].Style.Font.Bold = true;
            worksheet.Cells[row, 9].Value = "(n = 8)";
            worksheet.Cells[row, 9].Style.Font.Bold = true;

            row++;
            //Merge cells row to row + 5, column 1 to 1
            worksheet.Cells[row, 1].Value = "Search Time (secs)";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row + 4, 1].Merge = true;

            worksheet.Cells[row, 2].Value = "Mean (SD)";
            worksheet.Cells[row, 3].Value = "106.47 (36.53)";
            worksheet.Cells[row, 4].Value = "103.65 (27.86)";
            worksheet.Cells[row, 5].Value = "111.75 (29.08)";
            worksheet.Cells[row, 6].Value = "123.05 (33.32)";
            worksheet.Cells[row, 7].Value = "127.38 (32.29)";
            worksheet.Cells[row, 8].Value = "128.89 (34.05)";
            worksheet.Cells[row, 9].Value = "168.75 (63.19)";

            row++;
            worksheet.Cells[row, 2].Value = "Min";
            worksheet.Cells[row, 3].Value = "66";
            worksheet.Cells[row, 4].Value = "66";
            worksheet.Cells[row, 5].Value = "71";
            worksheet.Cells[row, 6].Value = "79";
            worksheet.Cells[row, 7].Value = "76";
            worksheet.Cells[row, 8].Value = "78";
            worksheet.Cells[row, 9].Value = "111";

            row++;
            worksheet.Cells[row, 2].Value = "Max";
            worksheet.Cells[row, 3].Value = "214";
            worksheet.Cells[row, 4].Value = "176";
            worksheet.Cells[row, 5].Value = "184";
            worksheet.Cells[row, 6].Value = "207";
            worksheet.Cells[row, 7].Value = "187";
            worksheet.Cells[row, 8].Value = "235";
            worksheet.Cells[row, 9].Value = "287";

            row++;
            worksheet.Cells[row, 2].Value = "5th";
            worksheet.Cells[row, 3].Value = "66.85";
            worksheet.Cells[row, 4].Value = "67.05";
            worksheet.Cells[row, 5].Value = "71.10";
            worksheet.Cells[row, 6].Value = "80.90";
            worksheet.Cells[row, 7].Value = "79.90";
            worksheet.Cells[row, 8].Value = "78.00";
            worksheet.Cells[row, 9].Value = "111.00";

            row++;
            worksheet.Cells[row, 2].Value = "95th";
            worksheet.Cells[row, 3].Value = "206.35";
            worksheet.Cells[row, 4].Value = "175.30";
            worksheet.Cells[row, 5].Value = "182.55";
            worksheet.Cells[row, 6].Value = "194.65";
            worksheet.Cells[row, 7].Value = "184.40";
            worksheet.Cells[row, 8].Value = "-";
            worksheet.Cells[row, 9].Value = "-";

            row++;
            worksheet.Cells[row, 1].Value = "Quality of Search";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row + 4, 1].Merge = true;

            row++;
            worksheet.Cells[row, 2].Value = "Mean (SD)";
            worksheet.Cells[row, 3].Value = "0.48 (0.13)";
            worksheet.Cells[row, 4].Value = "0.50 (0.12)";
            worksheet.Cells[row, 5].Value = "0.43 (0.11)";
            worksheet.Cells[row, 6].Value = "0.41 (0.09";
            worksheet.Cells[row, 7].Value = "0.40 (0.10)";
            worksheet.Cells[row, 8].Value = "0.37 (0.07)";
            worksheet.Cells[row, 9].Value = "0.30 (0.10)";

            row++;
            worksheet.Cells[row, 2].Value = "Min";
            worksheet.Cells[row, 3].Value = "0.22";
            worksheet.Cells[row, 4].Value = "0.26";
            worksheet.Cells[row, 5].Value = "0.26";
            worksheet.Cells[row, 6].Value = "0.24";
            worksheet.Cells[row, 7].Value = "0.27";
            worksheet.Cells[row, 8].Value = "0.20";
            worksheet.Cells[row, 9].Value = "0.12";

            row++;
            worksheet.Cells[row, 2].Value = "Max";
            worksheet.Cells[row, 3].Value = "0.75";
            worksheet.Cells[row, 4].Value = "0.73";
            worksheet.Cells[row, 5].Value = "0.65";
            worksheet.Cells[row, 6].Value = "0.61";
            worksheet.Cells[row, 7].Value = "0.66";
            worksheet.Cells[row, 8].Value = "0.51";
            worksheet.Cells[row, 9].Value = "0.40";

            row++;
            worksheet.Cells[row, 2].Value = "5th";
            worksheet.Cells[row, 3].Value = "0.24";
            worksheet.Cells[row, 4].Value = "0.26";
            worksheet.Cells[row, 5].Value = "0.26";
            worksheet.Cells[row, 6].Value = "0.24";
            worksheet.Cells[row, 7].Value = "0.27";
            worksheet.Cells[row, 8].Value = "0.20";
            worksheet.Cells[row, 9].Value = "0.13";

            row++;
            worksheet.Cells[row, 2].Value = "95th";
            worksheet.Cells[row, 3].Value = "0.72";
            worksheet.Cells[row, 4].Value = "0.72";
            worksheet.Cells[row, 5].Value = "0.65";
            worksheet.Cells[row, 6].Value = "0.58";
            worksheet.Cells[row, 7].Value = "0.60";
            worksheet.Cells[row, 8].Value = "-";
            worksheet.Cells[row, 9].Value = "-";

            row++;
            worksheet.Cells[row, 1].Value = "Search Speed";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row + 4, 1].Merge = true;

            row++;
            worksheet.Cells[row, 2].Value = "Mean (SD)";
            worksheet.Cells[row, 3].Value = "152.31 (25.93)";
            worksheet.Cells[row, 4].Value = "151.66 (28.65)";
            worksheet.Cells[row, 5].Value = "148.84 (21.93)";
            worksheet.Cells[row, 6].Value = "140.71 (26.39)";
            worksheet.Cells[row, 7].Value = "127.27 (24.71)";
            worksheet.Cells[row, 8].Value = "133.90 (25.67)";
            worksheet.Cells[row, 9].Value = "105.91 (17.05)";

            row++;
            worksheet.Cells[row, 2].Value = "Min";
            worksheet.Cells[row, 3].Value = "88.82";
            worksheet.Cells[row, 4].Value = "71.19";
            worksheet.Cells[row, 5].Value = "103.78";
            worksheet.Cells[row, 6].Value = "90.47";
            worksheet.Cells[row, 7].Value = "84.94";
            worksheet.Cells[row, 8].Value = "96.77";
            worksheet.Cells[row, 9].Value = "82.93";

            row++;
            worksheet.Cells[row, 2].Value = "Max";
            worksheet.Cells[row, 3].Value = "231.20";
            worksheet.Cells[row, 4].Value = "198.97";
            worksheet.Cells[row, 5].Value = "182.68";
            worksheet.Cells[row, 6].Value = "224.58";
            worksheet.Cells[row, 7].Value = "175.57";
            worksheet.Cells[row, 8].Value = "192.72";
            worksheet.Cells[row, 9].Value = "132.13";

            row++;
            worksheet.Cells[row, 2].Value = "5th";
            worksheet.Cells[row, 3].Value = "102.75";
            worksheet.Cells[row, 4].Value = "79.69";
            worksheet.Cells[row, 5].Value = "104.19";
            worksheet.Cells[row, 6].Value = "96.49";
            worksheet.Cells[row, 7].Value = "91.29";
            worksheet.Cells[row, 8].Value = "96.77";
            worksheet.Cells[row, 9].Value = "82.93";

            row++;
            worksheet.Cells[row, 2].Value = "95th";
            worksheet.Cells[row, 3].Value = "194.93";
            worksheet.Cells[row, 4].Value = "193.20";
            worksheet.Cells[row, 5].Value = "182.45";
            worksheet.Cells[row, 6].Value = "209.77";
            worksheet.Cells[row, 7].Value = "172.52";
            worksheet.Cells[row, 8].Value = "-";
            worksheet.Cells[row, 9].Value = "-";

            return row;
        }
    }
}
