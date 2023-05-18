using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface IFileUploadDataActions
    {
        string UploadReportingPeriodDocument(IFormFile file);
    }
}
