CREATE TABLE [dbo].[workflow_details] (
    [work_flow_det_id] BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]     BIGINT         NOT NULL,
    [state_id]         BIGINT         NOT NULL,
    [appvl_status_id]  INT            NOT NULL,
    [state_alias]      NVARCHAR (20)  NULL,
    [remarks]          NVARCHAR (MAX) NULL,
    [action_taken_at]  DATETIME       NULL,
    [reprocess_count]  INT            NULL,
    [active_status]    BIT            NULL,
    [created_by]       INT            NULL,
    [created_on]       DATETIME       NULL,
    [updated_by]       INT            NULL,
    [updated_on]       DATETIME       NULL,
    CONSTRAINT [PK_workflow_details] PRIMARY KEY CLUSTERED ([work_flow_det_id] ASC)
);

