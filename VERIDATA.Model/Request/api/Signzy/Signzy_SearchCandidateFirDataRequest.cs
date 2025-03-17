using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request.api.Signzy
{
    public class Signzy_SearchCandidateFirDataRequest
    {
        public string name { get; set; }
        public string address { get; set; }
        public string fatherName { get; set; }
        public string stateName { get; set; }
        public string distName { get; set; }
        public int firYear { get; set; }
        public string policeStationName { get; set; }
    }
}