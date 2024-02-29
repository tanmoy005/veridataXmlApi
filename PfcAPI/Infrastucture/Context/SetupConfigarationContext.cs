using Microsoft.EntityFrameworkCore;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Model.Company;
using PfcAPI.Model.RequestModel;
using PfcAPI.Model.ResponseModel;

namespace PfcAPI.Infrastucture.Context
{
    public class SetupConfigarationContext : ISetupConfigarationContext
    {
        private readonly DbContextDB _dbContextClass;

        public SetupConfigarationContext(DbContextDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }

        public async Task<GeneralSetupDetailsResponse> gettSetupData()
        {
            var res = new GeneralSetupDetailsResponse();

            var levelres = new List<EmailEscalationLevelDetails>();

            var getlevelquerydata = from a in _dbContextClass.EscalationLevelMaster
                                    join b in _dbContextClass.EscalationLevelEmailMapping
                                      on a.LevelId equals b.LevelId
                                    where a.ActiveStatus == true & b.ActiveStatus == true
                                    select new { a.LevelId, a.LevelCode, a.LevelName, a.SetupAlias, a.NoOfDays, b.Email };


            var leveldatares = await getlevelquerydata.ToListAsync().ConfigureAwait(false);

            var levelGroupdata = leveldatares.GroupBy(x => x.LevelId).ToList();

            foreach (var item in levelGroupdata.Select((value, index) => new { Value = value, Index = index }))
            {
                var x = item.Value;

                var _data = x.Select(r => r.Email).ToList();

                var leveldata = new EmailEscalationLevelDetails
                {
                    LevelId = x.Key,
                    LevelCode = x?.FirstOrDefault().LevelCode,
                    LevelName = x?.FirstOrDefault().LevelName,
                    SetupAlias = x?.FirstOrDefault().SetupAlias,
                    NoOfDays = x?.FirstOrDefault().NoOfDays ?? 0,
                    Emailaddress = _data ?? new List<string>(),

                };
                levelres.Add(leveldata);
            }

            var levelcasesetupres = new List<EmailEscalationSetupDetails>();

            var getcasesetupquerydata = from a in _dbContextClass.EmailEscalationSetupMapping
                                        join b in _dbContextClass.EmailEscalationCaseMaster
                                            on a.CaseId equals b.CaseId
                                        join c in _dbContextClass.EscalationLevelMaster
                                             on a.LevelId equals c.LevelId
                                        where a.ActiveStatus == true & b.ActiveStatus == true & c.ActiveStatus == true
                                        select new { a.CaseId, b.SetupDesc, b.SetupCode, a.SetupStatus, b.SetupAlias, c.LevelId, c.LevelCode, a.EmailId };


            var casedatares = await getcasesetupquerydata.ToListAsync().ConfigureAwait(false);
            var caseLevelGroupdata = casedatares.GroupBy(x => x.LevelId).ToList();

            foreach (var item in caseLevelGroupdata.Select((value, index) => new { Value = value, Index = index }))
            {
                var x = item.Value;

                var casedata = x.Select(r => new CaseOptionDetails
                {
                    SetupCaseId = r.CaseId,
                    SetupCaseCode = r.SetupCode,
                    SetupAlias = r.SetupAlias,
                    SetupCaseOption = r.SetupStatus ?? false,
                    CaseEmailAddress = r.EmailId ?? string.Empty,
                })?.OrderBy(x => x.SetupCaseId).ToList();

                var levelcasedata = new EmailEscalationSetupDetails
                {
                    LevelId = x.Key,
                    LevelCode = x?.FirstOrDefault().LevelCode,
                    SetupCaseDetails = casedata,
                };

                levelcasesetupres.Add(levelcasedata);


            }

            var CaseListDetails = await _dbContextClass.EmailEscalationCaseMaster?.Where(m => m.ActiveStatus == true)?.Select(x => new CaseSetupDetails
            {
                SetupCaseId = x.CaseId,
                SetupCaseDesc = x.SetupDesc,
                SetupAlias = x.SetupAlias,

            })?.OrderBy(x => x.SetupCaseId)?.ToListAsync();


            var generalsetupdata = await _dbContextClass.GeneralSetup.FirstOrDefaultAsync(m => m.ActiveStatus == true);
            res.CriticalDays = generalsetupdata.CriticalNoOfDays ?? 0;
            res.GracePeriod = generalsetupdata.GracePeriod ?? 0;
            res.EmailEscalationLevelDetails = levelres.OrderBy(x => x.LevelCode).ToList();
            res.EmailEscalationSetupDetails = levelcasesetupres.OrderBy(x => x.LevelCode).ToList();
            res.CaseSetupDetails = CaseListDetails;
            return res;
        }

