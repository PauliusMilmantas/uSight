using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Windows.Forms;

namespace uSight
{
    public partial class Form1 : Form
    {
        dynamic json;
        Image<Bgr, byte> image;

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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg";
            openFileDialog1.Title = "Select image file";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                image = new Image<Bgr, byte>(openFileDialog1.FileName);
                pictureBox1.Image = image.Bitmap;
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = LicenesePlateDetector.ShowContours(image);
        }
    }
}
