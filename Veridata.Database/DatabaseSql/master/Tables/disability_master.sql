CREATE TABLE [master].[disability_master] (
    [disability_id]   INT            IDENTITY (1, 1) NOT NULL,
    [disability_name] NVARCHAR (100) NULL,
    [disability_code] CHAR (1)       NULL,
    [active_status]   BIT            NULL,
    [created_by]      INT            NULL,
    [created_on]      DATETIME       NULL,
    [updated_by]      INT            NULL,
    [updated_on]      DATETIME       NULL,
    CONSTRAINT [PK_disability_master] PRIMARY KEY CLUSTERED ([disability_id] ASC)
);

