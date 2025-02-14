--USE [veridataDev]
GO
TRUNCATE TABLE [activity].[activity_master]
GO
SET IDENTITY_INSERT [activity].[activity_master] ON 

INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'LOGIN', N'Login', N'Login', N'User Login', N'User LoggedIn', N'#9A208C', 1, 1, CAST(N'2023-07-06T11:12:17.417' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'DTDRFT', N'Info', N'Data Drafted', N'User draft details', N'User Info Drafted', N'#8569DD', 1, 1, CAST(N'2023-07-06T11:12:17.443' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'DTSAVE', N'Info', N'Data Saved', N'User Info saved', N'User Info Saved', N'#4887e9', 1, 1, CAST(N'2023-07-06T11:12:17.443' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'ADHVSTRT', N'Aadhaar', N'On-process', N'Verification  process initiated', N'Aadhaar Verification Started', N'#FFAE1F', 1, 1, CAST(N'2023-07-06T11:12:17.443' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'ADHVCMPT', N'Aadhaar', N'Verified', N'Verification completed', N'Aadhaar Verified', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.447' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'ADHVFALD', N'Aadhaar', N'Failed', N'Site unreachable', N'Aadhaar Site Unreachable', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.447' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (7, N'DTSBMT', N'Info', N'Data Submitted', N'User Info submitted', N'User Info Submitted', N'#00b57c', 1, 1, CAST(N'2023-07-06T11:12:17.447' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (8, N'ADHDTVFLD', N'Aadhaar', N'Failed', N'Verification failed', N'Aadhaar Verification Failed', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.447' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (9, N'UANFTCH', N'UAN', N'Fecth', N'UAN fetched  succesfully', N'UAN Fetched', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.447' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (10, N'NOUAN', N'UAN', N'Fecth', N'No UAN available', N'No UAN Fetched', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (11, N'UANVSTRT', N'UAN', N'On-process', N'Verification  process initiated', N'UAN Verification Started', N'#FFAE1F', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (12, N'UANVCMPT', N'UAN', N'Verified', N'Verification completed', N'UAN Verified', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (13, N'UANVFALD', N'UAN', N'Failed', N'Site unreachable', N'UAN Site Unreachable', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (14, N'UANDTVFLD', N'UAN', N'Failed', N'Verification failed', N'UAN Verification Failed', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (15, N'UANDTVFLD', N'PF Passbook', N'Fecth', N'Fetch passbook data', N'Fetch Passbook Data', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (16, N'VRCMPLT', N'User', N'Verified', N'Verification successfull', N'User Verified', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (17, N'PSVCMPT', N'Passport', N'Verified', N'Verification successfull', N'Passport Verified', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.453' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (18, N'PSVFALD', N'Passport', N'Failed', N'Site unreachable', N'Passport Site Unreachable', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.457' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (19, N'PSVSTRT', N'Passport', N'On-process', N'Verification  process initiated', N'Passport Verification Started', N'#FFAE1F', 1, 1, CAST(N'2023-07-06T11:12:17.457' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (20, N'PSDTVFLD', N'Passport', N'Failed', N'Verification failed', N'Passport Verification Failed', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.457' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (21, N'PANVSTRT', N'PAN', N'On-process', N'Verification  process initiated', N'PAN Verification Started', N'#FFAE1F', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (22, N'PANVCMPT', N'PAN', N'Verified', N'Verification completed', N'PAN Verified', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (23, N'PANVFALD', N'PAN', N'Failed', N'Site unreachable', N'PAN Site Unreachable', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (24, N'PANDTVFLD', N'PAN', N'Failed', N'Verification failed', N'PAN Verification Failed', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (25, N'ADHINVLD', N'Aadhaar', N'Failed', N'Invalid Aadhaar', N'Invalid Aadhaar', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (26, N'CNSTGVN', N'Consent', N'Given', N'Consent Given', N'Consent Given', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (27, N'CNSTDCLN', N'Consent', N'Declined', N'Consent Decline', N'Consent Decline', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (28, N'CNSTRVK', N'Consent', N'Revoked', N'Consent Revoked', N'Consent Revoked', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (29, N'DCRUPLD', N'Doc', N'Reupload', N'Re Uploaded', N'Re Upload of Document ', N'#8569DD', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (30, N'MNFTRVCMPT', N'Father', N'Verified', N'Verification completed', N'Manual Fathers Veri', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (31, N'MNFTHRVFLD', N'Father', N'Failed', N'Verification completed', N'Manual Fathers Veri', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (32, N'MNUANVCMPT', N'UAN', N'Verified', N'Verification completed', N'Manual UAN Veri', N'#6EA191', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (33, N'MNUANVFLD', N'UAN', N'Failed', N'Verification completed', N'Manual UAN Veri', N'#E06666', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (34, N'DCRUPLDRQ', N'Doc', N'Reupload', N'Re Uploaded Request', N'Re Upload of Document Requested', N'#9A208C', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)
INSERT [activity].[activity_master] ([activity_id], [activity_code], [activity_type], [activity_name], [activity_info], [activity_desc], [activity_color], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (35, N'MNLVERREQ', N'Manual', N'Verification', N'Manual Verification Request', N'Manual Verification Request', N'#9A208C', 1, 1, CAST(N'2023-07-06T11:12:17.450' AS DateTime), NULL, NULL)

SET IDENTITY_INSERT [activity].[activity_master] OFF

