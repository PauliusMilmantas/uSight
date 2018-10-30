using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uSight
{
    public partial class FormAddRecord : Form
    {
        FormWantedList cp;

        public FormAddRecord(FormWantedList cp)
        {
            this.cp = cp;
            InitializeComponent();
        }
        public FormAddRecord()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveRecord(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            this.Close();
            cp.refreshView();
        }
        public void SaveRecord(string licensePlate, string engineNumber, string owner, string vin)
        { 
            licensePlate = Regex.Replace(licensePlate, @"\s+", "");

            DataExtraction de = new DataExtraction();
            dynamic json = de.GetJsonFromDisk();

            JObject record = new JObject();
            record["license_plate"] = licensePlate;
            record["engine_number"] = engineNumber;
            record["owner"] = owner;
            record["vin"] = vin;
            record["id"] = json.plates.Count + 1;

            json.plates.Add(record);
            de.writeToJson(json.ToString());
        }

        private void AddRecord_Load(object sender, EventArgs e)
        {

        }
    }
}
