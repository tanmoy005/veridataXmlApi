CREATE TABLE [admin].[escalation_case_master] (
    [case_id]       INT            IDENTITY (1, 1) NOT NULL,
    [setup_code]    NVARCHAR (10)  NULL,
    [setup_desc]    NVARCHAR (100) NULL,
    [setup_alias]   NVARCHAR (10)  NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_escalation_case_master] PRIMARY KEY CLUSTERED ([case_id] ASC)
);

