CREATE TABLE [master].[marital_status_master] (
    [mstatus_id]    INT            IDENTITY (1, 1) NOT NULL,
    [mstatus_name]  NVARCHAR (100) NULL,
    [mstatus_code]  CHAR (1)       NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_marital_status_master] PRIMARY KEY CLUSTERED ([mstatus_id] ASC)
);

