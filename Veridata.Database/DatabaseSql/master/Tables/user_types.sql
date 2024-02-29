CREATE TABLE [master].[user_types] (
    [user_type_id]   INT            IDENTITY (1, 1) NOT NULL,
    [user_type_name] NVARCHAR (50)  NULL,
    [user_type_desc] NVARCHAR (100) NULL,
    [active_status]  BIT            NULL,
    [created_by]     INT            NULL,
    [created_on]     DATETIME       NULL,
    [updated_by]     INT            NULL,
    [updated_on]     DATETIME       NULL,
    CONSTRAINT [PK_user_types] PRIMARY KEY CLUSTERED ([user_type_id] ASC)
);

