CREATE TABLE [dbo].[appointee_consent_maaping] (
    [consent_id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]    BIGINT         NULL,
    [candidate_id]    NVARCHAR (100) NULL,
    [consent_status]   INT            NULL,
    [active_status]   BIT            NULL,
    [created_by]      INT            NULL,
    [created_on]      DATETIME       NULL,
    [updated_by]      INT            NULL,
    [updated_on]      DATETIME       NULL,
    CONSTRAINT [PK_appointee_consent_maaping] PRIMARY KEY CLUSTERED ([consent_id] ASC)
);

