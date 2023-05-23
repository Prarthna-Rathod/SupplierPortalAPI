﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface IUploadDocuments
    {
        #region UploadFile method
        
        string UploadFile(IFormFile displayName);

        string validationError(IFormFile displayName);

        #endregion
    }
}