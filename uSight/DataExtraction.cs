using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace uSight
{
    class DataExtraction
    {
        public dynamic GetJsonFromDisk() {

            string data = File.ReadAllText("data.json");
            dynamic tmpObj = JObject.Parse(data);

            return tmpObj;
        }

        public void writeToJson(string json) {
            System.IO.File.WriteAllText("data.json", json);
        }
    }
}
