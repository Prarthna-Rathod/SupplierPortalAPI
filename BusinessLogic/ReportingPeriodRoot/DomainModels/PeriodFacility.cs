using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System;
using System.Collections.Immutable;


namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<PeriodFacilityElectricityGridMix> _periodFacilityElectricityGridMix;
        private HashSet<PeriodFacilityGasSupplyBreakDown> _periodFacilityGasSupplyBreakDown;
        private HashSet<PeriodFacilityDocument> _periodFacilityDocument;

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
                if (_periodFacilityElectricityGridMix == null)
                {
                    return new List<PeriodFacilityElectricityGridMix>();
                }
                return _periodFacilityElectricityGridMix;
            }
        }

        public IEnumerable<PeriodFacilityGasSupplyBreakDown> periodFacilityGasSupplyBreakDowns
        {
            get
            {
                if (_periodFacilityGasSupplyBreakDown == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakDown>();
                }
                return _periodFacilityGasSupplyBreakDown;
            }
        }

        public IEnumerable<PeriodFacilityDocument> periodFacilityDocuments
        {
            get
            {
                if (_periodFacilityDocument == null)
                {
                    return new List<PeriodFacilityDocument>();
                }
                return _periodFacilityDocument;
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
            _periodFacilityElectricityGridMix = new HashSet<PeriodFacilityElectricityGridMix>();
            _periodFacilityGasSupplyBreakDown = new HashSet<PeriodFacilityGasSupplyBreakDown>();
            _periodFacilityDocument = new HashSet<PeriodFacilityDocument>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }

        //private bool CheckPeriodFacilityRequiredDocumentType(IEnumerable<FacilityRequiredDocumentType> facilityRequiredDocumentTypeVOs, DocumentType documentType)
        //{
        //    var facilityReportingType = FacilityVO.ReportingType;
        //    var facilitySupplyChainStage = FacilityVO.SupplyChainStage;

        //    var facilityRequiredDocumentType = facilityRequiredDocumentTypeVOs.Where(x => x.ReportingType.Id == facilityReportingType.Id && x.SupplyChainStage.Id == facilitySupplyChainStage.Id && x.DocumentType.Id == documentType.Id).FirstOrDefault();

        //    if (facilityRequiredDocumentType is null)
        //        throw new NotFoundException("FacilityRequiredDocumentType not found !!");

        //    if (facilityRequiredDocumentType.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.NotAllowed)
        //        throw new BadRequestException("Document is not allowed for this ReportingPeriodFacility !!");

        //    return true;
        //}
        #region private methods

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


        #endregion

        #region PeriodFacilityElectricityGridMix
        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> reportingPeriodFacilityElectricityGridMixVOs, FercRegion fercRegion)
        {

            FercRegion = fercRegion;

            switch (fercRegion.Name)
            {
                case FercRegionvalues.Custom_Mix:
                    {
                        decimal contentValue = 0;
                        _periodFacilityElectricityGridMix.Clear();


                        foreach (var facilityElectricityGridMix in reportingPeriodFacilityElectricityGridMixVOs)
                        {
                            var existingElectricGridMixComponant = _periodFacilityElectricityGridMix.FirstOrDefault(x => x.ElectricityGridMixComponent.Id == facilityElectricityGridMix.ElectricityGridMixComponent.Id);

                            if (existingElectricGridMixComponant != null)
                                throw new Exception("ElectricityGridMix Component is Already Exists!!");


                            var gridmix = new PeriodFacilityElectricityGridMix(Id, facilityElectricityGridMix.ElectricityGridMixComponent, facilityElectricityGridMix.UnitOfMeasure, facilityElectricityGridMix.Content, facilityElectricityGridMix.IsActive);

                            _periodFacilityElectricityGridMix.Add(gridmix);

                            contentValue = contentValue + facilityElectricityGridMix.Content;


                        }
                        if (contentValue != 100)
                        {
                            throw new Exception("Content Value should be 100!!!");
                        }


                    }
                    break;
                default:
                    {
                        if (reportingPeriodFacilityElectricityGridMixVOs.Count() > 0)
                            throw new BadRequestException("FercRegion Should be Custom-Mix for Adding PeriodFacilityElectricityGridMix.");

                        if (_periodFacilityElectricityGridMix.Count() != 0)
                            _periodFacilityElectricityGridMix.Clear();
                    }
                    break;
            }
            return _periodFacilityElectricityGridMix;




        }



        internal bool LoadPeriodFacilityElecticGridMix(int reportingPeriodFacilityId, ElectricityGridMixComponent electricityComponent, UnitOfMeasure unitOfMeasure, decimal content, bool isActive)
        {

            var electricityGridMix = new PeriodFacilityElectricityGridMix(reportingPeriodFacilityId, electricityComponent, unitOfMeasure, content, isActive);

            return _periodFacilityElectricityGridMix.Add(electricityGridMix);

        }


        internal bool RemovePeriodFacilityElectricityGridMix()
        {
            _periodFacilityElectricityGridMix.Clear();
            return true;
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        internal IEnumerable<PeriodFacilityGasSupplyBreakDown> AddPeriodFacilityGasSupplyBreakDown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> periodFacilityGasSupplyBreakDownVOs)
        {
            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new Exception("SupplychainStage Should be Production !!");

            _periodFacilityGasSupplyBreakDown.Clear();

            foreach (var gasSupplyBreakdown in periodFacilityGasSupplyBreakDownVOs)
            {
                var existingSite = _periodFacilityGasSupplyBreakDown.Any(x => x.Site.Id == gasSupplyBreakdown.Site.Id);
                if (existingSite)
                    throw new BadRequestException("Site is Already Exist in PeriodFacility !!");

                var periodFacilityGasSupplyBreakDown = new PeriodFacilityGasSupplyBreakDown(Id, gasSupplyBreakdown.PeriodFacilityId, gasSupplyBreakdown.UnitOfMeasure, gasSupplyBreakdown.Site, gasSupplyBreakdown.Content);

                _periodFacilityGasSupplyBreakDown.Add(periodFacilityGasSupplyBreakDown);
            }

            return periodFacilityGasSupplyBreakDowns;


        }

        internal bool LoadPeriodFacilityGasSupplyBreakdown(int id, int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            var gasSupplyBreakdown = new PeriodFacilityGasSupplyBreakDown(id, Id, unitOfMeasure, site, content);

            return _periodFacilityGasSupplyBreakDown.Add(gasSupplyBreakdown);
        }

        internal bool RemovePeriodSupplierGasSupplyBreakdown(int periodFacilityId)
        {
            var periodFacilityGasSupplyBreakdowns = _periodFacilityGasSupplyBreakDown.Where(x => x.PeriodFacilityId == periodFacilityId).ToList();

            foreach (var gasSupply in periodFacilityGasSupplyBreakdowns)
            {
                _periodFacilityGasSupplyBreakDown.Remove(gasSupply);
            }

            return true;
        }
        #endregion

        #region PeriodFacilityDocument

        internal PeriodFacilityDocument AddPeriodFacilityDocument(string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, IEnumerable<FacilityRequiredDocumentType> facilityRequiredDocumentTypes, string collectionTimePeriod)
        {
            var facilityReportingType = FacilityVO.ReportingType;
            var facilitySupplyChainStage = FacilityVO.SupplyChainStage;

            var facilityRequiredDocumentType = facilityRequiredDocumentTypes.Where(x => x.ReportingType.Id == facilityReportingType.Id && x.SupplyChainStage.Id == facilitySupplyChainStage.Id && x.DocumentType.Id == documentType.Id).FirstOrDefault();

            if (facilityRequiredDocumentType is null)
                throw new NotFoundException("FacilityRequiredDocumentType not found !!");

            if (facilityRequiredDocumentType.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.NotAllowed)
                throw new BadRequestException("Document is not allowed for this ReportingPeriodFacility !!");

            var isExist = _periodFacilityDocument.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();


            var documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Processing);

            PeriodFacilityDocument? periodFacilityDocument = null;
            if (isExist.Count() == 0)
            {
                int version = 1;
                var GeneratedocumentName = GeneratedPeriodFacilityDocumentStoredName(collectionTimePeriod, documentType, version, displayName);
                periodFacilityDocument = new PeriodFacilityDocument(Id, version, displayName, GeneratedocumentName, null, null, documentStatus, documentType);

                _periodFacilityDocument.Add(periodFacilityDocument);
            }
            else
            {
                periodFacilityDocument = isExist.First();



                if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                {
                    var latestVersion = periodFacilityDocument.Version;
                    var updateVersion = latestVersion + 1;
                    var GenerateDocumentName = GeneratedPeriodFacilityDocumentStoredName(collectionTimePeriod, documentType, updateVersion, displayName);
                    periodFacilityDocument = new PeriodFacilityDocument(Id, updateVersion, displayName, GenerateDocumentName, null, null, documentStatus, documentType);
                    _periodFacilityDocument.Add(periodFacilityDocument);
                }
                else
                {
                    if (periodFacilityDocument.Path is not null)
                    throw new BadRequestException("This file already exists in.If you wish to the system upload a new version of the file, please delete the file and try the upload again");

                    var version = periodFacilityDocument.Version;

                    if (validationError is null)
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.NotValidated);
                    else
                    {
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.HasErrors);
                        path = null;
                        try
                        {
                            throw new Exception("Unable to Uploading Document..!");
                        }
                        catch (Exception ex)
                        {
                            validationError = ex.Message;
                        }
                    }

                    if (path is not null && validationError is null)
                    {
                        documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Validated);

                        //version += 1;
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

            _periodFacilityDocument.Add(periodFacilityDocument);

            return true;
        }

        internal bool RemovePeriodFacilityDocument(int periodFacilityDocumentId)
        {
      
                var periodFacilityDocument = _periodFacilityDocument.FirstOrDefault(x => x.Id == periodFacilityDocumentId);

            
                _periodFacilityDocument.Remove(periodFacilityDocument);
                return true;
            
        }
        #endregion

        #region UpdateFacilityDataStatuses
        internal void UpdatePeriodFacilityDataStatusSubmittedToInProgress(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Submitted)
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
        }
        internal void UpdatePeriodFacilityDataStatusCompleteToSubmitted(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            if (FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete)
                FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            else
                throw new Exception("PeriodFacilityDataStatus update is not completed !!");
        }
        #endregion

    }
}











