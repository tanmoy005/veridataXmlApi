--USE [VeridataDev_New]
GO

Truncate TABLE [master].[api_type_mapping]
SET IDENTITY_INSERT [master].[api_type_mapping] ON 

INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (1, N'AdharGenerateOTP', N'https://hub.perfios.com/', N'api/kyc/v3/aadhaar-xml/otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (1, N'AdharVerifyOTP', N'https://hub.perfios.com/', N'api/kyc/v3/aadhaar-xml/file', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (2, N'PanDetails', N'https://hub.perfios.com/', N'api/kyc/v3/pan-profile-detailed', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (3, N'PassportDetails', N'https://hub.perfios.com/', N'api/kyc/v3/passport-verification', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (4, N'FindUan', N'https://hub.perfios.com/', N'api/kyc/v2/employment-verification-advanced', 0, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (5, N'UanGenerateOTP', N'https://hub.perfios.com/', N'api/kyc/v2/epf-get-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (6, N'AdharGenerateOTP', N'https://sandbox.surepass.io/', N'api/v1/aadhaar-v2/generate-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (6, N'AdharVerifyOTP', N'https://sandbox.surepass.io/', N'api/v1/aadhaar-v2/submit-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (7, N'PanDetails', N'https://sandbox.surepass.io/', N'api/v1/pan/pan-comprehensive', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (8, N'PassportDetails', N'https://sandbox.surepass.io/', N'api/v1/passport/passport/passport-details', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (9, N'FindUan', N'https://sandbox.surepass.io/', N'api/v1/income/epfo/aadhaar-to-uan', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (10, N'UanGenerateOTP', N'https://sandbox.surepass.io/', N'api/v1/income/epfo/passbook/generate-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (10, N'UanSubmitOTP', N'https://sandbox.surepass.io/', N'api/v1/income/epfo/passbook/submit-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (10, N'UanPassbook', N'https://sandbox.surepass.io/', N'api/v1/income/epfo/passbook/get-passbook', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 3)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (11, N'PanDetails', N'https://api-preproduction.signzy.app/', N'api/v3/pan/fetchV2', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (12, N'PassportDetails', N'https://api-preproduction.signzy.app/', N'api/v3/passport/fetches', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (13, N'FindUan', N'https://api-preproduction.signzy.app/', N'api/v3/api/basic-employment-verification', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (14, N'UanGenerateOTP', N'https://api-preproduction.signzy.app/', N'api/v3/underwriting/get-passbook-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (14, N'UanSubmitOTP', N'https://api-preproduction.signzy.app/', N'api/v3/underwriting/submit-passbook-otp', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (14, N'UanPassbook', N'https://api-preproduction.signzy.app/', N'api/v3/get-passbook', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (15, N'UanValidation', N'https://hub.perfios.com/', N'api/kyc/v3/epf-auth', 0, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (15, N'EPFO Past Employment History', N'https://api-preproduction.signzy.app/', N'api/v3/underwriting/fetch-employment-history', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 2)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (1, N'AadharMobileLink', N'https://hub.perfios.com/', N'api/kyc/v3/aadhaar-mobilelink', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (4, N'FindUan', N'https://hub.perfios.com/', N'api/kyc/v2/uan-lookup', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
INSERT [master].[api_type_mapping] ([api_type_id], [api_name], [api_base_url], [api_url], [active_status], [created_on], [created_by], [api_Prioirty]) VALUES (5, N'UanSubmitOTP', N'https://hub.perfios.com/', N'api/kyc/v2/epf-get-passbook', 1, CAST(N'2023-09-05T09:56:19.650' AS DateTime), 1, 1)
SET IDENTITY_INSERT [master].[api_type_mapping] OFF
