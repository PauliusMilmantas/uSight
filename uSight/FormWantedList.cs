using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace uSight
{
    public partial class FormWantedList : Form
    {
        List<PlateRecord> records1 = new List<PlateRecord>();

        Label[] ownersLabel = new Label[10];         //Kiekviename puslapyje bus po 10 irasu
        Label[] licensePlateLabel = new Label[10];
        Label[] engineNoLabel = new Label[10];
        Label[] vinLabel = new Label[10];

        PictureBox[] delete = new PictureBox[10];

        dynamic obj;

        public FormWantedList()
        {
            InitializeComponent();
            refreshView();
          }

        public void refreshView() {

            ownersLabel = new Label[10];
            licensePlateLabel = new Label[10];
            engineNoLabel = new Label[10];
            vinLabel = new Label[10];
            delete = new PictureBox[10];

            records1 = new List<PlateRecord>();

            DataExtraction de = new DataExtraction();
            obj = de.GetJsonFromDisk();

            foreach (var json in obj.plates)
            {
                records1.Add(new PlateRecord((String)json.owner, (String)json.id, (String)json.license_plate, (String)json.engine_number, (String)json.vin));
            }

            for (int a = 0; a < 10; a++)
            {
                if (records1.Count > a)
                {
                    int width = 23;
                    
                    licensePlateLabel[a] = new Label        // License plate
                    {
                        Text = records1[a].License_Plate,
                        Location = new System.Drawing.Point(12, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14),
                        Size = new Size(154, width)
                    };
                    this.Controls.Add(licensePlateLabel[a]);


                    ownersLabel[a] = new Label          // Owner
                    {
                        Text = records1[a].Owner,
                        Location = new System.Drawing.Point(185, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14),
                        Size = new Size(244, width)
                    };
                    this.Controls.Add(ownersLabel[a]); 


                    engineNoLabel[a] = new Label            // Engine number
                    {
                        Text = records1[a].Engine_number,
                        Location = new System.Drawing.Point(435, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14),
                        Size = new Size(229,width)
                    };
                    this.Controls.Add(engineNoLabel[a]);


                    vinLabel[a] = new Label             // Vin
                    {
                        Text = records1[a].Vin,
                        Location = new System.Drawing.Point(705, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14),
                        Size = new Size(253, width)
                    };
                    this.Controls.Add(vinLabel[a]);
                    

                    delete[a] = new PictureBox          // delete picture
                    {
                        Location = new System.Drawing.Point(965, a * 25 + 40),
                        Image = Image.FromFile("../../../Resources/trashbin.png"),
                        Size = new Size(17, 17),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Name = a.ToString()
                    };
                    this.Controls.Add(delete[a]);

                    delete[a].Click += (e, s) => {
                        PictureBox p = (PictureBox)e;
                        obj.plates[Int32.Parse(p.Name)].Remove();
                        de.writeToJson(obj.ToString());

                        this.Close();
                        (new FormWantedList()).Show();
                    };
                }
            }
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            refreshView();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
