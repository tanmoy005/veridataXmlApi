using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_UanPassbookFetchResponse : Signzy_BaseResponse
    {
        public SignzyUanPassbookDetails? Data { get; set; }
    }

    public class Signzy_EmployeeDetails
    {
        public string? MemberName { get; set; }
        public string? FatherName { get; set; }
        public string? DOB { get; set; }
    }

    public class PfShare
    {
        public int? Debit { get; set; }
        public int? Credit { get; set; }
        public int? Balance { get; set; }
    }

    public class PfBalance
    {
        public int? NetBalance { get; set; }
        public bool? IsPfFullyWithdrawn { get; set; }
        public bool? IsPfPartialWithdrawn { get; set; }
        public PfShare? EmployeeShare { get; set; }
        public PfShare? EmployerShare { get; set; }
    }

    public class PassbookEntry
    {
        public string? TrDateMy { get; set; }
        public string? ApprovedOn { get; set; }
        public string? CrEeShare { get; set; }
        public string? CrErShare { get; set; }
        public string? CrPenBal { get; set; }
        public string? DbCrFlag { get; set; }
        public string? Particular { get; set; }
        public string? MonthYear { get; set; }
        public string? TrApproved { get; set; }
    }

    public class EstablishmentDetails
    {
        public string? EstName { get; set; }
        public string? MemberId { get; set; }
        public string? Office { get; set; }
        public string? DojEpf { get; set; }
        public string? DocEpf { get; set; }
        public string? DocEps { get; set; }
        public PfBalance? PfBalance { get; set; }
        public List<PassbookEntry>? Passbook { get; set; }
    }

    public class OverallPfBalance
    {
        public int? PensionBalance { get; set; }
        public int? CurrentPfBalance { get; set; }
        public PfShare? EmployeeShareTotal { get; set; }
        public PfShare? EmployerShareTotal { get; set; }
    }

    public class SignzyUanPassbookDetails
    {
        public Signzy_EmployeeDetails? EmployeeDetails { get; set; }
        public List<EstablishmentDetails>? EstDetails { get; set; }
        public OverallPfBalance? OverallPfBalance { get; set; }
    }

    //public class IncomeDetails
    //{
    //    public decimal Salary { get; set; }
    //    public float ConfidenceScore { get; set; }
    //    public string? CurrentCompanyName { get; set; }
    //    public string? CurrentEstablishmentId { get; set; }
    //}

    //public class PassbookEntry
    //{
    //    public string? MemberId { get; set; }
    //    public string? CreditDebitFlag { get; set; }
    //    public DateTime DoeEpf { get; set; }
    //    public DateTime DoeEps { get; set; }
    //    public DateTime DojEpf { get; set; }
    //    public string? Office { get; set; }
    //    public DateTime TransactionApproved { get; set; }
    //    public string? TransactionCategory { get; set; }
    //    public decimal EmployeeShare { get; set; }
    //    public decimal EmployerShare { get; set; }
    //    public decimal PensionShare { get; set; }
    //    public DateTime ApprovedOn { get; set; }
    //    public string? Year { get; set; }
    //    public string? Month { get; set; }
    //    public string? Description { get; set; }
    //}

    //public class Company
    //{
    //    public string? CompanyName { get; set; }
    //    public string? EstablishmentId { get; set; }
    //    public int EmployeeTotal { get; set; }
    //    public int EmployerTotal { get; set; }
    //    public int PensionTotal { get; set; }
    //    public List<PassbookEntry> Passbook { get; set; }
    //}

    //public class SignzyUanPassbookDetails
    //{
    //    public string? RequestId { get; set; }
    //    public string? Uan { get; set; }
    //    public string? FullName { get; set; }
    //    public string? FatherName { get; set; }
    //    public DateTime Dob { get; set; }
    //    public Dictionary<string, Company> Companies { get; set; }
    //    public IncomeDetails? IncomeDetails { get; set; }
    //}
}