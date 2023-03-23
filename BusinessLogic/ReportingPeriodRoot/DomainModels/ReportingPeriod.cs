using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.Entities;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class ReportingPeriod : IReportingPeriod
    {
        private HashSet<PeriodSupplier> _periodSupplier;

        private readonly string REPORTING_PERIOD_NAME_PREFIX = "Reporting Period Data";
        public ReportingPeriod(ReportingPeriodType types, string collectionTimePeriod, ReportingPeriodStatus status, DateTime startDate, DateTime? endDate, bool isActive)
        {
            ValidateReportingPeriod(collectionTimePeriod, startDate, endDate, types);
            DisplayName = GeneratedReportingPeriodName(types);
            CollectionTimePeriod = collectionTimePeriod;
            ReportingPeriodType = types;
            ReportingPeriodStatus = status;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            _periodSupplier = new HashSet<PeriodSupplier>();
        }

        public ReportingPeriod(int id, string displayName, ReportingPeriodType types, string collectionTimePeriod, ReportingPeriodStatus status, DateTime startDate, DateTime? endDate, bool isActive) : this(types, collectionTimePeriod, status, startDate, endDate, isActive)
        {
            Id = id;
        }
        public ReportingPeriod()
        {

        }

        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public string CollectionTimePeriod { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public string CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }

        public ReportingPeriodType ReportingPeriodType { get; private set; }

        public ReportingPeriodStatus ReportingPeriodStatus { get; private set; }


        private string[] SplitCollectionTimePeriod()
        {
            return CollectionTimePeriod.Split(" ");
        }

        private string GeneratedReportingPeriodName(ReportingPeriodType reportingPeriodType)
        {
            var reportingPeriodName = REPORTING_PERIOD_NAME_PREFIX;
            switch (reportingPeriodType.Name)

            {
                case ReportingPeriodTypeValues.Annual:
                    reportingPeriodName = $"{reportingPeriodName} Yearly";
                    break;
                case ReportingPeriodTypeValues.Quartly:
                    reportingPeriodName = $"{reportingPeriodName} Quarterly";
                    break;
                case ReportingPeriodTypeValues.Monthly:
                    reportingPeriodName = $"{reportingPeriodName} Monthly";
                    break;
                default:
                    reportingPeriodName = $"{reportingPeriodName} {SplitCollectionTimePeriod().FirstOrDefault()}";
                    break;
            }
            return $"{reportingPeriodName}";
        }

        private void ValidateReportingPeriod(string collectionTimePeriod, DateTime startDate, DateTime? endDate, ReportingPeriodType reportingPeriodType)
        {
            if (string.IsNullOrWhiteSpace(collectionTimePeriod))
                throw new ArgumentNullException("CollectionTimePeriod can not be null");

            if (startDate == null)
                throw new ArgumentNullException("StartDate can not be null");

            if (reportingPeriodType != null && reportingPeriodType.Name == ReportingPeriodTypeValues.Annual)
            {
                int convertedCollectionTimePeriod = Convert.ToInt32(collectionTimePeriod);
                if (convertedCollectionTimePeriod.ToString().Length != 4)
                {
                    throw new ArgumentException("Collection time period should be in Year(ex: YYYY) only");
                }

            }

            if (reportingPeriodType != null && reportingPeriodType.Name == ReportingPeriodTypeValues.Quartly)
            {
                string convertedCollectionTimePeriod = Convert.ToString(collectionTimePeriod);
                var format = "(^[0-9]{4}-[Q]{1}[0-9]{1}$)";
                var validateformat = format.Equals(convertedCollectionTimePeriod.ToString());
                if (convertedCollectionTimePeriod.ToString().Length != 7 && validateformat == false)
                {
                    throw new ArgumentException("Collection time period should be in Quater(ex: YYYY-Q1) only");
                }

            }

            if (reportingPeriodType != null && reportingPeriodType.Name == ReportingPeriodTypeValues.Monthly)
            {
                string convertedCollectionTimePeriod = Convert.ToString(collectionTimePeriod);
                var format = "(^[A-Z]{3}-[0-9]{4}$)";
                var x = format.Equals(convertedCollectionTimePeriod.ToString());

                if (convertedCollectionTimePeriod.ToString().Length != 8 && x == false)
                {
                    throw new ArgumentException("Collection time period should be in Month(ex: Jan-YYYY) only");
                }

            }

        }

        public IEnumerable<PeriodSupplier> PeriodSuppliers
        {
            get
            {
                if (_periodSupplier == null)
                {
                    return new List<PeriodSupplier>();
                }
                return _periodSupplier.ToList();
            }
        }

        public void UpdateReportingPeriod(int reportingPeriodTypeId, string collectionTimePeriod, int reportingPeriodStatusId, DateTime startDate, DateTime? endDate, bool isActive)
        {
            ReportingPeriodType.Id = reportingPeriodTypeId;
            CollectionTimePeriod = collectionTimePeriod;
            ReportingPeriodStatus.Id = reportingPeriodStatusId;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = "System";
        }

        public PeriodSupplier LoadPeriodSupplier(int id, SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
        {
            var reportingPeriodSupplier = new PeriodSupplier(id, supplier, reportingPeriodId, supplierReportingPeriodStatus);

            if (_periodSupplier.Contains(reportingPeriodSupplier))
            {
                throw new Exception("Supplier Already Exist!");
            }
            _periodSupplier.Add(reportingPeriodSupplier);

            return reportingPeriodSupplier;


        }

        public PeriodSupplier AddPeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
        {
            var reportingPeriodSupplier = new PeriodSupplier(supplier, reportingPeriodId, supplierReportingPeriodStatus);

            if (_periodSupplier.Contains(reportingPeriodSupplier))
            {
                throw new Exception("Supplier Already Exist!");
            }
            else
            {
                if(supplier.IsActive == true && ReportingPeriodStatus.Name == ReportingPeriodStatusValues.InActive)
                {
                    _periodSupplier.Add(reportingPeriodSupplier);
                }
               
            }

            return reportingPeriodSupplier;
        }

        public PeriodSupplier UpdatePeriodSupplierStatus(int periodSupplierId, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            var periodSupplier = _periodSupplier.Where(x => x.Id == periodSupplierId).FirstOrDefault();

            if (periodSupplier is null)
            {
                throw new ArgumentNullException("Unable to retrieve Period Supplier");
            }

            if (periodSupplier.Id == periodSupplierId)
            {
                if (periodSupplier.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
                {
                    var unlockedStatus = supplierReportingPeriodStatuses.Where(x => x.Name == "Unlocked").FirstOrDefault();
                    periodSupplier.UpdateSupplierReportingPeriodStatus(unlockedStatus);
                }
                else
                {
                    periodSupplier.SupplierReportingPeriodStatus.Name = "Locked";
                }
            }
            else
            {
                throw new ArgumentNullException("Unable to retrieve Period Supplier");

            }


            return periodSupplier;
        }

        //public PeriodSupplier RemovePeriodSupplier(int periodSupplierId)
        //{
        //   //var periodSupplierRelevantFacility = 
        //}

        public PeriodFacilityDocument AddDataSubmissionDocumentForReportingPeriod(int supplierId, int periodFacilityId, FacilityRequiredDocumentTypeEntity facilityRequiredDocumentType, IEnumerable<DocumentRequirementStatus> documentRequirementStatus)
        {
            throw new NotImplementedException();
        }

        public void AddDocumentToPeriodSupplierFacility(DocumentType documentType, DocumentStatus documentStatus)
        {
            throw new NotImplementedException();
        }

        public void AddPeriodFacilityToPeriodSupplier(int supplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, ReportingType reportingType, int reportingPeriodSupplierId)
        {
            var reportingPeriodFacility = new PeriodFacility();
        }


        public PeriodSupplierDocument AddSupplementalDataDocumentToReportingPeriodSupplier(int supplierId, string documentName, DocumentType documentType, IEnumerable<DocumentStatus> documentStatus)
        {
            throw new NotImplementedException();
        }

        public PeriodFacilityDocument RemoveDocumentFromPeriodSupplierFacility(int supplierId, int periodFacilityId, int documentId)
        {
            throw new NotImplementedException();
        }

        public PeriodSupplierDocument RemoveSupplementalDataDocumentToReportingPeriodSupplier(int supplierId, int documentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodFacility> UpdateDataStatusToSubmittedForCompletePeriodFacility(int supplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            throw new NotImplementedException();
        }


    }
}
