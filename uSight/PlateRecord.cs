using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSight
{
    public class PlateRecord
    {
        string owner;
        string license_plate;
        string engine_number;
        string vin;
        string id;

        public PlateRecord()
        {
        }

        public PlateRecord(string owner, string id, string license_plate, string engine_number, string vin)  //Ieskomu masinu duomenys
        {
            this.owner = owner;
            this.id = id;
            this.license_plate = license_plate;
            this.engine_number = engine_number;
            this.vin = vin;
        }

        public string Owner { get => owner; set => owner = value; }
        public string License_Plate { get => license_plate; set => license_plate = value; }
        public string Engine_number { get => engine_number; set => engine_number = value; }
        public string Vin { get => vin; set => vin = value; }

        public JObject PlateRecordJSON() {
            JObject record = new JObject();
            record["license_plate"] = license_plate;
            record["engine_number"] = engine_number;
            record["owner"] = owner;
            record["vin"] = vin;
            record["id"] = id;

            return record;
        }

        public void writeToJson() {
            DataExtraction de = new DataExtraction();

            dynamic json = de.GetJsonFromDisk();

            json.plates.Add(PlateRecordJSON());

            de.writeToJson(json.toString());
        }
    }
}
