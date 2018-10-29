using Microsoft.VisualStudio.TestTools.UnitTesting;
using uSight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace uSight.Tests
{
    class SingleImageSource : ImageSource
    {
        public SingleImageSource()
        {

        }

        public override Image<Bgr, byte> this[int index] { get => null; set { } }

        public override int Count { get => 1; set { } }
    }

    [TestClass]
    public class StatisticsFormTests
    {
        [TestMethod]
        public void GetPlatesList_NoWanted_ReturnsOnlyPlates()
        {
            var imgSource = new SingleImageSource();
            var date = DateTime.MinValue;
            var wantedJson = JObject.Parse(@"{""plates"": []}");
            Func<Image<Bgr, byte>, List<string>> processImage = x => new List<string>() {"ABC123", "DEF456"};

            var plateList = StatisticsForm.GetPlatesList(imgSource, date, wantedJson, processImage);

            var plate1 = new JObject();
            plate1["time"] = date;
            plate1["number"] = "ABC123";
            plate1["stolen"] = false;
            var plate2 = new JObject();
            plate2["time"] = date;
            plate2["number"] = "DEF456";
            plate2["stolen"] = false;
            var correctOutput = new List<JObject>() { plate1, plate2 };
            foreach (var plate in plateList)
            {
                Assert.IsTrue(correctOutput.Any(x => JToken.DeepEquals(plate, x)));
            }
        }

        [TestMethod]
        public void GetPlatesList_WantedAndNotWanted_ReturnsBothPlates()
        {
            var imgSource = new SingleImageSource();
            var date = DateTime.MinValue;
            var wantedJson = JObject.Parse(@"{""plates"": [ {""owner"": ""Jonas"",""license_plate"": ""ABC123"",""engine_number"": ""VW12345U123456P"",""vin"": ""1N6AD0EV7BC423878"",""id"": 1} ]}");
            Func<Image<Bgr, byte>, List<string>> processImage = x => new List<string>() { "ABC123", "DEF456" };

            var plateList = StatisticsForm.GetPlatesList(imgSource, date, wantedJson, processImage);

            var plate1 = new JObject();
            plate1["time"] = date;
            plate1["number"] = "ABC123";
            plate1["stolen"] = true;
            var plate2 = new JObject();
            plate2["time"] = date;
            plate2["number"] = "DEF456";
            plate2["stolen"] = false;
            var correctOutput = new List<JObject>() { plate1, plate2 };
            foreach (var plate in plateList)
            {
                Assert.IsTrue(correctOutput.Any(x => JToken.DeepEquals(plate, x)));
            }
        }

        [TestMethod]
        public void GetAllList_TwoOcurrences_ReturnsCorrectCount()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MinValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = false;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MinValue;
            plate2["number"] = "DEF456";
            plate2["stolen"] = false;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetAllList(json as JObject);
           
            var correctOutput = new List<Tuple<DateTime, int>>() { Tuple.Create(DateTime.MinValue, 2) };
            foreach (var plate in plateList)
            {
                Assert.IsTrue(correctOutput.Any(x => x.Item1 == plate.Item1 && x.Item2 == plate.Item2));
            }
        }

        [TestMethod]
        public void GetAllList_Unordered_ReturnsOrdered()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MaxValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = false;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MinValue;
            plate2["number"] = "DEF456";
            plate2["stolen"] = false;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetAllList(json as JObject);

            var correctOutput = new List<Tuple<DateTime, int>>() { Tuple.Create(DateTime.MinValue, 1), Tuple.Create(DateTime.MaxValue, 1) };
            for (int i = 0;i < correctOutput.Count;i++)
            {
                var x = correctOutput[i];
                var plate = plateList[i];
                Assert.IsTrue(x.Item1 == plate.Item1 && x.Item2 == plate.Item2);
            }
        }

        [TestMethod]
        public void GetStolenList_TwoOcurrences_ReturnsCorrectCount()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MinValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = true;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MinValue;
            plate2["number"] = "DEF456";
            plate2["stolen"] = true;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetStolenList(json as JObject);

            var correctOutput = new List<Tuple<DateTime, int>>() { Tuple.Create(DateTime.MinValue, 2) };
            foreach (var plate in plateList)
            {
                Assert.IsTrue(correctOutput.Any(x => x.Item1 == plate.Item1 && x.Item2 == plate.Item2));
            }
        }

        [TestMethod]
        public void GetStolenList_Unordered_ReturnsOrdered()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MaxValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = true;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MinValue;
            plate2["number"] = "DEF456";
            plate2["stolen"] = true;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetStolenList(json as JObject);

            var correctOutput = new List<Tuple<DateTime, int>>() { Tuple.Create(DateTime.MinValue, 1), Tuple.Create(DateTime.MaxValue, 1) };
            for (int i = 0; i < correctOutput.Count; i++)
            {
                var x = correctOutput[i];
                var plate = plateList[i];
                Assert.IsTrue(x.Item1 == plate.Item1 && x.Item2 == plate.Item2);
            }
        }

        [TestMethod]
        public void GetBreakdownList_TwoOcurrences_ReturnsCorrectCount()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MinValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = false;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MaxValue;
            plate2["number"] = "ABC123";
            plate2["stolen"] = false;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetBreakdownList(json as JObject);

            var correctOutput = new List<Tuple<string, int>>() { Tuple.Create("ABC123", 2)};
            for (int i = 0; i < correctOutput.Count; i++)
            {
                var x = correctOutput[i];
                var plate = plateList[i];
                Assert.IsTrue(x.Item1 == plate.Item1 && x.Item2 == plate.Item2);
            }
        }

        [TestMethod]
        public void GetBreakdownListTest_Unordered_ReturnsOrdered()
        {
            dynamic json = JObject.Parse(@"{""plates"": []}");
            var plate1 = new JObject();
            plate1["time"] = DateTime.MaxValue;
            plate1["number"] = "ABC123";
            plate1["stolen"] = false;
            json.plates.Add(plate1);
            var plate2 = new JObject();
            plate2["time"] = DateTime.MinValue;
            plate2["number"] = "DEF456";
            plate2["stolen"] = false;
            json.plates.Add(plate2);

            var plateList = StatisticsForm.GetBreakdownList(json as JObject);

            var correctOutput = new List<Tuple<string, int>>() { Tuple.Create("ABC123", 1), Tuple.Create("DEF456", 1) };
            for (int i = 0; i < correctOutput.Count; i++)
            {
                var x = correctOutput[i];
                var plate = plateList[i];
                Assert.IsTrue(x.Item1 == plate.Item1 && x.Item2 == plate.Item2);
            }
        }
    }
}
