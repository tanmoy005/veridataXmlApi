using Microsoft.EntityFrameworkCore;
//using VERIDATA.Model.Activity;
using PfcAPI.Model.Company;
using PfcAPI.Model.Maintainance;
using PfcAPI.Model.Table.Master;
using PfcAPI.Model.MenuRoleUser;
//using VERIDATA.Model.Table.Config;

namespace PfcAPI.Infrastucture.DBContext
{
    public class DbContextDB : DbContext
    {
        public DbContextDB(DbContextOptions<DbContextDB> options) : base(options)
        { }
        #region Users
        //  public DbSet<UserMaster> UserMaster { get; set; }
        //public DbSet<UserTypes> UserTypes { get; set; }
        //public DbSet<UserAuthentication> UserAuthentication { get; set; }
        //public DbSet<UserAuthenticationHist> UserAuthenticationHist { get; set; }
        #endregion
        #region RoleMenuPermission
        // public DbSet<MenuMaster> MenuMaster { get; set; }
        //public DbSet<RoleMaster> RoleMaster { get; set; }
        // public DbSet<MenuRoleMapping> MenuRoleMapping { get; set; }
        // public DbSet<RoleUserMapping> RoleUserMapping { get; set; }
        //public DbSet<ActionMaster> ActionMaster { get; set; }
        //public DbSet<MenuActionMapping> MenuActionMapping { get; set; }

        #endregion
        #region CompanyUsers
        // public DbSet<CompanyInfo> CompanyDetails { get; set; }
        //public DbSet<UploadedXSLfile> UploadedXSLfile { get; set; }
        #endregion
        #region Appointee
        //  public DbSet<AppointeeUsers> AppointeeUsers { get; set; }
        //public DbSet<RawFileData> RawFileData { get; set; }
        //public DbSet<RawFileHistoryData> RawFileHistoryData { get; set; }
        //public DbSet<UnderProcessFileData> UnderProcessFileData { get; set; }
        //public DbSet<UnProcessedFileData> UnProcessedFileData { get; set; }
        //   public DbSet<AppointeeProcess> AppointeeProcess { get; set; }
        //public DbSet<AppointeeMaster> AppointeeMaster { get; set; }
        //public DbSet<AppointeeDetails> AppointeeDetails { get; set; }
        //public DbSet<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }
        //public DbSet<WorkFlowDetails> WorkFlowDetails { get; set; }
        //public DbSet<WorkFlowDetailsHist> WorkFlowDetailsHist { get; set; }
        //public DbSet<ProcessedFileData> ProcessedFileData { get; set; }
        //public DbSet<RejectedFileData> RejectedFileData { get; set; }
        //public DbSet<AppointeeReasonMappingData> AppointeeReasonMappingData { get; set; }


        //public DbSet<ApointeeDocumentData> ApointeeDocumentData { get; set; }
        #endregion
        #region Master
        // public DbSet<AppointeeIdGen> AppointeeIdGen { get; set; }
        //public DbSet<DisabilityMaster> DisabilityMaster { get; set; }
        //public DbSet<GenderMaster> GenderMaster { get; set; }
        //public DbSet<MaritalStatusMaster> MaratialStatusMaster { get; set; }
        //public DbSet<NationilityMaster> NationilityMaster { get; set; }
        //public DbSet<WorkFlowStateMaster> WorkFlowStateMaster { get; set; }
        //public DbSet<WorkflowApprovalStatusMaster> WorkflowApprovalStatusMaster { get; set; }
        //public DbSet<UploadTypeMaster> UploadTypeMaster { get; set; }
        ////public DbSet<CompanyWiseUpldMaster> CompanyWiseUpldMaster { get; set; }
        //public DbSet<QualificationMaster> QualificationMaster { get; set; }
        //public DbSet<ReasonMaser> ReasonMaser { get; set; }
        #endregion

        #region Configuration
        //public DbSet<EscalationLevelMaster> EscalationLevelMaster { get; set; }
        //public DbSet<EmailEscalationCaseMaster> EmailEscalationCaseMaster { get; set; }
        //public DbSet<EscalationLevelEmailMapping> EscalationLevelEmailMapping { get; set; }
        //public DbSet<EmailEscalationSetupMapping> EmailEscalationSetupMapping { get; set; }
        // public DbSet<GeneralSetup> GeneralSetup { get; set; }
        #endregion
        #region Logs
        //public DbSet<Logs> Logs { get; set; }
        //public DbSet<CustomError> ErrorLogs { get; set; }
        //public DbSet<ApiLogs> ApiLogs { get; set; }
        #endregion
        //#region Acitivity
        //public DbSet<ActivityMaster> ActivityMaster { get; set; }
        //public DbSet<ActivityTransaction> ActivityTransaction { get; set; }
        //public DbSet<ApiCounter> ApiCounter { get; set; }
        //public DbSet<AppointeeUpdateLog> AppointeeUpdateLog { get; set; }
        //public DbSet<UploadAppointeeCounter> UploadAppointeeCounter { get; set; }
        //public DbSet<AppointeeDetailsUpdateActivity> AppointeeDetailsUpdateActivity { get; set; }
        //#endregion

    }
}
