CREATE TABLE [activity].[mail_transaction] (
    [mail_trans_id] INT      IDENTITY (1, 1) NOT NULL,
    [appointee_id]      BIGINT   NULL,
    [mail_type]        NVARCHAR(10)      NOT NULL,
    [active_status]     BIT      NULL,
    [created_by]        INT      NULL,
    [created_on]        DATETIME NULL,
    [updated_by]        INT      NULL,
    [updated_on]        DATETIME NULL,
    CONSTRAINT [PK_mail_transaction] PRIMARY KEY CLUSTERED ([mail_trans_id] ASC)
);

