using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace uSight
{
    public partial class FormWantedList : Form
    {
        List<PlateRecord> records1 = new List<PlateRecord>();

        Label[] labelsOwners = new Label[10]; //Kiekviename puslapyje bus po 10 irasu
        Label[] licensePlates = new Label[10];

        PictureBox[] delete = new PictureBox[10];

        dynamic obj;

        public FormWantedList()
        {
            InitializeComponent();
            refrestView();
          }

        public void refrestView() {

            labelsOwners = new Label[10];
            licensePlates = new Label[10];
            delete = new PictureBox[10];

            records1 = new List<PlateRecord>();

            DataExtraction de = new DataExtraction();
            obj = de.GetJsonFromDisk();

            foreach (var json in obj.plates)
            {
                records1.Add(new PlateRecord((String)json.owner, (String)json.id, (String)json.plate_number, (String)json.engine_number, (String)json.vehicle_number));
            }

            for (int a = 0; a < 10; a++)
            {
                if (records1.Count > a)
                {
                    labelsOwners[a] = new Label
                    {
                        Text = records1[a].Owner,
                        Location = new System.Drawing.Point(165, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14)
                    };
                    this.Controls.Add(labelsOwners[a]);

                    licensePlates[a] = new Label
                    {
                        Text = records1[a].P_number,
                        Location = new System.Drawing.Point(10, a * 25 + 38),
                        Font = new System.Drawing.Font("Verdana", 14)
                    };
                    this.Controls.Add(licensePlates[a]);

                    delete[a] = new PictureBox
                    {
                        Location = new System.Drawing.Point(280, a * 25 + 38),
                        Image = Image.FromFile("../../../Resources/trashbin.png"),
                        Size = new Size(20, 20),
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
            refrestView();
        }
    }
}
