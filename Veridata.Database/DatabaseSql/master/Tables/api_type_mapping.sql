CREATE TABLE [master].[api_type_mapping] (
    [id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [api_type_id]   BIGINT         NOT NULL,
    [api_name]      NVARCHAR (100) NULL,
    [api_base_url]  NVARCHAR (50)  NULL,
    [api_url]       NVARCHAR (100) NULL,
    [active_status] BIT            NULL,
    [created_on]    DATETIME       NULL,
    [created_by]    BIGINT         NOT NULL,
    [api_Prioirty]  INT            NULL,
    CONSTRAINT [PK_api_config] PRIMARY KEY CLUSTERED ([id] ASC)
);

