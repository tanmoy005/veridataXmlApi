USE [veridataDev]
GO
TRUNCATE TABLE [admin].[role_user_mapping]
SET IDENTITY_INSERT [admin].[role_user_mapping] ON 

INSERT [admin].[role_user_mapping] ([role_user_map_id], [user_id], [role_id], [active_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [user_id1]) VALUES (1, 1, 1, 1, 1, CAST(N'2023-07-06T14:28:15.537' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[role_user_mapping] ([role_user_map_id], [user_id], [role_id], [active_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [user_id1]) VALUES (2, 2, 2, 1, 1, CAST(N'2023-07-06T14:28:22.577' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[role_user_mapping] ([role_user_map_id], [user_id], [role_id], [active_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [user_id1]) VALUES (3, 3, 6, 1, 1, CAST(N'2023-09-22T11:24:27.877' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[role_user_mapping] ([role_user_map_id], [user_id], [role_id], [active_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [user_id1]) VALUES (4, 4, 3, 1, 1, CAST(N'2023-09-22T11:28:25.630' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[role_user_mapping] ([role_user_map_id], [user_id], [role_id], [active_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [user_id1]) VALUES (5, 5, 7, 1, 1, CAST(N'2023-09-22T11:29:25.453' AS DateTime), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [admin].[role_user_mapping] OFF
