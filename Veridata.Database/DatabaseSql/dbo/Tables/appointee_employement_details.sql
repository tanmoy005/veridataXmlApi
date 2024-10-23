CREATE TABLE [dbo].[appointee_employement_details] (
    [employement_det_id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]          BIGINT         NOT NULL,
    [type_code]             NVARCHAR (50)  NULL,
    [subtype]               NVARCHAR (50)  NULL,
    [data_info]             VARBINARY(MAX)  NULL,
    [active_status]         BIT            NULL,
    [created_by]            INT            NULL,
    [created_on]            DATETIME       NULL,
    [updated_by]            INT            NULL,
    [updated_on]            DATETIME       NULL,
    CONSTRAINT [PK_appointee_employement_details] PRIMARY KEY CLUSTERED ([employement_det_id] ASC),
);

