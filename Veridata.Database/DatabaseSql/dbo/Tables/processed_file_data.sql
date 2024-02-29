CREATE TABLE [dbo].[processed_file_data] (
    [processed_id]  BIGINT   IDENTITY (1, 1) NOT NULL,
    [appointee_id]  BIGINT   NULL,
    [active_status] BIT      NULL,
    [data_uploaded] BIT      NULL,
    [created_by]    INT      NULL,
    [created_on]    DATETIME NULL,
    [updated_by]    INT      NULL,
    [updated_on]    DATETIME NULL,
    [appointee_id1] BIGINT   NULL,
    [file_id]       BIGINT   NULL,
    CONSTRAINT [PK_processed_file_data] PRIMARY KEY CLUSTERED ([processed_id] ASC),
    CONSTRAINT [FK_processed_file_data_appointee_master_appointee_id1] FOREIGN KEY ([appointee_id1]) REFERENCES [dbo].[appointee_master] ([appointee_id]),
    CONSTRAINT [FK_processed_file_data_uploaded_xls_file_file_id] FOREIGN KEY ([file_id]) REFERENCES [dbo].[uploaded_xls_file] ([file_id])
);

