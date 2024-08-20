namespace VERIDATA.Model.Response
{
    public class AppointeePassbookDetailsViewResponse
    {
        public string? clientId { get; set; }
        public string? pfUan { get; set; }
        public string? fullName { get; set; }
        public string? fatherName { get; set; }
        public string? dob { get; set; }
        public List<PfCompanyDetails>? companies { get; set; }
    }
    public class PfCompanyDetails
    {
        public string? companyName { get; set; }
        public string? establishmentId { get; set; }
        public string? LastTransactionYear { get; set; }
        public string? LastTransactionMonth { get; set; }
        public string? LastTransactionApprovedOn { get; set; }
        public string? memberId { get; set; }
        public string? IsPensionApplicable { get; set; }
        public string? LastPensionDate { get; set; }
        public List<CompanyPassbookDetails>? passbook { get; set; }
    }
    public class CompanyPassbookDetails
    {
        public int? id { get; set; }
        public string? approvedOn { get; set; }
        public string? year { get; set; }
        public string? month { get; set; }
        public string? description { get; set; }
    }
}
