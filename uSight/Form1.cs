﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
<<<<<<< HEAD
using System.IO;
=======
using System.Collections.Generic;
>>>>>>> Development
using System.Windows.Forms;

namespace uSight
{
    public partial class Form1 : Form
    {
        dynamic json;
<<<<<<< HEAD
        //The current media stream
        ImageSource currentImageSource;
        //Frame index, updated by trackbar scroll
        int currentFrame = 0;
=======
        //Image<Bgr, byte> image;
        Mat image;
>>>>>>> Development

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
            openFileDialog1.Filter = "Media files|*.jpg;*.mp4";
            openFileDialog1.Title = "Select media file";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
<<<<<<< HEAD
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
=======
                //image = new Image<Bgr, byte>(openFileDialog1.FileName);
                //pictureBox1.Image = image.Bitmap;

                image = CvInvoke.Imread(openFileDialog1.FileName);
                pictureBox1.Image = image.Bitmap;
>>>>>>> Development
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            pictureBox1.Image = LicenesePlateDetector.ShowContours(currentImageSource[currentFrame]);
        }

        private void frameSelector_Scroll(object sender, EventArgs e)
        {
            if (currentImageSource != null)
            {
                currentFrame = frameSelector.Value;
                pictureBox1.Image = currentImageSource[currentFrame].Bitmap;
            }
=======
            //pictureBox1.Image = LicenesePlateDetector.ShowContours(image);
        }

        public void setImage(Image<Bgr, Byte> t) {
            pictureBox1.Image = t.Bitmap;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            UtilFunctions f = new UtilFunctions(this);
            UMat uImg = image.GetUMat(AccessType.ReadWrite);
            f.ProcessImage(uImg);
        }
        
        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            //UtilFunctions f = new UtilFunctions(this);
            //f.filterImage(pictureBox1.Image);
>>>>>>> Development
        }
    }
}
