--USE [veridataDev]
--GO
TRUNCATE TABLE [master].[faq_master]
SET IDENTITY_INSERT [master].[faq_master] ON 

INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'How to contact PwC Admin / HC?', N'Ans) If you are having queries / need further help / want to update your name, email ID, phone number,  
please get connected to PwC HC - you can either call HC or email them at in_hc_cw_ops@pwc.com.
Please mention  "Veridata_Queries_(candidate ID)" in subject line.', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'What is Trust / Private Provident Fund (PF)?', N'RPFC stands for Regional Provident Fund Commissioner, who is in charge of a region in which an establishment is covered under the Provident Fund Act. Organizations can choose to be covered under the RPFC or have their own setup (Trust).
A private provident fund (PF) trust is a company-formed trust that manages the PF accounts of its employees instead of sending them to the Employees Provident Fund Organisation (EPFO). The EPFO grants exemptions to companies to form their own PF trusts.', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'What is significance of PAN Card?', N'A PAN (Permanent Account Number) Card is a unique 10-character identification document issued by the Income Tax Department of India. It serves as a critical financial identity for tracking tax responsibilities and is essential for major financial activities like opening bank accounts, filing tax returns, applying for loans, and starting employment. Professionals and students can easily obtain a PAN Card by applying online through official websites like NSDL or UTI, submitting necessary identity and address proofs.', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'What is UAN? What is its significance?', N' A UAN (Universal Account Number) is a unique 12-digit identification number assigned to every Employee Provident Fund (EPF) member in India. It helps employees track their provident fund contributions, enables easy online access to EPF accounts, and facilitates seamless transfer of provident fund between jobs. Once generated, the UAN remains constant throughout an individuals working career, regardless of job changes. Employees can activate their UAN online, link it with their Aadhaar and PAN, and use it to check balance, download statements, and manage their provident fund contributions through the EPFO portal or mobile app.', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (5, N'Why is it important to read the "i"/information section?', N'You can find "i" / information sections in various pages   especially beside Headings and Sub-headings of sections and pages. Please read the "i" carefully, wherever you see them.
The "i" or information icon in an app is more than just a simple guide - its a key resource that provides essential context about the page or features purpose, significance, and critical usage instructions, For newcomers and experienced users alike.
These sections help users understand not just how to use the tool, but why it exists, what problems it solves, and insyructions of application workflow.', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[faq_master] OFF
