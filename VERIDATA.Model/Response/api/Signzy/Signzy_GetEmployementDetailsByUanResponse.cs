using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_GetEmployementDetailsByUanResponse : Signzy_BaseResponse
    {
        public List<EmploymentHistoryDetail>? Result { get; set; }
    }

    public class EmploymentHistoryDetail
    {
        public string? DateOfExit { get; set; }
        public string? DateOfJoining { get; set; }
        public string? EstablishmentName { get; set; }
        public string? MemberId { get; set; }
        public string? FatherOrHusbandName { get; set; }
        public string? Name { get; set; }
        public string? Uan { get; set; }
    }
}