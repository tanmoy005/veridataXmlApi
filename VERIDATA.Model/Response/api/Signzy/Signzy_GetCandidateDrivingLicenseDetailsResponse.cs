using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

using System.Collections.Generic;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_GetCandidateDrivingLicenseDetailsResponse : Signzy_BaseResponse
    {
        [JsonProperty("result")]
        public DrivingLicenseResult? Result { get; set; }
    }

    public class DrivingLicenseResult
    {
        [JsonProperty("dlNumber")]
        public string? DlNumber { get; set; }

        [JsonProperty("dob")]
        public string? Dob { get; set; }

        [JsonProperty("badgeDetails")]
        public List<BadgeDetail>? BadgeDetails { get; set; }

        [JsonProperty("dlValidity")]
        public DlValidity? DlValidity { get; set; }

        [JsonProperty("detailsOfDrivingLicence")]
        public DrivingLicenceDetails? DetailsOfDrivingLicence { get; set; }
    }

    public class BadgeDetail
    {
        [JsonProperty("badgeIssueDate")]
        public string? BadgeIssueDate { get; set; }

        [JsonProperty("badgeNo")]
        public string? BadgeNo { get; set; }

        [JsonProperty("classOfVehicle")]
        public List<string>? ClassOfVehicle { get; set; }
    }

    public class DlValidity
    {
        [JsonProperty("nonTransport")]
        public NonTransport? NonTransport { get; set; }

        [JsonProperty("hazardousValidTill")]
        public string? HazardousValidTill { get; set; }

        [JsonProperty("transport")]
        public Transport? Transport { get; set; }

        [JsonProperty("hillValidTill")]
        public string? HillValidTill { get; set; }
    }

    public class NonTransport
    {
        [JsonProperty("to")]
        public string? To { get; set; }

        [JsonProperty("from")]
        public string? From { get; set; }
    }

    public class Transport
    {
        [JsonProperty("to")]
        public string? To { get; set; }

        [JsonProperty("from")]
        public string? From { get; set; }
    }

    public class DrivingLicenceDetails
    {
        [JsonProperty("dateOfIssue")]
        public string? DateOfIssue { get; set; }

        [JsonProperty("dateOfLastTransaction")]
        public string? DateOfLastTransaction { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("lastTransactedAt")]
        public string? LastTransactedAt { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("fatherOrHusbandName")]
        public string? FatherOrHusbandName { get; set; }

        [JsonProperty("addressList")]
        public List<AddressDetail>? AddressList { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("photo")]
        public string? Photo { get; set; }

        [JsonProperty("splitAddress")]
        public SplitAddress? SplitAddress { get; set; }
    }

    public class AddressDetail
    {
        [JsonProperty("completeAddress")]
        public string? CompleteAddress { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("splitAddress")]
        public SplitAddress? SplitAddress { get; set; }
    }

    public class SplitAddress
    {
        [JsonProperty("district")]
        public List<string>? District { get; set; }

        [JsonProperty("state")]
        public List<List<string>>? State { get; set; }

        [JsonProperty("city")]
        public List<string>? City { get; set; }

        [JsonProperty("pincode")]
        public string? Pincode { get; set; }

        [JsonProperty("country")]
        public List<string>? Country { get; set; }

        [JsonProperty("addressLine")]
        public string? AddressLine { get; set; }
    }
}