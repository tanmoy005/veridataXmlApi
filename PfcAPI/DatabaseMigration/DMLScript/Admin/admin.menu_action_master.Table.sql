USE [veridataDev]
TRUNCATE TABLE [admin].[menu_action_master]
GO
SET IDENTITY_INSERT [admin].[menu_action_master] ON 

INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (1, N'A001', NULL, N'view', 1, CAST(N'2023-07-10T16:34:29.943' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (2, N'A002', NULL, N'approve', 1, CAST(N'2023-07-10T16:34:29.943' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (3, N'A003', NULL, N'reject', 1, CAST(N'2023-07-10T16:34:29.943' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (4, N'A004', NULL, N'start verify ', 1, CAST(N'2023-07-10T16:40:31.013' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (5, N'A005', NULL, N'notify user', 1, CAST(N'2023-07-10T17:01:13.817' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (6, N'A006', NULL, N'upload data', 1, CAST(N'2023-07-10T17:02:25.757' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (7, N'A007', NULL, N'edit', 1, CAST(N'2023-07-10T17:56:00.830' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (8, N'A008', NULL, N'download Report', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (9, N'A009', NULL, N'save', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (10, N'A010', NULL, N'reprocess', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (11, N'A011', NULL, N'remarks', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (12, N'A012', NULL, N'view Passbook', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (13, N'A013', NULL, N'download Passbook', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (14, N'A014', NULL, N'download Trust Passbook', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (15, N'A015', NULL, N'Manual Verification', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
INSERT [admin].[menu_action_master] ([action_id], [alias], [action_type], [action_name], [active_status], [created_on]) VALUES (16, N'A016', NULL, N'Go to Manual Verification', 1, CAST(N'2023-09-21T12:44:22.733' AS DateTime))
SET IDENTITY_INSERT [admin].[menu_action_master] OFF

