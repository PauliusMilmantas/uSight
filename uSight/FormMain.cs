using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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

        public FormMain()                           // Konstruktorius
        {
            InitializeComponent();

            textBox1.ForeColor = SystemColors.GrayText;
            textBox1.Text = textBox1WM;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
        }
        public FormMain(string text)                           // Konstruktorius
        {
            InitializeComponent();
            textBox1.Text = text;
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
            SearchInWantedList(plate_number);
        }

        public string SearchInWantedList (string licensePlate)
        {
            string original = Regex.Replace(licensePlate, @"\s+", " ");
            licensePlate = Regex.Replace(licensePlate, @"\s+", "");

            //JSON is disko
            DataExtraction de = new DataExtraction();
            json = de.GetJsonFromDisk();

            bool found = false;
            foreach (var obj in json.plates)
            {
                if (obj.license_plate == licensePlate)
                {
                    found = true;
                }
            }

            if (textBox1.Text.Equals(textBox1WM))
            {
                label1.Text = "";
            }
            else if (textBox1.Text.Equals(""))
            {
                label1.Text = "Vehicle not identified";
                label1.ForeColor = System.Drawing.Color.Black;
            }
            else if (found)
            {
                label1.Text = "Vehicle " + original + " is wanted";
                label1.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                label1.Text = "Vehicle " + original + " is not wanted";
                label1.ForeColor = System.Drawing.Color.Green;
            }

            return label1.Text;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {}

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Media files|*.png;*.jpg;*.mp4";
            openFileDialog1.Title = "Select media file";
            string foundLP = "";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName) == ".jpg" || Path.GetExtension(openFileDialog1.FileName) == ".png")
                {
                    currentImageSource = new ImageSource(new Image<Bgr, byte>(openFileDialog1.FileName));
                }
                else if (Path.GetExtension(openFileDialog1.FileName) == ".mp4")
                {
                    currentImageSource = new ImageSource(new VideoCapture(openFileDialog1.FileName));
                }

                var image = currentImageSource[currentFrame].Bitmap;
                foundLP = ScanImage(image);

                //frameSelector.Maximum = currentImageSource.Count - 1;
            }

            textBox1.Text = foundLP;
            textBox1.ForeColor = SystemColors.WindowText;
            SearchInWantedList(foundLP);
        }

        public string ScanImage (Image image)
        {
            pictureBox1.Image = ScaleImage(image, 1255, 745);               // Sumazina foto dydi pixeliais kad tilptu i forma
            var img = UtilFunctions.GetMatFromImage(pictureBox1.Image);
            UMat uImg = img.GetUMat(AccessType.ReadWrite);

            UtilFunctions f = new UtilFunctions(this);
            List<String> strings = f.ProcessImage(uImg);

            return new FormData(strings).GetLicensePlate();
        }

        private Image ScaleImage(Image image, int maxWidth, int maxHeight)              
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {}

        private void label1_Click(object sender, EventArgs e)
        {}

        private void button3_Click(object sender, EventArgs e)
        {
            Form stats = new StatisticsForm(currentImageSource);
            stats.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new DBConnect()).updateWantedListJSON();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new DBConnect()).updateWantedListDB();
        }
    }
}
