using Microsoft.VisualStudio.TestTools.UnitTesting;
using uSight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSight.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void DBFunctionsTest() //Downloading
        {
            DataExtraction de = new DataExtraction();            
            DBConnect db = new DBConnect();

            db.updateWantedListJSON();

            dynamic old = de.GetJsonFromDisk();

            string owner = "ow2n";
            string plate = "TES465";
            string id = "1";
            string engine_number = "asdg";
            string vin = "sdfgrth";

            PlateRecord pr = new PlateRecord(owner, id, plate, engine_number, vin);
            pr.writeToJson();

            dynamic mod = de.GetJsonFromDisk();

            DBConnect t = new DBConnect();
            t.updateWantedListJSON();

            dynamic newJson = de.GetJsonFromDisk();

            Assert.AreNotEqual(mod, newJson);  
        }

        /*[TestMethod()]
        public void testingConnection() { //Updating database
            string owner = "ow2n";
            string plate = "TES465";
            string id = "1";
            string engine_number = "asdg";
            string vin = "sdfgrth";

            DBConnect db = new DBConnect();
            DataExtraction de = new DataExtraction();

            db.updateWantedListJSON();

            dynamic jsonOld = de.GetJsonFromDisk();            

            PlateRecord pr = new PlateRecord(owner, id, plate, engine_number, vin);
            pr.writeToJson();

            db.updateWantedListDB();
            db.updateWantedListJSON();

            dynamic jsonNew = de.GetJsonFromDisk();

            de.writeToJson(jsonOld.ToString());
            db.updateWantedListDB();

            Assert.AreNotEqual(jsonOld.ToString(), jsonNew.ToString());
        }*/

        [TestMethod()]
        public void FileTest() {

            string owner = "ow2n";
            string plate = "TES123";
            string id = "1";
            string engine_number = "asdg";
            string vin = "sdfgrth";

            DataExtraction de = new DataExtraction();

            dynamic jsonOld = de.GetJsonFromDisk();

            PlateRecord pr = new PlateRecord(owner, id, plate, engine_number, vin);
            pr.writeToJson();

            dynamic jsonNew = de.GetJsonFromDisk();

            de.writeToJson(jsonOld.ToString());

            Assert.AreNotEqual(jsonOld, jsonNew);
        }
    }
}