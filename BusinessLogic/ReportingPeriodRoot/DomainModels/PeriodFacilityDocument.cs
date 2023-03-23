using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityDocument
    {
        private HashSet<Supplier> suppliers;
        private HashSet<PeriodFacility> periodFacilities;
        private HashSet<DocumentStatus> documentStatuses;
        private HashSet<DocumentType> documentTypes;

        public PeriodFacilityDocument(int reportingPeriodFacilityId, string version, string displayName,
                        string storedName, string path, string validationError)
        {
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            Version = version;
            DisplayName = displayName;
            StoredName = storedName;
            Path = path;
            ValidationError = validationError;

            documentStatuses = new HashSet<DocumentStatus>();
            documentTypes = new HashSet<DocumentType>();
            suppliers = new HashSet<Supplier>();
            periodFacilities = new HashSet<PeriodFacility>();
        }

        public PeriodFacilityDocument(int id, int reportingPeriodFacilityId, string version, string displayName,
                        string storedName, string path, string validationError) : this(reportingPeriodFacilityId, version, displayName, storedName, path, validationError)
        {
            Id = id;
        }

        public PeriodFacilityDocument()
        {

        }

        public int Id { get; set; }
        public int ReportingPeriodFacilityId { get; set; }
        public string Version { get; set; }
        public string DisplayName { get; set; }
        public string StoredName { get; set; }
        public string Path { get; set; }
        public string ValidationError { get; set; }

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

        public IEnumerable<PeriodFacility> PeriodFacility
        {
            get
            {
                if (periodFacilities == null)
                {
                    return new List<PeriodFacility>();
                }
                return periodFacilities.ToList();
            }
        }

    }
}
