CREATE TABLE [admin].[escalation_setup] (
    [setup_id]      INT           IDENTITY (1, 1) NOT NULL,
    [level_id]      INT           NOT NULL,
    [case_id]       INT           NOT NULL,
    [setup_option]  BIT           NULL,
    [email_id]      NVARCHAR (50) NULL,
    [active_status] BIT           NULL,
    [created_by]    INT           NULL,
    [created_on]    DATETIME      NULL,
    [updated_by]    INT           NULL,
    [updated_on]    DATETIME      NULL,
    [level_id1]     INT           NULL,
    CONSTRAINT [PK_escalation_setup] PRIMARY KEY CLUSTERED ([setup_id] ASC),
    CONSTRAINT [FK_escalation_setup_escalationlevel_master_level_id1] FOREIGN KEY ([level_id1]) REFERENCES [admin].[escalationlevel_master] ([level_id])
);

