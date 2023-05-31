using Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public class SendEmailService : ISendEmailService
    {
        public bool SendMail(List<string> contactsEmails, string? ccMail, string? bccMail, string body, string subject, string? filePath)
        {
            var senderEmailId = new MailAddress("rathodprathna11@gmail.com", "Supplier Portal");
            var password = "ksueqvunieuxcuye";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmailId.Address, password)
            };

            var message = new MailMessage();
            message.From = senderEmailId;
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            message.Body = message.Body.Replace("<<senderEmailId>>", senderEmailId.Address.ToString());

            foreach (var singleEmailAddress in contactsEmails)
            {
                var receiverEmailId = new MailAddress(singleEmailAddress.ToString(), subject);
                message.To.Add(receiverEmailId);
            }

            if (ccMail != null)
            {
                message.CC.Add(ccMail);
            }

            if (bccMail != null)
            {
                message.Bcc.Add(bccMail);
            }

            if (filePath != null)
            {
                FileInfo file = new FileInfo(filePath);
                string fileName = Path.GetFileName(file.Name);
                message.Attachments.Add(new Attachment(file.OpenRead(), fileName));
            }

            smtp.Send(message);

            return true;
        }
    }
}
