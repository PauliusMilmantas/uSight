using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;

namespace uSight
{
    public class LicenesePlateDetector {
        public LicenesePlateDetector() {

        }

        public static Image ShowContours(Image<Bgr, byte> image) {

            Image<Gray, byte> imageOutput = image.Convert<Gray, Byte>().ThresholdBinary(new Gray(100), new Gray(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();

            Mat hier = new Mat();

            CvInvoke.FindContours(imageOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.DrawContours(image, contours, -1, new MCvScalar(255, 0, 0));

            return imageOutput.Bitmap;
        }
    }
}
