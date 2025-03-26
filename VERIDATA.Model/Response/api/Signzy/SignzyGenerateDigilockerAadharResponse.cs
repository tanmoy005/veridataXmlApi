using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VERIDATA.Model.Response.api.Signzy.Base;

namespace VERIDATA.Model.Response.api.Signzy
{
    public class SignzyGenerateDigilockerAadharResponse : Signzy_BaseResponse
    {
        [JsonProperty("result")]
        public DigilockerAadharResult? Result { get; set; }
    }

    public class DigilockerAadharResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("dob")]
        public string Dob { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        //[JsonProperty("x509Data")]
        //public X509Data X509Data { get; set; }

        //[JsonProperty("address")]
        //public AddressData Address { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("signatureData")]
        public SignatureData SignatureData { get; set; }

        [JsonProperty("xmlFileLink")]
        public string xmlFileLink { get; set; }
    }

    public class X509Data
    {
        [JsonProperty("subjectName")]
        public string SubjectName { get; set; }

        [JsonProperty("certificate")]
        public string Certificate { get; set; }

        [JsonProperty("details")]
        public CertificateDetails Details { get; set; }
    }

    public class CertificateDetails
    {
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; }

        [JsonProperty("notBefore")]
        public string NotBefore { get; set; }

        [JsonProperty("notAfter")]
        public string NotAfter { get; set; }

        [JsonProperty("signatureAlgorithm")]
        public string SignatureAlgorithm { get; set; }

        [JsonProperty("fingerPrint")]
        public string FingerPrint { get; set; }
    }

    public class AddressData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("splitAddress")]
        public AadharSplitAddress SplitAddress { get; set; }
    }

    public class AadharSplitAddress
    {
        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("pincode")]
        public string Pincode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("addressLine")]
        public string AddressLine { get; set; }

        [JsonProperty("landMark")]
        public string LandMark { get; set; }
    }

    public class SignatureData
    {
        [JsonProperty("signatureMethod")]
        public string SignatureMethod { get; set; }

        [JsonProperty("digestValue")]
        public string DigestValue { get; set; }
    }
}