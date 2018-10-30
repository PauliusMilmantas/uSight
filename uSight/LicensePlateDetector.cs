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
    public class LicenesePlateDetector
    {
        private Tesseract ocr;

        public LicenesePlateDetector(String dataPath)
        {
            ocr = new Tesseract(dataPath, "eng", OcrEngineMode.TesseractLstmCombined);

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

            try
            {
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
                CvInvoke.Canny(gray, canny, 100, 50, 3, false);
                int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);

                FindLicensePlate(contours, hierachy, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
            } catch (Exception) {
                return null;
            }

            return licenses;
        }

        private static UMat FilterPlate(UMat plate)
        {
            //Segmentation method
            //separate out regions of an image corresponding to objects which we want to analyze
            UMat thresh = new UMat();
            CvInvoke.Threshold(plate, thresh, 120, 255, ThresholdType.BinaryInv);

            Mat plateMask = new Mat(plate.Size.Height, plate.Size.Width, DepthType.Cv8U, 1);
            Mat plateCanny = new Mat();
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            /*         
            Canny Edge detector for:
            Low error rate: Meaning a good detection of only existent edges.
            Good localization: The distance between edge pixels detected and real edge pixels have to be minimized.
            Minimal response: Only one detector response per edge.
            */

            plateMask.SetTo(new MCvScalar(255.0));
            CvInvoke.Canny(plate, plateCanny, 100, 50);

            /*
            Contours - curve joining all the continuous points 
            */

            CvInvoke.FindContours(plateCanny, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            int count = contours.Size;
            for (int i = 0; i < count; i++)
            {
                VectorOfPoint contour = contours[i];
                    Rectangle rect = CvInvoke.BoundingRectangle(contour);
                    if (rect.Height > (plate.Size.Height >> 1))
                    {
                        rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                        Rectangle roi = new Rectangle(Point.Empty, plate.Size);
                        rect.Intersect(roi);
                        CvInvoke.Rectangle(plateMask, rect, new MCvScalar(), -1);
                    }
            }

            thresh.SetTo(new MCvScalar(), plateMask);

            //The erosion makes the object in white smaller
            CvInvoke.Erode(thresh, thresh, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

            //The dilatation makes the object in white bigger
            CvInvoke.Dilate(thresh, thresh, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

            return thresh;
        }

        private static int GetNumberOfChildren(int[,] hierachy, int idx)
        {
            idx = hierachy[idx, 2];
            if (idx < 0)
                return 0;

            int count = 1;
            while (hierachy[idx, 0] > 0)
            {
                count++;
                idx = hierachy[idx, 0];
            }
            return count;
        }

        private void FindLicensePlate(
           VectorOfVectorOfPoint contours, int[,] hierachy, IInputArray gray, IInputArray canny,
           List<IInputOutputArray> licensePlateImagesList, List<IInputOutputArray> filteredLicensePlateImagesList, List<RotatedRect> detectedLicensePlateRegionList,
           List<String> licenses, int idx = 0) {          

            for (; idx >= 0; idx = hierachy[idx, 0])
            {
                int lettersCount = GetNumberOfChildren(hierachy, idx);
                //if it does not contains any children (charactor), it is not a license plate region
                if (lettersCount == 0) continue;

                using (VectorOfPoint contour = contours[idx])
                {
                    if (CvInvoke.ContourArea(contour) > 400)
                    {
                        if (lettersCount < 2)
                        {
                            //If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                            //However we should search the children of this contour to see if any of them is a license plate
                            FindLicensePlate(contours, hierachy, gray, canny, licensePlateImagesList,
                               filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses, hierachy[idx, 2]);
                            continue;
                        }

                        RotatedRect box = CvInvoke.MinAreaRect(contour);
                        if (box.Angle < -45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle += 90.0f;
                        }
                        else if (box.Angle > 45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle -= 90.0f;
                        }

                        using (UMat tmp1 = new UMat())
                        using (UMat tmp2 = new UMat())
                        {
                            PointF[] srcCorners = box.GetVertices();

                            PointF[] destCorners = new PointF[] {
                                new PointF(0, box.Size.Height - 1),
                                new PointF(0, 0),
                                new PointF(box.Size.Width - 1, 0),
                                new PointF(box.Size.Width - 1, box.Size.Height - 1)};

                            using (Mat rot = CvInvoke.GetAffineTransform(srcCorners, destCorners))
                            {
                                CvInvoke.WarpAffine(gray, tmp1, rot, Size.Round(box.Size));
                            }

                            //resize the license plate such that the front is ~ 10-12. This size of front results in better accuracy from tesseract
                            Size approxSize = new Size(240, 180);
                            double scale = Math.Min(approxSize.Width / box.Size.Width, approxSize.Height / box.Size.Height);
                            Size newSize = new Size((int)Math.Round(box.Size.Width * scale), (int)Math.Round(box.Size.Height * scale));
                            CvInvoke.Resize(tmp1, tmp2, newSize, 0, 0, Inter.Cubic);

                            //removes some pixels from the edge
                            int edgePixelSize = 3;
                            Rectangle newRoi = new Rectangle(new Point(edgePixelSize, edgePixelSize),
                               tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                            UMat plate = new UMat(tmp2, newRoi);

                            UMat filteredPlate = FilterPlate(plate);

                            ocr.SetImage(filteredPlate.Clone());
                            ocr.Recognize();

                            licenses.Add(ocr.GetUTF8Text());
                            licensePlateImagesList.Add(filteredPlate);
                            filteredLicensePlateImagesList.Add(filteredPlate);
                            detectedLicensePlateRegionList.Add(box);

                        }
                    }
                }
            }
        }
    }
}