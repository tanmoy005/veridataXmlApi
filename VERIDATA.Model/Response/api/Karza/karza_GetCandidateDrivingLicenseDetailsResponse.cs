using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class karza_GetCandidateDrivingLicenseDetailsResponse : Karza_BaseResponsev2
    {
        [JsonPropertyName("result")]
        public DlResult Result { get; set; }

        [JsonPropertyName("clientData")]
        public ClientData ClientData { get; set; }
    }

    public class DlResult
    {
        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }

        [JsonPropertyName("father/husband")]
        public string FatherHusband { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }

        [JsonPropertyName("bloodGroup")]
        public string BloodGroup { get; set; }

        [JsonPropertyName("dob")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("dlNumber")]
        public string DlNumber { get; set; }

        [JsonPropertyName("validity")]
        public DlValidity Validity { get; set; }

        [JsonPropertyName("covDetails")]
        public List<DlCovDetail> CovDetails { get; set; }

        [JsonPropertyName("address")]
        public List<DlAddress> Address { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusDetails")]
        public StatusDetails StatusDetails { get; set; }

        //[JsonPropertyName("endorsementAndHazardousDetails")]
        //public EndorsementAndHazardousDetails EndorsementAndHazardousDetails { get; set; }
    }

    public class DlValidity
    {
        [JsonPropertyName("nonTransport")]
        public string NonTransport { get; set; }

        [JsonPropertyName("transport")]
        public string Transport { get; set; }
    }

    public class DlCovDetail
    {
        [JsonPropertyName("cov")]
        public string Cov { get; set; }

        [JsonPropertyName("issueDate")]
        public string IssueDate { get; set; }
    }

    public class DlAddress
    {
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("pin")]
        public int Pin { get; set; }

        [JsonPropertyName("completeAddress")]
        public string CompleteAddress { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class StatusDetails
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("remarks")]
        public string Remarks { get; set; }
    }

    //public class EndorsementAndHazardousDetails
    //{
    //    [JsonPropertyName("initialIssuingOffice")]
    //    public string InitialIssuingOffice { get; set; }

    //    [JsonPropertyName("lastEndorsementDate")]
    //    public string LastEndorsementDate { get; set; }

    //    [JsonPropertyName("lastEndorsedOffice")]
    //    public string LastEndorsedOffice { get; set; }

    //    [JsonPropertyName("endorsementReason")]
    //    public string EndorsementReason { get; set; }

    //    [JsonPropertyName("hazardousValidTill")]
    //    public string HazardousValidTill { get; set; }

    //    [JsonPropertyName("hillValidTill")]
    //    public string HillValidTill { get; set; }
    //}
}