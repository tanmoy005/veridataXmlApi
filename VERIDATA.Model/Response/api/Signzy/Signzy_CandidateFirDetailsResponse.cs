using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class Signzy_CandidateFirDetailsResponse : Signzy_BaseResponse
    {
        public List<FIRRecord>? cases { get; set; }
        public string verifyId { get; set; }
        public int status { get; set; }
    }

    public class FIRRecord
    {
        [JsonProperty("stateName")]
        public string StateName { get; set; }

        [JsonProperty("distName")]
        public string DistrictName { get; set; }

        [JsonProperty("firNo")]
        public int FIRNumber { get; set; }

        [JsonProperty("firYear")]
        public string FIRYear { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fatherName")]
        public string FatherName { get; set; }

        [JsonProperty("rawAddress")]
        public string RawAddress { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("policeStation")]
        public string PoliceStation { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("uniqCaseId")]
        public string UniqueCaseId { get; set; }

        [JsonProperty("underActs")]
        public string UnderActs { get; set; }

        [JsonProperty("underSections")]
        public string UnderSections { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("md5")]
        public string MD5 { get; set; }

        [JsonProperty("nameWc")]
        public int NameWordCount { get; set; }

        [JsonProperty("addressWc")]
        public int AddressWordCount { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("addressStreet")]
        public string AddressStreet { get; set; }

        [JsonProperty("addressState")]
        public string AddressState { get; set; }

        [JsonProperty("addressTaluka")]
        public string AddressTaluka { get; set; }

        [JsonProperty("addressDistrict")]
        public string AddressDistrict { get; set; }

        [JsonProperty("addressPincode")]
        public string AddressPincode { get; set; }

        [JsonProperty("cleanName")]
        public string CleanName { get; set; }

        [JsonProperty("allCandidates")]
        public List<string> AllCandidates { get; set; }

        [JsonProperty("oparty")]
        public string Oparty { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("nameScore")]
        public double NameScore { get; set; }

        [JsonProperty("ref")]
        public string Reference { get; set; }
    }
}