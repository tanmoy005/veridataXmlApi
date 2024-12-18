using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_AadhaarSubmitOtpResponse : Karza_BaseResponse
    {
        public AadharResult result { get; set; }
    }

    public class AadharAddress
    {
        public SplitAddress splitAddress { get; set; }
        public string combinedAddress { get; set; }
    }

    public class ClientData
    {
        public string caseId { get; set; }
    }

    public class DataFromAadhaar
    {
        public string generatedDateTime { get; set; }
        public string maskedAadhaarNumber { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string mobileHash { get; set; }
        public object emailHash { get; set; }
        public string fatherName { get; set; }
        public string relativeName { get; set; }
        public string husbandName { get; set; }
        public AadharAddress address { get; set; }
        public string image { get; set; }
    }

    public class AadharResult
    {
        public DataFromAadhaar dataFromAadhaar { get; set; }
        public string message { get; set; }
        public string shareCode { get; set; }
    }

    public class SplitAddress
    {
        public string houseNumber { get; set; }
        public string street { get; set; }
        public object landmark { get; set; }
        public string subdistrict { get; set; }
        public string district { get; set; }
        public string vtcName { get; set; }
        public object location { get; set; }
        public string postOffice { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
    }
}