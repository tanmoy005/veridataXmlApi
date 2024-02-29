CREATE TABLE [config].[api_couter_log] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [api_name]   NVARCHAR (100) NOT NULL,
    [api_url]    NVARCHAR (50)  NOT NULL,
    [api_type]   NVARCHAR (50)  NULL,
    [api_status] INT            NULL,
    [payload]    TEXT           NULL,
    [created_on] DATETIME       NULL,
    [created_by] BIGINT         NOT NULL,
    CONSTRAINT [PK_api_couter_log] PRIMARY KEY CLUSTERED ([id] ASC)
);

