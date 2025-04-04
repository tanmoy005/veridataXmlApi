--USE [veridataDev]
GO
TRUNCATE TABLE [admin].[escalation_case_master]
SET IDENTITY_INSERT [admin].[escalation_case_master] ON 

INSERT [admin].[escalation_case_master] ([case_id], [setup_code], [setup_desc], [setup_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'NOLINK', N'Link could not be sent', N'NRML', 1, 1, CAST(N'2023-07-06T11:16:11.040' AS DateTime), NULL, NULL)
INSERT [admin].[escalation_case_master] ([case_id], [setup_code], [setup_desc], [setup_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'NORES', N'No response – link not used', N'NRML', 1, 1, CAST(N'2023-07-06T11:16:11.057' AS DateTime), NULL, NULL)
INSERT [admin].[escalation_case_master] ([case_id], [setup_code], [setup_desc], [setup_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'NOSUB', N'Response but no submission', N'NRML', 1, 1, CAST(N'2023-07-06T11:16:11.060' AS DateTime), NULL, NULL)
INSERT [admin].[escalation_case_master] ([case_id], [setup_code], [setup_desc], [setup_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'DOJ1W', N'Under process and DOJ critical', N'CRITCL', 1, 1, CAST(N'2023-07-06T11:16:11.060' AS DateTime), NULL, NULL)
INSERT [admin].[escalation_case_master] ([case_id], [setup_code], [setup_desc], [setup_alias], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'DOJ2W', N'Under process and DOJ 2 week left', N'CRITCL', 0, 1, CAST(N'2023-07-06T11:16:11.060' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [admin].[escalation_case_master] OFF
