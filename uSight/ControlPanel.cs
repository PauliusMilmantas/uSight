using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace uSight
{
    public partial class ControlPanel : Form
    {
        List<PlateRecord> records1 = new List<PlateRecord>();

        Label[] labelsOwners = new Label[10]; //Kiekviename puslapyje bus po 10 irasu
        Label[] licensePlates = new Label[10];

        public ControlPanel()
        {
            InitializeComponent();

            DataExtraction de = new DataExtraction();
            dynamic obj = de.getJson();

            foreach (var json in obj.plates) {
                records1.Add(new PlateRecord((String)json.owner, (String)json.id, (String)json.p_number, (String)json.e_number, (String)json.b_number));
            }

            for (int a = 0; a < 10; a++) {
                if (records1.Count > a) {
                    labelsOwners[a] = new Label
                    {
                        Text = records1[a].Owner,
                        Location = new System.Drawing.Point(165, a * 19 + 38),
                        Font = new System.Drawing.Font("Verdana", 14)
                    };
                    this.Controls.Add(labelsOwners[a]);

                    licensePlates[a] = new Label
                    {
                        Text = records1[a].P_number,
                        Location = new System.Drawing.Point(10, a * 19 + 38),
                        Font = new System.Drawing.Font("Verdana", 14)
                    };
                    this.Controls.Add(licensePlates[a]);
                }
            }
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
