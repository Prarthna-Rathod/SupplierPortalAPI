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
        public int Id { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public int Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public string ValidationError { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public DocumentType DocumentType { get; set; }

        private PeriodSupplierDocument() { }

        internal PeriodSupplierDocument(int reportingPeriodSupplierId, int version, string displayName,
                    string storedName, string path, string validationError,DocumentStatus documentStatus,DocumentType documentType)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            ValidationError = validationError;
            DocumentStatus = documentStatus;
            DocumentType = documentType;
        }

        internal PeriodSupplierDocument(int id, int reportingPeriodSupplierId, int version, string displayName,string storedName, string path, string validationError,DocumentStatus documentStatus,DocumentType documentType) : this(reportingPeriodSupplierId, version, displayName, storedName, path, validationError,documentStatus,documentType)
        {
            Id = id;
        }

    }
}