--USE [veridataDev]
GO
TRUNCATE TABLE [admin].[escaltionlevel_email_mapping]
GO
SET IDENTITY_INSERT [admin].[escaltionlevel_email_mapping] ON 

INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (1, 1, N'tanmoay@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:55:20.087' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (2, 2, N'sujoyade@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:55:20.087' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (3, 3, N'dibyojit@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (4, 4, N'tanmoay@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:55:20.087' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (5, 5, N'sujoyade@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:55:20.087' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[escaltionlevel_email_mapping] ([map_id], [level_id], [email], [active_status], [created_by], [created_on], [updated_by], [updated_on], [level_id1]) VALUES (6, 6, N'dibyojit@elogixmail.com', 1, 1, CAST(N'2023-07-06T11:40:20.617' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [admin].[escaltionlevel_email_mapping] OFF
