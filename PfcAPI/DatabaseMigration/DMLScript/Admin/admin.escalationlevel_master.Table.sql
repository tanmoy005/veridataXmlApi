--USE [veridataDev]
TRUNCATE TABLE [admin].[escalationlevel_master]
GO
SET IDENTITY_INSERT [admin].[escalationlevel_master] ON 

INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (1, N'Level 1', N'LVL1', 9, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), 1, CAST(N'2023-09-01T16:50:02.683' AS DateTime), N'NRML')
INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (2, N'Level 2', N'LVL2', 15, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), NULL, NULL, N'NRML')
INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (3, N'Level 3', N'LVL3', 15, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), NULL, NULL, N'NRML')
INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (4, N'Level 1', N'LVL1', 10, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), NULL, NULL, N'CRITCL')
INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (5, N'Level 2', N'LVL2', 17, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), 1, CAST(N'2023-08-31T12:55:43.530' AS DateTime), N'CRITCL')
INSERT [admin].[escalationlevel_master] ([level_id], [level_name], [level_code], [no_of_days], [active_status], [created_by], [created_on], [updated_by], [updated_on], [setup_alias]) VALUES (6, N'Level 3', N'LVL3', 17, 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), 1, CAST(N'2023-08-31T12:55:43.530' AS DateTime), N'CRITCL')
SET IDENTITY_INSERT [admin].[escalationlevel_master] OFF

