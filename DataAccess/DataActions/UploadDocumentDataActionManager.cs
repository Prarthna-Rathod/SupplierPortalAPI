using DataAccess.DataActions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Cryptography.X509Certificates;

namespace DataAccess.DataActions
{
    public class UploadDocumentDataActionManager : IUploadDocumentDataActions
    {
        public string UploadFile(IFormFile displayName,string storedName)
        {
            string path = Path.Combine("E:\\SupplierPortalAPI Repo\\DataAccess\\UploadedFiles", storedName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                displayName.CopyTo(fileStream);
            }
            return path;
        }

        public string FileSizevalidationError(IFormFile displayName)
        {
            string? error = null;
            //var totalBytes = 14000;
            var totalBytes = 20971520; //20 mb
            var uploadedBytes = displayName.Length;

            if (uploadedBytes >= totalBytes)
            {
                error = "FileSize Should be <= 20MB..!";
            }
            return error;
        }

        public FileStreamResult DownloadFile(string path)
        {
            FileInfo file = new FileInfo(path);

            string downloadpath = Path.Combine(Directory.GetCurrentDirectory(), path);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var stream = new MemoryStream(System.IO.File.ReadAllBytes(downloadpath));
            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = file.Name,
            };
        }

    }
}









