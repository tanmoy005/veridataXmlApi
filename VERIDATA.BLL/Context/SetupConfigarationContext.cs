using VERIDATA.BLL.Interfaces;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Admin;

namespace VERIDATA.BLL.Context
{
    public class SetupConfigarationContext : ISetupConfigarationContext
    {
        private readonly IMasterDalContext _masterDalContext;

        public SetupConfigarationContext(IMasterDalContext masterDalContext)
        {
            _masterDalContext = masterDalContext;
        }

        public async Task<GeneralSetupDetailsResponse> gettSetupData()
        {
            GeneralSetupDetailsResponse res = new();

            List<EmailEscalationLevelDetails> levelres = new();


            List<EscalationLevelMasterDataResponse> leveldatares = await _masterDalContext.GetEscalationLevelMasterData();


            List<IGrouping<int, EscalationLevelMasterDataResponse>> levelGroupdata = leveldatares.GroupBy(x => x.LevelId).ToList();

            foreach (var item in levelGroupdata.Select((value, index) => new { Value = value, Index = index }))
            {
                IGrouping<int, EscalationLevelMasterDataResponse>? x = item.Value;

                List<string?> _data = x.Select(r => r.Emailaddress).ToList();

                EmailEscalationLevelDetails leveldata = new()
                {
                    LevelId = x.Key,
                    LevelCode = x.FirstOrDefault()?.LevelCode ?? string.Empty,
                    LevelName = x.FirstOrDefault()?.LevelName ?? string.Empty,
                    SetupAlias = x.FirstOrDefault()?.SetupAlias ?? string.Empty,
                    NoOfDays = x.FirstOrDefault()?.NoOfDays ?? 0,
                    Emailaddress = _data ?? new List<string>(),

                };
                levelres.Add(leveldata);
            }

            List<EmailEscalationSetupDetails> levelcasesetupres = new();
            List<EscalationLevelCaseDetails> casedatares = await _masterDalContext.GetEscalationCaseMasterDetails();
            List<IGrouping<int, EscalationLevelCaseDetails>> caseLevelGroupdata = casedatares.GroupBy(x => x.LevelId).ToList();

            foreach (var item in caseLevelGroupdata.Select((value, index) => new { Value = value, Index = index }))
            {
                IGrouping<int, EscalationLevelCaseDetails> x = item.Value;

                List<CaseOptionDetails>? casedata = x.Select(r => new CaseOptionDetails
                {
                    SetupCaseId = r.CaseId,
                    SetupCaseCode = r.SetupCode,
                    SetupAlias = r.SetupAlias,
                    SetupCaseOption = r.SetupStatus ?? false,
                    CaseEmailAddress = r.EmailId ?? string.Empty,
                })?.OrderBy(x => x.SetupCaseId).ToList();

                EmailEscalationSetupDetails levelcasedata = new()
                {
                    LevelId = x.Key,
                    LevelCode = x?.FirstOrDefault().LevelCode,
                    SetupCaseDetails = casedata,
                };

                levelcasesetupres.Add(levelcasedata);
            }

            List<CaseSetupDetails> CaseListDetails = await _masterDalContext.GetCaseDetails();
            GeneralSetup generalsetupdata = await _masterDalContext.GetGeneralSetupData();

            res.CriticalDays = generalsetupdata.CriticalNoOfDays ?? 0;
            res.GracePeriod = generalsetupdata.GracePeriod ?? 0;
            res.EmailEscalationLevelDetails = levelres.OrderBy(x => x.LevelCode).ToList();
            res.EmailEscalationSetupDetails = levelcasesetupres.OrderBy(x => x.LevelCode).ToList();
            res.CaseSetupDetails = CaseListDetails;
            return res;
        }

        public async Task PostSetupData(GeneralSetupSubmitRequest setupRequest)
        {
            await _masterDalContext.PostSetupData(setupRequest);

        }
    }
}
