CREATE TABLE [admin].[role_user_mapping] (
    [role_user_map_id] BIGINT   IDENTITY (1, 1) NOT NULL,
    [user_id]          BIGINT   NOT NULL,
    [role_id]          INT      NOT NULL,
    [active_status]    BIT      NULL,
    [created_by]       INT      NULL,
    [created_on]       DATETIME NULL,
    [updated_by]       INT      NULL,
    [updated_on]       DATETIME NULL,
    [role_id1]         INT      NULL,
    [user_id1]         BIGINT   NULL,
    CONSTRAINT [PK_role_user_mapping] PRIMARY KEY CLUSTERED ([role_user_map_id] ASC),
    CONSTRAINT [FK_role_user_mapping_role_master_role_id1] FOREIGN KEY ([role_id1]) REFERENCES [master].[role_master] ([role_id]),
    CONSTRAINT [FK_role_user_mapping_user_master_user_id1] FOREIGN KEY ([user_id1]) REFERENCES [admin].[user_master] ([user_id])
);

