CREATE TABLE [admin].[escalationlevel_master] (
    [level_id]      INT            IDENTITY (1, 1) NOT NULL,
    [level_name]    NVARCHAR (100) NULL,
    [level_code]    NVARCHAR (10)  NULL,
    [no_of_days]    INT            NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    [setup_alias]   NVARCHAR (10)  NULL,
    CONSTRAINT [PK_escalationlevel_master] PRIMARY KEY CLUSTERED ([level_id] ASC)
);

