USE [veridataDev]
GO
TRUNCATE TABLE [master].[role_master]
SET IDENTITY_INSERT [master].[role_master] ON 
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'SuperAdmin', N'Super Admin', N'SADMN', 0, 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'Company Admin', N'Company  Admin', N'CADMN', 1, 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'GeneralAdmin', N'General Admin', N'GADMN', 1, 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'Application', N'Application User', N'APUSR', 0, 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'Appointee', N'Appointee User', N'APNTE', 0, 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'HrRole', N'HR ROLE', N'ADMN', 1, 1, 1, CAST(N'2023-09-14T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[role_master] ([role_id], [role_name], [role_desc], [roles_alias], [is_company_admin], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (7, N'Finance', N'finance', N'ADMN', 1, 1, 1, CAST(N'2023-09-14T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[role_master] OFF
