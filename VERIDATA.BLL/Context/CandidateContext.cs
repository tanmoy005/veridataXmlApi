using Newtonsoft.Json;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Response.api.Karza;
using VERIDATA.Model.Response.api.Signzy;
using VERIDATA.Model.Response.api.surepass;
using VERIDATA.Model.Table.Public;
using static VERIDATA.BLL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{
    public class CandidateContext : ICandidateContext
    {
        private readonly IFileContext _fileContext;
        private readonly ApiConfiguration _apiConfig;
        private readonly IEmailSender _emailSender;
        private readonly IAppointeeDalContext _appointeeDalContext;
        private readonly IActivityDalContext _activityDalContext;
        public CandidateContext(ApiConfiguration apiConfig, IFileContext fileContext, IAppointeeDalContext appointeeDalContext, IEmailSender emailSender, IActivityDalContext activityDalContext)
        {
            _apiConfig = apiConfig;
            _fileContext = fileContext;
            _emailSender = emailSender;
            _appointeeDalContext = appointeeDalContext;
            _activityDalContext = activityDalContext;
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
                    data.isPanVarified = null;
                    data.IsUanVarified = null;
                    data.IsAadhaarVarified = null;
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
                data.IsTrustPassbook = _appointeedetails?.IsTrustPassbook;
                //data.IsTrustPension = _appointeedetails?.IsTrustPension;
                data.IsProcessed = _appointeedetails?.IsProcessed;
                data.SaveStep = _appointeedetails?.SaveStep ?? 0;
                data.IsSubmit = _appointeedetails?.IsSubmit ?? false;
                data.UserId = _appointeedetails?.CreatedBy ?? 0;
                //GetFileDataModel
                string? _paddedName = _appointeedetails?.AppointeeName?.Length > 4 ? _appointeedetails.AppointeeName?[..3] : _appointeedetails?.AppointeeName?.PadRight(3, '0');
                string candidateFileName = $"{_appointeedetails?.CandidateId}_{_paddedName}";
                await _fileContext.getFiledetailsByAppointeeId(appointeeId, candidateFileName, _FileDataList);
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
                string? UANNumber = !string.IsNullOrEmpty(validationReq?.UanNumber) ? CommonUtility.CustomEncryptString(key, validationReq?.UanNumber) : null;
                validationReq.UanNumber = UANNumber;
            }

            await _appointeeDalContext.UpdateAppointeeVerifiedData(validationReq);

            if (validationReq?.Reasons?.Count > 0)
            {
                Remarks = await _appointeeDalContext.UpdateRemarksByType(validationReq.AppointeeId, validationReq.Reasons, validationReq?.Type ?? "", validationReq?.UserId ?? 0);
                if (!validationReq?.Status ?? false)
                {
                    string mailtype = GetMailType(validationReq.Type);
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
        private static string GetMailType(string Type)
        {
            string mailtype = string.Empty;
            if (!string.IsNullOrEmpty(Type))
            {
                mailtype = Type switch
                {
                    RemarksType.Adhaar => MailType.AdhrValidation,
                    RemarksType.UAN => MailType.UANValidation,
                    RemarksType.Passport => MailType.Passport,
                    RemarksType.Pan => MailType.Pan,
                    RemarksType.Others => MailType.Others,
                    _ => string.Empty,
                };
            }
            return mailtype;
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
            string filePath = string.Empty;
            List<AppointeeUploadDetails> _UploadDetails = await _appointeeDalContext.GetAppinteeUploadDetails(appointeeId);
            AppointeeUploadDetails? _DocList = _UploadDetails.Find(x => x.UploadTypeCode == FileTypealias.PFPassbook);
            if (_DocList != null)
            {
                string path = _DocList.UploadPath;
                if (File.Exists(path))
                {
                    // Read entire text file content in one string
                    string _passbookdata = File.ReadAllText(path);
                    if (_DocList.UploadSubTypeCode == ApiProviderType.SurePass)
                    {
                        Surepass_GetUanPassbookResponse PassBookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(_passbookdata);
                        passbookDetails = ParsePassbookDetailsSurePass(PassBookResponse);
                    }
                    if (_DocList.UploadSubTypeCode == ApiProviderType.Karza)
                    {
                        UanPassbookDetails PassBookResponse1 = JsonConvert.DeserializeObject<UanPassbookDetails>(_passbookdata);
                        passbookDetails = await ParsePassbookDetailsKarza(PassBookResponse1, appointeeId);
                    }
                    if (_DocList.UploadSubTypeCode == ApiProviderType.Signzy)
                    {
                        SignzyUanPassbookDetails PassBookResponse1 = JsonConvert.DeserializeObject<SignzyUanPassbookDetails>(_passbookdata);
                        passbookDetails = await ParsePassbookDetailsSignzy(PassBookResponse1, appointeeId);
                    }
                }
            }
            return passbookDetails;
            //  return filePath;
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

                foreach (EstDetail obj in PassBookResponse?.est_details ?? new List<EstDetail>())
                {
                    bool? isPensionApplicable = null;
                    DateTime? LastPensionDate = null;
                    DateTime transMonth = Convert.ToDateTime(obj.doe_epf);
                    List<CompanyPassbookDetails> passbookDetailsList = new();

                    int index = 0;
                    foreach (var x in obj.passbook ?? new List<Passbook>())
                    {
                        CompanyPassbookDetails pssbkData = new()
                        {
                            id = index + 1, // Set id as the index value (1-based index)
                            approvedOn = x.approved_on,
                            description = x.particular,
                            month = CommonUtility.getMonthName(Convert.ToDateTime(x.approved_on).Month),
                            year = Convert.ToDateTime(x.approved_on).Year.ToString()
                        };

                        passbookDetailsList.Add(pssbkData);

                        if (Convert.ToInt32(x?.cr_pen_bal) > 0 && (LastPensionDate == null || Convert.ToDateTime(x?.approved_on) > LastPensionDate))
                        {
                            isPensionApplicable = true;
                            LastPensionDate = Convert.ToDateTime(x?.approved_on);
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
                        LastPensionDate = LastPensionDate?.ToString("dd-MM-yyyy") ?? "NA",
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


        public async Task<AppointeeEmployementDetailsViewResponse> GetGetEmployementDetailsByAppointeeId(int appointeeId)
        {
            AppointeeEmployementDetailsViewResponse passbookDetails = new();
            string filePath = string.Empty;
            List<AppointeeUploadDetails> _UploadDetails = await _appointeeDalContext.GetAppinteeUploadDetails(appointeeId);
            AppointeeUploadDetails? _DocList = _UploadDetails.Find(x => x.UploadTypeCode == FileTypealias.PFPassbook);
            if (_DocList != null)
            {
                string path = _DocList.UploadPath;
                if (File.Exists(path))
                {
                    // Read entire text file content in one string
                    string _passbookdata = File.ReadAllText(path);
                    if (_DocList.UploadSubTypeCode == ApiProviderType.SurePass)
                    {
                        Surepass_GetUanPassbookResponse PassBookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(_passbookdata);
                        passbookDetails = ParseEmployementDetailsSurePass(PassBookResponse);
                    }
                    if (_DocList.UploadSubTypeCode == ApiProviderType.Karza)
                    {
                        UanPassbookDetails PassBookResponse1 = JsonConvert.DeserializeObject<UanPassbookDetails>(_passbookdata);
                        passbookDetails = await ParseEmployementDetailsKarza(PassBookResponse1, appointeeId);
                    }
                }
            }
            return passbookDetails;
            //  return filePath;
        }

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
        private async Task<AppointeeEmployementDetailsViewResponse> ParseEmployementDetailsKarza(UanPassbookDetails PassBookResponse, int appointeeId)
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
    }
}
