CREATE TABLE [master].[api_type_master] (
    [id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [api_type_code]   NVARCHAR (50)   NULL,
    [api_desc]        NVARCHAR (100)  NULL,
    [provider]        NVARCHAR (50)  NULL,
    [active_status]   BIT            NULL,
    [created_by]      BIGINT            NULL,
    [created_on]      DATETIME       NULL,
   
    CONSTRAINT [PK_api_type_master] PRIMARY KEY CLUSTERED ([id] ASC)
);

