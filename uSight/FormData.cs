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
    public partial class FormData : Form
    {
        List<string> words;

        public FormData(List<string> words)
        {
            InitializeComponent();

            this.words = words;

            foreach (string t in words)
            {
                textBox1.Text += t;
            }
        }

        private void DataForm_Load(object sender, EventArgs e)
        {

        }
    }
}