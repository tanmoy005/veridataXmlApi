USE [veridataDev]
GO
TRUNCATE TABLE [master].[upload_type_master]
SET IDENTITY_INSERT [master].[upload_type_master] ON 
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'PAN', N'PAN', N'pan card details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'ADHAAR', N'ADH', N'Adhaar card details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'PASSBOOKEXCEL', N'EPFPSBKEXCL', N'PF  passbook details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'EPFODOC', N'EPFPSBK', N'PF file details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'EPFODOC', N'EPFPSBKTRUST', N'PF trust passbook details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[upload_type_master] ([upload_type_id], [upload_type_name], [upload_type_code], [upload_type_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'PASSPORT', N'VISA', N'Visa details', 1, 1, CAST(N'2022-12-15T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[upload_type_master] OFF

