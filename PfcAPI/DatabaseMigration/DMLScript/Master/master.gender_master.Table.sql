USE [veridataDev]
GO
TRUNCATE TABLE [master].[gender_master]

SET IDENTITY_INSERT [master].[gender_master] ON 

INSERT [master].[gender_master] ([gender_id], [gender_name], [gender_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'MALE', N'M', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[gender_master] ([gender_id], [gender_name], [gender_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'FEMALE', N'F', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[gender_master] ([gender_id], [gender_name], [gender_code], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'OTHERS', N'T', 1, 1, CAST(N'2022-12-07T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[gender_master] OFF
