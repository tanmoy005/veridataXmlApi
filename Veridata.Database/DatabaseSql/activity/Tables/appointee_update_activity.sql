CREATE TABLE [activity].[appointee_update_activity] (
    [activity_id]   INT            IDENTITY (1, 1) NOT NULL,
    [appointee_id]  INT            NOT NULL,
    [update_type]   NVARCHAR (50)  NULL,
    [update_value]  NVARCHAR (100) NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    CONSTRAINT [PK_appointee_update_activity] PRIMARY KEY CLUSTERED ([activity_id] ASC)
);

