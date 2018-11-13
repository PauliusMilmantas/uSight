using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace uSight_Web.Entities
{
    public partial class FormData
    {
        List<string> words;
        private string licensePlate = "";

        public FormData(List<string> words)
        {
            this.words = words;
            string text = "";

            foreach (string t in words)
            {
                text += t;
            }

            licensePlate = GetFilteredLicensePlate(text);
        }

        private string GetFilteredLicensePlate(string input)
        {
            string pattern = "[A-Z]{3} *[0-9]{3}";
            Match match = Regex.Match(input, pattern);

            //Console.WriteLine("|" + match.ToString() + "|");
            return match.ToString();
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