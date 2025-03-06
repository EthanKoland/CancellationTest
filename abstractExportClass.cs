using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public abstract class abstractExportClass
    {
        public string patient_ID { get; protected set; }
        public abstractTestClass localExamObj { get;  set; }
        protected List<clickRow> clickRows;
        public bool invisableCancelation { get; set; } = false;

        //Variable used to store the target crosses data
        public int left_TargetCrossed { get; set; }
        public int right_TargetCrossed { get; set; }
        public int center_TargetCrossed { get; set; }
        public int total_Targets { get; set; }

        //Variable used to store the distractor crosses data
        public int leftGap_DistractorsCrossed { get; set; }
        public int rightGap_DistractorsCrossed { get; set; }
        public int total_DistractorsCrossed { get; set; }
        public int total_Distractors { get; set; }

        //Recancelations 
        public int recancelations { get; set; }

        //Time taken to complete the test
        public double right_TimeTaken { get; set; }
        public double left_TimeTaken { get; set; }
        public double total_TimeTaken { get; set; }

        //Search Speed
        public double right_SearchSpeed { get; set; }
        public double left_SearchSpeed { get; set; }
        public double total_SearchSpeed { get; set; }

        // Reclicks
        public int right_Reclicks { get; set; } = 0;
        public int left_Reclicks { get; set; } = 0;

        //Accuracy
        public double left_Accuracy { get; set; } = 0;
        public double center_Accuracy { get; set; } = 0;
        public double right_Accuracy { get; set; } = 0;

        //Distractors crossed in regions
        public double leftSide_leftGap_distractors { get; set; } = 0;
        public double leftSide_leftGap_distractorsCrossed { get; set; } = 0;
        public double leftSide_rightGap_distractors { get; set; } = 0;
        public double leftSide_rightGap_distractorsCrossed { get; set; } = 0;

        public double centerSide_leftGap_distractors { get; set; } = 0;
        public double centerSide_leftGap_distractorsCrossed { get; set; } = 0;
        public double centerSide_rightGap_distractors { get; set; } = 0;
        public double centerSide_rightGap_distractorsCrossed { get; set; } = 0;

        public double rightSide_leftGap_distractors { get; set; } = 0;
        public double rightSide_leftGap_distractorsCrossed { get; set; } = 0;
        public double rightSide_rightGap_distractors { get; set; } = 0;
        public double rightSide_rightGap_distractorsCrossed { get; set; } = 0;

        //Intersections
        public int intersections { get; set; } = 0;
        public double intersectionRate { get; set; } = 0;

        //Center of Cancellation
        public double centerOfCancellation { get; set; } = 0;

        //Adding the click table
        //Have an abstract function that will be used to add the click data to the table
        public void addClickData(List<clickRow> clickRows)
        {
            //Add the click data to the excel file
            this.clickRows = clickRows;
        }

        public abstract void export(string baseFolderLocation = null);

        public string getOutputPath(string basePath = null)
        {
            if (basePath == null)
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (!Directory.Exists(Path.Combine(basePath, "Output")))
            {
                Directory.CreateDirectory(Path.Combine(basePath, "Output"));
            }
            basePath = Path.Combine(basePath, "Output");

            if (!Directory.Exists(Path.Combine(basePath, this.patient_ID)))
            {
                Directory.CreateDirectory(Path.Combine(basePath, this.patient_ID));
            }
            basePath = Path.Combine(basePath, this.patient_ID);

            if (!Directory.Exists(Path.Combine(basePath, DateTime.Now.ToString("yyyy-MM-dd"))))
            {
                Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyy-MM-dd")));
            }
            return Path.Combine(basePath, DateTime.Now.ToString("yyyy-MM-dd"));

        }

        
        protected string getFileName(string folderPath, string fileExtension)
        {
            string fileName = this.localExamObj.isPratice ? "PracticeTest_" : "CancellationTest_";
            fileName += this.patient_ID + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileExtension;
            return Path.Combine(folderPath, fileName);
        }
    }
    public struct clickRow
    {
        public string success;
        public string time;
        public string matrixLocation;
        public string matrixSide;
        public string orientation;
        public string reCancel;
        public string PixelLocation;
        public Point normalizedLocation;
        public Point mugImageCenter;
    }
}
