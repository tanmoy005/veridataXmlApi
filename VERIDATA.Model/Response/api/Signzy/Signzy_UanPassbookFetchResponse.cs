
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_UanPassbookFetchResponse : Signzy_BaseResponse
    {
        public SignzyUanPassbookDetails? Data { get; set; }
    }

    public class IncomeDetails
    {
        public decimal Salary { get; set; }
        public float ConfidenceScore { get; set; }
        public string? CurrentCompanyName { get; set; }
        public string? CurrentEstablishmentId { get; set; }
    }

    public class PassbookEntry
    {
        public string? MemberId { get; set; }
        public string? CreditDebitFlag { get; set; }
        public DateTime DoeEpf { get; set; }
        public DateTime DoeEps { get; set; }
        public DateTime DojEpf { get; set; }
        public string? Office { get; set; }
        public DateTime TransactionApproved { get; set; }
        public string? TransactionCategory { get; set; }
        public decimal EmployeeShare { get; set; }
        public decimal EmployerShare { get; set; }
        public decimal PensionShare { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public string? Description { get; set; }
    }

    public class Company
    {
        public string? CompanyName { get; set; }
        public string? EstablishmentId { get; set; }
        public int EmployeeTotal { get; set; }
        public int EmployerTotal { get; set; }
        public int PensionTotal { get; set; }
        public List<PassbookEntry> Passbook { get; set; }
    }

    public class SignzyUanPassbookDetails
    {
        public string? RequestId { get; set; }
        public string? Uan { get; set; }
        public string? FullName { get; set; }
        public string? FatherName { get; set; }
        public DateTime Dob { get; set; }
        public Dictionary<string, Company> Companies { get; set; }
        public IncomeDetails? IncomeDetails { get; set; }
    }

}
