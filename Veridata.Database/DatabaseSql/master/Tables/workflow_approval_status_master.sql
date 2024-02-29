CREATE TABLE [master].[workflow_approval_status_master] (
    [appvl_status_id]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [appvl_status_code] NVARCHAR (20)  NOT NULL,
    [appvl_status_desc] NVARCHAR (100) NULL,
    [active_status]     BIT            NULL,
    [created_by]        INT            NULL,
    [created_on]        DATETIME       NULL,
    [updated_by]        INT            NULL,
    [updated_on]        DATETIME       NULL,
    CONSTRAINT [PK_workflow_approval_status_master] PRIMARY KEY CLUSTERED ([appvl_status_id] ASC)
);

