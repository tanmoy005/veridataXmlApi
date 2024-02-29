CREATE TABLE [master].[qualification_master] (
    [qualification_id]   INT            IDENTITY (1, 1) NOT NULL,
    [qualification_name] NVARCHAR (100) NULL,
    [qualification_code] CHAR (1)       NULL,
    [active_status]      BIT            NULL,
    [created_by]         INT            NULL,
    [created_on]         DATETIME       CONSTRAINT [DF_qualification_master_created_on] DEFAULT (getdate()) NULL,
    [updated_by]         INT            NULL,
    [updated_on]         DATETIME       NULL,
    CONSTRAINT [PK_qualification_master] PRIMARY KEY CLUSTERED ([qualification_id] ASC)
);

