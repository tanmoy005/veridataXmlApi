using VERIDATA.Model.DataAccess;
using VERIDATA.Model.ExchangeModels;
using VERIDATA.Model.Request;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Notification.Provider
{
    public interface IEmailSender
    {
        public Task SendAppointeeLoginMail(List<CreateUserDetailsRequest> UserList, string type);

        public Task SendEmailAsync(Message message, List<Filedata> attachment);

        public Task SendAppointeeMail(string emailId, MailDetails mailDetails);

        public Task SendMailWithAttachtment(string userName, string emailId, List<Filedata> attachment, ValidationType type);

        public Task SendNotificationMailToEmployer(int appointeeId, string? reason, string type);
    }
}