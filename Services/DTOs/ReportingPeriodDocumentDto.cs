using Microsoft.AspNetCore.Http;

namespace Services.DTOs
{
    public class ReportingPeriodDocumentDto
    {
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int PeriodFacilityId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public IFormFile DocumentFile { get; set; }

       /* public ReportingPeriodDocumentDto(int reportingPeriodId, int supplierId, int periodFacilityId, string documentName, int documentTypeId)
        {
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            PeriodFacilityId = periodFacilityId;
            DocumentName = documentName;
            DocumentTypeId = documentTypeId;
            //DocumentFile = formFile;
        }*/

    }
}
