using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISendEmailService
    {
        bool SendMail(List<string> contactsEmails, string? ccMail, string? bccMail, string body, string subject, string? filePath);
    }
}
