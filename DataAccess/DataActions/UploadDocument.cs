﻿using DataAccess.DataActions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DataAccess.DataActions
{
    public class UploadDocument : IUploadDocuments
    {

        #region UploadFile method

        public string UploadFile(IFormFile displayName)
        {
            string path = "";

            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"E:\Bigscal\ICT_4\SupplierPortalAPI\SupplierPortalAPI\DataAccess\UploadedFiles"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, displayName.FileName), FileMode.Create))
            {
                displayName.CopyTo(fileStream);
            }
            return path + @"\" + displayName.FileName;
        }

        public string validationError(IFormFile displayName)
        {
            string? error = null;
            var totalBytes = 20971520; //20 mb
            var uploadedBytes = displayName.Length;

            if (uploadedBytes >= totalBytes)
            {
                error = "File is not uploaded because file size is larger than 20 MB !!";
            }
            return error;
        }

        /*private string ValidateFile(string fileType, long fileSize, string path)
        {
            string? error = null;
            var fileTypes = new List<string>();
            fileTypes.Add(".xlsx");
            fileTypes.Add(".xml");

            var isCorrect = fileTypes.Contains(fileType);
            if (!isCorrect)
                error += "FileType is not match.";

            //Check file signature
            var fileError = CheckFileSignature(path, fileType);
            if (fileError != null)
                error += fileError;

            //Check File size (should be 20MB)
            long sizeInBytes = fileSize;
            long maxSizeInBytes = 20971520;
            if (sizeInBytes > maxSizeInBytes)
                error += "Filesize should be in 20Mb";

            return error;
        }*/

        #endregion
    }
}
