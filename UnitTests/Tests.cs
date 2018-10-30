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
            string foundPlate = new FormMain().ScanImage(img);
            Assert.AreEqual("HVH222", foundPlate);
        }

        [TestMethod]
        public void AddSearchRemove()
        {
            string plate = "GGG123";
            new FormAddRecord().SaveRecord(plate, "", "", "");

            string result = new FormMain(plate).SearchInWantedList(plate);
            Assert.AreEqual("Vehicle " + plate + " is wanted", result);

            FormWantedList fwl = new FormWantedList("");
            fwl.RemoveRecord(fwl.GetRecordCount());
        }

    }
}
