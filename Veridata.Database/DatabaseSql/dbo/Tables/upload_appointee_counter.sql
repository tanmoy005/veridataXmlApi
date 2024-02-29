CREATE TABLE [dbo].[upload_appointee_counter] (
    [id]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [file_id]    BIGINT   NULL,
    [count]      INT      NOT NULL,
    [created_by] INT      NULL,
    [created_on] DATETIME NULL,
    CONSTRAINT [PK_upload_appointee_counter] PRIMARY KEY CLUSTERED ([id] ASC)
);

