CREATE TABLE [admin].[company] (
    [company_id]      INT            IDENTITY (1, 1) NOT NULL,
    [company_name]    NVARCHAR (100) NULL,
    [company_address] NVARCHAR (200) NULL,
    [company_city]    NVARCHAR (50)  NULL,
    [no_doc_upld_req] INT            NULL,
    [active_status]   BIT            NULL,
    [created_by]      INT            NULL,
    [created_on]      DATETIME       NULL,
    [updated_by]      INT            NULL,
    [updated_on]      DATETIME       NULL,
    CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED ([company_id] ASC)
);

