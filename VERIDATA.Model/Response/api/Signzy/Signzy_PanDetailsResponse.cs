using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_PanDetailsResponse : Signzy_BaseResponse
    {
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("Number")]

        public string? Number { get; set; }
        [JsonProperty("firstName")]

        public string? FirstName { get; set; }
        [JsonProperty("middleName")]

        public string? MiddleName { get; set; }
        [JsonProperty("lastName")]

        public string? LastName { get; set; }
        [JsonProperty("typeOfHolder")]

        public string? TypeOfHolder { get; set; }
        [JsonProperty("gender")]

        public string? Gender { get; set; }
        public bool? IsIndividual { get; set; }
        public string? Category { get; set; }
        [JsonProperty("dateOfBirth")]

        public string? DateOfBirth { get; set; }
        public string? MaskedAadhaarNumber { get; set; }
        public string? EmailId { get; set; }
        [JsonProperty("mobileNumber")]

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