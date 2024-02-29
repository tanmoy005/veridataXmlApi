CREATE TABLE [admin].[user_master] (
    [user_id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ref_appointee_id] BIGINT        NULL,
    [user_code]        NVARCHAR (50) NULL,
    [users_name]       NVARCHAR (50) NULL,
    [date_of_birth]    DATETIME      NULL,
    [email_id]         NVARCHAR (50) NULL,
    [contact_no]       NVARCHAR (50) NULL,
    [user_type_id]     INT           NOT NULL,
    [role_id]          INT           NOT NULL,
    [active_status]    BIT           NULL,
    [cur_status]       BIT           NULL,
    [created_by]       INT           NULL,
    [created_on]       DATETIME      NULL,
    [updated_by]       INT           NULL,
    [updated_on]       DATETIME      NULL,
    [role_id1]         INT           NULL,
    [candidate_id]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_user_master] PRIMARY KEY CLUSTERED ([user_id] ASC),
    CONSTRAINT [FK_user_master_role_master_role_id1] FOREIGN KEY ([role_id1]) REFERENCES [master].[role_master] ([role_id])
);

