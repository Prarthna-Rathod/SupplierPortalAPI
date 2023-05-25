using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using Microsoft.VisualBasic.FileIO;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<PeriodFacilityElectricityGridMix> _periodFacilityElectricityGridMixes;
        private HashSet<PeriodFacilityGasSupplyBreakdown> _periodSupplierGasSupplyBreakdowns;
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

        public IEnumerable<PeriodFacilityGasSupplyBreakdown> periodFacilityGasSupplyBreakdowns
        {
            get
            {
                if (_periodSupplierGasSupplyBreakdowns == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakdown>();
                }
                return _periodSupplierGasSupplyBreakdowns;
            }
        }

        public IEnumerable<PeriodFacilityDocument> periodFacilityDocuments
        {
            get
            {
                if (_periodFacilityDocuments == null)
                {
                    return new List<PeriodFacilityDocument>();
                }
                return _periodFacilityDocuments;
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
            _periodSupplierGasSupplyBreakdowns = new HashSet<PeriodFacilityGasSupplyBreakdown>();
            _periodFacilityDocuments = new HashSet<PeriodFacilityDocument>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }

        private string GeneratedReportingPeriodFacilityDocumentName(string collectionTimePeriod, string documentTypeName, int version, string extension)
        {
            var fileTypes = new List<string>();
            fileTypes.Add(".xlsx");
            fileTypes.Add(".xml");

            var isCorrect = fileTypes.Contains(extension);
            if (!isCorrect)
                throw new BadRequestException("FileType should be ExcelSheet or XML !!");

            var facilityName = FacilityVO.FacilityName;
            var documentName = facilityName + "-" + collectionTimePeriod + "-" + documentTypeName + "-" + version + extension;

            return documentName;
        }

        private bool CheckFacilityRequiredDocumentType(IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs, DocumentType documentType)
        {
            var facilityReportingType = FacilityVO.ReportingType;
            var facilitySupplyChainStage = FacilityVO.SupplyChainStage;
            
            var facilityRequiredDocumentType = facilityRequiredDocumentTypeVOs.Where(x => x.ReportingType.Id == facilityReportingType.Id && x.SupplyChainStage.Id == facilitySupplyChainStage.Id && x.DocumentType.Id == documentType.Id ).FirstOrDefault();

            if (facilityRequiredDocumentType == null)
                throw new NotFoundException("FacilityRequiredDocumentType not found !!");

            if (facilityRequiredDocumentType.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.NotAllowed)
                throw new BadRequestException($"{documentType.Name} document is not allowed for this ReportingPeriodFacility !!");

            return true;
        }

        internal void UpdatePeriodFacilityDataStatus(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete)
            {
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            }
            else
                throw new BadRequestException("FacilityReportingPeriodDataStatus not completed !! So, can't update it to Submitted...");
        }

        internal void UpdatePeriodFacilityDataStatusSubmittedToInProgress(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
        }

        #region ElectricityGridMixes

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents, FercRegion fercRegion)
        {
            FercRegion = fercRegion;

            switch (FercRegion.Name)
            {
                case FercRegionValues.Custom_Mix:
                    {
                        if (gridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add gridMixComponents !!");

                        _periodFacilityElectricityGridMixes.Clear();

                        foreach (var facilityGridMix in gridMixComponentPercents)
                        {
                            var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, facilityGridMix.Content);

                            var checkExistsComponent = _periodFacilityElectricityGridMixes.Any(x => x.ElectricityGridMixComponent.Id == facilityGridMix.ElectricityGridMixComponent.Id);

                            if (checkExistsComponent)
                                throw new BadRequestException($"{facilityGridMix.ElectricityGridMixComponent.Name} component is already exists in this periodFacility !!");

                            _periodFacilityElectricityGridMixes.Add(periodFacilityGridMixDomain);
                        }

                        var totalContentValues = _periodFacilityElectricityGridMixes.Sum(x => x.Content);
                        if (totalContentValues != 100)
                            throw new BadRequestException("Please adjust ContentValues because total should be 100 !!");
                    }
                    break;
                default:
                    {
                        if (gridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion should be CustomMix for add electricityGridMix components !!");

                        //If user want to update only fercRegion then need to update that region and clear the gridMix data
                        if (_periodFacilityElectricityGridMixes.Count() != 0)
                            _periodFacilityElectricityGridMixes.Clear();

                    }
                    break;
            }

            return _periodFacilityElectricityGridMixes;
        }

        internal bool LoadElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            foreach (var facilityGridMix in gridMixComponentPercents)
            {
                var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(facilityGridMix.Id, Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, facilityGridMix.Content);

                _periodFacilityElectricityGridMixes.Add(periodFacilityGridMixDomain);
            }

            return true;
        }
        #endregion

        #region GasSupplyBreakdown

        internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> facilityDataVos)
        {
            _periodSupplierGasSupplyBreakdowns.Clear();

            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new BadRequestException("Facility SupplyChainStage is not Production !!");

            foreach (var singleVo in facilityDataVos)
            {
                var periodSupplierGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, singleVo.Site, singleVo.UnitOfMeasure, singleVo.Content);

                var isSiteExists = _periodSupplierGasSupplyBreakdowns.Any(x => x.Site.Id == singleVo.Site.Id);

                if (isSiteExists)
                    throw new Exception($"Duplicate Site '{singleVo.Site.Name}' exists in same facility !!");

                _periodSupplierGasSupplyBreakdowns.Add(periodSupplierGasSupplyBreakdown);
            }

            return _periodSupplierGasSupplyBreakdowns;
        }

        internal PeriodFacilityGasSupplyBreakdown LoadPeriodFacilityGasSupplyBreakdowns(Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            var periodSupplierGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, site, unitOfMeasure, content);

            _periodSupplierGasSupplyBreakdowns.Add(periodSupplierGasSupplyBreakdown);

            return periodSupplierGasSupplyBreakdown;
        }


        #endregion

        #region PeriodFacilityDocuments

        internal PeriodFacilityDocument AddUpdatePeriodFacilityDocument(string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, string collectionTimePeriod, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs)
        {
            CheckFacilityRequiredDocumentType(facilityRequiredDocumentTypeVOs, documentType);

            var existingData = _periodFacilityDocuments.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();

            //DocumentStatus -> Processing
            var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

            //Retrieve FileExtension from fileName
            FileInfo file = new FileInfo(displayName);
            var extension = file.Extension;

            PeriodFacilityDocument? document = null;

            if (existingData.Count() == 0)
            {
                var version = 1;
                var storedName = GeneratedReportingPeriodFacilityDocumentName(collectionTimePeriod, documentType.Name, version, extension);
                document = new PeriodFacilityDocument(Id, version, displayName, storedName, path, documentStatusProcessing, documentType, validationError);

                _periodFacilityDocuments.Add(document);
            }
            else
            {
                document = existingData.First();

                var version = document.Version;
                if (document.DocumentStatus.Name == DocumentStatusValues.HasErrors)
                    version += 1;

                if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                {
                    var newStoredName = GeneratedReportingPeriodFacilityDocumentName(collectionTimePeriod, documentType.Name, version, extension);
                    document = new PeriodFacilityDocument(Id, version, displayName, newStoredName, null, documentStatusProcessing, documentType, validationError);

                    _periodFacilityDocuments.Add(document);
                }
                else
                {
                    //Document cannot be versioned
                    if (document.Path is not null)
                        throw new Exception("This file already exists in the system. If you wish to upload a new version of the file, please delete existing file and try the upload again.");

                    //Update existing versioned data record
                    DocumentStatus? documentStatus = null;

                    if (validationError == null)
                    {
                        documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.NotValidated);
                    }
                    else
                    {
                        documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors);
                        path = null;
                        validationError = "Unable to save the uploaded file at this time.  Please attempt the upload again later.";
                    }

                    if(validationError == null && path != null)
                    {
                        documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.Validated);
                    }

                    var newStoredName = GeneratedReportingPeriodFacilityDocumentName(collectionTimePeriod, documentType.Name, version, extension);
                    document.Version = version;
                    document.DisplayName = file.Name;
                    document.StoredName = newStoredName;
                    document.Path = path;
                    document.DocumentStatus = documentStatus;
                    document.DocumentType = documentType;
                    document.ValidationError = validationError;
                }
            }

            return document;
        }

        public bool LoadPeriodFacilityDocuments(int documentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
        {
            var document = new PeriodFacilityDocument(documentId, Id, version, displayName, storedName, path, documentStatus, documentType, validationError);
            return _periodFacilityDocuments.Add(document);
        }

        public bool RemovePeriodFacilityDocument(int documentId)
        {
            var document = _periodFacilityDocuments.FirstOrDefault(x => x.Id == documentId);
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
            {
                _periodFacilityDocuments.Remove(document);
                return true;
            }
            else
                return false;

        }

        #endregion

    }
}
