using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface IUploadDocumentDataActions
    {
        string UploadFile(IFormFile displayName,string storedName);
        string FileSizevalidationError(IFormFile displayName);
        FileStreamResult DownloadFile(string? path);
    }
}