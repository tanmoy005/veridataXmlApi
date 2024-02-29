CREATE TABLE [config].[appointee_update_log] (
    [id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id] BIGINT         NULL,
    [candidate_id] NVARCHAR (20)  NULL,
    [update_type]  NVARCHAR (50)  NOT NULL,
    [update_value] NVARCHAR (100) NULL,
    [created_on]   DATETIME       NULL,
    [created_by]   BIGINT         NOT NULL,
    CONSTRAINT [PK_appointee_update_log] PRIMARY KEY CLUSTERED ([id] ASC)
);

