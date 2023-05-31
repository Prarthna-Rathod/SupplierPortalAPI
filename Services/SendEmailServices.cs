using Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public class SendEmailServices : ISendEmailServices
    {
        public bool SendEmailRequest(List<string> emails, string subject, string body, string? cc, string? bcc, string? filePath)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("dhruvikatrodiya01@gmail.com", "Supplier Portal"); //From Email Id  
            mailMessage.Subject = subject; //Subject of Email  
                                           //mailMessage.Body = emailTemplateEntity.Body.Replace("supplierName",supplierName); 
            mailMessage.Body = body.Replace("<<SenderEmailId>>", mailMessage.From.Address);  //body or message of Email  
            mailMessage.IsBodyHtml = true;

            foreach (string email in emails)
            {
                mailMessage.To.Add(new MailAddress(email)); //adding multiple TO Email Id  
            }

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
                mailMessage.Attachments.Add(new Attachment(file.OpenRead(),fileName));
            }

            SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
            smtp.Host = "smtp.gmail.com";              //host of emailaddress for example smtp.gmail.com etc 
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = "pgmyoejfqylsgcre";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage); //sending Email  

            return true;
        }

    }
}

