CREATE TABLE [admin].[generalSetup] (
    [id]                       INT           IDENTITY (1, 1) NOT NULL,
    [critical_no_days]         INT           NULL,
    [grace_period_days]        INT           NULL,
    [aadhar_verification_type] NVARCHAR (10) NULL,
    [employement_overlap_days] INT           NULL,
    [active_status]            BIT           NULL,
    [created_by]               INT           NULL,
    [created_on]               DATETIME      NULL,
    [updated_by]               INT           NULL,
    [updated_on]               DATETIME      NULL,
    [appointee_count_rate]     INT           NULL,
    CONSTRAINT [PK_generalSetup] PRIMARY KEY CLUSTERED ([id] ASC)
);

