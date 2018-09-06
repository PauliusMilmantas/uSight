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
    public partial class Form1 : Form
    {
        dynamic json;

        public Form1()
        {
            DataExtraction de = new DataExtraction();
            json = de.getJson();

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
    }
}
