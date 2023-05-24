using Microsoft.AspNetCore.Http;

namespace Services.DTOs
{
    public class ReportingPeriodFacilityDocumentDto
    {
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int PeriodFacilityId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public IFormFile DocumentFile { get; set; }

    }
}
