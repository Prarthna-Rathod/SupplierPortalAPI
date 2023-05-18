using BusinessLogic.ReferenceLookups;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityDocument
    {
        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public int Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public string ValidationError { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public DocumentType DocumentType { get; set; }

        private PeriodFacilityDocument() { }

        internal PeriodFacilityDocument(int reportingPeriodFacilityId, int version, string displayName, string storedName, string path, string validationError, DocumentStatus documentStatus, DocumentType documentType)
        {
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            ValidationError = validationError;
            DocumentStatus = documentStatus;
            DocumentType = documentType;
        }

        internal PeriodFacilityDocument(int id, int reportingPeriodFacilityId, int version, string displayName, string storedName, string path, string validationError, DocumentStatus documentStatus, DocumentType documentType) : this(reportingPeriodFacilityId, version, displayName, storedName, path, validationError, documentStatus, documentType)
        {
            Id = id;
        }



    }
}
