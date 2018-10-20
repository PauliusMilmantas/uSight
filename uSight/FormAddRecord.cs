using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void button2_Click(object sender, EventArgs e)
        {
            string license_plate = textBox1.Text;
            string owner = textBox3.Text;
            string engine_number = textBox2.Text;
            string body_number = textBox4.Text;

            DataExtraction de = new DataExtraction();
            dynamic json = de.GetJsonFromDisk();

            JObject record = new JObject();
            record["owner"] = owner;
            record["plate_number"] = license_plate;
            record["engine_number"] = engine_number;
            record["vehicle_number"] = body_number;
            record["id"] = json.plates.Count + 1;

            json.plates.Add(record);
            de.writeToJson(json.ToString());

            this.Close();
            cp.refrestView();
        }

        private void AddRecord_Load(object sender, EventArgs e)
        {

        }
    }
}
