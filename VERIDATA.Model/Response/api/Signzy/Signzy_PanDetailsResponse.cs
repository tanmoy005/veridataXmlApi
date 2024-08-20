

using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_PanDetailsResponse: Signzy_BaseResponse
    {
        public string? Name { get; set; }
        public string? Number { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? TypeOfHolder { get; set; }
        public string? Gender { get; set; }
        public bool? IsIndividual { get; set; }
        public string? Category { get; set; }
        public string? DateOfBirth { get; set; }
        public string? MaskedAadhaarNumber { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNumber { get; set; }
        public bool? AadhaarLinked { get; set; }
        public Address? Address { get; set; }
        public bool? IsValid { get; set; }
        public string? PanStatus { get; set; }
        public string? PanStatusCode { get; set; }
    }
    public class Address
    {
        public string? FullAddress { get; set; }
        public string? AddressLineOne { get; set; }
        public string? AddressLineTwo { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Country { get; set; }
    }


}
