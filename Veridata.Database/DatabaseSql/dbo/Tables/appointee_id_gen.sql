CREATE TABLE [dbo].[appointee_id_gen] (
    [seq_id]        INT            IDENTITY (1, 1) NOT NULL,
    [seq_desc]      NVARCHAR (200) NULL,
    [seq_no]        BIGINT         NOT NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_appointee_id_gen] PRIMARY KEY CLUSTERED ([seq_id] ASC)
);

