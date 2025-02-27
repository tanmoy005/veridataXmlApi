--USE [veridataDev]
--GO
TRUNCATE TABLE [master].[faq_master]
USE [Veridata_qa]
GO
SET IDENTITY_INSERT [master].[faq_master] ON 

INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (1, N'How to contact PwC Admin / HC?', N'If you are having queries / need further help / want to update your name, email ID, phone number, 
please get connected to PwC HC - you can either call HC or email them at in_hc_cw_ops@pwc.com.
Please mention  "Veridata_Queries_(candidate ID)" in subject line.', N'text      ', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (2, N'What is Trust / Private Provident Fund (PF)?', N'RPFC stands for Regional Provident Fund Commissioner, who is in charge of a region in which an establishment is covered under the Provident Fund Act. Organizations can choose to be covered under the RPFC or have their own setup (Trust).
A private provident fund (PF) trust is a company-formed trust that manages the PF accounts of its employees instead of sending them to the Employees Provident Fund Organisation (EPFO). The EPFO grants exemptions to companies to form their own PF trusts.', N'text      ', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (3, N'What is significance of PAN Card?', N'A PAN (Permanent Account Number) Card is a unique 10-character identification document issued by the Income Tax Department of India. It serves as a critical financial identity for tracking tax responsibilities and is essential for major financial activities like opening bank accounts, filing tax returns, applying for loans, and starting employment. Professionals and students can easily obtain a PAN Card by applying online through official websites like NSDL or UTI, submitting necessary identity and address proofs.', N'text      ', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (4, N'What is UAN?  What is its significance?', N'A UAN (Universal Account Number) is a unique 12-digit identification number assigned to every Employee Provident Fund (EPF) member in India. It helps employees track their provident fund contributions, enables easy online access to EPF accounts, and facilitates seamless transfer of provident fund between jobs. Once generated, the UAN remains constant throughout an individuals working career, regardless of job changes. Employees can activate their UAN online, link it with their Aadhaar and PAN, and use it to check balance, download statements, and manage their provident fund contributions through the EPFO portal or mobile app.', N'text      ', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (6, N'Why is it important to read the "i" /information section?


', N'You can find "i" / information sections in various pages   especially beside Headings and Sub-headings of sections and pages. Please read the "i" carefully, wherever you see them.

The "i" or information icon in an app is more than just a simple guide - it''s a key resource that provides essential context about the page or feature''s purpose, significance, and critical usage instructions, For newcomers and experienced users alike.
These sections help users understand not just how to use the tool, but why it exists, what problems it solves, and insyructions of application workflow.', N'text      ', 1, 1, CAST(N'2024-12-01T00:00:00.000' AS DateTime), NULL, NULL)
INSERT [master].[faq_master] ([faq_id], [faq_name], [faq_desc], [txt_type], [active_status], [created_by], [created_on], [updated_by], [updated_on]) VALUES (7, N'ELI Scheme: UAN Activation and seeding Bank Account with AADHAAR for availing the benefits -EPFO', N'<!doctype html>
<html lang="en">
<head>
<style>
<p> 1. In the light of directions received from the Ministry of Labour & Employment, to ensure that all eligible employees benefit from the Employment Linked Incentive (ELI) Scheme announced in Union Budget 2024-25, UAN activation and AADHAAR seeding in Bank Account of each employee are mandatory.
</p> </br> </br>
<p>
2. Every subscriber of EPFO is required to have an AADHAAR linked Universal Account Number (UAN) which needs to be activated by creating login on the Member Portal for availing a number of facilities through a single window. Such facilities include the ability to view and download PF passbooks; submit online claims for withdrawals, advances, or transfers; update personal details and track the status of claims in real time. 
</p> </br> </br>
<p>
3. The activation process is straightforward and can be completed using an Aadhaar-based OTP (One-Time Password) by following the steps given below: </p> </br>
	<p>i. Go to EPFO Member Portal.</p></br>
    <p>ii. Click on the "Activate UAN" link under "Important Links".</p></br>
    <p>iii. Enter your UAN, Aadhaar number, name, DOB, and Aadhaar-linked mobile number.</p></br>
	<p>iv. Employees should ensure their mobile number is Aadhaar-linked to access the full range of EPFO’s digital services.</p></br>
    <p>v. Agree to Aadhaar OTP verification.</p></br>
    <p>vi. Click "Get Authorization PIN" to receive an OTP on your Aadhaar-linked mobile number.</p></br>
    <p>vii. Enter the OTP to complete the activation.</p></br>
    <p>viii. A password will be sent to your registered mobile number upon successful activation.</p>
</p></br></br>
<p>4. Further, for availing the benefits of any Direct Benefit Transfer (DBT) Scheme, the Bank Account number of the beneficiary is required to be seeded with AADHAAR so as to credit the benefits directly into the beneficiary’s bank account.</p> </br></br>
<p>5. Since, the benefits under ELI Scheme, will be disbursed through DBT to eligible employees, Employers are urged to ensure UAN activation and AADHAAR seeding in Bank Account by 30th November 2024, in respect of all their employees who have joined in the current financial year, starting with the latest joinees. The concerned EPFO offices may kindly be contacted for necessary guidance in this matter, if required. 
</p> </br></br>
<p>6. EPFO remains committed to providing social security and retirement benefits to millions of employees across India. By enhancing accessibility through its online service portal, EPFO continues to improve ease of use and operational efficiency.
</p>
</style>
</head>
</html>', N'html      ', 1, 1, CAST(N'2024-12-10T00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [master].[faq_master] OFF
GO
