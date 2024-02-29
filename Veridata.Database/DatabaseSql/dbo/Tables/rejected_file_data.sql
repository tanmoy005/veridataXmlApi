CREATE TABLE [dbo].[rejected_file_data] (
    [rejected_id]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]  BIGINT         NULL,
    [reject_state]  INT            NOT NULL,
    [reject_reason] NVARCHAR (MAX) NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    [appointee_id1] BIGINT         NULL,
    CONSTRAINT [PK_rejected_file_data] PRIMARY KEY CLUSTERED ([rejected_id] ASC),
    CONSTRAINT [FK_rejected_file_data_appointee_master_appointee_id1] FOREIGN KEY ([appointee_id1]) REFERENCES [dbo].[appointee_master] ([appointee_id])
);

