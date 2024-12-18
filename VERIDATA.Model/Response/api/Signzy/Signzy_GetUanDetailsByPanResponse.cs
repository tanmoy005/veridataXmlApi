using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_GetUanDetailsByPanResponse : Signzy_BaseResponse
    {
        public ResultData? Result { get; set; }
    }

    public class ResultData
    {
        public int HttpResponseCode { get; set; }
        public string ClientRefNum { get; set; }
        public string RequestId { get; set; }
        public int ResultCode { get; set; }
        public UanResult Result { get; set; }
        public InputData InputData { get; set; }
    }

    public class UanResult
    {
        public List<string> Uan { get; set; }
        public Summary Summary { get; set; }
        public Dictionary<string, UanDetail> UanDetails { get; set; }
        public List<UanSource> UanSource { get; set; }
        public object NameDobFilteringScore { get; set; }
    }

    public class Summary
    {
        public RecentEmployerData RecentEmployerData { get; set; }
        public string MatchingUan { get; set; }
        public bool IsEmployed { get; set; }
        public object EmployeeNameMatch { get; set; }
        public object EmployerNameMatch { get; set; }
        public int UanCount { get; set; }
        public bool DateOfExitMarked { get; set; }
    }

    public class RecentEmployerData
    {
        public string MemberId { get; set; }
        public string EstablishmentId { get; set; }
        public string DateOfExit { get; set; }
        public string DateOfJoining { get; set; }
        public string EstablishmentName { get; set; }
        public object EmployerConfidenceScore { get; set; }
        public string MatchingUan { get; set; }
    }

    public class UanDetail
    {
        public BasicDetails BasicDetails { get; set; }
        public EmploymentDetails EmploymentDetails { get; set; }
    }

    public class BasicDetails
    {
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public object EmployeeConfidenceScore { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int AadhaarVerificationStatus { get; set; }
    }

    public class EmploymentDetails
    {
        public string MemberId { get; set; }
        public string EstablishmentId { get; set; }
        public string DateOfExit { get; set; }
        public string DateOfJoining { get; set; }
        public string LeaveReason { get; set; }
        public string EstablishmentName { get; set; }
        public object EmployerConfidenceScore { get; set; }
    }

    public class UanSource
    {
        public string Uan { get; set; }
        public string Source { get; set; }
    }

    public class InputData
    {
        public string Mobile { get; set; }
        public string Pan { get; set; }
        public string Uan { get; set; }
        public string Dob { get; set; }
        public string EmployeeName { get; set; }
        public string EmployerName { get; set; }
        public string NameMatchMethod { get; set; }
    }
}