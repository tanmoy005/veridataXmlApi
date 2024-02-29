using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Reflection;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.Configuration;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using VERIDATA.Model.Table.Public;
using static VERIDATA.BLL.utility.CommonEnum;
//using System.Xml;

namespace VERIDATA.BLL.Notification.Sender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IAppointeeDalContext _appointeeContext;
        private readonly IMasterDalContext _masterDataContext;
        public EmailSender(EmailConfiguration emailConfig, IAppointeeDalContext appointeeContext, IMasterDalContext masterDataContext)
        {
            _emailConfig = emailConfig;
            _appointeeContext = appointeeContext;
            _masterDataContext = masterDataContext;
        }

        public async Task SendAppointeeLoginMail(List<CreateUserDetailsRequest> UserList, string type)
        {
            foreach (CreateUserDetailsRequest? obj in UserList)
            {
                MailBodyParseDataDetails parsedata = new()
                {
                    Name = obj?.UserName,
                    UserCode = obj?.UserCode,
                    Password = obj?.Password?.ToString() ?? string.Empty,
                    Url = _emailConfig.HostUrl,
                };
                MailDetails _mailDetails = new();
                List<EmailAddress> ToList = new();
                EmailAddress _emailadd = new()
                {
                    DisplayName = obj?.UserName ?? string.Empty,
                    Address = obj?.EmailId ?? string.Empty
                };
                ToList.Add(_emailadd);
                _mailDetails.ToEmailList = ToList;
                _mailDetails.MailType = type;
                _mailDetails.ParseData = parsedata;

                MailDetails mailDetails = GenarateMailText(_mailDetails);
                Message message = new(mailDetails.ToEmailList, mailDetails.ToCcEmailList, mailDetails.Subject, mailDetails.BodyDetails);
                await SendEmailAsync(message, null);
            }
        }
        public async Task SendNotificationMailToEmployer(int appointeeId, string? reason, string type)
        {
            AppointeeDetails? _appointeedetails = await _appointeeContext.GetAppinteeDetailsById(appointeeId);
            string _appointeeName = _appointeedetails?.AppointeeName ?? string.Empty;
            string _candidateId = _appointeedetails?.CandidateId ?? string.Empty;
            string _companyName = _appointeedetails?.CompanyName ?? string.Empty;
            if (_appointeedetails == null)
            {
                UnderProcessFileData? _appntundrprocessdata = await _appointeeContext.GetUnderProcessAppinteeDetailsById(appointeeId);
                _appointeeName = _appntundrprocessdata?.AppointeeName ?? string.Empty;
                _candidateId = _appntundrprocessdata?.CandidateId ?? string.Empty;
                _companyName = _appntundrprocessdata?.CompanyName ?? string.Empty;
            }
            List<EscalationLevelMasterDataResponse> leveldatares = await _masterDataContext.GetEscalationLevelMasterData();

            List<string?> levelGroupEmailList = leveldatares.Select(x => x.Emailaddress).Distinct().ToList();

            MailBodyParseDataDetails ParseData = new()
            {
                Name = _appointeeName,
                CandidateId = _candidateId,
                Reason = reason
            };

            MailDetails mailDetails = new()
            {
                ToEmailList = new List<EmailAddress?>(),
                ToCcEmailList = new List<EmailAddress?>(),
                ParseData = new MailBodyParseDataDetails()
            };

            foreach (string? mailobj in levelGroupEmailList)
            {
                if (!string.IsNullOrEmpty(mailobj))
                {
                    EmailAddress _emailadd = new()
                    {
                        DisplayName = _companyName,
                        Address = mailobj
                    };
                    mailDetails.ToEmailList.Add(_emailadd);
                }
            }
            mailDetails.MailType = type;
            mailDetails.ParseData = ParseData;
            if (mailDetails.ToEmailList.Count > 0)
            {
                await SendMail(mailDetails);
            }
        }
        public async Task SendAppointeeMail(string emailId, MailDetails mailDetails)
        {
            mailDetails.ToEmailList = new List<EmailAddress?>();
            mailDetails.ToCcEmailList = new List<EmailAddress?>();
            EmailAddress _emailadd = new()
            {
                DisplayName = mailDetails.ParseData?.Name,
                Address = emailId
            };
            mailDetails.ToEmailList.Add(_emailadd);

            await SendMail(mailDetails);
        }
        public async Task SendMail(MailDetails mailDetails)
        {
            MailDetails generateMailDetails = GenarateMailText(mailDetails);
            Message message = new(mailDetails.ToEmailList, mailDetails.ToCcEmailList, generateMailDetails.Subject, generateMailDetails.BodyDetails);
            await SendEmailAsync(message, null);
        }
        private MailDetails GenarateMailText(MailDetails mailDetails)
        {
            string _filename = string.Empty;
            string validtionType = string.Empty;
            string mailSubject = string.Empty;
            switch (mailDetails.MailType)
            {
                case MailType.Reprocess:
                    _filename = "userReprocess";
                    mailSubject = "VERIDATA: Candidate Reprocessed";
                    break;
                case MailType.AdhrValidation:
                    _filename = "userValidityRemarks";
                    validtionType = "Aadhaar";
                    mailSubject = "VERIDATA: Aadhaar Verification";
                    break;
                case MailType.UANValidation:
                    _filename = "userValidityRemarks";
                    validtionType = "UAN";
                    mailSubject = "VERIDATA: UAN Verification";
                    break;
                case MailType.Passport:
                    _filename = "userValidityRemarks";
                    validtionType = "Passport";
                    mailSubject = "VERIDATA: Passport Verification";
                    break;
                case MailType.Remainder:
                    _filename = "userRemainder";
                    mailSubject = "VERIDATA: Reminder Alert";
                    break;
                case MailType.ForceApprove:
                    _filename = "userForcedApproved";
                    mailSubject = "VERIDATA: Forced Approval";
                    break;
                case MailType.Reject:
                    _filename = "userReject";
                    mailSubject = "VERIDATA: Verification Application Rejected ";
                    break;
                case MailType.CandidateCreate:
                    _filename = "newusermailsend";
                    mailSubject = "VERIDATA: User Creation";
                    break;
                case MailType.CandidateUpdate:
                    _filename = "updateusermailsend";
                    mailSubject = "VERIDATA: User Creation";
                    break;
                case MailType.SendOTP:
                    _filename = "userOtpAuth";
                    mailSubject = "VERIDATA: User Authentication ";
                    break;

            }
            MailBodyParseDataDetails? parsedata = mailDetails.ParseData;
            parsedata.Type = validtionType;
            string? mailtext = ParseMailMessage(_filename, parsedata);
            MailDetails ParsedMailDetails = new()
            {
                BodyDetails = mailtext,
                Subject = mailSubject,
                ToEmailList = mailDetails.ToEmailList ?? new List<EmailAddress?>(),
                ToCcEmailList = mailDetails.ToCcEmailList ?? new List<EmailAddress?>(),
                ParseData = parsedata ?? new MailBodyParseDataDetails(),

            };
            return ParsedMailDetails;
        }

        public async Task SendMailWithAttachtment(string userName, string emailId, List<Filedata> attachment, ValidationType type)
        {
            EmailAddress _emailadd = new()
            {
                DisplayName = userName,
                Address = emailId
            };

            MailDetails mailDetails = GenarateMailTextByValidatinType(type);
            mailDetails.ToEmailList.Add(_emailadd);

            Message message = new(mailDetails.ToEmailList, mailDetails.ToCcEmailList, mailDetails.Subject, mailDetails.BodyDetails);

            await SendEmailAsync(message, attachment);
        }
        private static MailDetails GenarateMailTextByValidatinType(ValidationType type)
        {
            string mailSubject = string.Empty;

            string mailtext;
            switch (type)
            {
                case ValidationType.Invalid:
                    mailtext = $"{"The invalid data details from the latest appointee data upload, is present in the attached excel report." +
                        " Please do the required rectification and reupload these appointee data."}";
                    mailSubject = "VERIDATA: Appointee Data Upload Failure List";
                    break;
                case ValidationType.Duplicate:
                    mailtext = $"{"The appointee details from the latest data upload, present in the attached excel report, are duplicate and are already present in VERIDATA." +
                        " Hence were not uploaded."}";
                    mailSubject = "VERIDATA: Duplicate Appointee Data Rejected";
                    break;
                case ValidationType.ApiCount:
                    mailtext = $"{"Daily API count data is attached in the excel report. Please go throuth it."}";
                    mailSubject = "VERIDATA REPORT: Total API Hit Count";
                    break;
                case ValidationType.AppointeeCount:
                    mailtext = $"{"Appointee uploded count data is attached in the excel report. Please go throuth it."}";
                    mailSubject = "VERIDATA REPORT: Total Appointee Upload Count";
                    break;
                case ValidationType.Critical1week:
                    mailtext = $"{"Only one week left to the Date of Joining of the appointees mentioned in the attached excel report. Please notify these candidate from VERIDATA app."}";
                    mailSubject = "VERIDATA REPORT: 1 Week Left to DOJ";
                    break;
                case ValidationType.Critical2week:
                    mailtext = $"{"Only two week left to the Date of Joining of the appointees mentioned in the attached excel report. Please notify these candidate from VERIDATA app."}";
                    mailSubject = "VERIDATA REPORT: 2 Weeks Left to DOJ";
                    break;
                case ValidationType.NoLinkSent:
                    mailtext = $"{"VERIDATA link has not been yet sent to the appointees, mentioned in the excel report." +
                        " Please note : 'The verification process has not been started till the verification lik sent'."}";
                    mailSubject = "VERIDATA REPORT: Link Not Sent";
                    break;
                case ValidationType.NoResponse:
                    mailtext = $"{"Appointees mentioned in the attached excel report, have not initiated the verification process yet. Please notify these candidate from VERIDATA app."}";
                    mailSubject = "VERIDATA REPORT: No Response from Appointees";
                    break;
                case ValidationType.Processing:
                    mailtext = $"{"Appointee mentioned in the verification excel report, have done some activity but not completed the verification process completely. Please notify these candidate from VERIDATA app."}";
                    mailSubject = "VERIDATA REPORT:Verification Onging Appointees";
                    break;
                case ValidationType.NonExsist:
                    mailtext = $"{"The appointee details from the latest Update data upload, present in the attached excel report, are not present in VERIDATA." +
                        " Hence were not updated."}";
                    mailSubject = "VERIDATA: Non Exsist Appointee Data Rejected";
                    break;
                default:
                    mailtext = string.Empty;
                    break;

            }
            MailDetails mailDetails = new()
            {
                BodyDetails = mailtext,
                Subject = mailSubject,
                ToEmailList = new List<EmailAddress?>(),
                ToCcEmailList = new List<EmailAddress?>(),
                ParseData = new MailBodyParseDataDetails()
            };
            return mailDetails;
        }
        private string ParseMailMessage<T1>(string messageName, T1 payload)
        {
            string content = GetEmbeddedResource($"NotificationTemplates\\{messageName}.txt");
            string msg = CommonUtility.ParseMessage(content, payload);
            return msg;
            //return string.Empty;
        }

        private static string GetEmbeddedResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            resourceName = CommonUtility.FormatResourceName(assembly, resourceName);
            //var names = this.GetType().Assembly.GetManifestResourceNames(resourceName);
            using Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                return string.Empty;
            }

            using StreamReader reader = new(resourceStream);
            return reader.ReadToEnd();
        }

        public async Task SendEmailAsync(Message message, List<Filedata>? attachment)
        {
            MimeMessage emailMessage = CreateEmailMessage(message, attachment);
            await Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message, List<Filedata> attachmentdata)
        {
            BodyBuilder builder = new()
            {
                HtmlBody = message.Content
            };
            if (attachmentdata != null)
            {
                foreach (Filedata? filedata in attachmentdata)
                {
                    if (!string.IsNullOrEmpty(filedata?.FileName))
                    {
                        string _fileName = string.Concat(filedata?.FileName, ".", filedata?.FileType);
                        _ = builder.Attachments.Add(_fileName, filedata?.FileData);
                        //builder.Attachments.Add();
                    }
                }
            }
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderId));
            emailMessage.To.AddRange(message.To);
            emailMessage.Cc.AddRange(message.Cc);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = builder.ToMessageBody();

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using SmtpClient client = new();
            try
            {

                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                // client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.Username, _emailConfig.Password);

                //client.Send(mailMessage);
                _ = await client.SendAsync(mailMessage);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}