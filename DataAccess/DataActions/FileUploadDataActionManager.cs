using DataAccess.DataActions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DataAccess.DataActions
{
    public class FileUploadDataActionManager : IFileUploadDataActions
    {
        public string UploadReportingPeriodDocument(IFormFile file)
        {
            var filepath = Path.Combine("E:\\Sem10_Project\\SupplierPortal_own\\SupplierPortalAPI\\DataAccess\\DocumentFiles", file.FileName);

            using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filepath;
        }

    }
}
