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

        [TestMethod]
        public void DBConnectionTest() {
            uSight.DataExtraction de = new uSight.DataExtraction();

            string owner = "own";
            string plate = "TES123";
            string id = "1";
            string engine_number = "asdg";
            string vin = "sdfgrth";

            dynamic json = de.GetJsonFromDisk();
            string backup = json.toString();

            int i = 0;
            foreach (dynamic t3 in json.plates) {
                json.plates.remove(0);
                i++;
            }

            de.writeToJson(json); //Empty json

            uSight.PlateRecord t = new uSight.PlateRecord(owner, id, plate, engine_number, vin);
            t.writeToJson();    //Local json file with 1 record

            uSight.DBConnect db = new uSight.DBConnect();
            db.updateWantedListDB(); //DB contains 1 record

            db.updateWantedListJSON();
            dynamic nde = de.GetJsonFromDisk();

            Assert.AreEqual((String) nde.plates.owner + (String) nde.plates.license_plate, owner + plate);

            de.writeToJson(backup); //Local JSON original v.
            db.updateWantedListDB();

            db.CloseConnection();
        }
    }
}
