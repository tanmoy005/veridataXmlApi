--USE [veridataDev]
GO
TRUNCATE TABLE [master].[workflow_approval_status_master]
SET IDENTITY_INSERT [master].[workflow_approval_status_master] ON 
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'PI', N'Process Initiated', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'AP', N'Data Verified And Approved', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'RE', N'Data Verified And Re-Processed', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'RJ', N'Data Verified And Rejected', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'CL', N'Data Verification Closed', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'FA', N'Data Verified And Forced Approved', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (7, N'MV', N'Manual Verification Required', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (8, N'RD', N'Document Reupload Requested', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_approval_status_master] ([appvl_status_id], [appvl_status_code], [appvl_status_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (9, N'MRV', N'Manual  Re-Verification Required', 1, 1, CAST(N'2022-12-19T13:03:51.000' AS DateTime), NULL, NULL)

SET IDENTITY_INSERT [master].[workflow_approval_status_master] OFF

