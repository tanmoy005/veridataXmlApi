CREATE TABLE [dbo].[appointee_reason_details] (
    [appointee_reason_id] BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]        BIGINT         NOT NULL,
    [reason_id]           INT            NOT NULL,
    [remarks]             NVARCHAR (MAX) NULL,
    [active_status]       BIT            NULL,
    [created_by]          INT            NULL,
    [created_on]          DATETIME       NULL,
    [updated_by]          INT            NULL,
    [updated_on]          DATETIME       NULL,
    [appointee_id1]       BIGINT         NULL,
    CONSTRAINT [PK_appointee_reason_details] PRIMARY KEY CLUSTERED ([appointee_reason_id] ASC),
    CONSTRAINT [FK_appointee_reason_details_appointee_master_appointee_id1] FOREIGN KEY ([appointee_id1]) REFERENCES [dbo].[appointee_master] ([appointee_id])
);

