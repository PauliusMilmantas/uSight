using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSight
{
    class PlateRecord
    {
        string owner;
        string p_number;
        string e_number;
        string b_number;
        string id;

        public PlateRecord()
        {
        }

        public PlateRecord(string owner, string id, string p_number, string e_number, string b_number)  //Ieskomu masinu duomenys
        {
            this.owner = owner;
            this.id = id;
            this.p_number = p_number;
            this.e_number = e_number;
            this.b_number = b_number;
        }

        public string Owner { get => owner; set => owner = value; }
        public string P_number { get => p_number; set => p_number = value; }
        public string E_number { get => e_number; set => e_number = value; }
        public string B_number { get => b_number; set => b_number = value; }
    }
}
