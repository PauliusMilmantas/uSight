using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace uSight
{
    public partial class FormMain : Form
    {
        dynamic json;
        //The current media stream
        ImageSource currentImageSource;
        //Frame index, updated by trackbar scroll
        int currentFrame = 0;
        string textBox1WM = "Enter plates";             // Searchbaro watermarkas

        public FormMain()
        {
            InitializeComponent();


            textBox1.ForeColor = SystemColors.GrayText;
            textBox1.Text = textBox1WM;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                textBox1.Text = textBox1WM;
                textBox1.ForeColor = SystemColors.GrayText;
                label1.Text = "";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox1WM)
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            FormWantedList forma = new FormWantedList();
            forma.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            string plate_number = textBox1.Text.ToUpper();

            //JSON is disko
            DataExtraction de = new DataExtraction();
            json = de.GetJsonFromDisk();

            bool found = false;
            foreach (var obj in json.plates) {
                if (obj.plate_number == plate_number) {
                    found = true;
                }
            }

            if (textBox1.Text.Equals(textBox1WM))
            {
                label1.Text = "";
            }
            else if (found)
            {
                label1.Text = "Vehicle wanted";
                label1.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                label1.Text = "No records found";
                label1.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Media files|*.jpg;*.mp4";
            openFileDialog1.Title = "Select media file";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName) == ".jpg")
                {
                    currentImageSource = new ImageSource(new Image<Bgr, byte>(openFileDialog1.FileName));
                }
                else if (Path.GetExtension(openFileDialog1.FileName) == ".mp4")
                {
                    currentImageSource = new ImageSource(new VideoCapture(openFileDialog1.FileName));
                }
                pictureBox1.Image = currentImageSource[currentFrame].Bitmap;
                frameSelector.Maximum = currentImageSource.Count - 1;
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = LicenesePlateDetector.ShowContours(currentImageSource[currentFrame]);
        }

        private void frameSelector_Scroll(object sender, EventArgs e)
        {
            if (currentImageSource != null)
            {
                currentFrame = frameSelector.Value;
                pictureBox1.Image = currentImageSource[currentFrame].Bitmap;
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            UtilFunctions f = new UtilFunctions(this);

            Mat img = UtilFunctions.GetMatFromImage(pictureBox1.Image);

            UMat uImg = img.GetUMat(AccessType.ReadWrite);

            f.ProcessImage(uImg);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
