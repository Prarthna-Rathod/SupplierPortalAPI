using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityDocument
    {
        public PeriodFacilityDocument(int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            ReportingPeriodFacilityId = periodFacilityId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            DocumentStatus = documentStatus;
            DocumentType = documentType;
            ValidationError = validationError;
           
        }

        public PeriodFacilityDocument(int id, int reportingPeriodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError) : this(reportingPeriodFacilityId, version, displayName, storedName, path, documentStatus, documentType, validationError)
        {
            Id = id;
        }

        private PeriodFacilityDocument()
        {

        }

        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public DocumentType DocumentType { get; set; }
        public string ValidationError { get; set; }

    }
}
