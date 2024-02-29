CREATE TABLE [master].[workflow_state_master] (
    [state_id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [state_name]    NVARCHAR (100) NOT NULL,
    [state_decs]    NVARCHAR (200) NULL,
    [seq_of_flow]   INT            NULL,
    [state_alias]   NVARCHAR (20)  NULL,
    [active_status] BIT            NULL,
    [created_by]    INT            NULL,
    [created_on]    DATETIME       NULL,
    [updated_by]    INT            NULL,
    [updated_on]    DATETIME       NULL,
    CONSTRAINT [PK_workflow_state_master] PRIMARY KEY CLUSTERED ([state_id] ASC)
);

