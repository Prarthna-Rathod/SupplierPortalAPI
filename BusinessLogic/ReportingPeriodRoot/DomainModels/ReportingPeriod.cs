using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Text.RegularExpressions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class ReportingPeriod : IReportingPeriod
    {
        private HashSet<PeriodSupplier> _periodSupplier;
        private HashSet<PeriodFacility> _periodfacilities;

        private readonly string REPORTING_PERIOD_NAME_PREFIX = "Reporting Period Data";
        public ReportingPeriod(ReportingPeriodType reportingPeriodType, string collectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool isActive)
        {
            CheckCollectionTimePeriodData(reportingPeriodType.Name, collectionTimePeriod);
            DisplayName = GeneratedReportingPeriodName(reportingPeriodType, collectionTimePeriod);
            CollectionTimePeriod = collectionTimePeriod;
            ReportingPeriodType = reportingPeriodType;
            ReportingPeriodStatus = reportingPeriodStatus;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            _periodSupplier = new HashSet<PeriodSupplier>();
            _periodfacilities = new HashSet<PeriodFacility>();
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

        public ReportingPeriodType ReportingPeriodType { get; private set; }

        public ReportingPeriodStatus ReportingPeriodStatus { get; private set; }


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

        public IEnumerable<PeriodFacility> PeriodFacilities
        {
            get
            {
                if (_periodfacilities == null)
                {
                    return new List<PeriodFacility>();
                }
                return _periodfacilities.ToList();
            }
            //Add GHGRPFacId, ReportingType and SupplyChainStage in periodFacility table
            //if supplier is unlocked then and then periodFacility can be add
        }

        #region Private Methods
        private string[] SplitCollectionTimePeriod()
        {
            return CollectionTimePeriod.Split(" ");
        }

        private string GeneratedReportingPeriodName(ReportingPeriodType reportingPeriodType, string collectionTimePeriod)
        {
            var reportingPeriodName = REPORTING_PERIOD_NAME_PREFIX;
            switch (reportingPeriodType.Name)
            {
                case ReportingPeriodTypeValues.Annual:
                    reportingPeriodName = $"{reportingPeriodName} Year {collectionTimePeriod}";
                    break;
                case ReportingPeriodTypeValues.Quartly:
                    reportingPeriodName = $"{reportingPeriodName} {collectionTimePeriod}";
                    break;
                case ReportingPeriodTypeValues.Monthly:
                    reportingPeriodName = $"{reportingPeriodName} {collectionTimePeriod}";
                    break;
                default:
                    reportingPeriodName = $"{reportingPeriodName} {SplitCollectionTimePeriod().FirstOrDefault()}";
                    break;
            }
            return $"{reportingPeriodName}";
        }

        private void CheckCollectionTimePeriodData(string reportingPeriodTypeName, string collectionTimePeriod)
        {
            switch (reportingPeriodTypeName)
            {
                case ReportingPeriodTypeValues.Annual:
                    {
                        Regex format = new Regex("^[0-9]{4}$");
                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Year(ex: '2023')");
                        }
                    }
                    break;

                case ReportingPeriodTypeValues.Quartly:
                    {
                        Regex format = new Regex("^[Q]{1}[1-4]{1}[ ][0-9]{4}$");
                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Quarter(ex: 'Q1 2023')");
                        }
                    }
                    break;

                case ReportingPeriodTypeValues.Monthly:
                    {
                        Regex format = new Regex("^[A-Za-z]{3,9}[ ][0-9]{4}$");

                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Month(ex: 'January 2023')");
                        }
                    }
                    break;
                default:
                    throw new NoContentException("Please enter valid CollectionTimePeriod for ReportingPeriodType !!");

            }

        }

        #endregion

        #region Update ReportingPeriod
        public void UpdateReportingPeriod(ReportingPeriodType reportingPeriodType, string collectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool isActive, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            switch (ReportingPeriodStatus.Name)
            {
                case ReportingPeriodStatusValues.InActive:
                    {
                        CheckCollectionTimePeriodData(reportingPeriodType.Name, collectionTimePeriod);
                        DisplayName = GeneratedReportingPeriodName(reportingPeriodType, collectionTimePeriod);

                        if (StartDate.Date != startDate.Date && startDate.Date < DateTime.UtcNow.Date)
                        {
                            throw new Exception("StartDate should be in future !!");
                        }

                        if (endDate != null && endDate < startDate)
                        {

                            throw new BadRequestException("EndDate should be greater than startDate !!");
                        }


                        if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Open || reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                        {
                            if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                            {
                                //ReportingPeriodStatus = reportingPeriodStatus;
                                isActive = false;
                            }
                        }
                        else if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                            throw new BadRequestException("You cannot set ReportingPeriodStatus to Complete !!");

                        CollectionTimePeriod = collectionTimePeriod;
                        ReportingPeriodStatus = reportingPeriodStatus;
                        StartDate = startDate;
                        EndDate = endDate;
                        IsActive = isActive;

                    }
                    break;
                case ReportingPeriodStatusValues.Open:
                    {
                        if (ReportingPeriodType.Id != reportingPeriodType.Id || CollectionTimePeriod != collectionTimePeriod ||
                            StartDate.Date != startDate.Date || IsActive != isActive)
                        {
                            throw new BadRequestException("You can't update any other data except ReportingPeriodStatus and EndDate !!");
                        }

                        if (ReportingPeriodStatus.Name != reportingPeriodStatus.Name && reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                        {
                            ReportingPeriodStatus = reportingPeriodStatus;

                            var periodSuppliersList = _periodSupplier.Where(x => x.IsActive && x.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked).ToList();

                            var supplierReportingPeriodLockedStatus = supplierReportingPeriodStatuses.First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
                            foreach (var periodSupplier in periodSuppliersList)
                            {
                                periodSupplier.UpdateSupplierReportingPeriodStatus(supplierReportingPeriodLockedStatus);
                            }
                        }
                        else
                            throw new BadRequestException("ReportingPeriodStatus can be Open as it is or else you can set it to Close only !!");


                        if (endDate != null)
                        {
                            if (endDate < startDate)
                                throw new BadRequestException("EndDate should be greater than startDate !!");
                        }
                        EndDate = endDate;

                    }
                    break;
                case ReportingPeriodStatusValues.Close:
                    {
                        if (ReportingPeriodType.Id != reportingPeriodType.Id || CollectionTimePeriod != collectionTimePeriod ||
                            StartDate.Date != startDate.Date || IsActive != isActive || EndDate != endDate)
                            throw new BadRequestException("You can't update any other data except ReportingPeriodStatus !!");

                        if (ReportingPeriodStatus.Name != reportingPeriodStatus.Name && reportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                        {
                            //Check if any PeriodSupplier is remains Unlocked or not
                            var periodSuppliersList = _periodSupplier.Where(x => x.IsActive && x.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked).ToList();

                            if (periodSuppliersList.Count() != 0)
                                throw new BadRequestException("Some periodSupplier is still unlocked so can't change ReportingPeriodStatus to Complete !!");

                            ReportingPeriodStatus = reportingPeriodStatus;
                        }
                        else
                            throw new BadRequestException("ReportingPeriodStatus can be Close as it is or else you can set it to Complete only !!");
                        
                    }
                    break;
                default:
                    throw new BadRequestException("You cannot update any data because ReportingPeriod is Completed !!");
            }
        }

        #endregion

        #region Period Supplier
        public bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus,bool activeForCurrentPeriod,bool initialDataRequest,bool resendInitialDataRequest)
        {
            var reportingPeriodSupplier = new PeriodSupplier(reportingPeriodSupplierId, supplierVO,Id, supplierReportingPeriodStatus,activeForCurrentPeriod,initialDataRequest,resendInitialDataRequest);

            return _periodSupplier.Add(reportingPeriodSupplier);
        }

        public PeriodSupplier AddPeriodSupplier(int periodSupplierId,SupplierVO supplier,SupplierReportingPeriodStatus supplierReportingPeriodStatus,bool activeForCurrentPeriod,bool initialDataRequest,bool resendInitialDataRequest)
        {
            var reportingPeriodSupplier = new PeriodSupplier(periodSupplierId,supplier, Id, supplierReportingPeriodStatus,activeForCurrentPeriod,initialDataRequest,resendInitialDataRequest);

            //Check existing PeriodSupplier
            foreach (var periodSupplier in _periodSupplier)
            {
                if (periodSupplier.Supplier.Id == supplier.Id && periodSupplier.ReportingPeriodId == Id)
                    throw new BadRequestException("ReportingPeriodSupplier is already exists !!");
            }

            //Add new PeriodSupplier
            if (supplier.IsActive && ReportingPeriodStatus.Name == ReportingPeriodStatusValues.InActive)
            {
                _periodSupplier.Add(reportingPeriodSupplier);
            }
            else
                throw new BadRequestException("Supplier is not Active or ReportingPeriodStatus is not InActive !!");

            return reportingPeriodSupplier;
        }

        public PeriodSupplier UpdateLockUnlockPeriodSupplierStatus(int periodSupplierId, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            var periodSupplier = _periodSupplier.Where(x => x.Id == periodSupplierId).FirstOrDefault();

            if (ReportingPeriodStatus.Name == ReportingPeriodStatusValues.InActive || ReportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                throw new BadRequestException("You can't update PeriodSupplierStatus because reportingPeriodStatus is InActive or Complete !!");
            else
            {
                if (periodSupplier.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
                {
                    var lockedStatus = supplierReportingPeriodStatuses.First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
                    periodSupplier.SupplierReportingPeriodStatus.Id = lockedStatus.Id;
                    periodSupplier.SupplierReportingPeriodStatus.Name = lockedStatus.Name;
                }
                else
                {
                    var unlockedStatus = supplierReportingPeriodStatuses.First(x => x.Name == SupplierReportingPeriodStatusValues.Unlocked);
                    periodSupplier.SupplierReportingPeriodStatus.Id = unlockedStatus.Id;
                    periodSupplier.SupplierReportingPeriodStatus.Name = unlockedStatus.Name;
                }
            }
            return periodSupplier;
        }

        #endregion

        #region Period Facility

        public PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod)
        {
            int counter = 0;
            var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, Id, periodSupplierId);

            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Id == periodSupplierId);

            if (periodSupplier == null)
                throw new BadRequestException("ReportingPeriodSupplier is not relavent with given reportingPeriodId !!");

            if (periodSupplier.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
            {
                var periodSupplierFacilities = periodSupplier.Supplier.Facilities;

                //Check existing PeriodSupplier
                foreach (var existingPeriodFacility in _periodfacilities)
                {
                    if (existingPeriodFacility.FacilityVO.Id == facilityVO.Id && existingPeriodFacility.ReportingPeriodId == Id)
                    {
                        if (facilityIsRelevantForPeriod)
                            throw new BadRequestException("ReportingPeriodFacility is already exists !!");
                        else
                            _periodfacilities.Remove(existingPeriodFacility);
                    }
                }

                if (facilityIsRelevantForPeriod)
                {
                    if (facilityReportingPeriodDataStatus.Name != FacilityReportingPeriodDataStatusValues.InProgress)
                        throw new BadRequestException("FacilityReportingPeriodStatus should be InProgress only !!");

                    foreach (var supplierFacility in periodSupplierFacilities)
                    {
                        if (supplierFacility.Id == facilityVO.Id)
                        {
                            _periodfacilities.Add(periodFacility);
                            counter = 0;
                            break;
                        }
                        else
                            counter++;
                    }
                }

                if (counter != 0)
                    throw new BadRequestException("The given facility is not relavent with given ReportingPeriodSupplier !!");

            }
            else
                throw new BadRequestException("SupplierReportingPeriodStatus is locked !! Can't add new PeriodFacility !!");

            return periodFacility;
        }


        public bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId)
        {
            var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, Id, periodSupplierId);

            return _periodfacilities.Add(periodFacility);
        }

        public PeriodFacility UpdatePeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod)
        {
            var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

            periodFacility.FacilityReportingPeriodDataStatus.Id = facilityReportingPeriodDataStatus.Id;
            periodFacility.FacilityReportingPeriodDataStatus.Name = facilityReportingPeriodDataStatus.Name;

            return periodFacility;
        }

        #endregion

        #region Period Document

        /*
         public PeriodFacilityDocument AddDataSubmissionDocumentForReportingPeriod(int supplierId, int periodFacilityId, FacilityRequiredDocumentTypeEntity facilityRequiredDocumentType, IEnumerable<DocumentRequirementStatus> documentRequirementStatus)
         {
             throw new NotImplementedException();
         }

         public void AddDocumentToPeriodSupplierFacility(DocumentType documentType, DocumentStatus documentStatus)
         {
             throw new NotImplementedException();
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



        */

        #endregion


    }
}
