using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PfcAPI.Migrations
{
    /// <inheritdoc />
    public partial class grace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escaltionlevel_email_mapping_escalationlevel_master_level_i~",
                schema: "admin",
                table: "escaltionlevel_email_mapping");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "workflow_state_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "workflow_state_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_name",
                schema: "master",
                table: "workflow_state_master",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<string>(
                name: "state_decs",
                schema: "master",
                table: "workflow_state_master",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                schema: "master",
                table: "workflow_state_master",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_of_flow",
                schema: "master",
                table: "workflow_state_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "workflow_state_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "workflow_state_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "workflow_state_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                schema: "master",
                table: "workflow_state_master",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "workflow_details_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "workflow_details_hist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                table: "workflow_details_hist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                table: "workflow_details_hist",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reprocess_count",
                table: "workflow_details_hist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "workflow_details_hist",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "workflow_details_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "workflow_details_hist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appvl_status_id",
                table: "workflow_details_hist",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "workflow_details_hist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "workflow_details_hist",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_taken_at",
                table: "workflow_details_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "work_flow_det_hist_id",
                table: "workflow_details_hist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "workflow_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "workflow_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                table: "workflow_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                table: "workflow_details",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reprocess_count",
                table: "workflow_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "workflow_details",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "workflow_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "workflow_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appvl_status_id",
                table: "workflow_details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "workflow_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "workflow_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_taken_at",
                table: "workflow_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "work_flow_det_id",
                table: "workflow_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appvl_status_desc",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appvl_status_code",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appvl_status_id",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "user_type_name",
                schema: "master",
                table: "user_types",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_type_desc",
                schema: "master",
                table: "user_types",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "user_types",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "user_types",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "user_types",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "user_types",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "user_types",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_type_id",
                schema: "master",
                table: "user_types",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "users_name",
                schema: "admin",
                table: "user_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_type_id",
                schema: "admin",
                table: "user_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                schema: "admin",
                table: "user_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "user_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "user_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<long>(
                name: "ref_appointee_id",
                schema: "admin",
                table: "user_master",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email_id",
                schema: "admin",
                table: "user_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                schema: "admin",
                table: "user_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "cur_status",
                schema: "admin",
                table: "user_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "contact_no",
                schema: "admin",
                table: "user_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                schema: "admin",
                table: "user_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_master",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "user_authentication_hist",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_authentication_hist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_authentication_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_authentication_hist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "token_no",
                schema: "admin",
                table: "user_authentication_hist",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                schema: "admin",
                table: "user_authentication_hist",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gip_address",
                schema: "admin",
                table: "user_authentication_hist",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "exit_time",
                schema: "admin",
                table: "user_authentication_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exit_status",
                schema: "admin",
                table: "user_authentication_hist",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "entry_time",
                schema: "admin",
                table: "user_authentication_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_authentication_hist",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_authentication_hist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "browser_name",
                schema: "admin",
                table: "user_authentication_hist",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_authentication_hist",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "autho_hist_id",
                schema: "admin",
                table: "user_authentication_hist",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "user_pwd_txt",
                schema: "admin",
                table: "user_authentication",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_pwd",
                schema: "admin",
                table: "user_authentication",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "user_authentication",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_authentication",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_authentication",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_authentication",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_authentication",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_authentication",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_authentication",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_autho_id",
                schema: "admin",
                table: "user_authentication",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "uploaded_xls_file",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "uploaded_xls_file",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_path",
                table: "uploaded_xls_file",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "uploaded_xls_file",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "uploaded_xls_file",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "uploaded_xls_file",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "uploaded_xls_file",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "uploaded_xls_file",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "uploaded_xls_file",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_name",
                schema: "master",
                table: "upload_type_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_desc",
                schema: "master",
                table: "upload_type_master",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_code",
                schema: "master",
                table: "upload_type_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "upload_type_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "upload_type_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "upload_type_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "upload_type_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "upload_type_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id",
                schema: "master",
                table: "upload_type_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id1",
                table: "upload_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id",
                table: "upload_details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_code",
                table: "upload_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_path",
                table: "upload_details",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "upload_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "upload_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mime_type",
                table: "upload_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "upload_details",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "upload_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "upload_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "upload_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "upload_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "upload_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "upload_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "upload_det_id",
                table: "upload_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "upload_appointee_counter",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "upload_appointee_counter",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "upload_appointee_counter",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "count",
                table: "upload_appointee_counter",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "upload_appointee_counter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "unprocessed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "unprocessed_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "recruitment_hr",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_to_unprocess",
                table: "unprocessed_file_data",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "people_manager",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "offer_date",
                table: "unprocessed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "unprocessed_file_data",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "unprocessed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "unprocessed_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "unprocessed_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "unprocessed_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "unprocessed_file_data",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "unprocessed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "unprocessed_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "unprocessed_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "unprocessed_file_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "unprocessed_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "unprocessed_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "unprocessed_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "unprocessed_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "unprocessed_id",
                table: "unprocessed_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "under_process_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "under_process_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "recruitment_hr",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "people_manager",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "offer_date",
                table: "under_process_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "under_process_file_data",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "under_process_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_processed",
                table: "under_process_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "under_process_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "under_process_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "under_process_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "under_process_file_data",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "under_process_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "under_process_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "under_process_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "under_process_file_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "under_process_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "under_process_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "under_process_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "under_process_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "under_process_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "underprocess_id",
                table: "under_process_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "role_user_mapping",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "role_user_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "role_user_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "role_user_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "role_user_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "role_user_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "role_user_mapping",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "role_user_map_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "role_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "role_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "roles_alias",
                schema: "master",
                table: "role_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role_name",
                schema: "master",
                table: "role_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role_desc",
                schema: "master",
                table: "role_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_company_admin",
                schema: "master",
                table: "role_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "role_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "role_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "role_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "master",
                table: "role_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "rejected_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "rejected_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reject_state",
                table: "rejected_file_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "reject_reason",
                table: "rejected_file_data",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "rejected_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "rejected_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "rejected_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "rejected_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "rejected_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "rejected_id",
                table: "rejected_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "reason_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "reason_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_type",
                schema: "master",
                table: "reason_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_remedy",
                schema: "master",
                table: "reason_master",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_info",
                schema: "master",
                table: "reason_master",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_code",
                schema: "master",
                table: "reason_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_category",
                schema: "master",
                table: "reason_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "reason_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "reason_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "reason_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reason_id",
                schema: "master",
                table: "reason_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "raw_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "raw_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "offer_date",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "raw_file_data",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "raw_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "raw_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "raw_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "raw_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "raw_file_data",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "raw_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "raw_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "raw_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "raw_file_data",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "raw_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "raw_file_data",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "raw_file_data",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "raw_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "rawfile_id",
                table: "raw_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "qualification_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "qualification_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "qualification_name",
                schema: "master",
                table: "qualification_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "qualification_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "qualification_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "qualification_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "qualification_id",
                schema: "master",
                table: "qualification_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "processed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "processed_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "processed_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "data_uploaded",
                table: "processed_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "processed_file_data",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "processed_file_data",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "processed_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "processed_file_data",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "processed_file_data",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "processed_id",
                table: "processed_file_data",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "nationility_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "nationility_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationility_name",
                schema: "master",
                table: "nationility_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nation_name",
                schema: "master",
                table: "nationility_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "nationility_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "nationility_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "nationility_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "nationility_id",
                schema: "master",
                table: "nationility_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "menu_role_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<int>(
                name: "menu_id1",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_role_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_role_mapping",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<int>(
                name: "menu_role_map_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "menu_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<int>(
                name: "parent_menu_id",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "menu_title",
                schema: "admin",
                table: "menu_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_level",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "menu_icon_url",
                schema: "admin",
                table: "menu_master",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_desc",
                schema: "admin",
                table: "menu_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_alias",
                schema: "admin",
                table: "menu_master",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_action",
                schema: "admin",
                table: "menu_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_action_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "alias",
                schema: "admin",
                table: "menu_action_master",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_action_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "action_type",
                schema: "admin",
                table: "menu_action_master",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "action_name",
                schema: "admin",
                table: "menu_action_master",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_action_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_action_mapping",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<int>(
                name: "mapping_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "marital_status_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "marital_status_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mstatus_name",
                schema: "master",
                table: "marital_status_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "marital_status_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "marital_status_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "marital_status_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "mstatus_id",
                schema: "master",
                table: "marital_status_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "generalSetup",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "generalSetup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "critical_no_days",
                schema: "admin",
                table: "generalSetup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "generalSetup",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "generalSetup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "generalSetup",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "admin",
                table: "generalSetup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "grace_period_days",
                schema: "admin",
                table: "generalSetup",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "gender_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "gender_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_name",
                schema: "master",
                table: "gender_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "gender_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "gender_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "gender_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gender_id",
                schema: "master",
                table: "gender_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id1",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "map_id",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalationlevel_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalationlevel_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_alias",
                schema: "admin",
                table: "escalationlevel_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "no_of_days",
                schema: "admin",
                table: "escalationlevel_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level_name",
                schema: "admin",
                table: "escalationlevel_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level_code",
                schema: "admin",
                table: "escalationlevel_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalationlevel_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalationlevel_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalationlevel_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escalationlevel_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalation_setup",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "setup_option",
                schema: "admin",
                table: "escalation_setup",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id1",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "email_id",
                schema: "admin",
                table: "escalation_setup",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalation_setup",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "case_id",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalation_setup",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "setup_id",
                schema: "admin",
                table: "escalation_setup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalation_case_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalation_case_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_desc",
                schema: "admin",
                table: "escalation_case_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_code",
                schema: "admin",
                table: "escalation_case_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_alias",
                schema: "admin",
                table: "escalation_case_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalation_case_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalation_case_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalation_case_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "case_id",
                schema: "admin",
                table: "escalation_case_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "disability_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "disability_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "disability_name",
                schema: "master",
                table: "disability_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "disability_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "disability_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "disability_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "disability_id",
                schema: "master",
                table: "disability_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "company",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "company",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "no_doc_upld_req",
                schema: "admin",
                table: "company",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "company",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "company",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                schema: "admin",
                table: "company",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_city",
                schema: "admin",
                table: "company",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_address",
                schema: "admin",
                table: "company",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "company",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                schema: "admin",
                table: "company",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "update_value",
                schema: "activity",
                table: "appointee_update_activity",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "update_type",
                schema: "activity",
                table: "appointee_update_activity",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "appointee_update_activity",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "appointee_update_activity",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appointee_id",
                schema: "activity",
                table: "appointee_update_activity",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "appointee_update_activity",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "appointee_update_activity",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_reason_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_reason_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "appointee_reason_details",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reason_id",
                table: "appointee_reason_details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_reason_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_reason_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "appointee_reason_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_reason_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_reason_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_reason_id",
                table: "appointee_reason_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "appointee_master",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "appointee_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "appointee_master",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "appointee_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "appointee_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "appointee_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_master",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_id_gen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_id_gen",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "seq_no",
                table: "appointee_id_gen",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<string>(
                name: "seq_desc",
                table: "appointee_id_gen",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_id_gen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_id_gen",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_id_gen",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_id",
                table: "appointee_id_gen",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "uan_number",
                table: "appointee_details",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "save_step",
                table: "appointee_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "process_status",
                table: "appointee_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "passport_validtill",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "passport_validfrom",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "passport_no",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "passport_fileno",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pan_number",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pan_name",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "origincountry",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationality",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name_from_aadhaar",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "appointee_details",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "member_name",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_uanvarified",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_trustpassbook",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_submit",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_save",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_processed",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pensionapplicable",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_passportvarified",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_panvarified",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_aadhaarvarified",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "handicape_type",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_from_aadhaar",
                table: "appointee_details",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fathers_name_from_pan",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "appointee_details",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Numeric(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "dob_from_aadhaar",
                table: "appointee_details",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_details",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "appointee_details",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "appointee_details",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "appointee_details",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_details",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_number_view",
                table: "appointee_details",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_number",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_name",
                table: "appointee_details",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_details_id",
                table: "appointee_details",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                schema: "config",
                table: "api_couter_log",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "config",
                table: "api_couter_log",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                schema: "config",
                table: "api_couter_log",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint");

            migrationBuilder.AlterColumn<string>(
                name: "api_url",
                schema: "config",
                table: "api_couter_log",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "api_type",
                schema: "config",
                table: "api_couter_log",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "api_status",
                schema: "config",
                table: "api_couter_log",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "api_name",
                schema: "config",
                table: "api_couter_log",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "config",
                table: "api_couter_log",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "Bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "activity",
                table: "activity_transaction",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "activity",
                table: "activity_transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "activity_transaction",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "activity_transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                schema: "activity",
                table: "activity_transaction",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "Bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "activity_transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "activity_transaction",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_trans_id",
                schema: "activity",
                table: "activity_transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "activity",
                table: "activity_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "activity",
                table: "activity_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "activity_master",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "activity_master",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                schema: "activity",
                table: "activity_master",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_name",
                schema: "activity",
                table: "activity_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_info",
                schema: "activity",
                table: "activity_master",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_color",
                schema: "activity",
                table: "activity_master",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_code",
                schema: "activity",
                table: "activity_master",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "activity_master",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "Boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "activity_master",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Integer")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_escaltionlevel_email_mapping_escalationlevel_master_level_id1",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                column: "level_id1",
                principalSchema: "admin",
                principalTable: "escalationlevel_master",
                principalColumn: "level_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escaltionlevel_email_mapping_escalationlevel_master_level_id1",
                schema: "admin",
                table: "escaltionlevel_email_mapping");

            migrationBuilder.DropColumn(
                name: "grace_period_days",
                schema: "admin",
                table: "generalSetup");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "workflow_state_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "workflow_state_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_name",
                schema: "master",
                table: "workflow_state_master",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "state_decs",
                schema: "master",
                table: "workflow_state_master",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                schema: "master",
                table: "workflow_state_master",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_of_flow",
                schema: "master",
                table: "workflow_state_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "workflow_state_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "workflow_state_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "workflow_state_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                schema: "master",
                table: "workflow_state_master",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "workflow_details_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "workflow_details_hist",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                table: "workflow_details_hist",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                table: "workflow_details_hist",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reprocess_count",
                table: "workflow_details_hist",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "workflow_details_hist",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "workflow_details_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "workflow_details_hist",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appvl_status_id",
                table: "workflow_details_hist",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "workflow_details_hist",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "workflow_details_hist",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_taken_at",
                table: "workflow_details_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "work_flow_det_hist_id",
                table: "workflow_details_hist",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "workflow_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "workflow_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "state_id",
                table: "workflow_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "state_alias",
                table: "workflow_details",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reprocess_count",
                table: "workflow_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "workflow_details",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "workflow_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "workflow_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appvl_status_id",
                table: "workflow_details",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "workflow_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "workflow_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_taken_at",
                table: "workflow_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "work_flow_det_id",
                table: "workflow_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appvl_status_desc",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appvl_status_code",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "VARCHAR(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appvl_status_id",
                schema: "master",
                table: "workflow_approval_status_master",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "user_type_name",
                schema: "master",
                table: "user_types",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_type_desc",
                schema: "master",
                table: "user_types",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "user_types",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "user_types",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "user_types",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "user_types",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "user_types",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_type_id",
                schema: "master",
                table: "user_types",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "users_name",
                schema: "admin",
                table: "user_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_type_id",
                schema: "admin",
                table: "user_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "user_code",
                schema: "admin",
                table: "user_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "user_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "user_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "ref_appointee_id",
                schema: "admin",
                table: "user_master",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email_id",
                schema: "admin",
                table: "user_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                schema: "admin",
                table: "user_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "cur_status",
                schema: "admin",
                table: "user_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "contact_no",
                schema: "admin",
                table: "user_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                schema: "admin",
                table: "user_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_master",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_authentication_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "token_no",
                schema: "admin",
                table: "user_authentication_hist",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                schema: "admin",
                table: "user_authentication_hist",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gip_address",
                schema: "admin",
                table: "user_authentication_hist",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "exit_time",
                schema: "admin",
                table: "user_authentication_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exit_status",
                schema: "admin",
                table: "user_authentication_hist",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "entry_time",
                schema: "admin",
                table: "user_authentication_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_authentication_hist",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "browser_name",
                schema: "admin",
                table: "user_authentication_hist",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "autho_hist_id",
                schema: "admin",
                table: "user_authentication_hist",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "user_pwd_txt",
                schema: "admin",
                table: "user_authentication",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_pwd",
                schema: "admin",
                table: "user_authentication",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "user_authentication",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "user_authentication",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "user_authentication",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "user_authentication",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "user_authentication",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "user_authentication",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "user_authentication",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_autho_id",
                schema: "admin",
                table: "user_authentication",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "uploaded_xls_file",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "uploaded_xls_file",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_path",
                table: "uploaded_xls_file",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "uploaded_xls_file",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "uploaded_xls_file",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "uploaded_xls_file",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "uploaded_xls_file",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "uploaded_xls_file",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "uploaded_xls_file",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_name",
                schema: "master",
                table: "upload_type_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_desc",
                schema: "master",
                table: "upload_type_master",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_code",
                schema: "master",
                table: "upload_type_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "upload_type_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "upload_type_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "upload_type_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "upload_type_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "upload_type_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id",
                schema: "master",
                table: "upload_type_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id1",
                table: "upload_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "upload_type_id",
                table: "upload_details",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "upload_type_code",
                table: "upload_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "upload_path",
                table: "upload_details",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "upload_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "upload_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mime_type",
                table: "upload_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "upload_details",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "upload_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "upload_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "upload_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "upload_details",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "upload_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "upload_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "upload_det_id",
                table: "upload_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "upload_appointee_counter",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "upload_appointee_counter",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "upload_appointee_counter",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "count",
                table: "upload_appointee_counter",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "upload_appointee_counter",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "unprocessed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "unprocessed_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "recruitment_hr",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_to_unprocess",
                table: "unprocessed_file_data",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "people_manager",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "offer_date",
                table: "unprocessed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "unprocessed_file_data",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "unprocessed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "unprocessed_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "unprocessed_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "unprocessed_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "unprocessed_file_data",
                type: "Numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "unprocessed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "unprocessed_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "unprocessed_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "unprocessed_file_data",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "unprocessed_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "unprocessed_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "unprocessed_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "unprocessed_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "unprocessed_id",
                table: "unprocessed_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "under_process_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "under_process_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "recruitment_hr",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "people_manager",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "offer_date",
                table: "under_process_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "under_process_file_data",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "under_process_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_processed",
                table: "under_process_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "under_process_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "under_process_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "under_process_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "under_process_file_data",
                type: "Numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "under_process_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "under_process_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "under_process_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "under_process_file_data",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "under_process_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "under_process_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "under_process_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "under_process_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "under_process_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "underprocess_id",
                table: "under_process_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "user_id1",
                schema: "admin",
                table: "role_user_mapping",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "role_user_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "role_user_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "role_user_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "role_user_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "role_user_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "role_user_mapping",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "role_user_map_id",
                schema: "admin",
                table: "role_user_mapping",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "role_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "role_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "roles_alias",
                schema: "master",
                table: "role_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role_name",
                schema: "master",
                table: "role_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role_desc",
                schema: "master",
                table: "role_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_company_admin",
                schema: "master",
                table: "role_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "role_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "role_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "role_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "master",
                table: "role_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "rejected_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "rejected_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reject_state",
                table: "rejected_file_data",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "reject_reason",
                table: "rejected_file_data",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "rejected_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "rejected_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "rejected_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "rejected_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "rejected_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "rejected_id",
                table: "rejected_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "reason_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "reason_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_type",
                schema: "master",
                table: "reason_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_remedy",
                schema: "master",
                table: "reason_master",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_info",
                schema: "master",
                table: "reason_master",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_code",
                schema: "master",
                table: "reason_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason_category",
                schema: "master",
                table: "reason_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "reason_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "reason_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "reason_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reason_id",
                schema: "master",
                table: "reason_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "raw_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "raw_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "offer_date",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "raw_file_data",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "raw_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "raw_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id1",
                table: "raw_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "raw_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "raw_file_data",
                type: "Numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "raw_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "raw_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "raw_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "raw_file_data",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "raw_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "raw_file_data",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "raw_file_data",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "raw_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "rawfile_id",
                table: "raw_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "qualification_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "qualification_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "qualification_name",
                schema: "master",
                table: "qualification_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "qualification_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "qualification_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "qualification_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "qualification_id",
                schema: "master",
                table: "qualification_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "processed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "processed_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "processed_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "data_uploaded",
                table: "processed_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "processed_file_data",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "processed_file_data",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "processed_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "processed_file_data",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "processed_file_data",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "processed_id",
                table: "processed_file_data",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "nationility_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "nationility_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationility_name",
                schema: "master",
                table: "nationility_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nation_name",
                schema: "master",
                table: "nationility_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "nationility_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "nationility_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "nationility_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "nationility_id",
                schema: "master",
                table: "nationility_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "menu_role_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id1",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "menu_id1",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_role_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "menu_role_map_id",
                schema: "admin",
                table: "menu_role_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "menu_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_no",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "parent_menu_id",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "menu_title",
                schema: "admin",
                table: "menu_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_level",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "menu_icon_url",
                schema: "admin",
                table: "menu_master",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_desc",
                schema: "admin",
                table: "menu_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_alias",
                schema: "admin",
                table: "menu_master",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "menu_action",
                schema: "admin",
                table: "menu_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "menu_action_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "alias",
                schema: "admin",
                table: "menu_action_master",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_action_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "action_type",
                schema: "admin",
                table: "menu_action_master",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "action_name",
                schema: "admin",
                table: "menu_action_master",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_action_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "menu_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "menu_action_mapping",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "action_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "mapping_id",
                schema: "admin",
                table: "menu_action_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "marital_status_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "marital_status_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mstatus_name",
                schema: "master",
                table: "marital_status_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "marital_status_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "marital_status_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "marital_status_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "mstatus_id",
                schema: "master",
                table: "marital_status_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "generalSetup",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "generalSetup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "critical_no_days",
                schema: "admin",
                table: "generalSetup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "generalSetup",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "generalSetup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "generalSetup",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "admin",
                table: "generalSetup",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "gender_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "gender_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_name",
                schema: "master",
                table: "gender_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "gender_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "gender_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "gender_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gender_id",
                schema: "master",
                table: "gender_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id1",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "map_id",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalationlevel_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalationlevel_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_alias",
                schema: "admin",
                table: "escalationlevel_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "no_of_days",
                schema: "admin",
                table: "escalationlevel_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level_name",
                schema: "admin",
                table: "escalationlevel_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level_code",
                schema: "admin",
                table: "escalationlevel_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalationlevel_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalationlevel_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalationlevel_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escalationlevel_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalation_setup",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "setup_option",
                schema: "admin",
                table: "escalation_setup",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id1",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "level_id",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "email_id",
                schema: "admin",
                table: "escalation_setup",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalation_setup",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "case_id",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalation_setup",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "setup_id",
                schema: "admin",
                table: "escalation_setup",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "escalation_case_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "escalation_case_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_desc",
                schema: "admin",
                table: "escalation_case_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_code",
                schema: "admin",
                table: "escalation_case_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "setup_alias",
                schema: "admin",
                table: "escalation_case_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "escalation_case_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "escalation_case_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "escalation_case_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "case_id",
                schema: "admin",
                table: "escalation_case_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "master",
                table: "disability_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "master",
                table: "disability_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "disability_name",
                schema: "master",
                table: "disability_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "master",
                table: "disability_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "master",
                table: "disability_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "master",
                table: "disability_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "disability_id",
                schema: "master",
                table: "disability_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "admin",
                table: "company",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "admin",
                table: "company",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "no_doc_upld_req",
                schema: "admin",
                table: "company",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "admin",
                table: "company",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "admin",
                table: "company",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                schema: "admin",
                table: "company",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_city",
                schema: "admin",
                table: "company",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_address",
                schema: "admin",
                table: "company",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "admin",
                table: "company",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                schema: "admin",
                table: "company",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "update_value",
                schema: "activity",
                table: "appointee_update_activity",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "update_type",
                schema: "activity",
                table: "appointee_update_activity",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "appointee_update_activity",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "appointee_update_activity",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "appointee_id",
                schema: "activity",
                table: "appointee_update_activity",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "appointee_update_activity",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "appointee_update_activity",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_reason_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_reason_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remarks",
                table: "appointee_reason_details",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "reason_id",
                table: "appointee_reason_details",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_reason_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_reason_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "appointee_reason_details",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_reason_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_reason_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_reason_id",
                table: "appointee_reason_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "appointee_master",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "appointee_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "file_id",
                table: "appointee_master",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "appointee_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "appointee_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "appointee_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_master",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_id_gen",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_id_gen",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "seq_no",
                table: "appointee_id_gen",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "seq_desc",
                table: "appointee_id_gen",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_id_gen",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_id_gen",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_id_gen",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "seq_id",
                table: "appointee_id_gen",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                table: "appointee_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "uan_number",
                table: "appointee_details",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "save_step",
                table: "appointee_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "process_status",
                table: "appointee_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "passport_validtill",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "passport_validfrom",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "passport_no",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "passport_fileno",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pan_number",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pan_name",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "origincountry",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationality",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name_from_aadhaar",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_no",
                table: "appointee_details",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "member_name",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level3_email",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level2_email",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "level1_email",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "joining_date",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_uanvarified",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_trustpassbook",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_submit",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_save",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_processed",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pf_verification_req",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_pensionapplicable",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_passportvarified",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_panvarified",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_aadhaarvarified",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "handicape_type",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender_from_aadhaar",
                table: "appointee_details",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fathers_name_from_pan",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "epf_wages",
                table: "appointee_details",
                type: "Numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "dob_from_aadhaar",
                table: "appointee_details",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_of_birth",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "appointee_details",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "appointee_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "company_name",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id1",
                table: "appointee_details",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "appointee_details",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "candidate_id",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "appointee_name",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id1",
                table: "appointee_details",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                table: "appointee_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "appointee_email",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                table: "appointee_details",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_number_view",
                table: "appointee_details",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_number",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadhaar_name",
                table: "appointee_details",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_details_id",
                table: "appointee_details",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                schema: "config",
                table: "api_couter_log",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "config",
                table: "api_couter_log",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                schema: "config",
                table: "api_couter_log",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "api_url",
                schema: "config",
                table: "api_couter_log",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "api_type",
                schema: "config",
                table: "api_couter_log",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "api_status",
                schema: "config",
                table: "api_couter_log",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "api_name",
                schema: "config",
                table: "api_couter_log",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "config",
                table: "api_couter_log",
                type: "Bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "activity",
                table: "activity_transaction",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "activity",
                table: "activity_transaction",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "activity_transaction",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "activity_transaction",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "appointee_id",
                schema: "activity",
                table: "activity_transaction",
                type: "Bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "activity_transaction",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "activity_transaction",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_trans_id",
                schema: "activity",
                table: "activity_transaction",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_on",
                schema: "activity",
                table: "activity_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "updated_by",
                schema: "activity",
                table: "activity_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "activity",
                table: "activity_master",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "activity",
                table: "activity_master",
                type: "Integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                schema: "activity",
                table: "activity_master",
                type: "VARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_name",
                schema: "activity",
                table: "activity_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_info",
                schema: "activity",
                table: "activity_master",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_color",
                schema: "activity",
                table: "activity_master",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_code",
                schema: "activity",
                table: "activity_master",
                type: "VARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "active_status",
                schema: "activity",
                table: "activity_master",
                type: "Boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "activity_id",
                schema: "activity",
                table: "activity_master",
                type: "Integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_escaltionlevel_email_mapping_escalationlevel_master_level_i~",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                column: "level_id1",
                principalSchema: "admin",
                principalTable: "escalationlevel_master",
                principalColumn: "level_id");
        }
    }
}
