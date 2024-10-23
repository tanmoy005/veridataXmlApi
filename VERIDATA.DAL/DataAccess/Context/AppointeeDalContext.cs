using Microsoft.EntityFrameworkCore;
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
            // List<AppointeeUploadDetails> _uploadDetails = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true).ToListAsync();
            List<AppointeeUploadDetails> _uploadDetails = await _dbContextClass.AppointeeUploadDetails
           .Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true)
           .ToListAsync();

            return _uploadDetails;

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
                _appointeedetails.IsPensionApplicable = validationReq.uanData?.IsPensionApplicable;
                if (validationReq.Type == RemarksType.Adhaar)
                {
                    _appointeedetails.IsAadhaarVarified = validationReq.Status;
                    _appointeedetails.AadhaarName = validationReq?.aadharData?.AadhaarName;
                    //_appointeedetails.AadhaarNumber = validationReq?.aadharData?.AadhaarNumber;
                    _appointeedetails.AadhaarNumber = validationReq?.aadharData?.AadhaarNumberView;
                    _appointeedetails.AadhaarNumberView = validationReq?.aadharData?.AadhaarNumberView;
                    _appointeedetails.NameFromAadhaar = validationReq?.aadharData?.NameFromAadhaar;
                    _appointeedetails.GenderFromAadhaar = validationReq?.aadharData?.GenderFromAadhaar;
                    _appointeedetails.DobFromAadhaar = validationReq?.aadharData?.DobFromAadhaar;

                }
                if (validationReq.Type == RemarksType.UAN)
                {
                    //var uanVerifiedStatus = _appointeedetails.IsUanVarified;
                    _appointeedetails.IsUanVarified = validationReq.Status;
                    //_appointeedetails.IsUanAvailable = validationReq.Status;
                    _appointeedetails.IsPassbookFetch = validationReq.uanData?.IsPassbookFetch;
                    _appointeedetails.UANNumber = validationReq.uanData?.UanNumber;
                    //_appointeedetails.IsEmployementVarified = validationReq.uanData?.IsEmployementVarified;
                }
                if (validationReq.Type == RemarksType.Passport)
                {
                    _appointeedetails.IsPasssportVarified = validationReq.Status;
                    _appointeedetails.PassportFileNo = validationReq.PassportFileNo;
                }
                if (validationReq.Type == RemarksType.Pan)
                {
                    _appointeedetails.IsPanVarified = validationReq.Status;
                    _appointeedetails.PANNumber = validationReq?.panData?.PanNumber;
                    _appointeedetails.PANName = validationReq?.panData?.PanName;
                    _appointeedetails.FathersNameFromPan = validationReq?.panData?.PanFatherName;
                }

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
                                      select new { rm.ReasonName, rm.ReasonCode, rm.ReasonId, r.Inputdata, r.Fetcheddata, r.Remarks };
                var ResonDetails = reasonListquery.ToList();
                // var ResonDetails = AllResonDetails.Where(x => ReasonCodeList.Contains(x.ReasonCode)).ToList();
                if (ResonDetails?.Count > 0)
                {

                    //var CurrReasonIdList = ResonDetails.Select(x => x.ReasonId).ToList();
                    List<int> AllReasonIdList = AllResonDetails.Select(x => x.ReasonId).ToList();
                    List<AppointeeReasonMappingData> PrevReason = AllPrevReason.Where(x => AllReasonIdList.Contains(x.ReasonId)).ToList();
                    List<AppointeeReasonMappingData>? _resaonList = ResonDetails?.Select(x => new AppointeeReasonMappingData
                    {
                        AppointeeId = AppointeeId,
                        ReasonId = x.ReasonId,
                        Remarks = x.ReasonCode != ReasonCode.OTHER ? CommonUtility.ParseMessage(x.ReasonName, new { x.Inputdata, x.Fetcheddata }) : x.Remarks,
                        CreatedBy = UserId,
                        ActiveStatus = true,
                        CreatedOn = DateTime.Now,
                    }).ToList();
                    await _dbContextClass.AppointeeReasonMappingData.AddRangeAsync(_resaonList);

                    if (PrevReason.Count > 0)
                    {
                        PrevReason.ForEach(x => x.ActiveStatus = false);
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
                        AllPrevReason.Where(x => allreasonIdByType.Contains(x.ReasonId)).ToList().ForEach(x => x.ActiveStatus = false);
                    }
                }
                _ = await _dbContextClass.SaveChangesAsync();
            }
            return AllRemarks;
        }
        public async Task UpdateRemarksStatusByType(int AppointeeId, string Type, int UserId)
        {
            string AllRemarks = string.Empty;

            List<ReasonMaser> AllResonDetails = await GetAllRemarksByType(Type);
            var AllReasonId = AllResonDetails?.Select(x => x.ReasonId)?.ToList();
            List<AppointeeReasonMappingData> AllPrevReason = await _dbContextClass.AppointeeReasonMappingData.Where(x => x.AppointeeId.Equals(AppointeeId) && x.ActiveStatus == true && AllReasonId.Contains(x.ReasonId)).ToListAsync();

            // var ResonDetails = AllResonDetails.Where(x => ReasonCodeList.Contains(x.ReasonCode)).ToList();
            if (AllPrevReason?.Count > 0)
            {
                AllPrevReason?.ForEach(x => { x.ActiveStatus = false; x.UpdatedOn = DateTime.Now; x.UpdatedBy = UserId; });

                await _dbContextClass.SaveChangesAsync();
            }
        }
        public async Task uploadFilesNUpdatePrevfiles(AppointeeUploadDetails uploadDetails, AppointeeUploadDetails prevDocDetails, int userId)
        {
            //using var transaction = _dbContextClass.Database.BeginTransaction();
            //try
            //{
            await uploadfiles(uploadDetails);
            await RemovePrevfiles(prevDocDetails, userId);
            //    transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        private async Task uploadfiles(AppointeeUploadDetails uploadDetails)
        {
            if (uploadDetails != null)
            {
                _ = _dbContextClass.AppointeeUploadDetails.Add(uploadDetails);
                _ = await _dbContextClass.SaveChangesAsync();
            }

        }
        private async Task RemovePrevfiles(AppointeeUploadDetails prevDocDetails, int userId)
        {

            if (prevDocDetails != null)
            {
                prevDocDetails.ActiveStatus = false;
                prevDocDetails.UpdatedOn = DateTime.Now;
                prevDocDetails.UpdatedBy = userId;
                _ = await _dbContextClass.SaveChangesAsync();
            }

        }
        public async Task UpdateAppointeeSubmit(int AppointeeId, bool IsSubmit, bool? IsManualPassbookUploaded)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(AppointeeId);
            appointeeDetails.IsSubmit = IsSubmit;
            appointeeDetails.IsManualPassbook = IsManualPassbookUploaded;
            //appointeeDetails.IsTrustPension = TrustPensionAvailable;
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
    }
}
