﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.DAL.utility;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Activity;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.DAL.DataAccess.Context
{
    public class AppointeeDalContext : IAppointeeDalContext
    {
        private readonly DbContextDalDB _dbContextClass;

        public AppointeeDalContext(DbContextDalDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public async Task<AppointeeDetails> GetAppinteeDetailsById(int appointeeId)
        {
            AppointeeDetails _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true);
            return _appointeedetails;
        }

        public async Task<UnderProcessFileData> GetUnderProcessAppinteeDetailsById(int appointeeId)
        {
            UnderProcessFileData? data = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true) ?? new UnderProcessFileData();
            return data;
        }

        public async Task<List<AppointeeUploadDetails>> GetAppinteeUploadDetails(int appointeeId)
        {
            List<AppointeeUploadDetails> _uploadDetails = await _dbContextClass.AppointeeUploadDetails
           .Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true)
           .ToListAsync();

            return _uploadDetails;
        }

        // mGhosh New code
        public async Task<List<AppointeeUploadDetails>> GetAppinteeUploadDetails(int appointeeId, string? uploadTypeCode = null)
        {
            IQueryable<AppointeeUploadDetails> query = _dbContextClass.AppointeeUploadDetails
                .Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true);

            if (!string.IsNullOrEmpty(uploadTypeCode))
            {
                query = query.Where(x => x.UploadTypeCode == uploadTypeCode);
            }

            return await query.ToListAsync();
        }

        public async Task<AppointeeUploadDetails> GetAppinteeUploadDetailsById(int appointeeId, int? uploadFileId)
        {
            AppointeeUploadDetails _fileDetails = new();
            IQueryable<AppointeeUploadDetails> query = _dbContextClass.AppointeeUploadDetails
                .Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true && x.UploadDetailsId == uploadFileId);
            _fileDetails = await query.FirstOrDefaultAsync();
            return _fileDetails;
        }

        public async Task<List<ReasonMaser>> GetAllRemarksByType(string Type)
        {
            List<ReasonMaser> AllResonDetails = await _dbContextClass.ReasonMaser.Where(x => x.ReasonType.Equals(Type) && x.ActiveStatus == true).ToListAsync();
            return AllResonDetails;
        }

        public async Task UpdateAppointeeVerifiedData(CandidateValidateUpdatedDataRequest validationReq)
        {
            AppointeeDetails _appointeedetails = await GetAppinteeDetailsById(validationReq.AppointeeId);

            if (_appointeedetails?.AppointeeDetailsId != null)
            {
                if (validationReq.Type == RemarksType.Adhaar)
                {
                    _appointeedetails.IsAadhaarVarified = validationReq.Status;
                    _appointeedetails.AadhaarName = validationReq?.aadharData?.AadhaarName;
                    _appointeedetails.AadhaarNumber = validationReq?.aadharData?.AadhaarNumber;
                    _appointeedetails.AadhaarNumberView = validationReq?.aadharData?.AadhaarNumberView;
                    _appointeedetails.NameFromAadhaar = validationReq?.aadharData?.NameFromAadhaar;
                    _appointeedetails.GenderFromAadhaar = validationReq?.aadharData?.GenderFromAadhaar;
                    _appointeedetails.DobFromAadhaar = validationReq?.aadharData?.DobFromAadhaar;
                }
                if (validationReq.Type == RemarksType.UAN)
                {
                    _appointeedetails.IsPensionApplicable = validationReq.uanData?.IsPensionApplicable;
                    _appointeedetails.IsPensionGap = validationReq.uanData?.IsPensionGap;
                    _appointeedetails.IsUanVarified = validationReq.Status;
                    _appointeedetails.IsPassbookFetch = validationReq.uanData?.IsPassbookFetch;
                    _appointeedetails.UANNumber = validationReq.uanData?.UanNumber;
                    _appointeedetails.IsFNameVarified = _appointeedetails.IsFNameVarified != true ? validationReq?.IsFNameVarified : _appointeedetails.IsFNameVarified;
                    _appointeedetails.IsDualEmployement = validationReq.uanData?.IsDualEmployementIdentified;
                    if (validationReq.uanData?.IsUanFromMobile ?? false)
                    {
                        _appointeedetails.IsUanAadharLink = validationReq.uanData?.AadharUanLinkYN;
                    }
                }
                if (validationReq.Type == RemarksType.Passport)
                {
                    _appointeedetails.IsPasssportVarified = validationReq.Status;
                    _appointeedetails.PassportFileNo = validationReq.PassportFileNo;
                }
                if (validationReq.Type == RemarksType.Pan)
                {
                    _appointeedetails.HasPan = validationReq?.HasData;
                    _appointeedetails.IsPanVarified = validationReq.Status;
                    _appointeedetails.PANNumber = validationReq?.panData?.PanNumber;
                    _appointeedetails.PANName = validationReq?.panData?.PanName;
                    _appointeedetails.FathersNameFromPan = validationReq?.panData?.PanFatherName;
                }
                if (validationReq.Type == RemarksType.Bank)
                {
                    _appointeedetails.IsBankVarified = validationReq.Status;
                    _appointeedetails.AccountNo = validationReq?.BankDetails?.AccountNo;
                    _appointeedetails.IfscCode = validationReq?.BankDetails?.IFSCCode;
                }
                if (validationReq.Type == RemarksType.Police)
                {
                    _appointeedetails.FirDetails = !string.IsNullOrEmpty(validationReq?.FirDetails) ? validationReq?.FirDetails : null;
                    _appointeedetails.IsPoliceVarified = validationReq?.Status;
                }
                if (validationReq.Type == RemarksType.DRLNC)
                {
                    _appointeedetails.DrivingLicense = !string.IsNullOrEmpty(validationReq?.DlNumber) ? validationReq?.DlNumber : null;
                    _appointeedetails.IsDlVarified = validationReq?.Status;
                    _appointeedetails.HasDrivingLicense = validationReq?.HasData;
                    _appointeedetails.IsFNameVarified = validationReq.IsFNameVarified;
                }
                _appointeedetails.SaveStep = validationReq.step ?? _appointeedetails.SaveStep;
            }
            _ = await _dbContextClass.SaveChangesAsync();
        }

        public async Task UpdateAppointeeAahdaarImage(int appointeeId, string candidateId, int userId, string imageBase64Data)
        {
            var req = new List<AppointeeUploadDetails>();
            var _fileDetails = await getFileTypeDataByAliasAsync(FileTypealias.AdhaarProfile);
            byte[] fileContent = Convert.FromBase64String(imageBase64Data);
            AppointeeUploadDetails uploaddata = new()
            {
                AppointeeId = appointeeId,
                UploadTypeId = _fileDetails.UploadTypeId,
                UploadTypeCode = _fileDetails.UploadTypeCode,
                FileName = candidateId + "_" + "profileImage",
                UploadPath = null,
                IsPathRefered = CheckType.no,
                MimeType = "image/jpeg",
                ActiveStatus = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Content = fileContent
            };
            req.Add(uploaddata);
            await Uploadfiles(req);
        }

        public async Task UpdateAppointeeUanNumber(int appointeeId, string uanNumber)
        {
            AppointeeDetails _appointeedetails = await GetAppinteeDetailsById(appointeeId);

            if (_appointeedetails?.AppointeeDetailsId != null)
            {
                _appointeedetails.UANNumber = uanNumber;
            }
            _ = await _dbContextClass.SaveChangesAsync();
        }

        public async Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks> Reasons, string Type, int UserId)
        {
            string AllRemarks = string.Empty;
            if (Reasons?.Count > 0)
            {
                List<ReasonMaser> AllResonDetails = await GetAllRemarksByType(Type);
                List<AppointeeReasonMappingData> AllPrevReason = await _dbContextClass.AppointeeReasonMappingData.Where(x => x.AppointeeId.Equals(AppointeeId) && x.ActiveStatus == true).ToListAsync();
                var reasonListquery = from rm in AllResonDetails
                                      join r in Reasons
                                      on rm.ReasonCode equals r.ReasonCode
                                      select new RemarksDetails { ReasonName = rm.ReasonName, ReasonCode = rm.ReasonCode, ReasonId = rm.ReasonId, Inputdata = r.Inputdata, Fetcheddata = r.Fetcheddata, Remarks = r.Remarks };
                var ResonDetails = reasonListquery.ToList();
                AllRemarks = await RemarksUpdateByType_SubType(AppointeeId, UserId, AllRemarks, AllResonDetails, AllPrevReason, ResonDetails, string.Empty);
            }
            return AllRemarks;
        }

        public async Task<string> UpdateRemarksByType(int AppointeeId, List<ReasonRemarks> Reasons, string Type, int UserId, string subType)
        {
            string AllRemarks = string.Empty;
            if (Reasons?.Count > 0)
            {
                List<ReasonMaser> AllResonDetails = await GetAllRemarksByType(Type);
                List<AppointeeReasonMappingData> AllPrevReason = await _dbContextClass.AppointeeReasonMappingData
                    .Where(x => x.AppointeeId.Equals(AppointeeId) && x.ActiveStatus == true)
                    .ToListAsync();

                var reasonListquery = from rm in AllResonDetails
                                      join r in Reasons
                                      on rm.ReasonCode equals r.ReasonCode
                                      select new RemarksDetails
                                      {
                                          ReasonName = rm.ReasonName,
                                          ReasonCode = rm.ReasonCode,
                                          ReasonId = rm.ReasonId,
                                          Inputdata = r.Inputdata,
                                          Fetcheddata = r.Fetcheddata,
                                          Remarks = r.Remarks
                                      };

                var ResonDetails = reasonListquery.ToList();

                // Call the updated method and pass the new parameter
                AllRemarks = await RemarksUpdateByType_SubType(AppointeeId, UserId, AllRemarks, AllResonDetails, AllPrevReason, ResonDetails, subType);
            }
            return AllRemarks;
        }

        private async Task<string> RemarksUpdateByType_SubType(int AppointeeId, int UserId, string AllRemarks, List<ReasonMaser> AllResonDetails, List<AppointeeReasonMappingData> AllPrevReason, List<RemarksDetails> ResonDetails, string? subType)
        {
            if (ResonDetails?.Count > 0)
            {
                List<int> AllReasonIdList = AllResonDetails.Select(x => x.ReasonId).ToList();
                List<AppointeeReasonMappingData> PrevReason = AllPrevReason.Where(x => AllReasonIdList.Contains(x.ReasonId)).ToList();
                List<AppointeeReasonMappingData>? _resaonList = ResonDetails?.Select(x => new AppointeeReasonMappingData
                {
                    AppointeeId = AppointeeId,
                    ReasonId = x.ReasonId ?? 0,
                    ReasonSubType = subType,
                    Remarks = x.ReasonCode != ReasonCode.OTHER ? CommonDalUtility.ParseMessage(x.ReasonName, new { x.Inputdata, x.Fetcheddata }) : x.Remarks,
                    CreatedBy = UserId,
                    ActiveStatus = true,
                    CreatedOn = DateTime.Now,
                }).ToList();
                await _dbContextClass.AppointeeReasonMappingData.AddRangeAsync(_resaonList);

                if (PrevReason.Count > 0)
                {
                    if (string.IsNullOrEmpty(subType))
                    {
                        PrevReason.ForEach(x => x.ActiveStatus = false);
                    }
                    else
                    {
                        PrevReason.Where(x => x.ReasonSubType == subType).ToList().ForEach(x => x.ActiveStatus = false);
                    }
                }
                if (_resaonList.Count > 0)
                {
                    AllRemarks = string.Join(", ", _resaonList.Select(x => x.Remarks).ToList());
                }
            }
            else
            {
                if (AllPrevReason.Count > 0)
                {
                    List<int> allreasonIdByType = AllResonDetails.Select(x => x.ReasonId).ToList();
                    if (string.IsNullOrEmpty(subType))
                    {
                        AllPrevReason.Where(x => allreasonIdByType.Contains(x.ReasonId)).ToList().ForEach(x => x.ActiveStatus = false);
                    }
                    else
                    {
                        AllPrevReason.Where(x => allreasonIdByType.Contains(x.ReasonId) && x.ReasonSubType == subType).ToList().ForEach(x => x.ActiveStatus = false);
                    }
                }
            }
            _ = await _dbContextClass.SaveChangesAsync();
            return AllRemarks;
        }

        public async Task UpdateRemarksStatusByType(int AppointeeId, string Type, string subType, int UserId)
        {
            string AllRemarks = string.Empty;

            List<ReasonMaser> AllResonDetails = await GetAllRemarksByType(Type);
            var AllReasonId = AllResonDetails?.Select(x => x.ReasonId)?.ToList();
            List<AppointeeReasonMappingData> AllPrevReason = await _dbContextClass.AppointeeReasonMappingData.Where(x => x.AppointeeId.Equals(AppointeeId) && x.ActiveStatus == true && AllReasonId.Contains(x.ReasonId)).ToListAsync();

            if (AllPrevReason?.Count > 0)
            {
                if (string.IsNullOrEmpty(subType))
                {
                    AllPrevReason?.ForEach(x => { x.ActiveStatus = false; x.UpdatedOn = DateTime.Now; x.UpdatedBy = UserId; });
                }
                else
                {
                    AllPrevReason?.Where(x => x.ReasonSubType == subType)?.ToList()?.ForEach(x => { x.ActiveStatus = false; x.UpdatedOn = DateTime.Now; x.UpdatedBy = UserId; });
                }
                await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task Uploadfiles(List<AppointeeUploadDetails> uploadDetails)
        {
            if (uploadDetails.Count > 0)
            {
                _dbContextClass.AppointeeUploadDetails.AddRange(uploadDetails);
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task RemovePrevfiles(List<AppointeeUploadDetails> prevDocDetails, int userId)
        {
            if (prevDocDetails.Count > 0)
            {
                foreach (var doc in prevDocDetails)
                {
                    doc.ActiveStatus = false;
                    doc.UpdatedOn = DateTime.Now;
                    doc.UpdatedBy = userId;
                }

                await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task UpdateAppointeeSubmit(int AppointeeId, bool IsSubmit, bool? IsManualPassbookUploaded)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(AppointeeId);
            appointeeDetails.IsSubmit = IsSubmit;
            appointeeDetails.IsManualPassbook = IsManualPassbookUploaded;
            _ = await _dbContextClass.SaveChangesAsync();
        }

        public async Task UpdateAppointeeTrustnUanAvailibility(int AppointeeId, bool? TrustPassbookAvailable, bool? IsUanAvailable, bool? IsFinalSubmit)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(AppointeeId);
            if (appointeeDetails.IsProcessed != true)
            {
                appointeeDetails.IsTrustPassbook = TrustPassbookAvailable;
                if (IsFinalSubmit == true)
                    appointeeDetails.IsUanAvailable = IsUanAvailable;

                _ = await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task UpdateAppointeeHandicapDetails(int AppointeeId, string? IsHandicap, string? HandicapType)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(AppointeeId);
            bool _isHandicap = IsHandicap?.ToString()?.ToUpper() == CheckType.yes;
            if (appointeeDetails.IsProcessed != true)
            {
                appointeeDetails.IsHandicap = string.IsNullOrEmpty(IsHandicap) ? null : IsHandicap.ToString()?.ToUpper();
                appointeeDetails.HandicapeType = _isHandicap ? HandicapType?.ToString() : string.Empty;
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }

        public async Task<List<GetRemarksResponse>> GetRemarks(int appointeeId)
        {
            List<GetRemarksResponse> remarks = new();
            var querydata = from r in _dbContextClass.ReasonMaser
                            join a in _dbContextClass.AppointeeReasonMappingData
                                on r.ReasonId equals a.ReasonId
                            where r.ActiveStatus == true && a.AppointeeId == appointeeId && a.ActiveStatus == true
                            select new { a.Remarks, r.ReasonId, r.ReasonCode, r.ReasonCategory, a.ActiveStatus };

            var list = await querydata.ToListAsync().ConfigureAwait(false);
            if (list.Count > 0)
            {
                remarks = list.Select(x => new GetRemarksResponse
                {
                    RemarksId = x.ReasonId,
                    RemarksCode = x.ReasonCode,
                    RemarksCategory = x.ReasonCategory,
                    Remarks = x.Remarks
                }).ToList();
            }
            return remarks;
        }

        public async Task<string?> GetRemarksRemedy(int remarksId)
        {
            string remedyhtml = await _dbContextClass.ReasonMaser?.Where(x => x.ReasonId == remarksId)?.Select(y => y.ReasonRemedy).FirstOrDefaultAsync();
            return remedyhtml;
        }

        public async Task<string?> GetRemarksRemedyByCode(string ReasonType, string remarksCode)
        {
            string remedyhtml = await _dbContextClass.ReasonMaser?.Where(x => x.ReasonCode == remarksCode.Trim() && x.ReasonType == ReasonType.Trim())?.Select(y => y.ReasonRemedy).FirstOrDefaultAsync();
            return remedyhtml;
        }

        public async Task<UploadTypeMaster> getFileTypeDataByAliasAsync(string? fileTypeAlias)
        {
            UploadTypeMaster? uploadFileType = await _dbContextClass.UploadTypeMaster.Where(x => x.UploadTypeCode.Equals(fileTypeAlias) && x.ActiveStatus == true).FirstOrDefaultAsync() ?? new UploadTypeMaster();

            return uploadFileType;
        }

        public async Task<AppointeeConsentMapping> getAppointeeContestAsync(int? appointeeId)
        {
            AppointeeConsentMapping? consentStatus = await _dbContextClass.AppointeeConsentMapping.Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true).FirstOrDefaultAsync() ?? new AppointeeConsentMapping();

            return consentStatus;
        }

        public async Task postAppointeeContestAsync(AppointeeConsentSubmitRequest req)
        {
            ///// note : ConsentStatus ==1 : consent Given ; 2=consent  decline;3 =consent  revoke , 4 = Prerequisite data available ;5 = Prerequisite data not available

            AppointeeConsentMapping newAppointeeConsentRequest = new();
            AppointeeConsentMapping? appointeeConsentMapping = await _dbContextClass.AppointeeConsentMapping.Where(x => x.AppointeeId.Equals(req.AppointeeId) && x.ActiveStatus == true).FirstOrDefaultAsync() ?? new AppointeeConsentMapping();

            var consentStatusList = await _dbContextClass.AppointeeConsentMapping.Where(x => x.AppointeeId.Equals(req.AppointeeId) && x.ActiveStatus == true).ToListAsync();
            consentStatusList.ForEach(x => x.ActiveStatus = false);
            if (req.AppointeeId != null && req.ConsentStatus != null)
            {
                newAppointeeConsentRequest.AppointeeId = req.AppointeeId;
                newAppointeeConsentRequest.ConsentStatus = req.ConsentStatus;
                newAppointeeConsentRequest.ActiveStatus = true;
                newAppointeeConsentRequest.CreatedOn = DateTime.Now;
                newAppointeeConsentRequest.CreatedBy = req.UserId;

                _dbContextClass.AppointeeConsentMapping.Add(newAppointeeConsentRequest);
                await _dbContextClass.SaveChangesAsync();
            }
            return;
        }

        public async Task PostOfflineKycStatus(OfflineAadharVarifyStatusUpdateRequest reqObj)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(reqObj.AppointeeId);
            appointeeDetails.IsOfflineKyc = reqObj.OfflineKycStatus;
            _ = await _dbContextClass.SaveChangesAsync();
        }

        public async Task PostMailTransDetails(mailTransactionRequest reqObj)
        {
            MailTransaction mailTransaction = new();
            mailTransaction.AppointeeId = reqObj.AppointeeId;
            mailTransaction.MailType = reqObj.Type;
            mailTransaction.ActiveStatus = true;
            mailTransaction.CreatedOn = DateTime.Now;
            mailTransaction.CreatedBy = reqObj.UserId;
            _dbContextClass.AppointeeMailTransaction.Add(mailTransaction);
            _ = await _dbContextClass.SaveChangesAsync();
        }

        public async Task<List<mailTransactionResponse>> GetMailTransDetails(int appointeeId, int userId)
        {
            List<mailTransactionResponse> response = new();
            var transactionListDetails = await _dbContextClass.AppointeeMailTransaction?.OrderByDescending(x => x.MailTransId)?.Where(x => x.AppointeeId.Equals(appointeeId))?.ToListAsync();
            if (transactionListDetails.Count > 0)
            {
                response = transactionListDetails?.Select(x => new mailTransactionResponse
                {
                    AppointeeId = x?.AppointeeId ?? 0,
                    UserId = x?.CreatedBy ?? 0,
                    CreatedOn = x.CreatedOn,
                }).ToList();
            }
            return response;
        }

        public async Task<AppointeeEmployementDetails> PostEmployementDetails(EmployementHistoryDetails reqObj)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(reqObj.AppointeeId);

            // Check if there are existing records for the appointee
            var res = await _dbContextClass.AppointeeEmployementDetails?
                .Where(x => x.AppointeeId == reqObj.AppointeeId
                            && x.ActiveStatus == true
                            && x.DataInfo != null
                            && x.SubTypeCode == reqObj.SubType)
                .ToListAsync();

            AppointeeEmployementDetails empHistData = new();

            if (res.Count > 0)
            {
                res.ForEach(x => x.ActiveStatus = false);
            }
            empHistData.AppointeeId = reqObj.AppointeeId;
            empHistData.TypeCode = reqObj.Provider?.Trim();
            // Convert the string to a byte array before storing it in varbinary field
            empHistData.DataInfo = string.IsNullOrEmpty(reqObj.EmpData)
                ? null
                : System.Text.Encoding.UTF8.GetBytes(reqObj.EmpData);

            empHistData.SubTypeCode = reqObj.SubType?.Trim();
            empHistData.ActiveStatus = true;
            empHistData.CreatedBy = reqObj.UserId;
            empHistData.CreatedOn = DateTime.Now;

            // Add the new entry to the context
            _dbContextClass.AppointeeEmployementDetails.Add(empHistData);

            // Save changes to the database
            _ = await _dbContextClass.SaveChangesAsync();

            return empHistData;
        }

        public async Task<AppointeeEmployementDetails> GetEmployementDetails(int appointeeId, string type)
        {
            var res = await _dbContextClass.AppointeeEmployementDetails?.Where(x => x.AppointeeId == appointeeId && x.SubTypeCode == type && x.ActiveStatus == true).ToListAsync();
            return res?.LastOrDefault();
        }

        public async Task<UserCredetialDetailsResponse> GetUserCredentialInfo(int RefAppointeeId)
        {
            var result = await (from userAuth in _dbContextClass.UserAuthentication
                                join candidate in _dbContextClass.UserMaster
                                on userAuth.UserId equals candidate.UserId
                                where candidate.RefAppointeeId == RefAppointeeId
                                && candidate.ActiveStatus == true
                                select new UserCredetialDetailsResponse
                                {
                                    AppointeeId = candidate.RefAppointeeId ?? 0,
                                    UserId = userAuth.UserId,
                                    EmailId = candidate.EmailId,
                                    userCode = candidate.UserCode,
                                    UserName = candidate.UserName,
                                    CandidateId = candidate.CandidateId,
                                    DefaultPassword = userAuth.IsDefaultPass,
                                    Password = userAuth.UserPwdTxt
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<AppointeeDetails> UpdateAppinteePensionById(AppointeeApprovePensionRequest reqObj)
        {
            var updatePension = await _dbContextClass.AppointeeDetails
         .FirstOrDefaultAsync(x => x.AppointeeId == reqObj.appointeeId && x.ActiveStatus == true && x.IsManualPassbook == true);

            if (updatePension.AppointeeDetailsId > 0)
            {
                updatePension.IsPensionApplicable = reqObj.IsPensionApplicable;
                updatePension.UpdatedBy = reqObj.userId;
                updatePension.UpdatedOn = DateTime.Now;
                _ = await _dbContextClass.SaveChangesAsync();
            }

            return updatePension;
        }

        public async Task<AppointeeDetails> VefifyAppinteeFathersNameManualById(int appointeeId, bool? isValid, string type, int userId)
        {
            var appointeeDetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId == appointeeId && x.ActiveStatus == true);

            if (appointeeDetails == null)
            {
                throw new Exception("Appointee not found.");
            }
            appointeeDetails.IsFNameVarified = isValid;
            appointeeDetails.UpdatedBy = userId;
            appointeeDetails.UpdatedOn = DateTime.Now;

            // Save the changes to the database
            await _dbContextClass.SaveChangesAsync();

            return appointeeDetails;
        }

        public async Task<AppointeeDetails> VefifyAppinteePfDetailsManualById(AppointeePfVerificationRequest reqObj)
        {
            var appointeeDetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId == reqObj.AppointeeId && x.ActiveStatus == true);

            if (appointeeDetails == null)
            {
                throw new Exception("Appointee not found.");
            }

            appointeeDetails.IsUanVarified = reqObj.IsValid;
            appointeeDetails.IsPensionApplicable = reqObj.IsPensionApplicable;
            appointeeDetails.IsPensionGap = reqObj.IsPensionGapFind;
            appointeeDetails.UpdatedBy = reqObj.UserId;
            appointeeDetails.UpdatedOn = DateTime.Now;
            await _dbContextClass.SaveChangesAsync();

            return appointeeDetails;
        }

        public async Task UpdateAppinteeDocAvailibilityById(DoctypeAvailibilityUpdateRequest reqObj)
        {
            var updateDocAvailibility = await _dbContextClass.AppointeeDetails
         .FirstOrDefaultAsync(x => x.AppointeeId == reqObj.AppointeeId && x.ActiveStatus == true);

            if (updateDocAvailibility.AppointeeDetailsId > 0)
            {
                if (reqObj.Type == "PAN")
                {
                    updateDocAvailibility.HasPan = reqObj.Value;
                }
                if (reqObj.Type == "DL")
                {
                    updateDocAvailibility.HasDrivingLicense = reqObj.Value;
                }
                updateDocAvailibility.UpdatedBy = reqObj.UserId;
                updateDocAvailibility.UpdatedOn = DateTime.Now;
                _ = await _dbContextClass.SaveChangesAsync();
            }
        }
    }
}