        public async Task PostSetupData(GeneralSetupSubmitRequest setupRequest)
        {
            var _EscalatedEmailSetup = setupRequest.EmailEscalationLevel;
            var _EmailEscalationSetup = setupRequest.EmailEscalationSetup;
            var newMailsetup = new List<EscalationLevelEmailMapping>();
            if (_EscalatedEmailSetup != null)
            {
                var levelMasterData = await _dbContextClass.EscalationLevelMaster?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                var levelEmailMappingData = await _dbContextClass.EscalationLevelEmailMapping?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                _EscalatedEmailSetup.ForEach(x =>
                {
                    if (levelMasterData != null)
                    {
                        levelMasterData.Where(y => y.LevelId.Equals(x.LevelId)).FirstOrDefault(a =>
                        {
                            if (a.NoOfDays != x.NoOfDays)
                            {
                                a.NoOfDays = x.NoOfDays;
                                a.UpdatedBy = setupRequest.UserId;
                                a.UpdatedOn = DateTime.Now;
                            }
                            return true;
                        });
                    }

                    //Emailaddresss  update
                    if (levelEmailMappingData.Count > 0)
                    {
                        levelEmailMappingData.Where(y => y.LevelId.Equals(x.LevelId)).ToList()?.ForEach(y =>
                            {
                                if (!x.Emailaddress.Contains(y.Email))
                                {
                                    y.ActiveStatus = false;
                                    y.UpdatedBy = setupRequest.UserId;
                                    y.UpdatedOn = DateTime.Now;

                                    var _currNewMail = x.Emailaddress.Where(m => m != y.Email).ToList();
                                    var currMailsetup = _currNewMail.Select(a => new EscalationLevelEmailMapping
                                    {
                                        Email = a,
                                        LevelId = x.LevelId,
                                        CreatedBy = setupRequest.UserId,
                                        CreatedOn = DateTime.Now,
                                        ActiveStatus = true,
                                    }).ToList();

                                    //var currMailsetup = new EscalationLevelEmailMapping
                                    //{
                                    //    Email = _currNewMail,
                                    //    LevelId = x.LevelId,
                                    //    CreatedBy = setupRequest.UserId,
                                    //    CreatedOn = DateTime.Now,
                                    //    ActiveStatus = true,
                                    //};
                                    if (currMailsetup.Count > 0)
                                    {
                                        newMailsetup.AddRange(currMailsetup);
                                    }
                                }
                            });
                    }

                });

                /*     var levelMailData = _dbContextClass.EscalationLevelEmailMapping?.Where(x => _EscalatedEmailSetup.x.LevelId)*/

                //case setup  update
                var EmailEscalationList = new List<EmailEscalationSetupMapping>();
                var levelCaseSetupData = await _dbContextClass.EmailEscalationSetupMapping?.Where(x => x.ActiveStatus == true)?.ToListAsync();
                _EmailEscalationSetup.ForEach(x =>
                            {
                                levelCaseSetupData.Where(y => y.LevelId.Equals(x.LevelId) & y.CaseId.Equals(x.CaseId)).FirstOrDefault(a =>
                                {
                                    if (a != null && (a.SetupStatus != x.CaseOption || a?.EmailId?.ToUpper()?.Trim() != x?.CaseEmail?.ToUpper()?.Trim()))
                                    {
                                        a.ActiveStatus = false;
                                        a.UpdatedBy = setupRequest.UserId;
                                        a.UpdatedOn = DateTime.Now;

                                        var currMailsetup = new EmailEscalationSetupMapping
                                        {
                                            CaseId = x.CaseId,
                                            LevelId = x.LevelId,
                                            SetupStatus = x.CaseOption,
                                            EmailId = x.CaseEmail,
                                            CreatedBy = setupRequest.UserId,
                                            CreatedOn = DateTime.Now,
                                            ActiveStatus = true,
                                        };
                                        EmailEscalationList.Add(currMailsetup);
                                    }
                                    return true;
                                });

                            });

                var generalsetupData = await _dbContextClass.GeneralSetup?.Where(x => x.ActiveStatus == true)?.FirstOrDefaultAsync();

                if (generalsetupData.CriticalNoOfDays != null || generalsetupData.GracePeriod != null)
                {
                    if (generalsetupData?.CriticalNoOfDays != setupRequest.CriticalDays)
                    {
                        generalsetupData.CriticalNoOfDays = setupRequest.CriticalDays;
                    }
                    if (generalsetupData?.GracePeriod != setupRequest.GracePeriod)
                    {
                        generalsetupData.GracePeriod = setupRequest.GracePeriod;
                    }
                    generalsetupData.UpdatedBy = setupRequest.UserId;
                    generalsetupData.UpdatedOn = DateTime.Now;

                }


                if (newMailsetup.Count > 0)
                {
                    //await _dbContextClass.SaveChangesAsync();
                    _dbContextClass.EscalationLevelEmailMapping.AddRange(newMailsetup);
                }
                if (EmailEscalationList.Count > 0)
                {
                    // await _dbContextClass.SaveChangesAsync();
                    _dbContextClass.EmailEscalationSetupMapping.AddRange(EmailEscalationList);

                }
                await _dbContextClass.SaveChangesAsync();
            }

        }
    }
}
