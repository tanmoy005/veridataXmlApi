USE [veridataDev]
GO

TRUNCATE TABLE [admin].[user_master]
SET IDENTITY_INSERT [admin].[user_master] ON 

INSERT [admin].[user_master] ([user_id], [ref_appointee_id], [user_code], [users_name], [date_of_birth], [email_id], [contact_no], [user_type_id], [role_id], [active_status], [cur_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [candidate_id]) VALUES (1, NULL, N'SysAdmin', N'Company Admin', CAST(N'1887-12-30T00:00:00.000' AS DateTime), N'pfcsrver005@gmail.com
', NULL, 1, 1, 1, 1, 1, CAST(N'2022-12-13T00:00:00.000' AS DateTime), 0, CAST(N'2023-09-21T15:58:45.813' AS DateTime), NULL, NULL)
INSERT [admin].[user_master] ([user_id], [ref_appointee_id], [user_code], [users_name], [date_of_birth], [email_id], [contact_no], [user_type_id], [role_id], [active_status], [cur_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [candidate_id]) VALUES (2, NULL, N'CcAdmin', N'Company Admin', CAST(N'1990-10-21T00:00:00.000' AS DateTime), N'pfcsrver005@gmail.com
', N'8756226844', 1, 2, 1, 1, 1, CAST(N'2022-12-13T00:00:00.000' AS DateTime), 1, CAST(N'2023-09-21T16:25:22.170' AS DateTime), NULL, NULL)
INSERT [admin].[user_master] ([user_id], [ref_appointee_id], [user_code], [users_name], [date_of_birth], [email_id], [contact_no], [user_type_id], [role_id], [active_status], [cur_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [candidate_id]) VALUES (3, NULL, N'HrAdmin', N'HR Admin', NULL, N'pfcsrver005@gmail.com', N'9038666441', 1, 6, 1, 1, 1, CAST(N'2023-09-22T11:24:27.853' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[user_master] ([user_id], [ref_appointee_id], [user_code], [users_name], [date_of_birth], [email_id], [contact_no], [user_type_id], [role_id], [active_status], [cur_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [candidate_id]) VALUES (4, NULL, N'GAdmin', N'General User', NULL, N'veridata24x7@gmail.com', N'7894561230', 1, 3, 1, 1, 1, CAST(N'2023-09-22T11:28:25.627' AS DateTime), NULL, NULL, NULL, NULL)
INSERT [admin].[user_master] ([user_id], [ref_appointee_id], [user_code], [users_name], [date_of_birth], [email_id], [contact_no], [user_type_id], [role_id], [active_status], [cur_status], [created_by], [created_on], [updated_by], [updated_on], [role_id1], [candidate_id]) VALUES (5, NULL, N'FinAdmin', N'Finance User', NULL, N'veridata24x7@gmail.com', N'1230987645', 1, 7, 1, 1, 1, CAST(N'2023-09-22T11:29:25.437' AS DateTime), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [admin].[user_master] OFF
