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
        public void DBFunctionsTest()
        {
            DataExtraction de = new DataExtraction();
            
            uSight.DBConnect db = new uSight.DBConnect();

            dynamic old = de.GetJsonFromDisk();

            db.updateWantedListJSON();

            dynamic newJson = de.GetJsonFromDisk();
            
            Assert.AreNotEqual(old, newJson);  
        }

        [TestMethod()]
        public void FileTest() {

            string owner = "own";
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