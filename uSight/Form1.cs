using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;

namespace uSight
{
    public partial class Form1 : Form
    {
        dynamic json;

        public Form1()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            ControlPanel forma = new ControlPanel();
            forma.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string plate_number = textBox1.Text;

            //JSON is disko
            DataExtraction de = new DataExtraction();
            json = de.GetJsonFromDisk();

            bool found = false;
            foreach (var obj in json.plates) {
                if (obj.p_number == plate_number) {
                    found = true;
                }
            }

            if (found)
            {
                label1.Text = "Vehicle wanted";
                label1.ForeColor = System.Drawing.Color.Red;
            }
            else {
                label1.Text = "No records found";
                label1.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
