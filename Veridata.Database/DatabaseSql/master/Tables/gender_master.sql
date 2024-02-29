CREATE TABLE [master].[gender_master] (
    [gender_id]     INT            IDENTITY (1, 1) NOT NULL,
    [gender_name]   NVARCHAR (100) NULL,
    [gender_code]   CHAR (1)       NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_gender_master] PRIMARY KEY CLUSTERED ([gender_id] ASC)
);

