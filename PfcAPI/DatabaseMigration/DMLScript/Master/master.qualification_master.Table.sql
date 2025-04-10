USE [veridataDev]
GO
TRUNCATE TABLE [master].[qualification_master]
SET IDENTITY_INSERT [master].[qualification_master] ON 

INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'Doctorate', N'D', 1, 1, CAST(N'2023-07-05T16:45:54.997' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'Post Graduate', N'P', 1, 1, CAST(N'2023-07-05T16:45:54.997' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'Graduate', N'G', 1, 1, CAST(N'2023-07-05T16:45:54.993' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'XII', N'S', 1, 1, CAST(N'2023-07-05T16:45:55.000' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'X', N'M', 1, 1, CAST(N'2023-07-05T16:45:55.000' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'Non Matric', N'N', 0, 1, CAST(N'2023-07-05T16:45:54.997' AS DateTime), NULL, NULL)
INSERT [master].[qualification_master] ([qualification_id], [qualification_name], [qualification_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (7, N'Illiterate', N'I', 0, 1, CAST(N'2023-07-05T16:45:54.997' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[qualification_master] OFF
