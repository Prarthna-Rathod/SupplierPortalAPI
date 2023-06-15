using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodSupplierDocumentDto
    {
        public int PeriodSupplierId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public IFormFile DocumentFile { get; set; }
    }
}
