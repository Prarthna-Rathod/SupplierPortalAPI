using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJwtTokenService
    {
        string LoginAndTokenGeneration(string emailId, string password);
    }
}
