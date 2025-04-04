﻿using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_UanPassbookFetchResponse : Signzy_BaseResponse
    {
        public SignzyUanPassbookDetails? Data { get; set; }
    }

    public class Signzy_EmployeeDetails
    {
        [JsonProperty("memberName")]
        public string? MemberName { get; set; }

        [JsonProperty("fatherName")]
        public string? FatherName { get; set; }

        [JsonProperty("dob")]
        public string? Dob { get; set; }
    }

    public class PfShare
    {
        [JsonProperty("debit")]
        public int? Debit { get; set; }

        [JsonProperty("credit")]
        public int? Credit { get; set; }

        [JsonProperty("balance")]
        public int? Balance { get; set; }
    }

    public class PfBalance
    {
        [JsonProperty("netBalance")]
        public int? NetBalance { get; set; }

        [JsonProperty("isPfFullyWithdrawn")]
        public bool? IsPfFullyWithdrawn { get; set; }

        [JsonProperty("isPfPartialWithdrawn")]
        public bool? IsPfPartialWithdrawn { get; set; }

        [JsonProperty("employeeShare")]
        public PfShare? EmployeeShare { get; set; }

        [JsonProperty("employerShare")]
        public PfShare? EmployerShare { get; set; }
    }

    public class PassbookEntry
    {
        [JsonProperty("trDateMy")]
        public string? TrDateMy { get; set; }

        [JsonProperty("approvedOn")]
        public string? ApprovedOn { get; set; }

        [JsonProperty("crEeShare")]
        public string? CrEeShare { get; set; }

        [JsonProperty("crErShare")]
        public string? CrErShare { get; set; }

        [JsonProperty("crPenBal")]
        public string? CrPenBal { get; set; }

        [JsonProperty("dbCrFlag")]
        public string? DbCrFlag { get; set; }

        [JsonProperty("particular")]
        public string? Particular { get; set; }

        [JsonProperty("monthYear")]
        public string? MonthYear { get; set; }

        [JsonProperty("trApproved")]
        public string? TrApproved { get; set; }
    }

    public class EstablishmentDetails
    {
        [JsonProperty("estName")]
        public string? EstName { get; set; }

        [JsonProperty("memberId")]
        public string? MemberId { get; set; }

        [JsonProperty("office")]
        public string? Office { get; set; }

        [JsonProperty("dojEpf")]
        public string? DojEpf { get; set; }

        [JsonProperty("docEpf")]
        public string? DocEpf { get; set; }

        [JsonProperty("docEps")]
        public string? DocEps { get; set; }

        [JsonProperty("pfBalance")]
        public PfBalance? PfBalance { get; set; }

        [JsonProperty("passbook")]
        public List<PassbookEntry>? Passbook { get; set; }
    }

    public class OverallPfBalance
    {
        [JsonProperty("pensionBalance")]
        public int? PensionBalance { get; set; }

        [JsonProperty("currentPfBalance")]
        public int? CurrentPfBalance { get; set; }

        [JsonProperty("employeeShareTotal")]
        public PfShare? EmployeeShareTotal { get; set; }

        [JsonProperty("employerShareTotal")]
        public PfShare? EmployerShareTotal { get; set; }
    }

    public class SignzyUanPassbookDetails
    {
        [JsonProperty("employeeDetails")]
        public Signzy_EmployeeDetails? EmployeeDetails { get; set; }

        [JsonProperty("estDetails")]
        public List<EstablishmentDetails>? EstDetails { get; set; }

        [JsonProperty("overallPfBalance")]
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