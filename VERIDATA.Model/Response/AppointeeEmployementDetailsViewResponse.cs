namespace VERIDATA.Model.Response
{
    public class AppointeeEmployementDetailsViewResponse
    {
        public string? clientId { get; set; }
        public string? pfUan { get; set; }
        public string? fullName { get; set; }
        public string? fatherName { get; set; }
        public string? dob { get; set; }
        public List<PfEmployementDetails>? companies { get; set; }
    }
    public class PfEmployementDetails
    {
        public string? companyName { get; set; }
        public string? establishmentId { get; set; }
        public string? FirstTransactionYear { get; set; }
        public string? FirstTransactionMonth { get; set; }
        public string? FirstTransactionApprovedOn { get; set; }
        public string? LastTransactionYear { get; set; }
        public string? LastTransactionMonth { get; set; }
        public string? LastTransactionApprovedOn { get; set; }
        public double? TotalWorkDays { get; set; }
        public int? WorkForYear { get; set; }
        public int? WorkForMonth { get; set; }
        public string? memberId { get; set; }
    }

}
