--USE [VeridataDev_New]
GO

Truncate TABLE [master].[api_type_master]
SET IDENTITY_INSERT [master].[api_type_master] ON 

INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (1, N'ADHAAR', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (2, N'PAN', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (3, N'PASSPORT', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (4, N'UAN', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (5, N'EPFO', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (6, N'ADHAAR', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (7, N'PAN', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (8, N'PASSPORT', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (9, N'UAN', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (10, N'EPFO', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (11, N'PAN', NULL, N'signzy', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (12, N'PASSPORT', NULL, N'signzy', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (13, N'UAN', NULL, N'signzy', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on]) VALUES (14, N'EPFO', NULL, N'signzy', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime))
SET IDENTITY_INSERT [master].[api_type_master] OFF
