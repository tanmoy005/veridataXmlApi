
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_GetUanDetailsByPanResponse : Karza_BaseResponse
    {
        public UanResult? result { get; set; }
    }
    public class Employer
    {
        public string? name { get; set; }
        public string? memberId { get; set; }
        public string? dateOfExit { get; set; }
        public string? dateOfJoining { get; set; }
        public string? lastMonthYear { get; set; }
        public string? startMonthYear { get; set; }
        public int? employmentPeriod { get; set; }
        public bool? isNameUnique { get; set; }
        public string? lastMonth { get; set; }
        public bool? isRecent { get; set; }
        public bool? isNameExact { get; set; }
        public bool? isEmployed { get; set; }
    }

    public class PersonalInfo
    {
        public string? uan { get; set; }
        public string? name { get; set; }
        public string? dateOfBirth { get; set; }
        public string? gender { get; set; }
        public string? fatherHusbandName { get; set; }
        public string? relation { get; set; }
        public string? nationality { get; set; }
        public string? maritalStatus { get; set; }
        public string? qualification { get; set; }
        public string? mobileNumber { get; set; }
        public string? emailId { get; set; }
        public string? pan { get; set; }
        public string? passport { get; set; }
    }

    public class UanResult
    {
        public List<Uan> uan { get; set; }
        public PersonalInfo personalInfo { get; set; }
        public Summary summary { get; set; }
    }
    public class Uan
    {
        public string? uan { get; set; }
        public string? uanSource { get; set; }
        public List<Employer>? employer { get; set; }
    }
    public class Summary
    {
        public NameLookup nameLookup { get; set; }
        public UanLookup uanLookup { get; set; }
        public bool waiveFi { get; set; }
    }
    public class UanLookup
    {
        public string currentEmployer { get; set; }
        //public object matchScore { get; set; }
        //public object result { get; set; }
        //public object uanNameMatch { get; set; }
    }
    public class NameLookup
    {
        public string? matchName { get; set; }
        public bool? isUnique { get; set; }
        public bool? isLatest { get; set; }
        public bool? result { get; set; }
    }
}
