CREATE TABLE [master].[faq_master] (
    [faq_id]        INT            IDENTITY (1, 1) NOT NULL,
    [faq_name]      NVARCHAR (MAX) NULL,
    [faq_desc]      NVARCHAR (MAX) NULL,
    [txt_type]      NVARCHAR(10) NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_faq_master] PRIMARY KEY CLUSTERED ([faq_id] ASC)
);

