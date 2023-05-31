using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISendEmailServices
    {
        bool SendEmailRequest(List<string> emails, string subject, string body, string? cc, string? bcc, string? filePath);
    }
}
