using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancellationTest
{
    public class mugObject
    {
        public int imageID { get; private set; }
        public imageTypes imageType { get; private set; }
        public Point imageCenter { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public bool isClicked { get; private set; }

        public leftRightCenter side { get; private set; }

        public int matrixLocation { get; private set; }

        public mugObject(int imageID, imageTypes imageType, Point imageCenter, int width, int height, leftRightCenter side, int matrixLocation)
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

        public void setClicked( int vOffset, double adjust)
        {
            //If the image is clicked then set the isClicked to true, but if it is already clicked then set it to false
            this.isClicked = !this.isClicked;
            Console.WriteLine("Image Clicked : " + this.imageID + "is clicked " + this.isClicked);
        }

        

        public Boolean isDistractor()
        {
            return this.imageType == imageTypes.DistractionLeft || this.imageType == imageTypes.DistractionRight;
        }

        //Static helper function to get an image object based on the image type
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

        //Helper function to get the orientation of the image
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
}
