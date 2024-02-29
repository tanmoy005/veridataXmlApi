CREATE TABLE [admin].[menu_action_mapping] (
    [mapping_id]    INT IDENTITY (1, 1) NOT NULL,
    [menu_id]       INT NOT NULL,
    [action_id]     INT NOT NULL,
    [active_status] BIT NULL,
    CONSTRAINT [PK_menu_action_mapping] PRIMARY KEY CLUSTERED ([mapping_id] ASC)
);

