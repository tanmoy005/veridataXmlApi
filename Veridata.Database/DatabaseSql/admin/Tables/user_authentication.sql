CREATE TABLE [admin].[user_authentication] (
    [user_autho_id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [user_id]         BIGINT        NOT NULL,
    [user_pwd]        NVARCHAR (100) NULL,
    [user_pwd_txt]    NVARCHAR (10) NULL,
    [user_profile_pwd] NVARCHAR(100) NULL, 
    [is_default_pass] CHAR (1)      NULL,
    [password_set_date]      DATETIME      NULL,
    [active_status]   BIT           NULL,
    [created_by]      INT           NULL,
    [created_on]      DATETIME      NULL,
    [updated_by]      INT           NULL,
    [updated_on]      DATETIME      NULL,
    [user_id1]        BIGINT        NULL,
  
    CONSTRAINT [PK_user_authentication] PRIMARY KEY CLUSTERED ([user_autho_id] ASC),
    CONSTRAINT [FK_user_authentication_user_master_user_id1] FOREIGN KEY ([user_id1]) REFERENCES [admin].[user_master] ([user_id])
);

