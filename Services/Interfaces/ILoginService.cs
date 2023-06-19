using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILoginService
    {
        string Login(string emailId, string password);
        string GetSupplierName(string emailId);
    }
}
