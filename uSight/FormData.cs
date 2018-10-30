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
    public partial class FormData : Form
    {
        List <string> words;
        private string licensePlate = "";

        public FormData(List<string> words)
        {
            InitializeComponent();

            this.words = words;

            foreach (string t in words)
            {
                textBox1.Text += t;
            }

            licensePlate = GetFilteredLicensePlate(textBox1.Text.ToString());
        }

        private string GetFilteredLicensePlate (string input)
        {
            string pattern = "[A-Z]{3} *[0-9]{3}";
            Match match = Regex.Match(input, pattern);
            return Regex.Replace(match.ToString(), @"\s+", "");
        }

        public string GetLicensePlate()
        {
            return licensePlate;
        }

        private void DataForm_Load(object sender, EventArgs e)
        {

        }
    }
}