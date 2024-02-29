using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VERIDATA.DAL.Migrations
{
    /// <inheritdoc />
    public partial class constable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "activity");

            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.EnsureSchema(
                name: "master");

            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "activity_master",
                schema: "activity",
                columns: table => new
                {
                    activity_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    activity_code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    activity_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    activity_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    activity_info = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    activity_color = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_master", x => x.activity_id);
                });

            migrationBuilder.CreateTable(
                name: "activity_transaction",
                schema: "activity",
                columns: table => new
                {
                    activity_trans_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    activity_id = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_transaction", x => x.activity_trans_id);
                });

            migrationBuilder.CreateTable(
                name: "api_couter_log",
                schema: "config",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    api_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    api_url = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    api_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    api_status = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_couter_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_logs",
                schema: "config",
                columns: table => new
                {
                    log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointeeId = table.Column<long>(type: "bigint", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    method_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    method_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_logs", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "api_type_mapping",
                schema: "master",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    api_type_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    api_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    api_base_url = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    api_url = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_type_mapping", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_type_master",
                schema: "master",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    api_type_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    api_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    provider = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_type_master", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_consent_maaping",
                columns: table => new
                {
                    consent_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    consent_status = table.Column<bool>(type: "bit", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_consent_maaping", x => x.consent_id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_id_gen",
                columns: table => new
                {
                    seq_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seq_desc = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    seq_no = table.Column<long>(type: "bigint", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_id_gen", x => x.seq_id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_master",
                columns: table => new
                {
                    appointee_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_master", x => x.appointee_id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_update_activity",
                schema: "activity",
                columns: table => new
                {
                    activity_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<int>(type: "int", nullable: false),
                    update_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    update_value = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_update_activity", x => x.activity_id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_update_log",
                schema: "config",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    update_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    update_value = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_update_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                schema: "admin",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    company_address = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    company_city = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    no_doc_upld_req = table.Column<int>(type: "int", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "disability_master",
                schema: "master",
                columns: table => new
                {
                    disability_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    disability_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    disability_code = table.Column<string>(type: "char", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disability_master", x => x.disability_id);
                });

            migrationBuilder.CreateTable(
                name: "error_logs",
                schema: "config",
                columns: table => new
                {
                    log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    log_info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    log_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_error_logs", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "escalation_case_master",
                schema: "admin",
                columns: table => new
                {
                    case_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    setup_code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    setup_alias = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    setup_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escalation_case_master", x => x.case_id);
                });

            migrationBuilder.CreateTable(
                name: "escalationlevel_master",
                schema: "admin",
                columns: table => new
                {
                    level_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    level_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    level_code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    no_of_days = table.Column<int>(type: "int", nullable: true),
                    setup_alias = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escalationlevel_master", x => x.level_id);
                });

            migrationBuilder.CreateTable(
                name: "gender_master",
                schema: "master",
                columns: table => new
                {
                    gender_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gender_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    gender_code = table.Column<string>(type: "char", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gender_master", x => x.gender_id);
                });

            migrationBuilder.CreateTable(
                name: "generalSetup",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    critical_no_days = table.Column<int>(type: "int", nullable: true),
                    grace_period_days = table.Column<int>(type: "int", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generalSetup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                schema: "config",
                columns: table => new
                {
                    log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    log_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    call_site = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    line_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stack_trace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    arguments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "marital_status_master",
                schema: "master",
                columns: table => new
                {
                    mstatus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mstatus_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    mstatus_code = table.Column<string>(type: "char", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marital_status_master", x => x.mstatus_id);
                });

            migrationBuilder.CreateTable(
                name: "menu_action_mapping",
                schema: "admin",
                columns: table => new
                {
                    mapping_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    menu_id = table.Column<int>(type: "int", nullable: false),
                    action_id = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_action_mapping", x => x.mapping_id);
                });

            migrationBuilder.CreateTable(
                name: "menu_action_master",
                schema: "admin",
                columns: table => new
                {
                    action_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    alias = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    action_type = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    action_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_action_master", x => x.action_id);
                });

            migrationBuilder.CreateTable(
                name: "menu_master",
                schema: "admin",
                columns: table => new
                {
                    menu_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_menu_id = table.Column<int>(type: "int", nullable: false),
                    menu_title = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    menu_alias = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    menu_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    menu_level = table.Column<int>(type: "int", nullable: false),
                    menu_action = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    menu_icon_url = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    seq_no = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_master", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "nationility_master",
                schema: "master",
                columns: table => new
                {
                    nationility_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nation_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    nationility_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nationility_master", x => x.nationility_id);
                });

            migrationBuilder.CreateTable(
                name: "qualification_master",
                schema: "master",
                columns: table => new
                {
                    qualification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    qualification_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    qualification_code = table.Column<string>(type: "char", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qualification_master", x => x.qualification_id);
                });

            migrationBuilder.CreateTable(
                name: "raw_file_history_data",
                columns: table => new
                {
                    rawfile_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    is_pf_verification_req = table.Column<bool>(type: "bit", nullable: true),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    offer_date = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    epf_wages = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    level1_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level2_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level3_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raw_file_history_data", x => x.rawfile_id);
                });

            migrationBuilder.CreateTable(
                name: "reason_master",
                schema: "master",
                columns: table => new
                {
                    reason_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reason_type = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    reason_category = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    reason_code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    reason_info = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    reason_remedy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reason_master", x => x.reason_id);
                });

            migrationBuilder.CreateTable(
                name: "role_master",
                schema: "master",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    role_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    roles_alias = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    is_company_admin = table.Column<bool>(type: "bit", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_master", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "upload_appointee_counter",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_id = table.Column<long>(type: "bigint", nullable: true),
                    count = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_appointee_counter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "upload_type_master",
                schema: "master",
                columns: table => new
                {
                    upload_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    upload_type_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    upload_type_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    upload_type_desc = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_type_master", x => x.upload_type_id);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_xls_file",
                columns: table => new
                {
                    file_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    file_path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    company_id = table.Column<int>(type: "int", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_xls_file", x => x.file_id);
                });

            migrationBuilder.CreateTable(
                name: "user_types",
                schema: "master",
                columns: table => new
                {
                    user_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_type_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    user_type_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_types", x => x.user_type_id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_approval_status_master",
                schema: "master",
                columns: table => new
                {
                    appvl_status_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appvl_status_code = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    appvl_status_desc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_approval_status_master", x => x.appvl_status_id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_details",
                columns: table => new
                {
                    work_flow_det_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: false),
                    state_id = table.Column<long>(type: "bigint", nullable: false),
                    appvl_status_id = table.Column<int>(type: "int", nullable: false),
                    state_alias = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    action_taken_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    reprocess_count = table.Column<int>(type: "int", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_details", x => x.work_flow_det_id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_details_hist",
                columns: table => new
                {
                    work_flow_det_hist_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: false),
                    state_id = table.Column<long>(type: "bigint", nullable: false),
                    appvl_status_id = table.Column<int>(type: "int", nullable: false),
                    state_alias = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    action_taken_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reprocess_count = table.Column<int>(type: "int", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_details_hist", x => x.work_flow_det_hist_id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_state_master",
                schema: "master",
                columns: table => new
                {
                    state_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    state_decs = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    seq_of_flow = table.Column<int>(type: "int", nullable: true),
                    state_alias = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_state_master", x => x.state_id);
                });

            migrationBuilder.CreateTable(
                name: "appointee_reason_details",
                columns: table => new
                {
                    appointee_reason_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: false),
                    reason_id = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    appointee_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_reason_details", x => x.appointee_reason_id);
                    table.ForeignKey(
                        name: "FK_appointee_reason_details_appointee_master_appointee_id1",
                        column: x => x.appointee_id1,
                        principalTable: "appointee_master",
                        principalColumn: "appointee_id");
                });

            migrationBuilder.CreateTable(
                name: "rejected_file_data",
                columns: table => new
                {
                    rejected_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    reject_state = table.Column<int>(type: "int", nullable: false),
                    reject_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    appointee_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rejected_file_data", x => x.rejected_id);
                    table.ForeignKey(
                        name: "FK_rejected_file_data_appointee_master_appointee_id1",
                        column: x => x.appointee_id1,
                        principalTable: "appointee_master",
                        principalColumn: "appointee_id");
                });

            migrationBuilder.CreateTable(
                name: "appointee_details",
                columns: table => new
                {
                    appointee_details_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime", nullable: true),
                    gender = table.Column<string>(type: "char", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    uan_number = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    member_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    member_relation = table.Column<string>(type: "char", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    epf_wages = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    qualification = table.Column<string>(type: "char", nullable: true),
                    maratialstatus = table.Column<string>(type: "char", nullable: true),
                    is_passportAvailable = table.Column<string>(type: "char", nullable: true),
                    is_internationalworker = table.Column<string>(type: "char", nullable: true),
                    origincountry = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    passport_no = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    passport_validfrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    passport_validtill = table.Column<DateTime>(type: "datetime", nullable: true),
                    passport_fileno = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_handicap = table.Column<string>(type: "char", nullable: true),
                    handicape_type = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_pf_verification_req = table.Column<bool>(type: "bit", nullable: true),
                    aadhaar_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    name_from_aadhaar = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    dob_from_aadhaar = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    gender_from_aadhaar = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    aadhaar_number = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    aadhaar_number_view = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    pan_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    fathers_name_from_pan = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    pan_number = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    is_processed = table.Column<bool>(type: "bit", nullable: true),
                    is_pensionapplicable = table.Column<bool>(type: "bit", nullable: true),
                    is_aadhaarvarified = table.Column<bool>(type: "bit", nullable: true),
                    is_passportvarified = table.Column<bool>(type: "bit", nullable: true),
                    is_uanvarified = table.Column<bool>(type: "bit", nullable: true),
                    is_panvarified = table.Column<bool>(type: "bit", nullable: true),
                    is_trustpassbook = table.Column<bool>(type: "bit", nullable: true),
                    save_step = table.Column<int>(type: "int", nullable: true),
                    process_status = table.Column<int>(type: "int", nullable: true),
                    level1_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level2_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level3_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    is_save = table.Column<bool>(type: "bit", nullable: true),
                    is_submit = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    appointee_id1 = table.Column<long>(type: "bigint", nullable: true),
                    company_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointee_details", x => x.appointee_details_id);
                    table.ForeignKey(
                        name: "FK_appointee_details_appointee_master_appointee_id1",
                        column: x => x.appointee_id1,
                        principalTable: "appointee_master",
                        principalColumn: "appointee_id");
                    table.ForeignKey(
                        name: "FK_appointee_details_company_company_id1",
                        column: x => x.company_id1,
                        principalSchema: "admin",
                        principalTable: "company",
                        principalColumn: "company_id");
                });

            migrationBuilder.CreateTable(
                name: "escalation_setup",
                schema: "admin",
                columns: table => new
                {
                    setup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    level_id = table.Column<int>(type: "int", nullable: false),
                    case_id = table.Column<int>(type: "int", nullable: false),
                    email_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    setup_option = table.Column<bool>(type: "bit", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    level_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escalation_setup", x => x.setup_id);
                    table.ForeignKey(
                        name: "FK_escalation_setup_escalationlevel_master_level_id1",
                        column: x => x.level_id1,
                        principalSchema: "admin",
                        principalTable: "escalationlevel_master",
                        principalColumn: "level_id");
                });

            migrationBuilder.CreateTable(
                name: "escaltionlevel_email_mapping",
                schema: "admin",
                columns: table => new
                {
                    map_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    level_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    level_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escaltionlevel_email_mapping", x => x.map_id);
                    table.ForeignKey(
                        name: "FK_escaltionlevel_email_mapping_escalationlevel_master_level_id1",
                        column: x => x.level_id1,
                        principalSchema: "admin",
                        principalTable: "escalationlevel_master",
                        principalColumn: "level_id");
                });

            migrationBuilder.CreateTable(
                name: "menu_role_mapping",
                schema: "admin",
                columns: table => new
                {
                    menu_role_map_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    menu_id = table.Column<int>(type: "int", nullable: false),
                    action_id = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    menu_id1 = table.Column<int>(type: "int", nullable: true),
                    role_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_role_mapping", x => x.menu_role_map_id);
                    table.ForeignKey(
                        name: "FK_menu_role_mapping_menu_master_menu_id1",
                        column: x => x.menu_id1,
                        principalSchema: "admin",
                        principalTable: "menu_master",
                        principalColumn: "menu_id");
                    table.ForeignKey(
                        name: "FK_menu_role_mapping_role_master_role_id1",
                        column: x => x.role_id1,
                        principalSchema: "master",
                        principalTable: "role_master",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "user_master",
                schema: "admin",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ref_appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    user_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    users_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime", nullable: true),
                    email_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    contact_no = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    user_type_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    cur_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    role_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_master", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_master_role_master_role_id1",
                        column: x => x.role_id1,
                        principalSchema: "master",
                        principalTable: "role_master",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "upload_details",
                columns: table => new
                {
                    upload_det_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: false),
                    upload_type_id = table.Column<int>(type: "int", nullable: false),
                    upload_path = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    file_name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    is_path_refered = table.Column<string>(type: "VARCHAR(1)", nullable: true),
                    mime_type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    upload_type_code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    upload_subtype = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    appointee_id1 = table.Column<long>(type: "bigint", nullable: true),
                    company_id = table.Column<int>(type: "int", nullable: true),
                    upload_type_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_details", x => x.upload_det_id);
                    table.ForeignKey(
                        name: "FK_upload_details_appointee_master_appointee_id1",
                        column: x => x.appointee_id1,
                        principalTable: "appointee_master",
                        principalColumn: "appointee_id");
                    table.ForeignKey(
                        name: "FK_upload_details_company_company_id",
                        column: x => x.company_id,
                        principalSchema: "admin",
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_upload_details_upload_type_master_upload_type_id1",
                        column: x => x.upload_type_id1,
                        principalSchema: "master",
                        principalTable: "upload_type_master",
                        principalColumn: "upload_type_id");
                });

            migrationBuilder.CreateTable(
                name: "processed_file_data",
                columns: table => new
                {
                    processed_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    data_uploaded = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    appointee_id1 = table.Column<long>(type: "bigint", nullable: true),
                    file_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processed_file_data", x => x.processed_id);
                    table.ForeignKey(
                        name: "FK_processed_file_data_appointee_master_appointee_id1",
                        column: x => x.appointee_id1,
                        principalTable: "appointee_master",
                        principalColumn: "appointee_id");
                    table.ForeignKey(
                        name: "FK_processed_file_data_uploaded_xls_file_file_id",
                        column: x => x.file_id,
                        principalTable: "uploaded_xls_file",
                        principalColumn: "file_id");
                });

            migrationBuilder.CreateTable(
                name: "raw_file_data",
                columns: table => new
                {
                    rawfile_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    is_pf_verification_req = table.Column<bool>(type: "bit", nullable: true),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    offer_date = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    epf_wages = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    level1_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level2_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level3_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    company_id1 = table.Column<int>(type: "int", nullable: true),
                    file_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raw_file_data", x => x.rawfile_id);
                    table.ForeignKey(
                        name: "FK_raw_file_data_company_company_id1",
                        column: x => x.company_id1,
                        principalSchema: "admin",
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_raw_file_data_uploaded_xls_file_file_id1",
                        column: x => x.file_id1,
                        principalTable: "uploaded_xls_file",
                        principalColumn: "file_id");
                });

            migrationBuilder.CreateTable(
                name: "under_process_file_data",
                columns: table => new
                {
                    underprocess_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appointee_id = table.Column<long>(type: "bigint", nullable: true),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    is_pf_verification_req = table.Column<bool>(type: "bit", nullable: true),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    offer_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    epf_wages = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    recruitment_hr = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    people_manager = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_processed = table.Column<bool>(type: "bit", nullable: true),
                    level1_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level2_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level3_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    company_id1 = table.Column<int>(type: "int", nullable: true),
                    file_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_under_process_file_data", x => x.underprocess_id);
                    table.ForeignKey(
                        name: "FK_under_process_file_data_company_company_id1",
                        column: x => x.company_id1,
                        principalSchema: "admin",
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_under_process_file_data_uploaded_xls_file_file_id1",
                        column: x => x.file_id1,
                        principalTable: "uploaded_xls_file",
                        principalColumn: "file_id");
                });

            migrationBuilder.CreateTable(
                name: "unprocessed_file_data",
                columns: table => new
                {
                    unprocessed_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    candidate_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    appointee_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    mobile_no = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    is_pf_verification_req = table.Column<bool>(type: "bit", nullable: true),
                    joining_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    offer_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    epf_wages = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    recruitment_hr = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    people_manager = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    reason_to_unprocess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    level1_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level2_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    level3_email = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    company_id1 = table.Column<int>(type: "int", nullable: true),
                    file_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unprocessed_file_data", x => x.unprocessed_id);
                    table.ForeignKey(
                        name: "FK_unprocessed_file_data_company_company_id1",
                        column: x => x.company_id1,
                        principalSchema: "admin",
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_unprocessed_file_data_uploaded_xls_file_file_id1",
                        column: x => x.file_id1,
                        principalTable: "uploaded_xls_file",
                        principalColumn: "file_id");
                });

            migrationBuilder.CreateTable(
                name: "role_user_mapping",
                schema: "admin",
                columns: table => new
                {
                    role_user_map_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    role_id1 = table.Column<int>(type: "int", nullable: true),
                    user_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_user_mapping", x => x.role_user_map_id);
                    table.ForeignKey(
                        name: "FK_role_user_mapping_role_master_role_id1",
                        column: x => x.role_id1,
                        principalSchema: "master",
                        principalTable: "role_master",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_role_user_mapping_user_master_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "admin",
                        principalTable: "user_master",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_authentication",
                schema: "admin",
                columns: table => new
                {
                    user_autho_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    user_pwd = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    user_pwd_txt = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    user_profile_pwd = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    is_default_pass = table.Column<string>(type: "char", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    user_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_authentication", x => x.user_autho_id);
                    table.ForeignKey(
                        name: "FK_user_authentication_user_master_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "admin",
                        principalTable: "user_master",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_authentication_hist",
                schema: "admin",
                columns: table => new
                {
                    autho_hist_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    client_id = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    entry_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    exit_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    gip_address = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    browser_name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    token_no = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    otp_no = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    exit_status = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    active_status = table.Column<bool>(type: "bit", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<int>(type: "int", nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    user_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_authentication_hist", x => x.autho_hist_id);
                    table.ForeignKey(
                        name: "FK_user_authentication_hist_user_master_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "admin",
                        principalTable: "user_master",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointee_details_appointee_id1",
                table: "appointee_details",
                column: "appointee_id1");

            migrationBuilder.CreateIndex(
                name: "IX_appointee_details_company_id1",
                table: "appointee_details",
                column: "company_id1");

            migrationBuilder.CreateIndex(
                name: "IX_appointee_reason_details_appointee_id1",
                table: "appointee_reason_details",
                column: "appointee_id1");

            migrationBuilder.CreateIndex(
                name: "IX_escalation_setup_level_id1",
                schema: "admin",
                table: "escalation_setup",
                column: "level_id1");

            migrationBuilder.CreateIndex(
                name: "IX_escaltionlevel_email_mapping_level_id1",
                schema: "admin",
                table: "escaltionlevel_email_mapping",
                column: "level_id1");

            migrationBuilder.CreateIndex(
                name: "IX_menu_role_mapping_menu_id1",
                schema: "admin",
                table: "menu_role_mapping",
                column: "menu_id1");

            migrationBuilder.CreateIndex(
                name: "IX_menu_role_mapping_role_id1",
                schema: "admin",
                table: "menu_role_mapping",
                column: "role_id1");

            migrationBuilder.CreateIndex(
                name: "IX_processed_file_data_appointee_id1",
                table: "processed_file_data",
                column: "appointee_id1");

            migrationBuilder.CreateIndex(
                name: "IX_processed_file_data_file_id",
                table: "processed_file_data",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_raw_file_data_company_id1",
                table: "raw_file_data",
                column: "company_id1");

            migrationBuilder.CreateIndex(
                name: "IX_raw_file_data_file_id1",
                table: "raw_file_data",
                column: "file_id1");

            migrationBuilder.CreateIndex(
                name: "IX_rejected_file_data_appointee_id1",
                table: "rejected_file_data",
                column: "appointee_id1");

            migrationBuilder.CreateIndex(
                name: "IX_role_user_mapping_role_id1",
                schema: "admin",
                table: "role_user_mapping",
                column: "role_id1");

            migrationBuilder.CreateIndex(
                name: "IX_role_user_mapping_user_id1",
                schema: "admin",
                table: "role_user_mapping",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "IX_under_process_file_data_company_id1",
                table: "under_process_file_data",
                column: "company_id1");

            migrationBuilder.CreateIndex(
                name: "IX_under_process_file_data_file_id1",
                table: "under_process_file_data",
                column: "file_id1");

            migrationBuilder.CreateIndex(
                name: "IX_unprocessed_file_data_company_id1",
                table: "unprocessed_file_data",
                column: "company_id1");

            migrationBuilder.CreateIndex(
                name: "IX_unprocessed_file_data_file_id1",
                table: "unprocessed_file_data",
                column: "file_id1");

            migrationBuilder.CreateIndex(
                name: "IX_upload_details_appointee_id1",
                table: "upload_details",
                column: "appointee_id1");

            migrationBuilder.CreateIndex(
                name: "IX_upload_details_company_id",
                table: "upload_details",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_upload_details_upload_type_id1",
                table: "upload_details",
                column: "upload_type_id1");

            migrationBuilder.CreateIndex(
                name: "IX_user_authentication_user_id1",
                schema: "admin",
                table: "user_authentication",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "IX_user_authentication_hist_user_id1",
                schema: "admin",
                table: "user_authentication_hist",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "IX_user_master_role_id1",
                schema: "admin",
                table: "user_master",
                column: "role_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_master",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "activity_transaction",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "api_couter_log",
                schema: "config");

            migrationBuilder.DropTable(
                name: "api_logs",
                schema: "config");

            migrationBuilder.DropTable(
                name: "api_type_mapping",
                schema: "master");

            migrationBuilder.DropTable(
                name: "api_type_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "appointee_consent_maaping");

            migrationBuilder.DropTable(
                name: "appointee_details");

            migrationBuilder.DropTable(
                name: "appointee_id_gen");

            migrationBuilder.DropTable(
                name: "appointee_reason_details");

            migrationBuilder.DropTable(
                name: "appointee_update_activity",
                schema: "activity");

            migrationBuilder.DropTable(
                name: "appointee_update_log",
                schema: "config");

            migrationBuilder.DropTable(
                name: "disability_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "error_logs",
                schema: "config");

            migrationBuilder.DropTable(
                name: "escalation_case_master",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "escalation_setup",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "escaltionlevel_email_mapping",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "gender_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "generalSetup",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "logs",
                schema: "config");

            migrationBuilder.DropTable(
                name: "marital_status_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "menu_action_mapping",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "menu_action_master",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "menu_role_mapping",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "nationility_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "processed_file_data");

            migrationBuilder.DropTable(
                name: "qualification_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "raw_file_data");

            migrationBuilder.DropTable(
                name: "raw_file_history_data");

            migrationBuilder.DropTable(
                name: "reason_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "rejected_file_data");

            migrationBuilder.DropTable(
                name: "role_user_mapping",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "under_process_file_data");

            migrationBuilder.DropTable(
                name: "unprocessed_file_data");

            migrationBuilder.DropTable(
                name: "upload_appointee_counter");

            migrationBuilder.DropTable(
                name: "upload_details");

            migrationBuilder.DropTable(
                name: "user_authentication",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "user_authentication_hist",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "user_types",
                schema: "master");

            migrationBuilder.DropTable(
                name: "workflow_approval_status_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "workflow_details");

            migrationBuilder.DropTable(
                name: "workflow_details_hist");

            migrationBuilder.DropTable(
                name: "workflow_state_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "escalationlevel_master",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "menu_master",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "uploaded_xls_file");

            migrationBuilder.DropTable(
                name: "appointee_master");

            migrationBuilder.DropTable(
                name: "company",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "upload_type_master",
                schema: "master");

            migrationBuilder.DropTable(
                name: "user_master",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "role_master",
                schema: "master");
        }
    }
}
