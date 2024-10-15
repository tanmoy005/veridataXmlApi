
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_GetEmployementDetailsByUanResponse : Karza_BaseResponsev2
    {
        public string RequestId { get; set; }
        public EmploymentResult Result { get; set; }
        public int StatusCode { get; set; }
        public ClientData ClientData { get; set; }

    }
    public class EmploymentResult
    {
        public List<EmployerHistory> Employers { get; set; }
        public EmployeePersonalDetails PersonalDetails { get; set; }
        public EmployeeSummary Summary { get; set; }
        public string Remark { get; set; }
    }

    public class EmployerHistory
    {
        public string StartMonthYear { get; set; }
        public string LastMonthYear { get; set; }
        public string EstablishmentId { get; set; }
        public string EstablishmentName { get; set; }
        public EmployeeAddress Address { get; set; }
        public string MemberId { get; set; }
        public string ExitReason { get; set; }
        public string Status { get; set; }
    }

    public class EmployeeAddress
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string District { get; set; }
        public string AddressLine { get; set; }  // "address" in JSON could be renamed for clarity
    }

    public class EmployeePersonalDetails
    {
        public string Name { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string AadhaarVerificationStatus { get; set; }
        public string BankAccountStatus { get; set; }
        public string PanVerificationStatus { get; set; }
        public string ContactNo { get; set; }
    }

    public class EmployeeSummary
    {
        //public int MinimumWorkExperienceInMonths { get; set; }
        public LastEmployer LastEmployer { get; set; }
    }

    public class LastEmployer
    {
        public string EmployerName { get; set; }
        public string StartMonthYear { get; set; }
        public string LastMonthYear { get; set; }
        public int VintageInMonths { get; set; }
    }

    //public class ClientData
    //{
    //    public string CaseId { get; set; }
    //}

}
