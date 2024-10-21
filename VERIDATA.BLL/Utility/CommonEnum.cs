namespace VERIDATA.BLL.utility
{
    public class CommonEnum
    {
        public enum FileType
        {
            Xlxs = 1,
            DOCX = 2
        }

        public enum UserType
        {
            Application = 1,
            Company = 2,
            Appoientee = 3
        }
        public static class CheckType
        {
            public const string yes = "Y";
            public const string no = "N";
        }
        public static class AadhaarData
        {
            public const string CanNotRead = "CNR";
            public const string CanNotFind = "CNF";
        }

        public enum NotificationType
        {
            Email,
            SMS
        }
        public enum ValidationType
        {
            Invalid = 1,
            Duplicate = 2,
            ApiCount = 3,
            AppointeeCount = 4,
            Critical1week = 5,
            Critical2week = 6,
            NoResponse = 7,
            Processing = 8,
            NoLinkSent = 9,
            NonExsist = 10,
        }
        public static class MailType
        {
            public const string SendOTP = "OTPSEND";
            public const string Reprocess = "REPRCESS";
            public const string Remainder = "REMAINDER";
            public const string AdhrValidation = "ADHVALIDTN";
            public const string UANValidation = "UANVALIDTN";
            public const string Passport = "PASSPORT";
            public const string Pan = "PAN";
            public const string ForceApprove = "FRCEAPPRVE";
            public const string Reject = "REJECT";
            public const string CandidateCreate = "CNDCREATE";
            public const string CandidateUpdate = "CNDUPDATE";
            public const string Others = "OTHERS";
            public const string AdminUserCreate = "ADMNCREATE";
            public const string MailResend = "MAILRESEND";

        }
        public static class WorkFlowType
        {
            public const string sendMail = "SM";
            public const string UploadDetails = "UD";
            public const string DataVarified = "DV";
            public const string Approved = "AP";
            public const string ForcedApproved = "FA";
            public const string Rejected = "RJ";
            public const string Reprocess = "RE";
            public const string ProcessIni = "PI";
            public const string ProcessClose = "CL";
        }
        public static class MasterDataType
        {
            public const string COUNTRY = "CON";
            public const string NATIONALITY = "NAT";
            public const string GENDER = "GEN";
            public const string MARATIALSTAT = "MAR";
            public const string DISABILITY = "DIS";
            public const string FILETYPE = "FLT";
            public const string QUALIFICATION = "QUA";
            public const string ROLE = "RLE";
            public const string REPORTFILTERSTATUS = "RPTSTS";
            public const string ENTITY = "ENTITY";
        }

        public static class FileTypealias
        {
            public const string Pan = "PAN";
            public const string Adhaar = "ADH";
            public const string PFFile = "PFD";
            public const string PFPassbook = "EPFPSBK";
            public const string PFPassbookExcel = "EPFPSBKEXCL";
            public const string PFPassbookTrust = "EPFPSBKTRUST";
            public const string HandicapCert = "HANDCERT";

        }
        public static class RemarksType
        {
            public const string UAN = "UAN";
            public const string Adhaar = "ADH";
            public const string Passport = "PSPRT";
            public const string Pan = "PAN";
            public const string Others = "OTH";

        }
        public static class RemedyType
        {
            public const string Issue = "ISSU";
            public const string Others = "OTH";

        }
        public static class RemedySubType
        {
            public const string INACTIVEUAN = "INACTUAN";
            public const string ADHAR = "ADHAR";
            public const string Others = "OTH";

        }
        public static class ApiType
        {
            public const string UAN = "UAN";
            public const string Adhaar = "ADHAAR";
            public const string Passport = "PASSPORT";
            public const string Pan = "PAN";
            public const string EPFO = "EPFO";
            public const string EPFOUAN = "EPFOUAN";

        }
        public static class ApiSubTYpeName
        {
            public const string Pan = "PanDetails";
            public const string FindUan = "FindUan";
            public const string Passport = "PassportDetails";
            public const string AadharGenerateOTP = "AdharGenerateOTP";
            public const string AadharVerifyOTP = "AdharVerifyOTP";
            public const string UANGenerateOTP = "UanGenerateOTP";
            public const string UANSubmitOTP = "UanSubmitOTP";
            public const string UanPassbook = "UanPassbook";
            public const string UanValidation = "UanValidation";

        }
        public static class ApiProviderType
        {
            public const string Karza = "karza";
            public const string Signzy = "signzy";
            public const string SurePass = "surepass";
        }
        public static class RoleTypeAlias
        {
            public const string Appointee = "APNTE";
            public const string SuperAdmin = "SADMN";
            public const string CompanyAdmin = "CADMN";
            public const string GeneralAdmin = "GADMN";
            public const string AppUser = "APUSR";

        }
        public enum RejectState
        {
            ApprovalReject = 1,
            UnprocessReject = 2
        }

        public static class ReasonCode
        {
            public const string DOB = "100";
            public const string CAREOFNAME = "101";
            public const string NAME = "102";
            public const string AADHARNO = "103";
            public const string GENDER = "104";
            public const string ISPFREQUIRED = "105";
            public const string INACTIVE = "106";
            public const string OTHER = "107";
            public const string ADHNOTVARIFIRED = "108";
            public const string UANNOTVARIDIED = "109";
            public const string ADHNOTAVAILABLE = "110";
            public const string PHNOTPINWITHADH = "111";
            public const string PASSPORTNO = "112";
            public const string INVDPASSPRT = "113";
            public const string PASSPRTNAME = "114";
            public const string PASSPRTDOB = "115";
            public const string UPLOADEDNAME = "116";
            public const string PANDOB = "117";
            public const string PANMOBILE = "118";
            public const string PANMOBILENOTAVAIL = "119";
            public const string INVDADHAR = "120";
            //public const string MANUALOVERRIDE = "121";
            //public const string REJECT = "122";
            //public const string QUALIFICATION = "QUA";
            //public const string ROLE = "RLE";
        }
        public static class FilterCode
        {
            public const string All = "All";
            public const string UNDERPROCESS = "001";
            public const string LINKNOTSENT = "002";
            public const string VERIFIED = "003";
            public const string REJECTED = "004";
            public const string LAPSED = "005";
        }
        public static class EscalationLevel
        {
            public const string Level1 = "LVL1";
            public const string Level2 = "LVL2";
            public const string Level3 = "LVL3";
        }
        public static class EscalationCase
        {
            public const string NoLinkSend = "NOLINK";
            public const string NoResponse = "NORES";
            public const string ResponsedNotSubmitted = "NOSUB";
            public const string DOJ1Week = "DOJ1W";
            public const string DOJ2Week = "DOJ2W";
        }
        public static class ActivityLog
        {
            public const string LOGIN = "LOGIN";
            public const string DATADRAFT = "DTDRFT";
            public const string DATASBMT = "DTSBMT";
            public const string DATASAVED = "DTSAVE";
            public const string ADHVERIFICATIONSTART = "ADHVSTRT";
            public const string ADHVERIFICATIONCMPLTE = "ADHVCMPT";
            public const string ADHVERIFIFAILED = "ADHVFALD";
            public const string ADHINVALID = "ADHINVLD";
            public const string ADHDATAVERIFICATIONFAILED = "ADHDTVFLD";
            public const string UANFETCH = "UANFTCH";
            public const string NOUAN = "NOUAN";
            public const string UANVERIFICATIONSTART = "UANVSTRT";
            public const string UANVERIFICATIONCMPLTE = "UANVCMPT";
            public const string UANVERIFIFAILED = "UANVFALD";
            public const string UANDATAVERIFICATIONFAILED = "UANDTVFLD";
            public const string UANFETCHPASSBOOK = "UANPSBKFCH";
            public const string APNTVERIFICATIONCOMPLETE = "VRCMPLT";
            public const string PASPRTVERIFICATIONSTART = "PSVSTRT";
            public const string PASPRTVERIFICATIONCMPLTE = "PSVCMPT";
            public const string PASPRTVERIFIFAILED = "PSVFALD";
            public const string PASPRTDATAVERIFICATIONFAILED = "PSDTVFLD";
            public const string PANVERIFICATIONSTART = "PANVSTRT";
            public const string PANVERIFICATIONCMPLTE = "PANVCMPT";
            public const string PANVERIFIFAILED = "PANVFALD";
            public const string PANDATAVERIFICATIONFAILED = "PANDTVFLD";
            public const string CONSENTGIVEN = "CNSTGVN";
            public const string CONSENTDECLINE = "CNSTDCLN";
            public const string CONSENTREVOKED = "CNSTRVK";
            public const string PREREQUISITECONFIRMYES = "PREREQCNFYES";
            public const string PREREQUISITECONFIRMNO = "PREREQCNFNO";
        }
        public static class MenuCode
        {
            public const string HOME = "HOME";
            public const string DASHBOARD = "DASHBRD";
            public const string COMPANY = "COMPNY";
            public const string DATAUPLOAD = "DTUPLOAD";
            public const string UPLOADEDDATA = "UPLDDATA";
            public const string LINKNOTSENT = "LNKNTSNT";
            public const string VERIFIED = "VERFD";
            public const string REJECTED = "REJECT";
            public const string REGISTER = "REGISTR";
            public const string PROCESSING = "PROCCSNG";
            public const string REPROCESS = "REPRCSS";
            public const string EXPIRED = "EXPRED";
            public const string CRITICAL = "CRITCL";
            public const string SETUP = "SETUP";

        }
        public static class UpdateType
        {
            public const string DOJ = "DOJ";
            public const string NAME = "NAME";
            public const string MOBILE = "MOBILE";
            public const string EMAIL = "EMAIL";
        }
        public static class CandidateUpdateTableType
        {
            public const string Raw = "rawData";
            public const string underProcess = "underprocess";
            public const string linknotsend = "linknotsend";
            public const string userMaster = "userMaster";
        }
        public static class CandidateIdType
        {
            public const string All = "All";
            public const string UnderProcess = "UnderProcess";
            public const string UnProcess = "NonProcess";
            public const string Raw = "Raw";
            public const string Processed = "Varified";
        }
        public static class ReportFilterStatus
        {
            public const string ProcessIniSubmit = "PISUB";
            public const string ProcessIniOnGoing = "PIONG";
            public const string ProcessIniNoResponse = "PINORS";
            public const string LinkNotSent = "LNKNS";
            public const string Approved = "AP";
            public const string Rejected = "RJ";
            public const string ForcedApproved = "FA";

        }
        public static class NationalityType
        {
            public const string All = "All";
            public const string Indian = "IN";
            public const string NonIndian = "OTH";

        }
        public enum KarzaStatusCode
        {
            Invalid = 102,
            Sent = 101,
            //NotSent = 10,
            NotFound = 103,
            MaxTry = 104,
        }
        public enum SignzyStatusCode
        {
            Invalid = 400,
            NotFound = 404,
            UpServerDown = 409,
            Succed = 101,
        }
        public enum ConsentStatus
        {
            Agree = 1,
            Disagree = 2,
            Revoked = 3,
        }
        //public static class LogLevel
        //{
        //    public const string Error = "Error";
        //    public const string Debug = "Debug";
        //    public const string Info = "Info";

        //}
        public static class JsonTypeAlias
        {
            public const string EmployementHist = "EMPHIST";
          

        }
        public static class Priority  // mGhosh 
        {
            public const string api_Priority = "1";
        }
    }
}
