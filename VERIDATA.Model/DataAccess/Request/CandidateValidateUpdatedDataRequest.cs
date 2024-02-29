

namespace VERIDATA.Model.DataAccess.Request
{
    public class CandidateValidateUpdatedDataRequest
    {
        public CandidateValidateUpdatedDataRequest()
        {
            aadharData = new AadharDetailsData();
            panData = new PanData();
        }
        public int AppointeeId { get; set; }
        public string? Type { get; set; }
        public string? EmailId { get; set; }
        public string? UserName { get; set; }
        public int UserId { get; set; }
        public bool? Status { get; set; }
        public bool? IsPensionApplicable { get; set; }
        public string? PassportFileNo { get; set; }
        public string? UanNumber { get; set; }
        public List<ReasonRemarks> Reasons { get; set; }
        public AadharDetailsData? aadharData { get; set; }
        public PanData? panData { get; set; }
    }
    public class AadharDetailsData
    {
        public string? AadhaarNumber { get; set; }
        public string? AadhaarNumberView { get; set; }
        public string? AadhaarName { get; set; }
        public string? NameFromAadhaar { get; set; }
        public string? GenderFromAadhaar { get; set; }
        public string? DobFromAadhaar { get; set; }
    }
    public class PanData
    {
        public string? PanNumber { get; set; }
        public string? PanName { get; set; }
        public string? PanFatherName { get; set; }

    }
}
