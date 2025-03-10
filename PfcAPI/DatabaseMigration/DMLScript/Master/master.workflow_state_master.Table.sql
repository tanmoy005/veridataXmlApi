--USE [veridataDev]
GO
TRUNCATE TABLE [master].[workflow_state_master]
SET IDENTITY_INSERT [master].[workflow_state_master] ON 

INSERT [master].[workflow_state_master] ([state_id], [state_name], [state_decs], [seq_of_flow], [state_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'SendMail', N'Sending mail to the appoientee to fill data', 1, N'SM', 1, 1, CAST(N'2022-12-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_state_master] ([state_id], [state_name], [state_decs], [seq_of_flow], [state_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'UploadDeatils', N'Upload Deatils of the appoientee ', 2, N'UD', 1, 1, CAST(N'2022-12-12T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[workflow_state_master] ([state_id], [state_name], [state_decs], [seq_of_flow], [state_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'DataVarified', N'Appointee Data Verified', 3, N'DV', 1, 1, CAST(N'2022-12-12T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[workflow_state_master] OFF
