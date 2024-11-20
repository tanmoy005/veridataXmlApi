using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.Model.DataAccess.Response
{
    public class ManualVerificationProcessQueryDataResponse
    {
        public UnderProcessFileData? UnderProcess { get; set; }
        public AppointeeDetails? AppointeeDetails { get; set; }
        public DateTime? WorkflowCreatedDate { get; set; }
        //public int? AppvlStatusId { get; set; }
        public int? VerificationAttempted { get; set; }
        public int? AppointeeId { get; set; }
        public bool IsJoiningDateLapsed { get; set; }
        public string? Status { get; set; }
    }
}
