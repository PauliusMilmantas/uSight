﻿using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace uSight_Web.Entities
{
    class UtilFunctions
    {
        delegate List<string> DetectorDelegate(IInputArray img, List<IInputOutputArray> licensePlateImagesList, List<IInputOutputArray> filteredLicensePlateImagesList, List<RotatedRect> detectedLicensePlateRegionList);
        private DetectorDelegate DetectLicensePlate;

        public UtilFunctions(string path)
        {
            DetectLicensePlate = new LicenesePlateDetector(path).DetectLicensePlate;
        }

        public UtilFunctions(IDetector d)
        {
            DetectLicensePlate = d.DetectLicensePlate;
        }

        public static Image ShowContours(Image<Bgr, byte> image)
        {

            Image<Gray, byte> imageOutput = image.Convert<Gray, Byte>();
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();

            Mat hier = new Mat();

            CvInvoke.BitwiseNot(imageOutput, imageOutput);
            CvInvoke.AdaptiveThreshold(imageOutput, imageOutput, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.GaussianC, Emgu.CV.CvEnum.ThresholdType.Binary, 15, -26);
            CvInvoke.FindContours(imageOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.DrawContours(imageOutput, contours, -1, new MCvScalar(255, 0, 0));
            CvInvoke.BitwiseNot(imageOutput, imageOutput);


            return imageOutput.Bitmap;
        }

        public static Mat GetMatFromImage(System.Drawing.Image image)
        {
            int stride = 0;
            Bitmap bmp = new Bitmap(image);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            System.Drawing.Imaging.PixelFormat pf = bmp.PixelFormat;
            if (pf == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                stride = bmp.Width * 4;
            }
            else
            {
                stride = bmp.Width * 3;
            }

            Image<Bgra, byte> cvImage = new Image<Bgra, byte>(bmp.Width, bmp.Height, stride, (IntPtr)bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            return cvImage.Mat;
        }

        public List<string> ProcessImage(IInputOutputArray image)
        {
            List<IInputOutputArray> licensePlateImagesList = new List<IInputOutputArray>();
            List<IInputOutputArray> filteredLicensePlateImagesList = new List<IInputOutputArray>();

            List<RotatedRect> licenseBoxList = new List<RotatedRect>();

            List<string> words = DetectLicensePlate(
               image,
               licensePlateImagesList,
               filteredLicensePlateImagesList,
               licenseBoxList);

            return words;
        }
    }
}