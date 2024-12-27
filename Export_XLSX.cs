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
                if (excel.Workbook.Worksheets["Sheet1"] != null)
                {
                    excel.Workbook.Worksheets.Delete("Sheet1");
                }

                excel.Workbook.Worksheets.Add("Sheet1");

                var worksheet = excel.Workbook.Worksheets["Sheet1"];

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

                //Session Resume
                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Session Resume";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Targets Cancelled";
                worksheet.Cells[row, 2].Value = "" + (left_TargetCrossed + right_TargetCrossed + center_TargetCrossed) + "/" + total_Targets;

                row++;
                worksheet.Cells[row, 1].Value = "Distractors Cancelled";
                worksheet.Cells[row, 2].Value = "" + total_DistractorsCrossed + "/" + total_Distractors;

                row++;
                worksheet.Cells[row, 1].Value = "Re-cancellations";
                worksheet.Cells[row, 2].Value = recancelations;

                row++;
                worksheet.Cells[row, 1].Value = "Total Time Taken";
                worksheet.Cells[row, 2].Value = total_TimeTaken;

                row++;
                worksheet.Cells[row, 1].Value = "Quality Score";
                worksheet.Cells[row, 2].Value = Math.Pow((left_TargetCrossed + right_TargetCrossed + center_TargetCrossed), 2) / (total_Targets * total_TimeTaken);

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Time Spent on the right side";
                worksheet.Cells[row, 2].Value = (Math.Round(right_TimeTaken / total_TimeTaken, 4) * 100) + "%";

                row++;
                worksheet.Cells[row, 1].Value = "Time Spent on the left side";
                worksheet.Cells[row, 2].Value = (Math.Round(left_TimeTaken / total_TimeTaken, 4) * 100) + "%";

                row++;
                worksheet.Cells[row, 1].Value = "Asymmetry Score";
                worksheet.Cells[row, 2].Value = Math.Round((right_TimeTaken - left_TimeTaken) / total_TimeTaken, 4);

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Search Speed";
                worksheet.Cells[row, 2].Value = total_SearchSpeed;

                row++;
                worksheet.Cells[row, 1].Value = "Left Search Speed";
                worksheet.Cells[row, 2].Value = left_SearchSpeed;

                row++;
                worksheet.Cells[row, 1].Value = "Right Search Speed";
                worksheet.Cells[row, 2].Value = right_SearchSpeed;

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
                worksheet.Cells[row, 2].Value = this.right_TargetCrossed - this.left_TargetCrossed;

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
                worksheet.Cells[row, 2].Value = this.rightGap_DistractorsCrossed - this.leftGap_DistractorsCrossed;

                row++;
                row++;
                worksheet.Cells[row, 1].Value = "Intersections";
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                row++;
                worksheet.Cells[row, 1].Value = "Number of intersections";
                worksheet.Cells[row, 2].Value = intersections;

                row++;
                worksheet.Cells[row, 1].Value = "Intersection Rate";
                worksheet.Cells[row, 2].Value = intersectionRate;

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



                excel.Save();

            }


        }
    }
}
