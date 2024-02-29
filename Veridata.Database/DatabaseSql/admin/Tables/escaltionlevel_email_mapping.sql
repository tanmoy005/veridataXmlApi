CREATE TABLE [admin].[escaltionlevel_email_mapping] (
    [map_id]        INT            IDENTITY (1, 1) NOT NULL,
    [level_id]      INT            NOT NULL,
    [email]         NVARCHAR (100) NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    [level_id1]     INT            NULL,
    CONSTRAINT [PK_escaltionlevel_email_mapping] PRIMARY KEY CLUSTERED ([map_id] ASC),
    CONSTRAINT [FK_escaltionlevel_email_mapping_escalationlevel_master_level_id1] FOREIGN KEY ([level_id1]) REFERENCES [admin].[escalationlevel_master] ([level_id])
);

