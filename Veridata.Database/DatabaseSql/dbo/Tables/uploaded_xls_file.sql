CREATE TABLE [dbo].[uploaded_xls_file] (
    [file_id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [file_name]     NVARCHAR (100) NULL,
    [file_path]     NVARCHAR (MAX) NULL,
    [company_id]    INT            NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_uploaded_xls_file] PRIMARY KEY CLUSTERED ([file_id] ASC)
);

