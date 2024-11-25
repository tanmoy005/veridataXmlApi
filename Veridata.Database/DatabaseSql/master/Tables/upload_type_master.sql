CREATE TABLE [master].[upload_type_master] (
    [upload_type_id]       INT            IDENTITY (1, 1) NOT NULL,
    [upload_type_name]     NVARCHAR (50)  NULL,
    [upload_type_code]     NVARCHAR (50)  NULL,
    [upload_type_desc]     NVARCHAR (200) NULL,
    [upload_type_category] NVARCHAR (50)  NULL,
    [category_name]        NVARCHAR (50)  NULL,
    [active_status]        BIT            NULL,
    [created_by]           INT            NULL,
    [created_on]           DATETIME       NULL,
    [updated_by]           INT            NULL,
    [updated_on]           DATETIME       NULL,
    [upload_doc_type]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_upload_type_master] PRIMARY KEY CLUSTERED ([upload_type_id] ASC)
);



