CREATE TABLE [master].[reason_master] (
    [reason_id]       INT            IDENTITY (1, 1) NOT NULL,
    [reason_type]     NVARCHAR (10)  NULL,
    [reason_category] NVARCHAR (10)  NULL,
    [reason_code]     NVARCHAR (10)  NULL,
    [reason_info]     NVARCHAR (200) NULL,
    [reason_remedy]   NVARCHAR (MAX) NULL,
    [active_status]   BIT            NULL,
    [created_by]      INT            NULL,
    [created_on]      DATETIME       CONSTRAINT [DF_reason_master_created_on] DEFAULT (getdate()) NULL,
    [updated_by]      INT            NULL,
    [updated_on]      DATETIME       NULL,
    CONSTRAINT [PK_reason_master] PRIMARY KEY CLUSTERED ([reason_id] ASC)
);

