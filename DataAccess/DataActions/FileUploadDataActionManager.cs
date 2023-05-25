using DataAccess.DataActions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.DataActions
{
    public class FileUploadDataActionManager : IFileUploadDataActions
    {
        public string UploadReportingPeriodDocument(IFormFile file, string storedName)
        {
            var filepath = Path.Combine("E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles", storedName);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filepath;
        }

        public FileContentResult DownloadDocument(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            var fileName = file.Name;
            var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            //System.Net.Mime.MediaTypeNames.Application.Octet for contentType
            FileContentResult result = new FileContentResult
            (System.IO.File.ReadAllBytes(downloadPath), System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = fileName
            };

            return result;
        }

        public bool RemoveDocumentFromFolder(string path)
        {
            //Delete file from "DocumentFiles" folder
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            else
                return false;
        }

    }
}
