using Microsoft.EntityFrameworkCore;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.DAL.utility;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
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
            AppointeeDetails _appointeedetails = await _dbContextClass.AppointeeDetails.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true) ;
            return _appointeedetails;
        }
        public async Task<UnderProcessFileData> GetUnderProcessAppinteeDetailsById(int appointeeId)
        {
            UnderProcessFileData? data = await _dbContextClass.UnderProcessFileData.FirstOrDefaultAsync(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true) ?? new UnderProcessFileData();
            return data;
        }
        public async Task<List<AppointeeUploadDetails>> GetAppinteeUploadDetails(int appointeeId)
        {
            List<AppointeeUploadDetails> _uploadDetails = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) && x.ActiveStatus == true).ToListAsync();
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
                _appointeedetails.IsPensionApplicable = validationReq?.IsPensionApplicable;
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
                    _appointeedetails.IsUanVarified = validationReq.Status;
                    _appointeedetails.UANNumber = validationReq?.UanNumber;
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
        public async Task UpdateAppointeeSubmit(int AppointeeId, bool TrustPassbookAvailable, bool IsSubmit)
        {
            AppointeeDetails appointeeDetails = await GetAppinteeDetailsById(AppointeeId);
            appointeeDetails.IsSubmit = IsSubmit;
            appointeeDetails.IsTrustPassbook = TrustPassbookAvailable;
            _ = await _dbContextClass.SaveChangesAsync();
        }
        public async Task<List<GetRemarksResponse>> GetRemarks(int appointeeId)
        {
            List<GetRemarksResponse> remarks = new();
            var querydata = from r in _dbContextClass.ReasonMaser
                            join a in _dbContextClass.AppointeeReasonMappingData
                                on r.ReasonId equals a.ReasonId
                            where r.ActiveStatus == true && a.AppointeeId == appointeeId && a.ActiveStatus == true
                            select new { a.Remarks, r.ReasonId, r.ReasonCode, r.ReasonCategory };

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
    }
}
