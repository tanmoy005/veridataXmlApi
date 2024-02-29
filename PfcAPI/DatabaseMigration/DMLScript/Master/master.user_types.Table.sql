USE [veridataDev]
GO
TRUNCATE TABLE [master].[user_types]
SET IDENTITY_INSERT [master].[user_types] ON 

INSERT [master].[user_types] ([user_type_id], [user_type_name], [user_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'Application', N'App admin user', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[user_types] ([user_type_id], [user_type_name], [user_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'Company', N'Company user', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[user_types] ([user_type_id], [user_type_name], [user_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'Appoientee', N'Appoientee user', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[user_types] OFF
