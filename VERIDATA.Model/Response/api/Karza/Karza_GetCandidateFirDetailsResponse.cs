using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VERIDATA.Model.Response.api.Karza.Base;

namespace VERIDATA.Model.Response.api.Karza
{
    public class Karza_GetCandidateFirDetailsResponse : Karza_BaseResponsev2
    {
        public FirResult Result { get; set; }
        public ClientData ClientData { get; set; }
    }

    public class FirResult
    {
        public List<Record> Records { get; set; }
        public string ConfidenceLevel { get; set; }
        public int TotalCount { get; set; }
    }

    public class Record
    {
        public List<ComplainantDetail> ComplainantDetails { get; set; }
        public List<ActAndSection> ActsAndSections { get; set; }
        public List<AccusedDetail> AccusedDetails { get; set; }
        public UnidentifiedDetails AccusedUnidentifiedDetails { get; set; }
        public UnidentifiedDetails ComplainantsUnidentifiedDetails { get; set; }
        public string SourceId_ { get; set; }
        public string District { get; set; }
        public string FirDate { get; set; }
        public string FirDescription { get; set; }
        public string FirDescriptionTranslation { get; set; }
        public string FirNo { get; set; }
        public string FirStatus { get; set; }
        public string FirTime { get; set; }
        public string FirYear { get; set; }
        public List<IncidentDetail> IncidentDetails { get; set; }
        public string PoliceStation { get; set; }
        public List<PropertyStolenInvolvedDetail> PropertiesStolenInvolvedDetails { get; set; }
        public string PropertiesStolenInvolvedTotalValue { get; set; }
        public string State { get; set; }
        public string Timestamp { get; set; }
        public MatchFlags MatchFlags { get; set; }
        public AdditionalMatchFlags AdditionalMatchFlags { get; set; }
        public string ConfidenceLevel { get; set; }
    }

    public class ComplainantDetail
    {
        public List<string> ContactDetails { get; set; }
        public string Name { get; set; }
        public string NameInLocale { get; set; }
        public string AliasName { get; set; }
        public string Occupation { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Age { get; set; }
        public List<AddressDetail> AddressDetails { get; set; }
        public List<RelativeDetail> RelativeDetails { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public List<IdDetail> IdDetails { get; set; }
        public List<string> VehicleNumbers { get; set; }
    }

    public class AddressDetail
    {
        public string Address { get; set; }
        public string AddressType { get; set; }
        public string AddressInLocale { get; set; }
    }

    public class RelativeDetail
    {
        public string Name { get; set; }
        public string RelationType { get; set; }
        public string NameInLocale { get; set; }
        public string AliasName { get; set; }
    }

    public class IdDetail
    {
        public string IdNumber { get; set; }
        public string IdType { get; set; }
        public string IdPlaceOfIssue { get; set; }
        public string IdDateOfIssue { get; set; }
    }

    public class ActAndSection
    {
        public string Acts { get; set; }
        public List<string> Sections { get; set; }
    }

    public class AccusedDetail
    {
        public List<string> ContactDetails { get; set; }
        public string Name { get; set; }
        public string NameInLocale { get; set; }
        public string AliasName { get; set; }
        public string Occupation { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Age { get; set; }
        public List<AddressDetail> AddressDetails { get; set; }
        public List<RelativeDetail> RelativeDetails { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public List<IdDetail> IdDetails { get; set; }
        public List<string> VehicleNumbers { get; set; }
    }

    public class UnidentifiedDetails
    {
        public List<string> Contacts { get; set; }
        public List<string> VehicleNumbers { get; set; }
    }

    public class IncidentDetail
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string TimePeriod { get; set; }
        public string Place { get; set; }
    }

    public class PropertyStolenInvolvedDetail
    {
        public string EstimatedValue { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    public class MatchFlags
    {
        public string ContactNoMatch { get; set; }
        public string RelativeNameMatch { get; set; }
        public string DobMatch { get; set; }
        public string AddressMatch { get; set; }
    }

    public class AdditionalMatchFlags
    {
        public string SplitMiddleNameMatch { get; set; }
    }
}