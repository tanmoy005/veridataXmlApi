--USE [veridataDev]
GO
TRUNCATE TABLE [admin].[generalSetup]
SET IDENTITY_INSERT [admin].[generalSetup] ON 

INSERT [admin].[generalSetup] ([id], [critical_no_days], [grace_period_days], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, 54, 23, 1, 1, CAST(N'2023-07-06T11:56:29.993' AS DateTime), 1, CAST(N'2023-10-04T02:10:24.657' AS DateTime))
SET IDENTITY_INSERT [admin].[generalSetup] OFF
