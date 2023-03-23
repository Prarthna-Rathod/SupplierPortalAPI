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
        private HashSet<Supplier> suppliers;
        private HashSet<DocumentStatus> documentStatuses;
        private HashSet<DocumentType> documentTypes;

        public PeriodSupplierDocument(int reportingPeriodSupplierId, string version, string displayName,
                    string storedName, string path, string validationError)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            ValidationError = validationError;

            documentStatuses = new HashSet<DocumentStatus>();
            documentTypes = new HashSet<DocumentType>();
            suppliers = new HashSet<Supplier>();
        }

        public PeriodSupplierDocument(int id, int reportingPeriodSupplierId, string version, string displayName,
                       string storedName, string path, string validationError) : this(reportingPeriodSupplierId, version, displayName, storedName, path, validationError)
        {
            Id = id;
        }

        public PeriodSupplierDocument()
        {

        }

        public int Id { get; set; }
        public int ReportingPeriodSupplierId { get; set; }
        public string Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public string ValidationError { get; set; }

        public IEnumerable<Supplier> Supplier
        {
            get
            {
                if (suppliers == null)
                {
                    return new List<Supplier>();
                }
                return suppliers.ToList();
            }
        }

        public IEnumerable<DocumentStatus> DocumentStatus
        {
            get
            {
                if (documentStatuses == null)
                {
                    return new List<DocumentStatus>();
                }
                return documentStatuses.ToList();
            }
        }

        public IEnumerable<DocumentType> DocumentType
        {
            get
            {
                if (documentTypes == null)
                {
                    return new List<DocumentType>();
                }
                return documentTypes.ToList();
            }
        }

    }
}