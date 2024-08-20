
using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_UanSubmitOtpResponse
    {
        [JsonProperty("status-code")]
        public int? statusCode { get; set; }

        [JsonProperty("request_id")]
        public string? requestId { get; set; }
        public UanPassbookDetails? result { get; set; }
    }

    public class EmployeeDetails
    {
        public string? member_name { get; set; }
        public string? father_name { get; set; }
        public string? dob { get; set; }
    }

    public class EmployeeShare
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    //public class EmployeeShareTotal
    //{
    //    public int debit { get; set; }
    //    public int credit { get; set; }
    //    public int balance { get; set; }
    //}

    public class EmployerShare
    {
        public int debit { get; set; }
        public int credit { get; set; }
        public int balance { get; set; }
    }

    //public class EmployerShareTotal
    //{
    //    public int debit { get; set; }
    //    public int credit { get; set; }
    //    public int balance { get; set; }
    //}

    public class EstDetail
    {
        public string? est_name { get; set; }
        public string? member_id { get; set; }
        public string? office { get; set; }
        public string? doj_epf { get; set; }
        public string? doe_epf { get; set; }
        public string? doe_eps { get; set; }
        //public PfBalance pf_balance { get; set; }
        public List<Passbook> passbook { get; set; }
    }

    public class OverallPfBalance
    {
        public int pension_balance { get; set; }
        //public int current_pf_balance { get; set; }
        //public EmployeeShareTotal employee_share_total { get; set; }
        //public EmployerShareTotal employer_share_total { get; set; }
    }

    public class Passbook
    {
        public string tr_date_my { get; set; }
        public string approved_on { get; set; }
        //public string cr_ee_share { get; set; }
        //public string cr_er_share { get; set; }
        public string cr_pen_bal { get; set; }
        public string db_cr_flag { get; set; }
        public string particular { get; set; }
        public string month_year { get; set; }
        public string tr_approved { get; set; }
    }

    //public class Pdf
    //{
    //    public string est_id { get; set; }
    //    public string pdf_data { get; set; }
    //}

    //public class PfBalance
    //{
    //    public int net_balance { get; set; }
    //    public bool is_pf_full_withdrawn { get; set; }
    //    public bool is_pf_partial_withdrawn { get; set; }
    //    public EmployeeShare employee_share { get; set; }
    //    public EmployerShare employer_share { get; set; }
    //}

    public class UanPassbookDetails
    {
        public EmployeeDetails employee_details { get; set; }
        public List<EstDetail> est_details { get; set; }
        public OverallPfBalance overall_pf_balance { get; set; }
        //public List<Pdf> pdf { get; set; }
    }

}
