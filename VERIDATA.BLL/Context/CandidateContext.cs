using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using VERIDATA.BLL.apiContext.karza;
using VERIDATA.BLL.apiContext.signzy;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using VERIDATA.Model.Response.api.surepass;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class CandidateContext : ICandidateContext
    {
        private readonly IFileContext _fileContext;
        private readonly ApiConfiguration _apiConfig;
        private readonly IEmailSender _emailSender;
        private readonly IAppointeeDalContext _appointeeDalContext;
        private readonly IActivityDalContext _activityDalContext;
        private readonly IkarzaApiContext _karzaApiContext;
        private readonly IsignzyApiContext _signzyApiContext;
        private readonly IMasterDalContext _masterContext;
        private readonly IWorkFlowContext _workFlowContext;


        public CandidateContext(ApiConfiguration apiConfig, IFileContext fileContext, IAppointeeDalContext appointeeDalContext, 
            IMasterDalContext masterDalContext, IEmailSender emailSender, IActivityDalContext activityDalContext, 
            IkarzaApiContext karzaApiContext, IsignzyApiContext signzyApiContext, IWorkFlowContext workFlowContext)
        {
            _apiConfig = apiConfig;
            _fileContext = fileContext;
            _emailSender = emailSender;
            _appointeeDalContext = appointeeDalContext;
            _activityDalContext = activityDalContext;
            _masterContext = masterDalContext;
            _karzaApiContext = karzaApiContext;
            _signzyApiContext = signzyApiContext;
            _workFlowContext = workFlowContext;
        }
        public async Task<AppointeeDetailsResponse> GetAppointeeDetailsAsync(int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            List<FileDetailsResponse> _FileDataList = new();
            AppointeeDetails _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            AppointeeDetailsResponse data = new();
            if (_appointeedetails?.AppointeeDetailsId == null)
            {
               UnderProcessFileData _appntundrprocessdata = await _appointeeDalContext.GetUnderProcessAppinteeDetailsById(appointeeId);
                if (_appntundrprocessdata?.UnderProcessId != null)
                {
                    data.AppointeeDetailsId = _appointeedetails?.AppointeeDetailsId ?? 0;
                    data.AppointeeId = _appntundrprocessdata?.AppointeeId;
                    data.CandidateId = _appntundrprocessdata?.CandidateId;
                    data.CompanyName = _appntundrprocessdata?.CompanyName;
                    data.AppointeeName = _appntundrprocessdata?.AppointeeName;
                    data.AppointeeEmailId = _appntundrprocessdata?.AppointeeEmailId;
                    data.CompanyId = _appntundrprocessdata?.CompanyId;
                    data.DateOfJoining = _appntundrprocessdata?.DateOfJoining;
                    data.EPFWages = _appntundrprocessdata?.EPFWages ?? 0;
                    data.MobileNo = _appntundrprocessdata?.MobileNo;
                    data.IsPFverificationReq = _appntundrprocessdata?.IsPFverificationReq;
                    data.SaveStep = 0;
                    data.IsPassportValid = null;
                    data.IsPassportValid = null;
                    data.isPanVarified = null;
                    data.IsUanAvailable = null;
                    data.IsUanVarified = null;
                    data.IsManualPassbook = null;
                    data.IsAadhaarVarified = null;
                    data.IsFnameVarified = null;
                    data.IsUanLinkWithAadhar = null;
                    data.IsProcessed = false;
                    data.IsSubmit = false;
                    data.FileUploaded = _FileDataList;
                }
            }
            else
            {
                // var createdDate = _appointeedetails?.CreatedOn.Value.ToShortDateString() ?? "TEST";
                data.AppointeeDetailsId = _appointeedetails?.AppointeeDetailsId ?? 0;
                data.AppointeeId = _appointeedetails?.AppointeeId ?? appointeeId;
                data.CandidateId = _appointeedetails?.CandidateId;
                data.CompanyName = _appointeedetails?.CompanyName;
                data.AppointeeName = _appointeedetails?.AppointeeName;
                data.AppointeeEmailId = _appointeedetails?.AppointeeEmailId;
                data.AadhaarName = string.IsNullOrEmpty(_appointeedetails?.AadhaarName) ? null : _appointeedetails.AadhaarName;
                //data.AadhaarNumber = string.IsNullOrEmpty(_appointeedetails?.AadhaarNumber) ? null : CommonUtility.DecryptString(key, _appointeedetails.AadhaarNumber);
                data.AadhaarNumber = string.IsNullOrEmpty(_appointeedetails?.AadhaarNumberView) ? null : _appointeedetails.AadhaarNumberView;
                data.AadhaarNumberView = string.IsNullOrEmpty(_appointeedetails?.AadhaarNumberView) ? null : _appointeedetails.AadhaarNumberView;
                data.NameFromAadhaar = _appointeedetails?.NameFromAadhaar;
                data.DobFromAadhaar = _appointeedetails?.DobFromAadhaar;
                data.PANNumber = string.IsNullOrEmpty(_appointeedetails?.PANNumber) ? null : CommonUtility.DecryptString(key, _appointeedetails?.PANNumber);
                data.MaskedPANNumber = string.IsNullOrEmpty(_appointeedetails?.PANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.PANNumber));
                data.PANName = _appointeedetails?.PANName;
                data.FathersNameFromPan = _appointeedetails?.FathersNameFromPan;
                data.CompanyId = _appointeedetails?.CompanyId;
                data.DateOfBirth = _appointeedetails?.DateOfBirth;
                data.UANNumber = string.IsNullOrEmpty(_appointeedetails?.UANNumber) ? null : CommonUtility.DecryptString(key, _appointeedetails?.UANNumber);
                data.MaskedUANNumber = string.IsNullOrEmpty(_appointeedetails?.UANNumber) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                data.DateOfJoining = _appointeedetails?.DateOfJoining;
                data.Gender = _appointeedetails?.Gender;
                data.EPFWages = _appointeedetails?.EPFWages ?? 0;
                data.IsHandicap = _appointeedetails?.IsHandicap?.ToUpper();
                data.HandicapeType = _appointeedetails?.HandicapeType?.ToUpper();
                data.isPassportAvailable = _appointeedetails?.IsPassportAvailable?.ToUpper();
                data.IsInternationalWorker = _appointeedetails?.IsInternationalWorker?.ToUpper();
                data.OriginCountry = _appointeedetails?.OriginCountry;
                data.PassportNo = string.IsNullOrEmpty(_appointeedetails?.PassportNo) ? null : CommonUtility.DecryptString(key, _appointeedetails?.PassportNo);
                data.MaskedPassportNo = string.IsNullOrEmpty(_appointeedetails?.PassportNo) ? null : CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.PassportNo));
                data.PassportFileNo = _appointeedetails?.PassportFileNo;
                data.PassportValidFrom = _appointeedetails?.PassportValidFrom;
                data.PassportValidTill = _appointeedetails?.PassportValidTill;
                data.Qualification = _appointeedetails?.Qualification;
                data.MaratialStatus = _appointeedetails?.MaratialStatus;
                data.MemberName = _appointeedetails?.MemberName;
                data.MemberRelation = _appointeedetails?.MemberRelation;
                data.MobileNo = _appointeedetails?.MobileNo;
                data.Nationality = _appointeedetails?.Nationality;
                data.IsPassportValid = _appointeedetails?.IsPasssportVarified;
                data.isPanVarified = _appointeedetails?.IsPanVarified;
                data.IsPensionApplicable = _appointeedetails?.IsPensionApplicable;
                data.IsPFverificationReq = _appointeedetails?.IsPFverificationReq;
                data.IsAadhaarVarified = _appointeedetails?.IsAadhaarVarified;
                data.IsUanVarified = _appointeedetails?.IsUanVarified;
                data.IsManualPassbook = _appointeedetails?.IsManualPassbook;
                data.IsTrustPassbook = _appointeedetails?.IsTrustPassbook;
                data.IsUanAvailable = _appointeedetails?.IsUanAvailable;
                data.IsFnameVarified = _appointeedetails?.IsFNameVarified;
                data.IsProcessed = _appointeedetails?.IsProcessed;
                data.SaveStep = _appointeedetails?.SaveStep ?? 0;
                data.IsSubmit = _appointeedetails?.IsSubmit ?? false;
                data.IsUanLinkWithAadhar = _appointeedetails?.IsUanAadharLink == null ? "NA" : _appointeedetails?.IsUanAadharLink ?? false ? "Yes" : "No";
                data.UanAadharLinkStatus = _appointeedetails?.IsUanAadharLink ;
                data.UserId = _appointeedetails?.CreatedBy ?? 0;
                data.workFlowStatus = await _workFlowContext?.AppointeeWorkflowCurrentState(appointeeId) ;
                             //GetFileDataModel
                string? _paddedName = _appointeedetails?.AppointeeName?.Length > 4 ? _appointeedetails.AppointeeName?[..3] : _appointeedetails?.AppointeeName?.PadRight(3, '0');
                string candidateFileName = $"{_appointeedetails?.CandidateId}_{_paddedName}";
                await _fileContext.getFiledetailsByAppointeeId(appointeeId, _FileDataList);
                data.FileUploaded = _FileDataList;

            }
            return data;
        }

        public async Task<CandidateValidateResponse> UpdateCandidateValidateData(CandidateValidateUpdatedDataRequest validationReq)
        {
            string? key = _apiConfig.EncriptKey;
            CandidateValidateResponse Response = new();
            string Remarks = string.Empty;
            if (validationReq.Type == RemarksType.Adhaar && validationReq?.aadharData != null)
            {
                string? aadharNumber = validationReq?.aadharData?.AadhaarNumber;
                validationReq.aadharData.AadhaarNumber = !string.IsNullOrEmpty(aadharNumber) ? CommonUtility.CustomEncryptString(key, aadharNumber) : null;
                validationReq.aadharData.AadhaarNumberView = !string.IsNullOrEmpty(aadharNumber) ? CommonUtility.MaskedString(aadharNumber) : null;
            }
            if (validationReq.Type == RemarksType.Pan && validationReq?.panData != null)
            {
                string? PANNumber = !string.IsNullOrEmpty(validationReq?.panData?.PanNumber) ? CommonUtility.CustomEncryptString(key, validationReq?.panData?.PanNumber) : null;
                validationReq.panData.PanNumber = PANNumber;
            }
            if (validationReq.Type == RemarksType.UAN)
            {
                string? UANNumber = !string.IsNullOrEmpty(validationReq.uanData?.UanNumber) ? CommonUtility.CustomEncryptString(key, validationReq.uanData?.UanNumber) : null;
                validationReq.uanData.UanNumber = UANNumber;
            }
            
            await _appointeeDalContext.UpdateAppointeeVerifiedData(validationReq);

            if (validationReq?.Reasons?.Count > 0)
            {
                Remarks = await _appointeeDalContext.UpdateRemarksByType(validationReq.AppointeeId, validationReq.Reasons, validationReq?.Type ?? "", validationReq?.UserId ?? 0);
                if (!validationReq?.Status ?? false)
                {
                    string mailtype =CommonUtility.GetMailType(validationReq.Type);
                    if (!string.IsNullOrEmpty(validationReq?.EmailId))
                    {
                        MailDetails mailDetails = new();
                        MailBodyParseDataDetails bodyDetails = new()
                        {
                            Name = validationReq?.UserName,
                            Reason = Remarks,
                        };
                        mailDetails.MailType = mailtype;
                        mailDetails.ParseData = bodyDetails;
                        await _emailSender.SendAppointeeMail(validationReq?.EmailId, mailDetails);
                    }
                }
            }
            else
            {
                await _appointeeDalContext.UpdateRemarksStatusByType(validationReq.AppointeeId, validationReq?.Type ?? "", validationReq?.UserId ?? 0);
            }
            Response.IsValid = validationReq.Status ?? false;
            Response.Remarks = Remarks;

            return Response;
        }
      
        public async Task<List<GetRemarksResponse>> GetRemarks(int appointeeId)
        {
            List<GetRemarksResponse> remarks = await _appointeeDalContext.GetRemarks(appointeeId);
            return remarks;
        }
        public async Task<string?> GetRemarksRemedy(GetRemarksRemedyRequest reqObj)
        {
            string? remarksRemedy = string.Empty;
            if (reqObj.RemedyType == RemedyType.Issue)
            {
                remarksRemedy = await _appointeeDalContext.GetRemarksRemedy(reqObj?.RemarksId ?? 0);
            }
            if (reqObj.RemedyType == RemedyType.Others)
            {
                string? reasonType = string.Empty;
                string? remarksCode = string.Empty;
                if (!string.IsNullOrEmpty(reqObj.RemedySubType))
                {
                    switch (reqObj.RemedySubType)
                    {
                        case RemedySubType.INACTIVEUAN:
                            reasonType = RemarksType.UAN;
                            remarksCode = ReasonCode.INACTIVE;
                            break;
                        case RemedySubType.ADHAR:
                            reasonType = RemarksType.Adhaar;
                            remarksCode = ReasonCode.DOB;
                            break;
                        case RemedySubType.ADHARMBLE:
                            reasonType = RemarksType.Adhaar;
                            remarksCode = ReasonCode.PHNOTPINWITHADH;
                            break;
                        default:
                            reasonType = string.Empty;
                            remarksCode = string.Empty;
                            break;

                    }
                }

                remarksRemedy = await _appointeeDalContext.GetRemarksRemedyByCode(reasonType, remarksCode);
            }
            return remarksRemedy;
        }

        public async Task<List<AppointeeActivityDetailsResponse>> GetActivityDetails(int appointeeId)
        {
            _ = new List<AppointeeActivityDetailsResponse>();
            List<AppointeeActivityDetailsResponse> activityList = await _activityDalContext.GetActivityDetails(appointeeId);
            return activityList;
        }
        public async Task<AppointeePassbookDetailsViewResponse> GetPassbookDetailsByAppointeeId(int appointeeId)
        {
            AppointeePassbookDetailsViewResponse passbookDetails = new();

            // Get appointee upload details
            List<AppointeeUploadDetails> uploadDetails = await _appointeeDalContext.GetAppinteeUploadDetails(appointeeId);
            AppointeeUploadDetails? doc = uploadDetails?.Find(x => x.UploadTypeCode == FileTypealias.PFPassbook);

            string passbookData = string.Empty;
            if (doc != null && File.Exists(doc.UploadPath))
            {
                // Read passbook data from the file
                passbookData = File.ReadAllText(doc.UploadPath);
                passbookDetails = await ParsePassbookDataAsync(doc.UploadSubTypeCode, passbookData, appointeeId);
            }
            else
            {
                // Fallback: Read from employment details if file doesn't exist
                var empDetails = await _appointeeDalContext.GetEmployementDetails(appointeeId, JsonTypeAlias.EmployeePassBook);
                if (empDetails?.DataInfo != null)
                {
                    passbookData = System.Text.Encoding.UTF8.GetString(empDetails.DataInfo);
                    if (!string.IsNullOrEmpty(passbookData))
                    {
                        passbookDetails = await ParsePassbookDataAsync(empDetails.TypeCode, passbookData, appointeeId);
                    }
                }
            }

            return passbookDetails;
        }

        // Helper method to handle provider-specific parsing
        private async Task<AppointeePassbookDetailsViewResponse> ParsePassbookDataAsync(string providerType, string passbookData, int appointeeId)
        {
            AppointeePassbookDetailsViewResponse passbookDetails = new();

            switch (providerType)
            {
                case ApiProviderType.SurePass:
                    var surePassResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(passbookData);
                    passbookDetails = ParsePassbookDetailsSurePass(surePassResponse);
                    break;

                case ApiProviderType.Karza:
                    var karzaResponse = JsonConvert.DeserializeObject<UanPassbookDetails>(passbookData);
                    passbookDetails = await ParsePassbookDetailsKarza(karzaResponse, appointeeId);
                    break;

                case ApiProviderType.Signzy:
                    var signzyResponse = JsonConvert.DeserializeObject<SignzyUanPassbookDetails>(passbookData);
                    passbookDetails = await ParsePassbookDetailsSignzy(signzyResponse, appointeeId);
                    break;
            }

            return passbookDetails;
        }

        public async Task UpdateCandidateUANData(int appointeeId, string UanNumber)
        {
            string? key = _apiConfig.EncriptKey;
            string? UAN = !string.IsNullOrEmpty(UanNumber) ? CommonUtility.CustomEncryptString(key, UanNumber) : null;
            await _appointeeDalContext.UpdateAppointeeUanNumber(appointeeId, UAN);

        }

        private AppointeePassbookDetailsViewResponse ParsePassbookDetailsSurePass(Surepass_GetUanPassbookResponse PassBookResponse)
        {
            AppointeePassbookDetailsViewResponse passbookDetails = new();
            PassbookData? PassBookResponseData = PassBookResponse?.data;
            if (PassBookResponseData != null)
            {
                List<PfCompanyDetails> _companyDetailsList = new();
                passbookDetails.clientId = PassBookResponseData.client_id;
                passbookDetails.fullName = PassBookResponseData.full_name;
                passbookDetails.fatherName = PassBookResponseData.father_name;
                passbookDetails.pfUan = PassBookResponseData.pf_uan;
                passbookDetails.dob = PassBookResponseData.dob;

                foreach (KeyValuePair<string, CandidateCompanies> obj in PassBookResponseData.companies)
                {
                    CandidateCompanies _copmnyData = obj.Value;
                    PfCompanyDetails _companyDetails = new()
                    {
                        passbook = new List<CompanyPassbookDetails>(),
                        companyName = _copmnyData?.company_name,
                        establishmentId = _copmnyData?.establishment_id,
                        memberId = _copmnyData?.passbook.LastOrDefault()?.member_id,
                        LastTransactionApprovedOn = _copmnyData?.passbook.LastOrDefault()?.approved_on,
                        LastTransactionMonth = CommonUtility.getMonthName(Convert.ToInt32(_copmnyData?.passbook.LastOrDefault()?.month)),
                        LastTransactionYear = _copmnyData?.passbook.LastOrDefault()?.year
                    };
                    List<CompanyPassbookDetails>? passbookDetailsList = _copmnyData?.passbook?.Select((x, index) => new CompanyPassbookDetails
                    {
                        id = index + 1,
                        approvedOn = x.approved_on,
                        description = x.description,
                        month = CommonUtility.getMonthName(Convert.ToInt32(x.month)),
                        year = x.year
                    })?.ToList();
                    _companyDetails.passbook = passbookDetailsList;
                    _companyDetailsList.Add(_companyDetails);
                }
                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        private async Task<AppointeePassbookDetailsViewResponse> ParsePassbookDetailsKarza(UanPassbookDetails PassBookResponse, int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            AppointeePassbookDetailsViewResponse passbookDetails = new();
            AppointeeDetails _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            if (PassBookResponse != null)
            {
                EmployeeDetails PassBookResponseData = PassBookResponse.employee_details;
                List<PfCompanyDetails> _companyDetailsList = new();
                passbookDetails.clientId = "NA"; //PassBookResponseData.client_id;
                passbookDetails.fullName = PassBookResponseData?.member_name;
                passbookDetails.fatherName = PassBookResponseData?.father_name;
                passbookDetails.pfUan = CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                passbookDetails.dob = PassBookResponseData?.dob;
                var sortedEstDetails = PassBookResponse.est_details.OrderByDescending(estDetail => DateTime.TryParse(estDetail.doj_epf, out var dojDate) ? dojDate : DateTime.MinValue).ToList();

                foreach (EstDetail obj in sortedEstDetails)
                {
                    bool? isPensionApplicable = null;
                    bool? isPensionGapIdentified = null;
                    DateTime? LastPensionDate = null;
                    string LastApprovedOn = obj?.passbook?.LastOrDefault().approved_on;
                    string LastPensionApprovedOn = obj?.passbook?.LastOrDefault(x => Convert.ToInt32(x.cr_pen_bal ?? "0") > 0 && x.particular.ToLower().Contains("cont.")).approved_on;
                    DateTime transMonth = string.IsNullOrEmpty(LastApprovedOn) ? Convert.ToDateTime(obj.doe_epf) : Convert.ToDateTime(LastApprovedOn);
                    List<CompanyPassbookDetails> passbookDetailsList = new();

                    int index = 0;
                    foreach (var x in obj.passbook ?? new List<Passbook>())
                    {
                        bool? penstionContribution = x.particular.ToLower().Contains("cont.") ? Convert.ToInt32(x?.cr_pen_bal) > 0 ? true : false : null;
                        CompanyPassbookDetails pssbkData = new()
                        {
                            id = index + 1, // Set id as the index value (1-based index)
                            approvedOn = x?.approved_on,
                            description = x?.particular,
                            month = string.IsNullOrEmpty(x?.approved_on?.Trim()) ? "NA" : CommonUtility.getMonthName(Convert.ToDateTime(x.approved_on?.Trim()).Month),
                            year = string.IsNullOrEmpty(x?.approved_on?.Trim()) ? "NA" : Convert.ToDateTime(x.approved_on?.Trim()).Year.ToString(),
                            ispensionContributed = penstionContribution == null ? "NA" : penstionContribution == true ? "Yes" : "No"
                        };

                        passbookDetailsList.Add(pssbkData);
                        if (penstionContribution == true)
                        {
                            isPensionGapIdentified = false;
                            isPensionApplicable = true;
                        }
                        if (penstionContribution == false)
                        {
                            if (isPensionGapIdentified == false)
                                isPensionGapIdentified = true;

                        }

                        index++;
                    }


                    PfCompanyDetails _companyDetails = new()
                    {
                        passbook = passbookDetailsList,
                        companyName = obj?.est_name,
                        establishmentId = "NA", // Placeholder value; update as needed
                        memberId = obj?.member_id,
                        LastTransactionApprovedOn = obj?.passbook?.LastOrDefault()?.approved_on,
                        LastTransactionMonth = CommonUtility.getMonthName(transMonth.Month),
                        LastTransactionYear = transMonth.Year.ToString(),
                        IsPensionApplicable = isPensionApplicable == null ? "NA" : isPensionApplicable ?? false ? "Yes" : "No",
                        IsPensionGap = isPensionGapIdentified == null ? "NA" : isPensionGapIdentified ?? false ? "Yes" : "No",
                        //LastPensionDate = LastPensionDate?.ToString("dd-MM-yyyy") ?? "NA",
                        LastPensionDate = !string.IsNullOrEmpty(LastPensionApprovedOn) ? LastPensionApprovedOn : "NA",
                    };

                    _companyDetailsList.Add(_companyDetails);
                }

                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        private async Task<AppointeePassbookDetailsViewResponse> ParsePassbookDetailsSignzy(SignzyUanPassbookDetails passBookResponse, int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            AppointeePassbookDetailsViewResponse passbookDetails = new();
            AppointeeDetails appointeeDetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);

            if (passBookResponse != null)
            {
                passbookDetails.clientId = "NA";
                passbookDetails.fullName = passBookResponse?.FullName;
                passbookDetails.fatherName = passBookResponse?.FatherName;
                passbookDetails.pfUan = CommonUtility.MaskedString(CommonUtility.DecryptString(key, appointeeDetails?.UANNumber));
                passbookDetails.dob = passBookResponse?.Dob.ToShortDateString();

                List<PfCompanyDetails> companyDetailsList = new();

                foreach (var company in passBookResponse.Companies)
                {
                    var estDetails = company.Value;

                    DateTime transMonth = Convert.ToDateTime(estDetails.Passbook.LastOrDefault()?.ApprovedOn);
                    PfCompanyDetails companyDetails = new()
                    {
                        passbook = new List<CompanyPassbookDetails>(),
                        companyName = estDetails.CompanyName,
                        establishmentId = estDetails.EstablishmentId,
                        memberId = estDetails.Passbook.FirstOrDefault()?.MemberId,
                        LastTransactionApprovedOn = estDetails.Passbook.LastOrDefault()?.ApprovedOn.ToShortDateString(),
                        LastTransactionMonth = CommonUtility.getMonthName(transMonth.Month),
                        LastTransactionYear = transMonth.Year.ToString()
                    };

                    List<CompanyPassbookDetails> passbookDetailsList = estDetails.Passbook.Select((x, index) => new CompanyPassbookDetails
                    {
                        id = index + 1,
                        approvedOn = x.ApprovedOn.ToShortDateString(),
                        description = x.Description,
                        month = CommonUtility.getMonthName(Convert.ToDateTime(x.ApprovedOn).Month),
                        year = Convert.ToDateTime(x.ApprovedOn).Year.ToString()
                    }).ToList();

                    companyDetails.passbook = passbookDetailsList;
                    companyDetailsList.Add(companyDetails);
                }

                passbookDetails.companies = companyDetailsList;
            }

            return passbookDetails;
        }

        public async Task<AppointeeEmployementDetailsViewResponse> GetEmployementDetailsByAppointeeId(int appointeeId, int userId)
        {
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            string? key = _apiConfig.EncriptKey;

            // Get appointee details
            AppointeeDetails appointeeDetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);

            // Check if UAN number is available
            if (string.IsNullOrEmpty(appointeeDetails?.UANNumber))
            {
                return new AppointeeEmployementDetailsViewResponse
                {
                    isEmployementDetailsAvailable = false,
                    remarks = "No UAN available to fetch passbook details"
                };
            }

            // Try to get the uploaded passbook file details
            List<AppointeeUploadDetails> uploadDetails = await _appointeeDalContext.GetAppinteeUploadDetails(appointeeId);
            AppointeeUploadDetails? docList = uploadDetails.Find(x => x.UploadTypeCode == FileTypealias.PFPassbook);

            if (docList != null && File.Exists(docList.UploadPath))
            {
                // Read the passbook data
                string passbookData = await File.ReadAllTextAsync(docList.UploadPath);

                // Handle passbook parsing based on provider type
                passbookDetails = await HandlePassbookParsing(docList.UploadSubTypeCode, passbookData, appointeeId);
                passbookDetails.isEmployementDetailsAvailable = true;

            }
            else
            {
                // Get employment history details from database if passbook file is not found
                passbookDetails = await GetEmploymentDetailsFromHistory(appointeeId, userId, key, appointeeDetails);
            }
            return passbookDetails;
        }

        // Method to handle passbook parsing based on provider type
        private async Task<AppointeeEmployementDetailsViewResponse> HandlePassbookParsing(string providerType, string passbookData, int appointeeId)
        {
            switch (providerType)
            {
                case ApiProviderType.SurePass:
                    var surePassResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(passbookData);
                    return ParseEmployementDetailsSurePass(surePassResponse);

                case ApiProviderType.Karza:
                    var karzaResponse = JsonConvert.DeserializeObject<UanPassbookDetails>(passbookData);
                    return await ParseEmployementDetailsKarzaByPassbook(karzaResponse, appointeeId);

                default:
                    throw new ArgumentException("Invalid provider type for passbook");
            }
        }

        // Method to get employment history details if the passbook file is not available
        private async Task<AppointeeEmployementDetailsViewResponse> GetEmploymentDetailsFromHistory(int appointeeId, int userId, string key, AppointeeDetails appointeeDetails)
        {
            var passbookDetails = new AppointeeEmployementDetailsViewResponse();
            var empPassbookDetails = await _appointeeDalContext.GetEmployementDetails(appointeeId, JsonTypeAlias.EmployeePassBook);
            if (empPassbookDetails?.DataInfo != null)
            {
                var passbookData = System.Text.Encoding.UTF8.GetString(empPassbookDetails.DataInfo);
                if (!string.IsNullOrEmpty(passbookData))
                {
                    return passbookDetails = await HandlePassbookParsing(empPassbookDetails.TypeCode, passbookData, appointeeId);
                }
            }
            var empHistDetails = await _appointeeDalContext.GetEmployementDetails(appointeeId, JsonTypeAlias.EmployementHist);

            // If employment history is found, parse and return based on provider type
            if (empHistDetails?.DataInfo != null)
            {
                return await ParseEmploymentDetailsBasedOnProvider(empHistDetails.TypeCode, empHistDetails.DataInfo, appointeeId);
            }

            // If no employment history, decrypt UAN number and fetch external data
            var decryptedUANNumber = CommonUtility.DecryptString(key, appointeeDetails?.UANNumber);

            var employmentHistData = await GetEmployementHistoryDetails(new GetEmployemntDetailsRequest
            {
                appointeeId = appointeeId,
                uanNummber = decryptedUANNumber,
                userId = userId,
            });

            // Parse based on provider type from external API
            if (employmentHistData?.EmployementData != null)
            {
                return await ParseEmploymentDetailsBasedOnProvider(employmentHistData.Provider, employmentHistData.EmployementData, appointeeId);
            }

            // If no valid data found, return failure response
            passbookDetails.isEmployementDetailsAvailable = false;
            passbookDetails.remarks = employmentHistData?.Remarks;
            return passbookDetails;
        }

        private async Task<AppointeeEmployementDetailsViewResponse> ParseEmploymentDetailsBasedOnProvider(string provider, byte[] dataInfo, int appointeeId)
        {
            if (dataInfo == null)
            {
                return new AppointeeEmployementDetailsViewResponse { isEmployementDetailsAvailable = false };
            }
            string _dataInfo = dataInfo != null ? System.Text.Encoding.UTF8.GetString(dataInfo) : null;

            switch (provider)
            {
                case ApiProviderType.Karza:
                    var karzaResponse = JsonConvert.DeserializeObject<Karza_GetEmployementDetailsByUanResponse>(_dataInfo);
                    return await ParseEmployementDetailsKarza(karzaResponse, appointeeId);

                case ApiProviderType.Signzy:
                    var signzyResponse = JsonConvert.DeserializeObject<Signzy_GetEmployementDetailsByUanResponse>(_dataInfo);
                    return await ParseEmployementDetailsSignzy(signzyResponse, appointeeId);

                default:
                    return new AppointeeEmployementDetailsViewResponse { isEmployementDetailsAvailable = false };
            }
        }

        //public async Task<AppointeeEmployementDetailsViewResponse> GetGetEmployementDetailsByAppointeeId(int appointeeId,int userId)
        //{
        //    AppointeeEmployementDetailsViewResponse passbookDetails = new();
        //    string filePath = string.Empty;
        //    string? key = _apiConfig.EncriptKey;
        //    AppointeeDetails _appointeeDetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);

        //    if (string.IsNullOrEmpty(_appointeeDetails?.UANNumber))
        //    {
        //        passbookDetails.isUanAvailable = false;
        //        passbookDetails.remarks = "No UAN available to fetch passbook details";
        //    }
        //    else
        //    {
        //        List<AppointeeUploadDetails> _UploadDetails = await _appointeeDalContext.GetAppinteeUploadDetails(appointeeId);
        //        AppointeeUploadDetails? _DocList = _UploadDetails.Find(x => x.UploadTypeCode == FileTypealias.PFPassbook);
        //        if (_DocList != null)
        //        {
        //            string path = _DocList.UploadPath;
        //            if (File.Exists(path))
        //            {
        //                // Read entire text file content in one string
        //                string _passbookdata = File.ReadAllText(path);
        //                if (_DocList.UploadSubTypeCode == ApiProviderType.SurePass)
        //                {
        //                    Surepass_GetUanPassbookResponse PassBookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(_passbookdata);
        //                    passbookDetails = ParseEmployementDetailsSurePass(PassBookResponse);
        //                }
        //                if (_DocList.UploadSubTypeCode == ApiProviderType.Karza)
        //                {
        //                    UanPassbookDetails PassBookResponse1 = JsonConvert.DeserializeObject<UanPassbookDetails>(_passbookdata);
        //                    passbookDetails = await ParseEmployementDetailsKarzaByPassbook(PassBookResponse1, appointeeId);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            AppointeeEmployementDetails _empHistDetails = await _appointeeDalContext.GetEmployementDetails(appointeeId);
        //            if (_empHistDetails?.EmploymentDetailsId == null)
        //            {
        //                var UANNumber = CommonUtility.DecryptString(key, _appointeeDetails?.UANNumber);
        //                GetEmployemntDetailsRequest request = new GetEmployemntDetailsRequest
        //                {
        //                    appointeeId = appointeeId,
        //                    uanNummber = UANNumber,
        //                    userId = 1
        //                };
        //               var employementHistData=await _verifyContext.GetEmployementHistoryDetails(request);
        //                if (employementHistData.Provider == ApiProviderType.Karza)
        //                {
        //                    if (!string.IsNullOrEmpty(employementHistData.EmployementData))
        //                    {
        //                        Karza_GetEmployementDetailsByUanResponse EmpHistResponse = JsonConvert.DeserializeObject<Karza_GetEmployementDetailsByUanResponse>(_empHistDetails.DataInfo);
        //                        passbookDetails = await ParseEmployementDetailsKarza(EmpHistResponse, appointeeId);

        //                    }
        //                }
        //            }

        //            if (_empHistDetails.TypeCode == ApiProviderType.Karza)
        //            {
        //                if (!string.IsNullOrEmpty(_empHistDetails.DataInfo))
        //                {
        //                    Karza_GetEmployementDetailsByUanResponse EmpHistResponse = JsonConvert.DeserializeObject<Karza_GetEmployementDetailsByUanResponse>(_empHistDetails.DataInfo);
        //                    passbookDetails = await ParseEmployementDetailsKarza(EmpHistResponse, appointeeId);

        //                }
        //            }
        //        }

        //    }
        //    return passbookDetails;
        //}

        private AppointeeEmployementDetailsViewResponse ParseEmployementDetailsSurePass(Surepass_GetUanPassbookResponse PassBookResponse)
        {
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            PassbookData? PassBookResponseData = PassBookResponse?.data;
            if (PassBookResponseData != null)
            {
                List<PfEmployementDetails> _companyDetailsList = new();
                passbookDetails.clientId = PassBookResponseData.client_id;
                passbookDetails.fullName = PassBookResponseData.full_name;
                passbookDetails.fatherName = PassBookResponseData.father_name;
                passbookDetails.pfUan = PassBookResponseData.pf_uan;
                passbookDetails.dob = PassBookResponseData.dob;

                foreach (KeyValuePair<string, CandidateCompanies> obj in PassBookResponseData.companies)
                {
                    CandidateCompanies _copmnyData = obj.Value;
                    var TotalWorkDays = Convert.ToInt32(((Convert.ToDateTime(_copmnyData?.passbook.LastOrDefault()?.approved_on)) - (Convert.ToDateTime(_copmnyData?.passbook.FirstOrDefault()?.approved_on))).TotalDays);

                    PfEmployementDetails _companyDetails = new()
                    {
                        companyName = _copmnyData?.company_name,
                        establishmentId = _copmnyData?.establishment_id,
                        memberId = _copmnyData?.passbook.LastOrDefault()?.member_id,
                        FirstTransactionApprovedOn = _copmnyData?.passbook.FirstOrDefault()?.approved_on,
                        FirstTransactionMonth = CommonUtility.getMonthName(Convert.ToInt32(_copmnyData?.passbook.FirstOrDefault()?.month)),
                        FirstTransactionYear = _copmnyData?.passbook.FirstOrDefault()?.year,
                        LastTransactionApprovedOn = _copmnyData?.passbook.LastOrDefault()?.approved_on,
                        LastTransactionMonth = CommonUtility.getMonthName(Convert.ToInt32(_copmnyData?.passbook.LastOrDefault()?.month)),
                        LastTransactionYear = _copmnyData?.passbook.LastOrDefault()?.year,
                        TotalWorkDays = TotalWorkDays,
                        WorkForYear = TotalWorkDays / 365,
                        WorkForMonth = (TotalWorkDays % 365) / 30,
                    };
                    _companyDetailsList.Add(_companyDetails);
                }
                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        private async Task<AppointeeEmployementDetailsViewResponse> ParseEmployementDetailsKarzaByPassbook(UanPassbookDetails PassBookResponse, int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            AppointeeDetails _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            if (PassBookResponse != null)
            {
                EmployeeDetails PassBookResponseData = PassBookResponse.employee_details;
                List<PfEmployementDetails> _companyDetailsList = new();
                passbookDetails.clientId = "NA"; //PassBookResponseData.client_id;
                passbookDetails.fullName = PassBookResponseData?.member_name;
                passbookDetails.fatherName = PassBookResponseData?.father_name;
                passbookDetails.pfUan = CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                passbookDetails.dob = PassBookResponseData?.dob;

                foreach (EstDetail obj in PassBookResponse?.est_details)
                {
                    //var _copmnyData = obj.Value;
                    var TotalWorkDays = Convert.ToInt32(((Convert.ToDateTime(obj?.passbook.LastOrDefault()?.approved_on)) - (Convert.ToDateTime(obj?.passbook.FirstOrDefault()?.approved_on))).TotalDays);

                    DateTime transFirstMonth = Convert.ToDateTime(obj?.passbook.FirstOrDefault()?.approved_on);
                    DateTime transLastMonth = Convert.ToDateTime(obj?.passbook.LastOrDefault()?.approved_on);
                    PfEmployementDetails _companyDetails = new()
                    {
                        companyName = obj?.est_name,
                        establishmentId = "NA",// _copmnyData?.establishment_id;
                        memberId = obj?.member_id,
                        FirstTransactionApprovedOn = obj?.passbook.FirstOrDefault()?.approved_on,
                        FirstTransactionMonth = CommonUtility.getMonthName(transFirstMonth.Month),
                        FirstTransactionYear = transFirstMonth.Year.ToString(),
                        LastTransactionApprovedOn = obj?.passbook.LastOrDefault()?.approved_on,
                        LastTransactionMonth = CommonUtility.getMonthName(transLastMonth.Month),
                        LastTransactionYear = transLastMonth.Year.ToString(),
                        TotalWorkDays = TotalWorkDays,
                        WorkForYear = TotalWorkDays / 365,
                        WorkForMonth = (TotalWorkDays % 365) / 30,
                    };
                    _companyDetailsList.Add(_companyDetails);
                }
                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        private async Task<AppointeeEmployementDetailsViewResponse> ParseEmployementDetailsKarza(Karza_GetEmployementDetailsByUanResponse? EmpHistResponse, int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            AppointeeDetails _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            if (EmpHistResponse != null)
            {
                var EmployementData = EmpHistResponse.Result;
                EmployeePersonalDetails PassBookResponseData = EmployementData.PersonalDetails;
                List<PfEmployementDetails> _companyDetailsList = new();
                passbookDetails.clientId = "NA"; //PassBookResponseData.client_id;
                passbookDetails.fullName = PassBookResponseData?.Name ?? _appointeedetails.AppointeeName;
                passbookDetails.fatherName = PassBookResponseData?.FatherOrHusbandName;
                passbookDetails.pfUan = CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                passbookDetails.dob = _appointeedetails?.DateOfBirth?.ToShortDateString();

                foreach (EmployerHistory obj in EmployementData?.Employers)
                {
                    //var _copmnyData = obj.Value;
                    var TotalWorkMonth = 0;
                    if (!string.IsNullOrEmpty(obj?.StartMonthYear) && !string.IsNullOrEmpty(obj?.LastMonthYear))
                        TotalWorkMonth = GetTotalMonths(obj?.StartMonthYear, obj?.LastMonthYear, "MM-yyyy");

                    PfEmployementDetails _companyDetails = new()
                    {
                        companyName = obj?.EstablishmentName,
                        establishmentId = obj.EstablishmentId,
                        memberId = obj?.MemberId,
                        FirstTransactionApprovedOn = "NA",
                        FirstTransactionMonth = string.IsNullOrEmpty(obj?.StartMonthYear) ? "NA" : CommonUtility.GetMonthYearFullName(obj.StartMonthYear, "month", "MM-yyyy"),
                        FirstTransactionYear = string.IsNullOrEmpty(obj?.StartMonthYear) ? "NA" : CommonUtility.GetMonthYearFullName(obj.StartMonthYear, "year", "MM-yyyy"),
                        LastTransactionApprovedOn = "NA",
                        LastTransactionMonth = string.IsNullOrEmpty(obj?.LastMonthYear) ? "NA" : CommonUtility.GetMonthYearFullName(obj.LastMonthYear, "month", "MM-yyyy"),
                        LastTransactionYear = string.IsNullOrEmpty(obj?.LastMonthYear) ? "NA" : CommonUtility.GetMonthYearFullName(obj.LastMonthYear, "year", "MM-yyyy"),
                        TotalWorkDays = 0,
                        WorkForYear = TotalWorkMonth > 0 ? TotalWorkMonth / 12 : 0,
                        WorkForMonth = TotalWorkMonth,
                    };
                    _companyDetailsList.Add(_companyDetails);
                }
                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        private async Task<AppointeeEmployementDetailsViewResponse> ParseEmployementDetailsSignzy(Signzy_GetEmployementDetailsByUanResponse? EmpHistResponse, int appointeeId)
        {
            string? key = _apiConfig.EncriptKey;
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            AppointeeDetails _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            if (EmpHistResponse != null)
            {
                var EmployementData = EmpHistResponse.Result;
                var personalData = EmployementData.FirstOrDefault();
                List<PfEmployementDetails> _companyDetailsList = new();
                passbookDetails.clientId = "NA"; //PassBookResponseData.client_id;
                passbookDetails.fullName = personalData?.Name;
                passbookDetails.fatherName = personalData?.FatherOrHusbandName;
                passbookDetails.pfUan = CommonUtility.MaskedString(CommonUtility.DecryptString(key, _appointeedetails?.UANNumber));
                passbookDetails.dob = _appointeedetails?.DateOfBirth?.ToShortDateString();

                foreach (EmploymentHistoryDetail obj in EmployementData)
                {
                    //var _copmnyData = obj.Value;
                    var TotalWorkMonth = 0;
                    if (!string.IsNullOrEmpty(obj?.DateOfJoining) && !string.IsNullOrEmpty(obj?.DateOfExit))
                        TotalWorkMonth = GetTotalMonths(obj?.DateOfJoining, obj?.DateOfExit, "MMM-yyyy");

                    PfEmployementDetails _companyDetails = new()
                    {
                        companyName = obj?.EstablishmentName,
                        establishmentId = "NA",
                        memberId = obj?.MemberId,
                        FirstTransactionApprovedOn = "NA",
                        FirstTransactionMonth = string.IsNullOrEmpty(obj?.DateOfJoining) ? "NA" : CommonUtility.GetMonthYearFullName(obj.DateOfJoining, "month", "MMM-yyyy"),
                        FirstTransactionYear = string.IsNullOrEmpty(obj?.DateOfJoining) ? "NA" : CommonUtility.GetMonthYearFullName(obj.DateOfJoining, "year", "MMM-yyyy"),
                        LastTransactionApprovedOn = "NA",
                        LastTransactionMonth = string.IsNullOrEmpty(obj?.DateOfExit) ? "NA" : CommonUtility.GetMonthYearFullName(obj.DateOfExit, "month", "MMM-yyyy"),
                        LastTransactionYear = string.IsNullOrEmpty(obj?.DateOfExit) ? "NA" : CommonUtility.GetMonthYearFullName(obj.DateOfExit, "year", "MMM-yyyy"),
                        TotalWorkDays = 0,
                        WorkForYear = TotalWorkMonth > 0 ? TotalWorkMonth / 12 : 0,
                        WorkForMonth = TotalWorkMonth,
                    };
                    _companyDetailsList.Add(_companyDetails);
                }
                passbookDetails.companies = _companyDetailsList;
            }

            return passbookDetails;
        }
        public static int GetTotalMonths(string startMonthYear, string lastMonthYear, string format)
        {
            // Parse the input strings to DateTime objects
            DateTime startDate = DateTime.ParseExact(startMonthYear, format, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(lastMonthYear, format, CultureInfo.InvariantCulture);

            // Calculate the difference in months
            int totalMonths = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;

            return totalMonths;
        }
        public async Task PostAppointeefileUploadAsync(AppointeeFileDetailsRequest AppointeeFileDetails)
        {
            int appointeeId = AppointeeFileDetails.AppointeeId;
            AppointeeDetails? _appointeedetails = await _appointeeDalContext.GetAppinteeDetailsById(appointeeId);
            if (_appointeedetails.IsProcessed != true)
            {
                bool _isSubmit = _appointeedetails?.IsSubmit ?? false;
                if (_appointeedetails != null && !_isSubmit)
                {
                    await _fileContext.postappointeeUploadedFiles(AppointeeFileDetails);
                }
            }
        }
        public async Task PostAppointeeTrusUanDetailsAsync(AppointeeUpdatePfUanDetailsRequest AppointeeTrustDetails)
        {
            await _appointeeDalContext.UpdateAppointeeTrustnUanAvailibility(AppointeeTrustDetails.AppointeeId, AppointeeTrustDetails.TrustPassbookAvailable, AppointeeTrustDetails.IsUanAvailable, AppointeeTrustDetails.IsFinalSubmit);
        }
        public async Task PostAppointeeHandicapDetailsAsync(AppointeeHadicapFileDetailsRequest AppointeeHandicapDetails)
        {
            await _appointeeDalContext.UpdateAppointeeHandicapDetails(AppointeeHandicapDetails.AppointeeId, AppointeeHandicapDetails.IsHandicap, AppointeeHandicapDetails.HandicapType);
        }

        public async Task<AppointeeEmployementDetails> PostAppointeeEmployementDetailsAsync(EmployementHistoryDetails reqObj)
        {
            var res = new AppointeeEmployementDetails();
            res = await _appointeeDalContext.PostEmployementDetails(reqObj);
            return res;
        }
        private async Task<EmployementHistoryDetailsRespons> GetEmployementHistoryDetails(GetEmployemntDetailsRequest reqObj)
        {
            GetEmployemntDetailsResponse ApiResponse = new();
            EmployementHistoryDetails empReq = new();
            EmployementHistoryDetailsRespons Response = new();
            empReq.AppointeeId = reqObj.appointeeId;
            empReq.UserId = reqObj.userId;
            empReq.SubType = JsonTypeAlias.EmployementHist;
            var apiProvider = await _masterContext.GetApiProviderData(ApiType.UAN);

            if (apiProvider?.ToLower() == ApiProviderType.Karza)
            {
                empReq.Provider = ApiProviderType.Karza;
                ApiResponse = await _karzaApiContext.GetEmployementDetais(reqObj.uanNummber, reqObj.userId);
                Response.StatusCode = ApiResponse.StatusCode;
            }
            if (apiProvider?.ToLower() == ApiProviderType.Signzy)
            {
                empReq.Provider = ApiProviderType.Signzy;
                ApiResponse = await _signzyApiContext.GetEmploymentHistoryByUan(reqObj.uanNummber, reqObj.userId);
                Response.StatusCode = ApiResponse.StatusCode;
            }
            if (ApiResponse.StatusCode == HttpStatusCode.OK)
            {
                empReq.EmpData = ApiResponse.EmployementData;
                var response = await PostAppointeeEmployementDetailsAsync(empReq);
                Response.EmployementData = response.DataInfo;
                Response.AppointeeId = response.AppointeeId ?? 0;
                Response.Provider = response.TypeCode;
                Response.SubType = response.SubTypeCode;

            }
            else
            {
                //await _activityContext.PostActivityDetails(reqObj.appointeeId, reqObj.userId, ActivityLog.UANVERIFIFAILED);
                Response.Remarks = GenarateErrorMsg((int)ApiResponse.StatusCode, ApiResponse?.ReasonPhrase?.ToString(), "EPFO");
            }
            return Response;
        }
        public string GenarateErrorMsg(int statusCode, string reasonCode, string type)
        {
            string msg = statusCode == (int)HttpStatusCode.InternalServerError
                ? $"{type} {"server is currently busy! Please try again later"}"
                : $"{reasonCode}, {"Please try again later."}";
            return msg;
        }

        public async Task<AppointeeDetails> PostAppointeepensionAsync(AppointeeApprovePensionRequest reqObj)
        {
            var updatedAppointeeDetails = await _appointeeDalContext.UpdateAppinteePensionById(reqObj);


            if (updatedAppointeeDetails == null)
            {
                throw new Exception("Appointee not found.");
            }

            return updatedAppointeeDetails;
        }

        //public async Task<AppointeeDetails> VerifyAppointeeManualAsync(AppointeeApproveVerificationRequest reqObj)
        //{
        //    var updatedAppointeeDetails = await _appointeeDalContext.VefifyAppinteeManualById(reqObj);

        //    if (updatedAppointeeDetails == null)
        //    {
        //        throw new Exception("Appointee not found.");
        //    }

        //    // Apply the verification updates
        //    foreach (var update in reqObj.VerificationUpdates)
        //    {
        //        switch (update.FieldName.ToLower())
        //        {
        //            case "isfnamevarified":
        //                updatedAppointeeDetails.IsFNameVarified = update.IsVerified;
        //                break;
        //            case "ispasssportvarified":
        //                updatedAppointeeDetails.IsPasssportVarified = update.IsVerified;
        //                break;
        //            case "isaadhaarvarified":
        //                updatedAppointeeDetails.IsAadhaarVarified = update.IsVerified;
        //                break;
        //            default:
        //                throw new Exception($"Unknown field name: {update.FieldName}");
        //        }
        //    }

        //    return updatedAppointeeDetails;
        //}

        //public async Task<AppointeeFileViewDetailResponse> GetNotVeriedfileView(AppointeeNotVerifiedFileViewRequest reqObj)
        //{
        //    // Initialize response object
        //    /*  var response = new AppointeeFileViewDetailResponse();

        //      // Fetch relevant UploadTypeMaster records based on AppointeeId and FileCategory
        //      var uploadTypeMasters = await _appointeeDalContext.GetAppinteeFileViewDetail(reqObj);

        //      // Filter UploadTypeMaster results based on the specified conditions

        //      response.UploadTypeMasters = uploadTypeMasters
        //          .Where(u => u.UploadTypeCategory == reqObj.FileCategory &&
        //          u.UploadTypeCode != null && // Ensure UploadTypeCode is not null
        //          (u.AppointeeDetails == null || // Check AppointeeDetails conditions if they exist
        //          (u.AppointeeDetails.IsPasssportVarified == null &&
        //          u.AppointeeDetails.IsManualPassbook == null &&
        //          u.AppointeeDetails.IsFNameVarified == null)))
        //          .ToList();

        //      return response;
        //    */
        //    return await _appointeeDalContext.GetAppinteeFileViewDetail(reqObj);

        //}
    }
}
