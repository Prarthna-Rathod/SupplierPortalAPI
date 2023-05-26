using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Text.RegularExpressions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class ReportingPeriod : IReportingPeriod
    {
        private HashSet<PeriodSupplier> _periodSupplier;

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

        private void CheckReportingPeriodStatus()
        {
            if (ReportingPeriodStatus.Name == ReportingPeriodStatusValues.InActive || ReportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                throw new BadRequestException("ReportingPeriod is not open or close !!");
        }

        private PeriodSupplier FindPeriodSupplier(int periodSupplierId)
        {
            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Id == periodSupplierId);

            if (periodSupplier == null)
                throw new NotFoundException("ReportingPeriodSupplier is not found !!");

            return periodSupplier;
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
        public bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate, bool isActive)
        {
            var reportingPeriodSupplier = new PeriodSupplier(reportingPeriodSupplierId, supplierVO, Id, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate, isActive);

            return _periodSupplier.Add(reportingPeriodSupplier);
        }

        public PeriodSupplier AddPeriodSupplier(int periodSupplierId, SupplierVO supplier, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate)
        {
            var reportingPeriodSupplier = new PeriodSupplier(periodSupplierId, supplier, Id, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate, true);

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
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

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

        public PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);
            return periodSupplier.AddPeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, Id, facilityIsRelevantForPeriod, fercRegion, isActive);
        }

        public bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, FercRegion fercRegion, bool isActive)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

            return periodSupplier.LoadPeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, Id, periodSupplierId, fercRegion, isActive);
        }

        public IEnumerable<PeriodFacility> UpdateAllPeriodFacilityDataStatus(int supplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Supplier.Id == supplierId);

            if (periodSupplier is null)
                throw new NotFoundException("PeriodSupplier not found !!");

            return periodSupplier.UpdatePeriodFacilityDataStatus(facilityReportingPeriodDataStatus);
        }

        public bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int supplierId, int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Supplier.Id == supplierId);

            if (periodSupplier is null)
                throw new NotFoundException("PeriodSupplier not found !!");

            return periodSupplier.UpdatePeriodFacilityDataStatusSubmittedToInProgress(periodFacilityId, facilityReportingPeriodDataStatus);
        }

        public IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(int periodFacilityId, int supplierId,UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            CheckReportingPeriodStatus();

            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Supplier.Id == supplierId);

            if (periodSupplier is null)
                throw new NotFoundException("PeriodSupplier not found !!");

            return periodSupplier.AddRemoveElectricityGridMixComponents(periodFacilityId, unitOfMeasure, fercRegion, gridMixComponentPercents);
        }

        public bool LoadElectricityGridMixComponents(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

            return periodSupplier.LoadElectricityGridMixComponents(periodFacilityId, unitOfMeasure, gridMixComponentPercents);
        }

        #endregion

        #region GasSupplyBreakdown

        public IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
        {
            CheckReportingPeriodStatus();
            var periodSupplier = FindPeriodSupplier(periodSupplierId);
            return periodSupplier.AddPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownVOs);
        }

        public bool LoadPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

            return periodSupplier.LoadPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownVOs);
        }

        #endregion

        #region Period Document

        public PeriodFacilityDocument AddUpdatePeriodFacilityDocuments(int supplierId, int periodFacilityId, string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs)
        {
            CheckReportingPeriodStatus();
            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Supplier.Id == supplierId);

            if (periodSupplier is null)
                throw new NotFoundException("PeriodSupplier not found !!");

            return periodSupplier.AddUpdatePeriodFacilityDocument(periodFacilityId, displayName, path, documentStatuses, documentType, validationError, CollectionTimePeriod, facilityRequiredDocumentTypeVOs);
        }

        public bool LoadPeriodFacilityDocuments(int documentId, int periodSupplierId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

            return periodSupplier.LoadPeriodFacilityDocuments(documentId, periodFacilityId, version, displayName, storedName, path, documentStatus, documentType, validationError);
        }

        public bool RemovePeriodFacilityDocument(int periodFacilityId, int documentId)
        {
            CheckReportingPeriodStatus();

            PeriodSupplier? periodSupplierDomain = null;
            foreach(var periodSupplier in _periodSupplier)
            {
                var findFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);
                if (findFacility != null)
                    periodSupplierDomain = periodSupplier;
                    break;
            }

            return periodSupplierDomain.RemovePeriodFacilityDocument(periodFacilityId, documentId);
        }

        #endregion

        #region PeriodSupplierDocument

        public PeriodSupplierDocument AddUpdatePeriodSupplierDocument(int supplierId, string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError)
        {
            if (ReportingPeriodStatus.Name != ReportingPeriodStatusValues.Close)
                throw new BadRequestException("ReportingPeriod not closed !! You can't add/update SupplierDocument !!");

            var periodSupplier = _periodSupplier.FirstOrDefault(x => x.Supplier.Id == supplierId);

            if (periodSupplier is null)
                throw new NotFoundException("PeriodSupplier not found !!");

            return periodSupplier.AddUpdatePeriodSupplierDocument(displayName, path, documentStatuses, documentType, validationError, CollectionTimePeriod);
        }

        public bool LoadPeriodSupplierDocuments(int periodSupplierId, int documentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            var periodSupplier = FindPeriodSupplier(periodSupplierId);

            return periodSupplier.LoadPeriodSupplierDocuments(documentId, version, displayName, storedName, path, documentStatus, documentType, validationError);
        }

        public bool RemovePeriodSupplierDocument(int reportingPeriodSupplierId, int documentId)
        {
            CheckReportingPeriodStatus();
            var periodSupplier = FindPeriodSupplier(reportingPeriodSupplierId);
            return periodSupplier.RemovePeriodSupplierDocument(documentId);
        }

        #endregion

        #region Delete methods
        
        public int DeletePeriodFacilityElectricityGridMixes(int periodSupplierId, int periodFacilityId)
        {
            CheckReportingPeriodStatus();
            var periodSupplier = FindPeriodSupplier(periodSupplierId);
            return periodSupplier.DeletePeriodFacilityElectricityGridMixes(periodFacilityId);
        }

        public int DeletePeriodSupplierGasSupplyBreakdowns(int periodSupplierId)
        {
            CheckReportingPeriodStatus();
            var periodSupplier = FindPeriodSupplier(periodSupplierId);
            return periodSupplier.DeletePeriodSupplierGasSupplyBreakdowns();
        }

        #endregion

    }
}
