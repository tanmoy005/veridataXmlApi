USE [veridataDev]
GO

TRUNCATE TABLE [master].[disability_master] 

SET IDENTITY_INSERT [master].[disability_master] ON 

INSERT [master].[disability_master] ([disability_id], [disability_name], [disability_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'Locomotive', N'L', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[disability_master] ([disability_id], [disability_name], [disability_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'Hearing', N'H', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[disability_master] ([disability_id], [disability_name], [disability_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'Visual', N'V', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[disability_master] OFF
