CREATE TABLE [master].[nationility_master] (
    [nationility_id]   INT            IDENTITY (1, 1) NOT NULL,
    [nation_name]      NVARCHAR (100) NULL,
    [nationility_name] NVARCHAR (100) NULL,
    [active_status]    BIT            NULL,
    [created_by]       INT            NULL,
    [created_on]       DATETIME       NULL,
    [updated_by]       INT            NULL,
    [updated_on]       DATETIME       NULL,
    CONSTRAINT [PK_nationility_master] PRIMARY KEY CLUSTERED ([nationility_id] ASC)
);

