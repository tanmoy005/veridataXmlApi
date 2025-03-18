using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_PanDetailsResponse : Signzy_BaseResponse
    {
        [JsonProperty("result")]
        public PanInfo? Result { get; set; }

        //public Address? Address { get; set; }
    }

    public class PanInfo
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        //[JsonProperty("number")]
        //public string? Number { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("panStatus")]
        public string? PanStatus { get; set; }

        [JsonProperty("dob")]
        public string? DateOfBirth { get; set; }
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