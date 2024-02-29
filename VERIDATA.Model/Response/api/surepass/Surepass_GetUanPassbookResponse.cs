
using VERIDATA.Model.Response.api.surepass.Base;

namespace VERIDATA.Model.Response.api.surepass
{
    public class Surepass_GetUanPassbookResponse : Surepass_BaseResponse
    {
        public PassbookData data { get; set; }
    }

    public class CandidateCompanies
    {
        public List<CandidatePassbook> passbook { get; set; }
        public string company_name { get; set; }
        public string establishment_id { get; set; }
        public int employee_total { get; set; }
        public int employer_total { get; set; }
        public int pension_total { get; set; }
    }

    public class PassbookData
    {
        public string client_id { get; set; }
        public string pf_uan { get; set; }
        public string full_name { get; set; }
        public string father_name { get; set; }
        public string dob { get; set; }
        public Dictionary<string, CandidateCompanies> companies { get; set; }
    }

    public class CandidatePassbook
    {
        public string member_id { get; set; }
        //public string credit_debit_flag { get; set; }
        //public string doe_epf { get; set; }
        //public string doe_eps { get; set; }
        //public string doj_epf { get; set; }
        public object office { get; set; }
        //public string transaction_approved { get; set; }
        //public string transaction_category { get; set; }
        //public string employee_share { get; set; }
        //public string employer_share { get; set; }
        public string pension_share { get; set; }
        public string approved_on { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string description { get; set; }
    }

}
