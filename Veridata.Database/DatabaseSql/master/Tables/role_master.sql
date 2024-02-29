CREATE TABLE [master].[role_master] (
    [role_id]          INT            IDENTITY (1, 1) NOT NULL,
    [role_name]        NVARCHAR (50)  NULL,
    [role_desc]        NVARCHAR (100) NULL,
    [roles_alias]      NVARCHAR (10)  NULL,
    [is_company_admin] BIT            NULL,
    [active_status]    BIT            NULL,
    [created_by]       INT            NULL,
    [created_on]       DATETIME       NULL,
    [updated_by]       INT            NULL,
    [updated_on]       DATETIME       NULL,
    CONSTRAINT [PK_role_master] PRIMARY KEY CLUSTERED ([role_id] ASC)
);

