CREATE TABLE [admin].[user_authentication_hist] (
    [autho_hist_id] INT           IDENTITY (1, 1) NOT NULL,
    [user_id]       BIGINT        NOT NULL,
    [client_id]     NVARCHAR(100) NULL, 
    [entry_time]    DATETIME      NULL,
    [exit_time]     DATETIME      NULL,
    [ip_address]    NVARCHAR (50) NULL,
    [gip_address]   NVARCHAR (50) NULL,
    [browser_name]  NVARCHAR (50) NULL,
    [token_no]      NVARCHAR (50) NULL,
    [refresh_token_expiry_time]    DATETIME      NULL,
    [otp_no]        NVARCHAR(10) NULL, 
    [exit_status]   NVARCHAR (10) NULL,
    [active_status] BIT           NULL,
    [created_by]    INT           NULL,
    [created_on]    DATETIME      NULL,
    [updated_by]    INT           NULL,
    [updated_on]    DATETIME      NULL,
    [user_id1]      BIGINT        NULL,
     
    CONSTRAINT [PK_user_authentication_hist] PRIMARY KEY CLUSTERED ([autho_hist_id] ASC),
    CONSTRAINT [FK_user_authentication_hist_user_master_user_id1] FOREIGN KEY ([user_id1]) REFERENCES [admin].[user_master] ([user_id])
);

