using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodSupplierDocument
    {
        internal PeriodSupplierDocument(int reportingPeriodSupplierId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            DocumentStatus = documentStatus;
            DocumentType = documentType;
            ValidationError = validationError;

        }

        internal PeriodSupplierDocument(int id, int reportingPeriodSupplierId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError) : this(reportingPeriodSupplierId, version, displayName, storedName, path, documentStatus, documentType, validationError)
        {
            Id = id;
        }

        private PeriodSupplierDocument()
        {

        }

        public int Id { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public int Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public DocumentType DocumentType { get; set; }
        public string ValidationError { get; set; }
    }
}