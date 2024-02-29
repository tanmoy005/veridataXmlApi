CREATE TABLE [dbo].[appointee_master] (
    [appointee_id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [candidate_id]    NVARCHAR (100) NULL,
    [appointee_name]  NVARCHAR (100) NULL,
    [appointee_email] NVARCHAR (50)  NULL,
    [mobile_no]       NVARCHAR (20)  NULL,
    [file_id]         BIGINT         NOT NULL,
    [joining_date]    DATETIME       NULL,
    [active_status]   BIT            NULL,
    [created_by]      INT            NULL,
    [created_on]      DATETIME       NULL,
    [updated_by]      INT            NULL,
    [updated_on]      DATETIME       NULL,
    CONSTRAINT [PK_appointee_master] PRIMARY KEY CLUSTERED ([appointee_id] ASC)
);

