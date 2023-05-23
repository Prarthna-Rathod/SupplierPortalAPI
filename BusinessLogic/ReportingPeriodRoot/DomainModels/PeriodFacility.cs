using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<PeriodFacilityElectricityGridMix> _periodFacilityElectricityGridMixes;
        private HashSet<PeriodFacilityGasSupplyBreakdown> _periodFacilityGasSupplyBreakdowns;
        private HashSet<PeriodFacilityDocument> _periodFacilityDocuments;

        public int Id { get; private set; }
        public FacilityVO FacilityVO { get; private set; }
        public FacilityReportingPeriodDataStatus FacilityReportingPeriodDataStatus { get; private set; }
        public int ReportingPeriodId { get; private set; }
        public int ReportingPeriodSupplierId { get; private set; }
        public FercRegion FercRegion { get; private set; }
        public bool IsActive { get; private set; }

        public IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes
        {
            get
            {
                if (_periodFacilityElectricityGridMixes == null)
                {
                    return new List<PeriodFacilityElectricityGridMix>();
                }
                return _periodFacilityElectricityGridMixes;
            }
        }

        public IEnumerable<PeriodFacilityGasSupplyBreakdown> PeriodFacilityGasSupplyBreakdowns
        {
            get
            {
                if (_periodFacilityGasSupplyBreakdowns == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakdown>();
                }
                return _periodFacilityGasSupplyBreakdowns.ToList();
            }
        }

        public IEnumerable<PeriodFacilityDocument> PeriodFacilityDocuments
        {
            get
            {
                if (_periodFacilityDocuments == null)
                {
                    return new List<PeriodFacilityDocument>();
                }
                return _periodFacilityDocuments.ToList();
            }
        }

        internal PeriodFacility() { }

        internal PeriodFacility(FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive)
        {
            FacilityVO = facilityVO;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            FercRegion = fercRegion;
            IsActive = isActive;
            _periodFacilityElectricityGridMixes = new HashSet<PeriodFacilityElectricityGridMix>();
            _periodFacilityGasSupplyBreakdowns = new HashSet<PeriodFacilityGasSupplyBreakdown>();
            _periodFacilityDocuments = new HashSet<PeriodFacilityDocument>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }

        private string GeneratedPeriodFacilityDocumentStoredName(string collectionTimePeriod, DocumentType documentType, int version, string fileName)
        {
            var facilityName = FacilityVO.FacilityName;

            var fileTypes = new[] { "xml", "xlsx" };

            var fileExtension = Path.GetExtension(fileName).Substring(1);
            if (!fileTypes.Contains(fileExtension))
                throw new BadRequestException("File Extension Is InValid !!");

            var storedName = facilityName + "-" + collectionTimePeriod + "-" + documentType.Name + "-" + version + "." + fileExtension;

            return $"{storedName}";
        }

        private bool CheckPeriodFacilityRequiredDocumentType(IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs, DocumentType documentType)
        {
            var facilityReportingType = FacilityVO.ReportingType;
            var facilitySupplyChainStage = FacilityVO.SupplyChainStage;

            var facilityRequiredDocumentType = facilityRequiredDocumentTypeVOs.Where(x => x.ReportingType.Id == facilityReportingType.Id && x.SupplyChainStage.Id == facilitySupplyChainStage.Id && x.DocumentType.Id == documentType.Id).FirstOrDefault();

            if (facilityRequiredDocumentType is null)
                throw new NotFoundException("FacilityRequiredDocumentType not found !!");

            if (facilityRequiredDocumentType.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.NotAllowed)
                throw new BadRequestException("Document is not allowed for this ReportingPeriodFacility !!");

            return true;
        }

        #region UpdateFacilityDataStatus

        internal void UpdatePeriodFacilityDataStatusCompleteToSubmitted(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete)
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            else
                throw new Exception("PeriodFacilityDataStatus is not completed !!");
        }

        internal void UpdatePeriodFacilityDataStatusSubmittedToInProgress(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
        }

        #endregion       

        #region PeriodFacilityElecetricityGridMix

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents, FercRegion fercRegion)
        {
            FercRegion = fercRegion;

            switch (FercRegion.Name)
            {
                case FercRegionValues.CustomMix:
                    {
                        if (electricityGridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add PeriodFacilityElectricityGridMixComponents.");

                        _periodFacilityElectricityGridMixes.Clear();

                        foreach (var gridMixComponent in electricityGridMixComponentPercents)
                        {
                            var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, gridMixComponent.Content);

                            var existingGridMixComponent = _periodFacilityElectricityGridMixes.Any(x => x.ElectricityGridMixComponent.Id == gridMixComponent.ElectricityGridMixComponent.Id);

                            if (existingGridMixComponent)
                                throw new BadRequestException("ElectricityGridMixComponent is already exists in this PeriodFacility !!");

                            _periodFacilityElectricityGridMixes.Add(periodFacilityElectricityGridMixDomain);
                        }

                        var contentValue = _periodFacilityElectricityGridMixes.Sum(x => x.Content);
                        if (contentValue != 100)
                            throw new BadRequestException("ContentValues is not 100 !!");

                    }
                    break;
                default:
                    {
                        if (electricityGridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion is not Custom Mix. Please select fercRegion is Custom Mix then add PeriodFacilityElectricityGridMix.");

                        if (_periodFacilityElectricityGridMixes.Count() != 0)
                            _periodFacilityElectricityGridMixes.Clear();
                    }
                    break;
            }

            return _periodFacilityElectricityGridMixes;
        }

        internal bool LoadPeriodFacilityElectricityGridMix(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
        {
            foreach (var gridMixComponent in electricityGridMixComponentPercents)
            {
                var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(gridMixComponent.Id, Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, gridMixComponent.Content);

                _periodFacilityElectricityGridMixes.Add(periodFacilityElectricityGridMixDomain);
            }
            return true;
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
        {
            _periodFacilityGasSupplyBreakdowns.Clear();

            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new BadRequestException("SupplyChainStage is not Production !!");

            foreach (var gasSupplyBreakdown in gasSupplyBreakdownVOs)
            {
                var periodFacilityGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, gasSupplyBreakdown.Site, gasSupplyBreakdown.UnitOfMeasure, gasSupplyBreakdown.Content);

                var existingSite = _periodFacilityGasSupplyBreakdowns.Any(x => x.Site.Id == gasSupplyBreakdown.Site.Id);
                if (existingSite)
                    throw new BadRequestException($"Site '{gasSupplyBreakdown.Site.Name}' is already exists in same PeriodFacility !!");

                _periodFacilityGasSupplyBreakdowns.Add(periodFacilityGasSupplyBreakdown);
            }

            return _periodFacilityGasSupplyBreakdowns;

        }

        internal bool LoadPeriodFacilityGasSupplyBreakdown(Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            var periodFacilityGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, site, unitOfMeasure, content);

            _periodFacilityGasSupplyBreakdowns.Add(periodFacilityGasSupplyBreakdown);

            return true;
        }

        #endregion

        #region PeriodFacilityDocument

        internal PeriodFacilityDocument AddPeriodFacilityDocument(string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string collectionTimePeriod, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs)
        {
            CheckPeriodFacilityRequiredDocumentType(facilityRequiredDocumentTypeVOs, documentType);

            //check existingDocument
            var existingDocument = _periodFacilityDocuments.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();

            var documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Processing);

            PeriodFacilityDocument? periodFacilityDocument = null;

            if (existingDocument.Count() == 0)
            {
                int version = 1;
                var documentStoredName = GeneratedPeriodFacilityDocumentStoredName(collectionTimePeriod, documentType, version, displayName);

                periodFacilityDocument = new PeriodFacilityDocument(Id, version, displayName, documentStoredName, null, null, documentStatus, documentType);
                _periodFacilityDocuments.Add(periodFacilityDocument);
            }
            else
            {
                periodFacilityDocument = existingDocument.First();

                //check facilityReportingPeriodDataStatus is submitted
                if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                {
                    var latestVersion = periodFacilityDocument.Version;
                    var incrementVersion = latestVersion + 1;

                    var documentStoredName = GeneratedPeriodFacilityDocumentStoredName(collectionTimePeriod, documentType, incrementVersion, displayName);

                    periodFacilityDocument = new PeriodFacilityDocument(Id, incrementVersion, displayName, documentStoredName, null, null, documentStatus, documentType);
                    _periodFacilityDocuments.Add(periodFacilityDocument);

                }
                else
                {
                    if (periodFacilityDocument.Path is not null)
                        throw new BadRequestException("This file already exists in the system. If you wish to upload a new version of the file, please delete the file and try the upload again.");

                    var version = periodFacilityDocument.Version;

                    if (validationError is null)
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.NotValidated);
                    else
                    {
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.HasErrors);
                        path = null;
                        try
                        {
                            throw new Exception("Unable to save the uploaded file at this time.Please attempt the upload again later.");
                        }
                        catch (Exception ex)
                        {
                            validationError = ex.Message;
                        }
                    }

                    if (path is not null && validationError is null)
                    {
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Validated);
                        version += 1;
                    }

                    var documentStoredName = GeneratedPeriodFacilityDocumentStoredName(collectionTimePeriod, documentType, version, displayName);

                    periodFacilityDocument.ReportingPeriodFacilityId = Id;
                    periodFacilityDocument.Version = version;
                    periodFacilityDocument.DisplayName = displayName;
                    periodFacilityDocument.StoredName = documentStoredName;
                    periodFacilityDocument.Path = path;
                    periodFacilityDocument.ValidationError = validationError;
                    periodFacilityDocument.DocumentStatus = documentStatus;
                    periodFacilityDocument.DocumentType = documentType;

                }
            }

            return periodFacilityDocument;
        }

        internal bool LoadPeriodFacilityDocument(int periodFacilityDocumentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            var periodFacilityDocument = new PeriodFacilityDocument(periodFacilityDocumentId, Id, version, displayName, storedName, path, validationError, documentStatus, documentType);

            _periodFacilityDocuments.Add(periodFacilityDocument);

            return true;
        }

        #endregion

    }
}
