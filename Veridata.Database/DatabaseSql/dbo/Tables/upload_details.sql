﻿CREATE TABLE [dbo].[upload_details] (
    [upload_det_id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [appointee_id]     BIGINT         NOT NULL,
    [upload_type_id]   INT            NOT NULL,
    [upload_path]      NVARCHAR (200) NULL,
    [file_name]        NVARCHAR (200) NULL,
    [is_path_refered]  VARCHAR (1)    NULL,
    [mime_type]        NVARCHAR (100) NULL,
    [upload_type_code] NVARCHAR (50)  NULL,
    [upload_subtype]   NVARCHAR (50)  NULL,
    [active_status]    BIT            NULL,
    [created_by]       INT            NULL,
    [created_on]       DATETIME       NULL,
    [updated_by]       INT            NULL,
    [updated_on]       DATETIME       NULL,
    [appointee_id1]    BIGINT         NULL,
    [company_id]       INT            NULL,
    [upload_type_id1]  INT            NULL,
    CONSTRAINT [PK_upload_details] PRIMARY KEY CLUSTERED ([upload_det_id] ASC),
    CONSTRAINT [FK_upload_details_appointee_master_appointee_id1] FOREIGN KEY ([appointee_id1]) REFERENCES [dbo].[appointee_master] ([appointee_id]),
    CONSTRAINT [FK_upload_details_company_company_id] FOREIGN KEY ([company_id]) REFERENCES [admin].[company] ([company_id]),
    CONSTRAINT [FK_upload_details_upload_type_master_upload_type_id1] FOREIGN KEY ([upload_type_id1]) REFERENCES [master].[upload_type_master] ([upload_type_id])
);

