using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface IFileUploadDataActions
    {
        string UploadReportingPeriodDocument(IFormFile file, string storedName);
        FileContentResult DownloadDocument(string filePath);
        bool RemoveDocumentFromFolder(string path);
    }
}
