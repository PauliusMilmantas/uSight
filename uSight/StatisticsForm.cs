using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uSight
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm()
        {
            InitializeComponent();
        }

        private dynamic GetJsonFromDisk()
        {

            string data = File.ReadAllText("stats.json");
            dynamic tmpObj = JObject.Parse(data);

            return tmpObj;
        }

        private void writeToJson(string json)
        {
            System.IO.File.WriteAllText("stats.json", json);
        }

        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            JObject json = GetJsonFromDisk();



            writeToJson(json.ToString());
        }
    }
}
