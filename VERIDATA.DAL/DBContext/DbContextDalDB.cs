using Microsoft.EntityFrameworkCore;
using VERIDATA.Model.Table.Activity;
using VERIDATA.Model.Table.Admin;
using VERIDATA.Model.Table.Config;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;

namespace VERIDATA.DAL.DBContext
{
    public class DbContextDalDB : DbContext
    {
        public DbContextDalDB(DbContextOptions<DbContextDalDB> options) : base(options)
        { }

        #region admin

        public DbSet<UserMaster> UserMaster { get; set; }

        public DbSet<UserAuthentication> UserAuthentication { get; set; }
        public DbSet<UserAuthenticationHist> UserAuthenticationHist { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<MenuRoleMapping> MenuRoleMapping { get; set; }
        public DbSet<RoleUserMapping> RoleUserMapping { get; set; }
        public DbSet<ActionMaster> ActionMaster { get; set; }
        public DbSet<MenuActionMapping> MenuActionMapping { get; set; }
        public DbSet<GeneralSetup> GeneralSetup { get; set; }
        public DbSet<AppointeeUpdateLog> AppointeeUpdateLog { get; set; }
        public DbSet<EscalationLevelMaster> EscalationLevelMaster { get; set; }
        public DbSet<EmailEscalationCaseMaster> EmailEscalationCaseMaster { get; set; }
        public DbSet<EscalationLevelEmailMapping> EscalationLevelEmailMapping { get; set; }
        public DbSet<EmailEscalationSetupMapping> EmailEscalationSetupMapping { get; set; }
        public DbSet<CompanyInfo> CompanyDetails { get; set; }

        #endregion admin

        #region Public

        public DbSet<AppointeeIdGen> AppointeeIdGen { get; set; }
        public DbSet<UploadedXSLfile> UploadedXSLfile { get; set; }

        ////  public DbSet<AppointeeUsers> AppointeeUsers { get; set; }
        public DbSet<RawFileData> RawFileData { get; set; }

        public DbSet<RawFileHistoryData> RawFileHistoryData { get; set; }
        public DbSet<UnderProcessFileData> UnderProcessFileData { get; set; }
        public DbSet<UnProcessedFileData> UnProcessedFileData { get; set; }

        ////   public DbSet<AppointeeProcess> AppointeeProcess { get; set; }
        public DbSet<AppointeeMaster> AppointeeMaster { get; set; }

        public DbSet<AppointeeDetails> AppointeeDetails { get; set; }
        public DbSet<AppointeeUploadDetails> AppointeeUploadDetails { get; set; }
        public DbSet<WorkFlowDetails> WorkFlowDetails { get; set; }
        public DbSet<WorkFlowDetailsHist> WorkFlowDetailsHist { get; set; }
        public DbSet<ProcessedFileData> ProcessedFileData { get; set; }
        public DbSet<RejectedFileData> RejectedFileData { get; set; }
        public DbSet<AppointeeReasonMappingData> AppointeeReasonMappingData { get; set; }
        public DbSet<AppointeeConsentMapping> AppointeeConsentMapping { get; set; }
        public DbSet<AppointeeEmployementDetails> AppointeeEmployementDetails { get; set; }

        #endregion Public

        #region Master

        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<RoleMaster> RoleMaster { get; set; }
        public DbSet<DisabilityMaster> DisabilityMaster { get; set; }
        public DbSet<GenderMaster> GenderMaster { get; set; }
        public DbSet<MaritalStatusMaster> MaratialStatusMaster { get; set; }
        public DbSet<NationilityMaster> NationilityMaster { get; set; }
        public DbSet<WorkFlowStateMaster> WorkFlowStateMaster { get; set; }
        public DbSet<WorkflowApprovalStatusMaster> WorkflowApprovalStatusMaster { get; set; }
        public DbSet<UploadTypeMaster> UploadTypeMaster { get; set; }
        public DbSet<QualificationMaster> QualificationMaster { get; set; }
        public DbSet<ReasonMaser> ReasonMaser { get; set; }
        public DbSet<ApiTypeMaster> ApiTypeMaster { get; set; }
        public DbSet<ApiTypeMappingConfig> ApiTypeMapping { get; set; }
        public DbSet<FaqMaster> FaqMaster { get; set; }

        #endregion Master

        #region Configuration

        public DbSet<Logs> Logs { get; set; }
        public DbSet<CustomError> ErrorLogs { get; set; }
        public DbSet<ApiLogs> ApiLogs { get; set; }

        #endregion Configuration

        #region Acitivity

        public DbSet<ActivityMaster> ActivityMaster { get; set; }
        public DbSet<ActivityTransaction> ActivityTransaction { get; set; }
        public DbSet<ApiCounter> ApiCounter { get; set; }
        public DbSet<UploadAppointeeCounter> UploadAppointeeCounter { get; set; }
        public DbSet<AppointeeDetailsUpdateActivity> AppointeeDetailsUpdateActivity { get; set; }
        public DbSet<MailTransaction> AppointeeMailTransaction { get; set; }

        #endregion Acitivity
    }
}