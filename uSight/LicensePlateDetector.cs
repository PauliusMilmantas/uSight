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
        private Tesseract ocr;

        public LicenesePlateDetector(String dataPath)
        {
            ocr = new Tesseract("", "eng", OcrEngineMode.TesseractLstmCombined);

            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890");
        }

        public List<String> DetectLicensePlate(
           IInputArray img,
           List<IInputOutputArray> licensePlateImagesList,
           List<IInputOutputArray> filteredLicensePlateImagesList,
           List<RotatedRect> detectedLicensePlateRegionList)
        {
            List<String> licenses = new List<String>();
            Mat gray = new Mat();
            Mat canny = new Mat();
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
            CvInvoke.Canny(gray, canny, 100, 50, 3, false);
            int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);

            FindLicensePlate(contours, hierachy, 0, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);

            return licenses;
        }

        public static Image ShowContours(Image<Bgr, byte> image) {

                            //removes some pixels from the edge
                            int edgePixelSize = 3;
                            Rectangle newRoi = new Rectangle(new Point(edgePixelSize, edgePixelSize),
                               tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                            UMat plate = new UMat(tmp2, newRoi);

                            ocr.SetImage(plate.Clone());
                            ocr.Recognize();

<<<<<<< HEAD
            CvInvoke.FindContours(imageOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.DrawContours(imageOutput, contours, -1, new MCvScalar(255, 0, 0));
=======
                            licenses.Add(ocr.GetUTF8Text());
                            licensePlateImagesList.Add(plate);
                            filteredLicensePlateImagesList.Add(plate);
                            detectedLicensePlateRegionList.Add(box);
>>>>>>> Development

                        }
                    }
                }
            }
        }
    }
}
