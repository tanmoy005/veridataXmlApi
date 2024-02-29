CREATE TABLE [admin].[menu_action_master] (
    [action_id]     INT            IDENTITY (1, 1) NOT NULL,
    [alias]         NVARCHAR (20)  NULL,
    [action_type]   NVARCHAR (20)  NULL,
    [action_name]   NVARCHAR (100) NOT NULL,
    [active_status] BIT            NULL,
    [created_on]    DATETIME       CONSTRAINT [DF_menu_action_master_created_on] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_menu_action_master] PRIMARY KEY CLUSTERED ([action_id] ASC)
);

