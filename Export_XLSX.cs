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

                worksheet.Cells[1, 1].Value = "Date:";
                worksheet.Cells[1, 2].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                worksheet.Cells[1, 3].Value = "Patient ID:";
                worksheet.Cells[1, 4].Value = this.patient_ID;

                //Some sort of informatino on the cellencaltion condition
                worksheet.Cells[2, 1].Value = "Unsure";

                //Session Resume
                worksheet.Cells[3, 1].Value = "Session Resume";

                worksheet.Cells[4, 1].Value = "Targets Cancelled";
                //Ration of the number of targets cancelled versus the total number of targets
                worksheet.Cells[4, 2].Value = "" + (left_TargetCrossed + right_TargetCrossed + center_TargetCrossed) + "/" + total_Targets;

                worksheet.Cells[5, 1].Value = "Distractors Cancelled";
                //Ration of the number of distractors cancelled versus the total number of distractors
                worksheet.Cells[5, 2].Value = "" + total_DistractorsCrossed + "/" + total_Distractors;

                worksheet.Cells[6, 1].Value = "Re-cancellations";
                worksheet.Cells[6, 2].Value = recancelations;

                worksheet.Cells[7, 1].Value = "Total Time Taken";
                worksheet.Cells[7, 2].Value = total_TimeTaken;

                worksheet.Cells[9, 1].Value = "Time Spent on the right side";
                worksheet.Cells[9, 2].Value = (Math.Round(right_TimeTaken / total_TimeTaken, 4) * 100) + "%";


                worksheet.Cells[10, 1].Value = "Time Spent on the left side";
                worksheet.Cells[10, 2].Value = (Math.Round(left_TimeTaken / total_TimeTaken, 4) * 100) + "%";

                worksheet.Cells[11, 1].Value = "Asymmetry Score";
                worksheet.Cells[11, 2].Value = Math.Round((right_TimeTaken - left_TimeTaken) / total_TimeTaken, 4);

                worksheet.Cells[13, 1].Value = "Search Speed";
                worksheet.Cells[13, 2].Value = total_SearchSpeed;

                worksheet.Cells[14, 1].Value = "Left Search Speed";
                worksheet.Cells[14, 2].Value = left_SearchSpeed;

                worksheet.Cells[15, 1].Value = "Right Search Speed";
                worksheet.Cells[15, 2].Value = right_SearchSpeed;

                worksheet.Cells[17, 1].Value = "Reclicks on the right side";
                worksheet.Cells[17, 2].Value = right_Reclicks;

                worksheet.Cells[18, 1].Value = "Reclicks on the left side";
                worksheet.Cells[18, 2].Value = left_Reclicks;

                worksheet.Cells[20, 1].Value = "Accuracy per location";

                worksheet.Cells[21, 1].Value = "Left";
                worksheet.Cells[21, 2].Value = left_Accuracy;

                worksheet.Cells[22, 1].Value = "Center";
                worksheet.Cells[22, 2].Value = center_Accuracy;

                worksheet.Cells[23, 1].Value = "Right";
                worksheet.Cells[23, 2].Value = right_Accuracy;

                worksheet.Cells[25, 1].Value = "Distractors crossed in each screen region";
                worksheet.Cells[26, 1].Value = "Left Gap";

                worksheet.Cells[27, 1].Value = "Left";
                worksheet.Cells[27, 2].Value = leftSide_leftGap_distractors;

                worksheet.Cells[28, 1].Value = "Center";
                worksheet.Cells[28, 2].Value = centerSide_leftGap_distractors;

                worksheet.Cells[29, 1].Value = "Right";
                worksheet.Cells[29, 2].Value = rightSide_leftGap_distractors;

                worksheet.Cells[30, 1].Value = "Right Gap";

                worksheet.Cells[31, 1].Value = "Left";
                worksheet.Cells[31, 2].Value = leftSide_rightGap_distractors;

                worksheet.Cells[32, 1].Value = "Center";
                worksheet.Cells[32, 2].Value = centerSide_rightGap_distractors;

                worksheet.Cells[33, 1].Value = "Right";
                worksheet.Cells[33, 2].Value = rightSide_rightGap_distractors;

                worksheet.Cells[35, 1].Value = "Egocentric neglect Subscores";

                worksheet.Cells[37, 1].Value = "Total Targets Cancelled in right side of grid";
                worksheet.Cells[37, 2].Value = this.right_TargetCrossed;

                worksheet.Cells[38, 1].Value = "Total Targets Cancelled in left side of grid";
                worksheet.Cells[38, 2].Value = this.left_TargetCrossed;

                worksheet.Cells[39, 1].Value = "Egocentric neglect";
                worksheet.Cells[39, 2].Value = this.right_TargetCrossed - this.left_TargetCrossed;

                worksheet.Cells[41, 1].Value = "Allocentric neglect Subscores";

                worksheet.Cells[42, 1].Value = "Total number of left-gap distractors cancelled";
                worksheet.Cells[42, 2].Value = this.leftGap_DistractorsCrossed;

                worksheet.Cells[43, 1].Value = "Total number of right-gap distractors cancelled";
                worksheet.Cells[43, 2].Value = this.rightGap_DistractorsCrossed;

                worksheet.Cells[44, 1].Value = "Allocentric neglect";
                worksheet.Cells[44, 2].Value = this.rightGap_DistractorsCrossed - this.leftGap_DistractorsCrossed;

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
