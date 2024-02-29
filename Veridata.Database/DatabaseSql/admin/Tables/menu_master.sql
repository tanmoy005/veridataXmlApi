CREATE TABLE [admin].[menu_master] (
    [menu_id]        INT            IDENTITY (1, 1) NOT NULL,
    [parent_menu_id] INT            NOT NULL,
    [menu_title]     NVARCHAR (50)  NULL,
    [menu_alias]     NVARCHAR (20)  NULL,
    [menu_desc]      NVARCHAR (100) NULL,
    [menu_level]     INT            NOT NULL,
    [menu_action]    NVARCHAR (100) NULL,
    [menu_icon_url]  NVARCHAR (200) NULL,
    [seq_no]         INT            NOT NULL,
    [active_status]  BIT            NULL,
    [created_by]     INT            NULL,
    [created_on]     DATETIME       NULL,
    [updated_by]     INT            NULL,
    [updated_on]     DATETIME       NULL,
    CONSTRAINT [PK_menu_master] PRIMARY KEY CLUSTERED ([menu_id] ASC)
);

