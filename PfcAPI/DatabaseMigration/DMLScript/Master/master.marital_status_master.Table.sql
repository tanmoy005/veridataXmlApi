USE [veridataDev]
GO

TRUNCATE TABLE [master].[marital_status_master]

SET IDENTITY_INSERT [master].[marital_status_master] ON 

INSERT [master].[marital_status_master] ([mstatus_id], [mstatus_name], [mstatus_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'Married', N'M', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[marital_status_master] ([mstatus_id], [mstatus_name], [mstatus_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'Widow/Widower', N'W', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[marital_status_master] ([mstatus_id], [mstatus_name], [mstatus_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'Unmarried', N'U', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[marital_status_master] ([mstatus_id], [mstatus_name], [mstatus_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'Divorced', N'D', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[marital_status_master] OFF
