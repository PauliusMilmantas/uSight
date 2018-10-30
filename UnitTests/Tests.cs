using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uSight;

namespace UnitTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ImageScan()
        {
            Image img = Image.FromFile("..\\..\\..\\Test Photos\\Unit Tests\\test1.jpg");
            string plate = new FormMain().ScanImage(img);
            Console.WriteLine(plate);
        }

        [TestMethod]
        public void WantedListSearch()
        {
            string plate = "HVH 222";
            string result = new FormMain(plate).SearchInWantedList(plate);
            Console.WriteLine(result);
        }

    }
}
