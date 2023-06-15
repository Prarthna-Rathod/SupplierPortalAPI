using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface ISendEmailService
    {
       bool SendMailRequest(List<string> toEmail, string subject, string body, string? cc, string? bcc, string? filePath);
    }
}
