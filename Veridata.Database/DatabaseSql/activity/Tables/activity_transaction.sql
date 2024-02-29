CREATE TABLE [activity].[activity_transaction] (
    [activity_trans_id] INT      IDENTITY (1, 1) NOT NULL,
    [appointee_id]      BIGINT   NULL,
    [activity_id]       INT      NOT NULL,
    [active_status]     BIT      NULL,
    [created_by]        INT      NULL,
    [created_on]        DATETIME NULL,
    [updated_by]        INT      NULL,
    [updated_on]        DATETIME NULL,
    CONSTRAINT [PK_activity_transaction] PRIMARY KEY CLUSTERED ([activity_trans_id] ASC)
);

