using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class GetReportingPeriodSupplierDocumentDto
    {
        public int DocumentId { get; set; }
        public string DocumentDisplayName { get; set; }
        public int DocumentVersion { get; set; }
        public int DocumentStatusId { get; set; }
        public string DocumentStatusName { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumenTypeName { get; set; }
        public string ValidationError { get; set; }

        public GetReportingPeriodSupplierDocumentDto(int id, string documentDisplayName, int documentVersion, int documentStatusId, string documentStatusName, int documentTypeId, string documenTypeName, string validationError)
        {
            DocumentId = id;
            DocumentDisplayName = documentDisplayName;
            DocumentVersion = documentVersion;
            DocumentStatusId = documentStatusId;
            DocumentStatusName = documentStatusName;
            DocumentTypeId = documentTypeId;
            DocumenTypeName = documenTypeName;
            ValidationError = validationError;
        }
    }
}
