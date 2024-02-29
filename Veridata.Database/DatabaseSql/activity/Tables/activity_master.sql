CREATE TABLE [activity].[activity_master] (
    [activity_id]    INT            IDENTITY (1, 1) NOT NULL,
    [activity_code]  NVARCHAR (10)  NULL,
    [activity_type]  NVARCHAR (50)  NULL,
    [activity_name]  NVARCHAR (100) NULL,
    [activity_info]  NVARCHAR (200) NULL,
    [activity_color] NVARCHAR (100) NULL,
    [active_status]  BIT            NULL,
    [created_by]     INT            NULL,
    [created_on]     DATETIME       NULL,
    [updated_by]     INT            NULL,
    [updated_on]     DATETIME       NULL,
    CONSTRAINT [PK_activity_master] PRIMARY KEY CLUSTERED ([activity_id] ASC)
);

