CREATE TABLE [admin].[menu_role_mapping] (
    [menu_role_map_id] INT      IDENTITY (1, 1) NOT NULL,
    [role_id]          INT      NOT NULL,
    [menu_id]          INT      NOT NULL,
    [action_id]        INT      NULL,
    [active_status]    BIT      NULL,
    [created_by]       INT      NULL,
    [created_on]       DATETIME NULL,
    [updated_by]       INT      NULL,
    [updated_on]       DATETIME NULL,
    [menu_id1]         INT      NULL,
    [role_id1]         INT      NULL,
    CONSTRAINT [PK_menu_role_mapping] PRIMARY KEY CLUSTERED ([menu_role_map_id] ASC),
    CONSTRAINT [FK_menu_role_mapping_menu_master_menu_id1] FOREIGN KEY ([menu_id1]) REFERENCES [admin].[menu_master] ([menu_id]),
    CONSTRAINT [FK_menu_role_mapping_role_master_role_id1] FOREIGN KEY ([role_id1]) REFERENCES [master].[role_master] ([role_id])
);

