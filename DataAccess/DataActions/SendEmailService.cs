using DataAccess.DataActions.Interfaces;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace DataAccess.DataActions
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;
        public SendEmailService(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }
        public bool SendMailRequest(List<string> toEmail, string subject, string body, string? cc, string? bcc, string? filePath)
        {
            MailMessage mailMessage = new MailMessage();
            var fromEmail = _configuration["SendGridEmailSettings:FromEmail"];
            var fromName = _configuration["SendGridEmailSettings:FromName"];
            if (cc != null)
            {
                string[] CCId = cc.Split(',');
                foreach (string CCEmail in CCId)
                {
                    mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
                }
            }

            if (bcc != null)
            {
                string[] bccid = bcc.Split(',');
                foreach (string bccEmailId in bccid)
                {
                    mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
                }
            }

            if (filePath != null)
            {
                FileInfo file = new FileInfo(filePath);
                string fileName = Path.GetFileName(file.Name);
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(file.OpenRead(), fileName));
            }
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                HtmlContent = body,

            };
            foreach (var email in toEmail)
            {
                msg.AddTo(new EmailAddress(email));
            }
            var response =  _sendGridClient.SendEmailAsync(msg);
            return true;
        }
    }
}
