USE [veridataDev]
GO

TRUNCATE TABLE [admin].[user_authentication]
SET IDENTITY_INSERT [admin].[user_authentication] ON 

INSERT [admin].[user_authentication] ([user_autho_id], [user_id], [user_pwd], [user_pwd_txt], [is_default_pass], [active_status], [created_by], [created_on], [updated_by], [updated_on], [user_id1]) VALUES (1, 1, N'/bBGszESlD6m7fOTx/gfI2CvOaqbaVkYr61FnGhg98Y=', N'6KGS9N93', N'Y', 1, 1, CAST(N'2023-07-06T14:26:36.777' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[user_authentication] ([user_autho_id], [user_id], [user_pwd], [user_pwd_txt], [is_default_pass], [active_status], [created_by], [created_on], [updated_by], [updated_on], [user_id1]) VALUES (2, 2, N'/bBGszESlD6m7fOTx/gfI2CvOaqbaVkYr61FnGhg98Y=', N'6KGS9N93', N'Y', 1, 1, CAST(N'2023-07-06T14:26:46.467' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[user_authentication] ([user_autho_id], [user_id], [user_pwd], [user_pwd_txt], [is_default_pass], [active_status], [created_by], [created_on], [updated_by], [updated_on], [user_id1]) VALUES (3, 3, N'dnaqr7AnyCW9mrq3iyNAcOcCdS9iW3UuVeVbSOYH41g=', N'admin@123', N'Y', 1, 1, CAST(N'2023-09-22T11:24:27.877' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[user_authentication] ([user_autho_id], [user_id], [user_pwd], [user_pwd_txt], [is_default_pass], [active_status], [created_by], [created_on], [updated_by], [updated_on], [user_id1]) VALUES (4, 4, N'dnaqr7AnyCW9mrq3iyNAcOcCdS9iW3UuVeVbSOYH41g=', N'admin@123', N'Y', 1, 1, CAST(N'2023-09-22T11:28:25.630' AS DateTime), NULL, NULL, NULL)
INSERT [admin].[user_authentication] ([user_autho_id], [user_id], [user_pwd], [user_pwd_txt], [is_default_pass], [active_status], [created_by], [created_on], [updated_by], [updated_on], [user_id1]) VALUES (5, 5, N'dnaqr7AnyCW9mrq3iyNAcOcCdS9iW3UuVeVbSOYH41g=', N'admin@123', N'Y', 1, 1, CAST(N'2023-09-22T11:29:25.453' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [admin].[user_authentication] OFF
