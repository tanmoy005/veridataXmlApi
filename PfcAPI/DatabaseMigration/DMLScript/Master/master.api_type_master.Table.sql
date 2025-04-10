--USE [VeridataDev_New]
GO

Truncate TABLE [master].[api_type_master]
SET IDENTITY_INSERT [master].[api_type_master] ON 
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (1, N'ADHAAR', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (2, N'PAN', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (3, N'PASSPORT', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (4, N'UAN', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (5, N'EPFO', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (6, N'ADHAAR', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 3)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (7, N'PAN', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 3)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (8, N'PASSPORT', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 3)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (9, N'UAN', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 3)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (10, N'EPFO', NULL, N'surepass', 0, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 3)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (11, N'PAN', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (12, N'PASSPORT', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (13, N'UAN', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (14, N'EPFO', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (15, N'EPFOUAN', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (16, N'BANK', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (17, N'POLICE', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (18, N'POLICE', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (19, N'BANK', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (20, N'DRIVING', NULL, N'signzy', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 2)
INSERT [master].[api_type_master] ([id], [api_type_code], [api_desc], [provider], [active_status], [created_by], [created_on], [api_priority]) VALUES (21, N'DRIVING', NULL, N'karza', 1, 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1)
SET IDENTITY_INSERT [master].[api_type_master] OFF