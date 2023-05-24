using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Factories.Interface;
using Services.Interfaces;
using Services.Mappers.Interfaces;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace Services;

public class ReportingPeriodServices : IReportingPeriodServices
{
    private IReportingPeriodFactory _reportingPeriodFactory;
    private readonly ILogger _logger;
    private IReportingPeriodEntityDomainMapper _reportingPeriodEntityDomainMapper;
    private ISupplierEntityDomainMapper _supplierEntityDomainMapper;
    private ISupplierDomainDtoMapper _supplierDomainDtoMapper;
    private IReportingPeriodDataActions _reportingPeriodDataActions;
    private IUploadDocuments _uploadDocuments;
    private ISupplierDataActions _supplierDataActions;
    private IReferenceLookUpMapper _referenceLookUpMapper;
    private IReadOnlyEntityToDtoMapper _readOnlyEntityToDtoMapper;
    private IReportingPeriodDomainDtoMapper _reportingPeriodDomainDtoMapper;

    public ReportingPeriodServices(IReportingPeriodFactory reportingPeriodFactory, ILoggerFactory loggerFactory, IReportingPeriodEntityDomainMapper reportingPeriodEntityDomainMapper,
            ISupplierEntityDomainMapper supplierEntityDomainMapper,
            ISupplierDomainDtoMapper supplierDomainDtoMapper,
           IReportingPeriodDataActions reportingPeriodDataActions, IUploadDocuments uploadDocuments, ISupplierDataActions supplierDataActions, IReferenceLookUpMapper referenceLookUpMapper,
           IReadOnlyEntityToDtoMapper readOnlyEntityToDtoMapper,
           IReportingPeriodDomainDtoMapper reportingPeriodDomainDtoMapper)
    {
        _reportingPeriodFactory = reportingPeriodFactory;
        _logger = loggerFactory.CreateLogger<SupplierServices>();
        _reportingPeriodEntityDomainMapper = reportingPeriodEntityDomainMapper;
        _supplierEntityDomainMapper = supplierEntityDomainMapper;
        _supplierDomainDtoMapper = supplierDomainDtoMapper;
        _reportingPeriodDataActions = reportingPeriodDataActions;
        _uploadDocuments = uploadDocuments;
        _supplierDataActions = supplierDataActions;
        _referenceLookUpMapper = referenceLookUpMapper;
        _readOnlyEntityToDtoMapper = readOnlyEntityToDtoMapper;
        _reportingPeriodDomainDtoMapper = reportingPeriodDomainDtoMapper;

    }

    #region Private Methods

    private IEnumerable<ReportingPeriodType> GetAndConvertReportingPeriodTypes()
    {
        var reportingPeriodTypeEntity = _reportingPeriodDataActions.GetReportingPeriodTypes().Where(x => x.IsActive).ToList();

        return _referenceLookUpMapper.GetReportingPeriodTypesLookUp(reportingPeriodTypeEntity);
    }

    private IEnumerable<ReportingPeriodStatus> GetAndConvertReportingPeriodStatuses()
    {
        var reportingPeriodStatusEntity = _reportingPeriodDataActions.GetReportingPeriodStatus().Where(x => x.IsActive).ToList();

        return _referenceLookUpMapper.GetReportingPeriodStatusesLookUp(reportingPeriodStatusEntity);
    }

    private IEnumerable<SupplierReportingPeriodStatus> GetAndConvertSupplierPeriodStatuses()
    {
        var supplierPeriodStatusEntity = _reportingPeriodDataActions.GetSupplierReportingPeriodStatus();

        return _referenceLookUpMapper.GetSupplierReportingPeriodStatusesLookUp(supplierPeriodStatusEntity);
    }

    private IEnumerable<FacilityReportingPeriodDataStatus> GetAndConvertFacilityReportingPeriodDataStatuses()
    {
        var facilityReportingPeriodDataStatus = _reportingPeriodDataActions.GetFacilityReportingPeriodDataStatus();

        return _referenceLookUpMapper.GetFacilityReportingPeriodDataStatusLookUp(facilityReportingPeriodDataStatus);
    }

    private IEnumerable<ReportingType> GetAndConvertReportingTypes()
    {
        var reportingType = _reportingPeriodDataActions.GetReportingTypes();

        return _referenceLookUpMapper.GetReportingTypeLookUp(reportingType);
    }

    private IEnumerable<SupplyChainStage> GetAndConvertSupplyChainStages()
    {
        var supplyChainStageEntity = _supplierDataActions.GetSupplyChainStages();
        return _referenceLookUpMapper.GetSupplyChainStagesLookUp(supplyChainStageEntity);
    }

    private IEnumerable<ElectricityGridMixComponent> GetAndConvertElectricityGridMixComponents()
    {
        var electricityGridMixComponents = _reportingPeriodDataActions.GetElectricityGridMixComponentEntities();
        return _referenceLookUpMapper.GetElectricityGridMixComponentsLookUp(electricityGridMixComponents);
    }

    private IEnumerable<UnitOfMeasure> GetAndConvertUnitOfMeasures()
    {
        var unitOfMeasures = _reportingPeriodDataActions.GetUnitOfMeasureEntities();
        return _referenceLookUpMapper.GetUnitOfMeasuresLookUp(unitOfMeasures);
    }

    private IEnumerable<FercRegion> GetAndConvertFercRegions()
    {
        var fercRegions = _reportingPeriodDataActions.GetFercRegionEntities();
        return _referenceLookUpMapper.GetFercRegionsLookUp(fercRegions);
    }

    private IEnumerable<Site> GetAndConvertSites()
    {
        var sites = _reportingPeriodDataActions.GetSiteEntities();
        return _referenceLookUpMapper.GetSitesLookUp(sites);
    }

    private IEnumerable<DocumentStatus> GetDocumentStatuses()
    {
        var documentStatuses = _reportingPeriodDataActions.GetDocumentStatusEntities();
        return _referenceLookUpMapper.GetDocumentStatusesLookUp(documentStatuses);
    }

    private IEnumerable<DocumentType> GetDocumentTypes()
    {
        var documentTypes = _reportingPeriodDataActions.GetDocumentTypeEntities();
        return _referenceLookUpMapper.GetDocumentTypesLookUp(documentTypes);
    }

    private IEnumerable<DocumentRequiredStatus> GetDocumentRequiredStatuses()
    {
        var documentRequiredStatus = _reportingPeriodDataActions.GetDocumentRequiredStatus();
        return _referenceLookUpMapper.GetDocumentRequiredStatusesLookUp(documentRequiredStatus);
    }


    private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            {".xlsx",new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                    new byte[] { 0x44, 0xB1, 0xE6, 0x01, 0x00, 0x00, 0x15, 0x0E },
                    new byte[] { 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x54 },
                    new byte[] { 0x6C, 0x20, 0xA2, 0xD0, 0x01, 0x28, 0xA0, 0x00 },
                    new byte[] { 0x08, 0x00, 0x00, 0x00, 0x21, 0x00, 0x7E, 0xCB },
                    new byte[] { 0x00, 0x00, 0x13, 0x00, 0xD4, 0x01, 0x5B, 0x43 },
                    new byte[] { 0x79, 0x70, 0x65, 0x73, 0x5D, 0x2E, 0x78, 0x6D },
                    new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                }
            },
            {
                ".xml",new List<byte[]>
                {
                    new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20},
                    new byte[] { 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x6D, 0x00, 0x6C, 0x00, 0x20},
                    new byte[] { 0x00, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20},
                    new byte[] { 0x3C, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00 },
                    new byte[] { 0x4C, 0x6F, 0xA7, 0x94, 0x93, 0x40}
                }
            }
        };

    /// <summary>
    /// Validate file signature
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private bool ValidateFileSignature(string filePath, string fileName)
    {
        using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            string[] permittedExtensions = { ".xlsx", ".xml" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !permittedExtensions.Contains(fileExtension))
            {
                throw new Exception("File extension is invalid.");
            }

            var signatures = _fileSignature[fileExtension];
            var headerBytes = reader.ReadBytes(signatures.Max(x => x.Length));
            return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            /*string? error = null;
            if (!isFileSignature)
                error = "File Signature is not match !!";
            return error;*/
        }
    }

    /*
     private bool CheckDocumentsUploadedForFacility(int periodFacilityId)
    {
        bool isDone = false;
        int counter = 0;
        var periodFacility = _reportingPeriodDataActions.GetPeriodFacilityById(periodFacilityId);

        var facility = periodFacility.Facility;
        var reportingType = facility.ReportingType;
        var supplyChainStage = facility.SupplyChainStage;
        var facilityRequiredDocumentTypes = GetFacilityRequiredDocumentTypeVOs().Where(x => x.ReportingType.Id == reportingType.Id && x.SupplyChainStage.Id == supplyChainStage.Id && x.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.Required).ToList();

        if (facilityRequiredDocumentTypes.Count() <= periodFacility.ReportingPeriodFacilityDocumentEntities.Count())
        {
            for (int i = 0; i < facilityRequiredDocumentTypes.Count(); i++)
            {
                var requiredDocument = facilityRequiredDocumentTypes[i].DocumentType.Id;
                var uploadedDocument = periodFacility.ReportingPeriodFacilityDocumentEntities.ToList()[i].DocumentTypeId;

                if (requiredDocument == uploadedDocument)
                    counter++;
            }
        }
        if (counter == facilityRequiredDocumentTypes.Count())
            isDone = true;

        return isDone;
    }

    private void UpdatePeriodFacilityStatusToComplete(int periodFacilityId)
    {
        var isDone = CheckDocumentsUploadedForFacility(periodFacilityId);

        var facilityDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);

        if (isDone)
            _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityId, facilityDataStatus.Id);
    }
     */

    private void UpdatePeriodFacilityDataStatusInProgressToComplete(int periodFacilityId)
    {
        var facilityDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Complete);

        _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityId, facilityDataStatus.Id);
    }

    private void UpdatePeriodFacilityDataStatusSubmittedToInprogress(int periodFacilityId)
    {
        var periodFacilityDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
        _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityId, periodFacilityDataStatus.Id);
    }

    /*private string ValidateFile(string fileType, long fileSize, string path)
    {
        string? error = null;
        var fileTypes = new List<string>();
        fileTypes.Add(".xlsx");
        fileTypes.Add(".xml");

        var isCorrect = fileTypes.Contains(fileType);
        if (!isCorrect)
            error += "FileType is not match.";

        //Check file signature
        var fileError = ValidateFileSignature(path, fileType);
        if (fileError != null)
            error += fileError;

        //Check File size (should be 20MB)
        long sizeInBytes = fileSize;
        long maxSizeInBytes = 20971520;
        if (sizeInBytes > maxSizeInBytes)
            error += "Filesize should be in 20Mb";

        return error;
    }*/

    /// <summary>
    /// Retrieve ReportingPeriod Entity and Convert it to DomainModel
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private ReportingPeriod RetrieveAndConvertReportingPeriod(int reportingPeriodId)
    {
        var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);
        var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses();

        if (reportingPeriodEntity is null)
            throw new ArgumentNullException("ReportingPeriodEntity not found !!");

        var reportingPeriodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

        //Load existing PeriodSuppliersList in ReportingPeriodDomain
        foreach (var periodSupplier in reportingPeriodEntity.ReportingPeriodSupplierEntities)
        {
            var supplierVO = GetAndConvertSupplierValueObject(periodSupplier.SupplierId);
            var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == periodSupplier.SupplierReportingPeriodStatusId);
            reportingPeriodDomain.LoadPeriodSupplier(periodSupplier.Id, supplierVO, supplierReportingPeriodStatus, periodSupplier.InitialDataRequestDate, periodSupplier.ResendDataRequestDate);
        }

        foreach (var periodFacility in reportingPeriodEntity.ReportingPeriodFacilityEntities)
        {
            GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(periodFacility, reportingPeriodDomain);

            GetAndLoadPeriodFacilityDocuments(periodFacility, reportingPeriodDomain);

            /* var facilityVO = GetAndConvertFacilityValueObject(periodFacility.FacilityId);
             var facilityReportingPeriodStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Id == periodFacility.FacilityReportingPeriodDataStatusId);
             var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacility.FercRegionId);

             var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacility.ReportingPeriodSupplierId);

             //Load existing periodFacilityList
             reportingPeriodDomain.LoadPeriodFacility(periodFacility.Id, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, fercRegion, periodFacility.IsActive);

             //Load existing periodFacilityElecrticityGridMixList
             var electricityGridMixEntities = periodFacility.ReportingPeriodFacilityElectricityGridMixEntities;

             if (electricityGridMixEntities.Count() != 0)
             {
                 var electricityGridMixComponents = GetAndConvertElectricityGridMixComponents();
                 var electricityGridMixComponentPercentVOs = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects(electricityGridMixEntities, electricityGridMixComponents);

                 var unitOfMeasureId = electricityGridMixEntities.First().UnitOfMeasureId;
                 var unitOfMeasure = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == unitOfMeasureId);

                 reportingPeriodDomain.LoadPeriodFacilityElectricityGridMix(periodFacility.Id, periodSupplier.Id, unitOfMeasure, electricityGridMixComponentPercentVOs);

             }

             //Load existing periodFacilityGasSupplyBreakdownList
             var gasSupplyBreakdownEntities = periodFacility.ReportingPeriodFacilityGasSupplyBreakDownEntities;

             if (gasSupplyBreakdownEntities.Count() != 0)
             {
                 var sites = GetAndConvertSites();
                 var unitOfMeasures = GetAndConvertUnitOfMeasures();
                 var gasSupplyBreakdownVOs = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjects(gasSupplyBreakdownEntities, sites, unitOfMeasures);

                 reportingPeriodDomain.LoadPeriodFacilityGasSupplyBreakdown(periodSupplier.Id, gasSupplyBreakdownVOs);
             }*/

            //Load existing PeriodFacilityDocument
            /*var periodFacilityDocumentEntities = periodFacility.ReportingPeriodFacilityDocumentEntities;
            if (periodFacilityDocumentEntities.Count() != 0)
                GetAndConvertPeriodFacilityToPeriodFacilityDocument(periodFacility.Id, reportingPeriodDomain);*/

        }

        return reportingPeriodDomain;

    }

    private PeriodSupplier RetrieveAndConvertReportingPeriodSupplierFacility(int periodFacilityId)
    {
        var periodFacilityEntity = _reportingPeriodDataActions.GetPeriodFacilityById(periodFacilityId);
        var reportingPeriodEntity = periodFacilityEntity.ReportingPeriod;

        if (periodFacilityEntity is null)
            throw new NotFoundException("ReportingPeriodFacilityEntity not found !!");

        var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses();

        var reportingPeriodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

        //Load PeriodSupplier
        var supplierVO = GetAndConvertSupplierValueObject(periodFacilityEntity.ReportingPeriodSupplier.SupplierId);
        var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplier.SupplierReportingPeriodStatusId);
        reportingPeriodDomain.LoadPeriodSupplier(periodFacilityEntity.ReportingPeriodSupplierId, supplierVO, supplierReportingPeriodStatus, periodFacilityEntity.ReportingPeriodSupplier.InitialDataRequestDate, periodFacilityEntity.ReportingPeriodSupplier.ResendDataRequestDate);

        /*if (periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities.Count() == 0)
            throw new NotFoundException("ElectricityGridMixes not available in this PeriodFacility !!");*/

        GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(periodFacilityEntity, reportingPeriodDomain);
        GetAndLoadPeriodFacilityDocuments(periodFacilityEntity, reportingPeriodDomain);
        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        return periodSupplier;
    }

    private void GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(ReportingPeriodFacilityEntity periodFacilityEntity, ReportingPeriod reportingPeriodDomain)
    {
        var facilityVO = GetAndConvertFacilityValueObject(periodFacilityEntity.FacilityId);
        var facilityReportingPeriodStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Id == periodFacilityEntity.FacilityReportingPeriodDataStatusId);
        var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacilityEntity.FercRegionId);

        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        reportingPeriodDomain.LoadPeriodFacility(periodFacilityEntity.Id, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, fercRegion, periodFacilityEntity.IsActive);

        //Load PeriodFacilityElectricityGridMixes
        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityEntity.Id);
        var gridMixEntities = periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities;

        if (gridMixEntities.Count() != 0)
        {
            var gridMixComponents = GetAndConvertElectricityGridMixComponents();

            var gridMixValueObjectList = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixEntitiesToValueObjects(gridMixEntities, gridMixComponents);
            var unitOfMeasureId = gridMixEntities.First().UnitOfMeasureId;

            var unitOfMeasure = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == unitOfMeasureId);

            reportingPeriodDomain.LoadPeriodFacilityElectricityGridMix(periodFacility.Id, periodFacility.ReportingPeriodSupplierId, unitOfMeasure, gridMixValueObjectList);
        }

        //Load PeriodFacilityGasSupplyBreakdowns
        var gasSupplyEntities = periodFacilityEntity.ReportingPeriodFacilityGasSupplyBreakDownEntities;

        if (gasSupplyEntities.Count() != 0)
        {
            var sites = GetAndConvertSites();
            var unitOfMeasures = GetAndConvertUnitOfMeasures();
            var gasSupplyValueObjectList = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjects(gasSupplyEntities, sites, unitOfMeasures);

            reportingPeriodDomain.LoadPeriodFacilityGasSupplyBreakdown(periodFacility.ReportingPeriodSupplierId, gasSupplyValueObjectList);
        }
    }

    private void GetAndLoadPeriodFacilityDocuments(ReportingPeriodFacilityEntity periodFacilityEntity, ReportingPeriod reportingPeriodDomain)
    {
        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        // Load PeriodFacilityDocuments
        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityEntity.Id);
        var periodFacilityDocumentEntities = periodFacilityEntity.ReportingPeriodFacilityDocumentEntities;

        if (periodFacilityDocumentEntities.Count() != 0)
        {
            foreach (var periodFacilityDocumentEntity in periodFacilityDocumentEntities)
            {
                var documentStatus = GetDocumentStatuses().First(x => x.Id == periodFacilityDocumentEntity.DocumentStatusId);
                var documentType = GetDocumentTypes().First(x => x.Id == periodFacilityDocumentEntity.DocumentTypeId);

                reportingPeriodDomain.LoadPeriodFacilityDocument(periodFacilityDocumentEntity.Id, periodSupplier.Id, periodFacilityDocumentEntity.ReportingPeriodFacilityId, periodFacilityDocumentEntity.Version, periodFacilityDocumentEntity.DisplayName, periodFacilityDocumentEntity.StoredName, periodFacilityDocumentEntity.Path, documentStatus, documentType, periodFacilityDocumentEntity.ValidationError);
            }
        }
    }

    /// <summary>
    /// GetAndConvert PeriodFacilityEntity To PeriodFacilityDocument
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    private PeriodSupplier GetAndConvertPeriodFacilityToPeriodFacilityDocument(int periodFacilityId, ReportingPeriod reportingPeriod)
    {
        var periodFacilityEntity = _reportingPeriodDataActions.GetPeriodFacilityById(periodFacilityId);
        if (periodFacilityEntity is null)
            throw new NotFoundException("PeriodFacilityEntity is not found !!");

        var periodSupplierDomain = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        var periodFacilityDocumentEntities = periodFacilityEntity.ReportingPeriodFacilityDocumentEntities;

        foreach (var periodFacilityDocument in periodFacilityDocumentEntities)
        {
            var documentStatus = GetDocumentStatuses().First(x => x.Id == periodFacilityDocument.DocumentStatusId);
            var documentType = GetDocumentTypes().First(x => x.Id == periodFacilityDocument.DocumentTypeId);

            reportingPeriod.LoadPeriodFacilityDocument(periodFacilityDocument.Id, periodSupplierDomain.Id, periodFacilityDocument.ReportingPeriodFacilityId, periodFacilityDocument.Version, periodFacilityDocument.DisplayName, periodFacilityDocument.StoredName, periodFacilityDocument.Path, documentStatus, documentType, periodFacilityDocument.ValidationError);
        }

        return periodSupplierDomain;
    }

    /// <summary>
    /// Get SupplierEntity and Convert it to SupplierVO
    /// </summary>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    private SupplierVO GetAndConvertSupplierValueObject(int supplierId)
    {
        var supplierEntity = _supplierDataActions.GetSupplierById(supplierId);
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();

        if (supplierEntity == null)
            throw new BadRequestException("Supplier not found !!");

        var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierEntityToSupplierValueObject(supplierEntity, supplyChainStages, reportingTypes);
        return supplierVO;
    }

    /// <summary>
    /// Get FacilityEntity and convert it to FacilityVO
    /// </summary>
    /// <param name="facilityId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    private FacilityVO GetAndConvertFacilityValueObject(int facilityId)
    {
        var facilityEntity = _supplierDataActions.GetFacilityById(facilityId);
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();

        if (facilityEntity == null)
            throw new BadRequestException("Facility not found !!");

        var facilityVO = _reportingPeriodEntityDomainMapper.ConvertFacilityEntityToFacilityValueObject(facilityEntity, supplyChainStages, reportingTypes);

        return facilityVO;
    }

    private IEnumerable<FacilityRequiredDocumentTypeVO> GetAndConvertFacilityRequiredDocumentTypeVO()
    {
        var facilityRequiredDocumentTypeEntities = _reportingPeriodDataActions.GetFacilityRequiredDocumentTypeEntities();
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();
        var documentTypes = GetDocumentTypes();
        var documentRequiredStatuses = GetDocumentRequiredStatuses();

        var facilityRequiredDocumentTypeVo = _reportingPeriodEntityDomainMapper.ConvertFacilityRequiredDocumentTypeEntitiesToValueObjects(facilityRequiredDocumentTypeEntities, reportingTypes, supplyChainStages, documentTypes, documentRequiredStatuses);

        return facilityRequiredDocumentTypeVo;
    }

    private ReportingPeriod ConfigureReportingPeriod(ReportingPeriodEntity reportingPeriodEntity, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses)
    {
        var reportingPeriodType = reportingPeriodTypes.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodTypeId).ToList();
        var reportingPeriodStatus = reportingPeriodStatuses.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodStatusId).ToList();

        //Convert entity to domain
        var reportingPeriodDomain = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodType, reportingPeriodStatus);
        return reportingPeriodDomain;

    }

    #endregion

    #region Add-Update-Remove Methods

    /// <summary>
    /// Add ReportingPeriod
    /// </summary>
    /// <param name="reportingPeriodDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
    {
        if (reportingPeriodDto.Id == 0)
        {
            //Add ReportingPeriod

            var reportingPeriodType = GetAndConvertReportingPeriodTypes().FirstOrDefault(x => x.Id == reportingPeriodDto.ReportingPeriodTypeId);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses().FirstOrDefault(x => x.Id == reportingPeriodDto.ReportingPeriodStatusId);

            if (reportingPeriodType is null)
                throw new Exception("ReportingPeriodType not found !!");
            if (reportingPeriodStatus is null)
                throw new Exception("ReportingPeriodStatus not found !!");

            var reportingPeriod = _reportingPeriodFactory.CreateNewReportingPeriod(reportingPeriodType, reportingPeriodDto.CollectionTimePeriod, reportingPeriodStatus, reportingPeriodDto.StartDate, reportingPeriodDto.EndDate, reportingPeriodDto.IsActive);

            var reportingPeriodEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodDomainToEntity(reportingPeriod);
            _reportingPeriodDataActions.AddReportingPeriod(reportingPeriodEntity);
        }
        else
        {
            //Update ReportingPeriod
            var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodDto.Id ?? 0);

            //Fetch existing ReportingPeriodStatus and ReportingPeriodType data
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses().Where(x => x.Id == reportingPeriodDto.ReportingPeriodStatusId).FirstOrDefault();
            var reportingPeriodType = GetAndConvertReportingPeriodTypes().Where(x => x.Id == reportingPeriodDto.ReportingPeriodTypeId).FirstOrDefault();
            var supplierReportingPeriodStatuses = GetAndConvertSupplierPeriodStatuses();

            if (reportingPeriodType is null)
                throw new Exception("ReportingPeriodType not found !!");
            if (reportingPeriodStatus is null)
                throw new Exception("ReportingPeriodStatus not found !!");

            reportingPeriod.UpdateReportingPeriod(reportingPeriodType, reportingPeriodDto.CollectionTimePeriod, reportingPeriodStatus, reportingPeriodDto.StartDate, reportingPeriodDto.EndDate, reportingPeriodDto.IsActive, supplierReportingPeriodStatuses);

            //Convert domain to entity
            var entity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodDomainToEntity(reportingPeriod);
            _reportingPeriodDataActions.UpdateReportingPeriod(entity);

        }

        return "ReportingPeriod added or updated successfully !!";
    }

    /// <summary>
    /// Set MultiplePeriodSuppliers
    /// </summary>
    /// <param name="multiplePeriodSuppliersDto"></param>
    /// <returns></returns>
    public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(multiplePeriodSuppliersDto.ReportingPeriodId);

        var supplierPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == multiplePeriodSuppliersDto.SupplierReportingPeriodStatusId);

        foreach (var supplierId in multiplePeriodSuppliersDto.SupplierIds)
        {
            var supplierVO = GetAndConvertSupplierValueObject(supplierId);

            var periodSupplier = reportingPeriod.AddPeriodSupplier(multiplePeriodSuppliersDto.Id, supplierVO, supplierPeriodStatus ?? new SupplierReportingPeriodStatus(), multiplePeriodSuppliersDto.InitialDataRequestDate, multiplePeriodSuppliersDto.ResendDataRequestDate);

            var periodSupplierEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplier);
            _reportingPeriodDataActions.AddPeriodSupplier(periodSupplierEntity);
        }

        return "Multiple PeriodSupplier added successfully...";
    }

    /// <summary>
    /// Add Multiple PeriodFacility
    /// </summary>
    /// <param name="reportingPeriodFacilityDto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string AddRemovePeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodFacilityDto.ReportingPeriodId);

        var facilityReportingPeriodDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.FacilityReportingPeriodDataStatusId);

        var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.None);

        foreach (var facilityId in reportingPeriodFacilityDto.FacilityIds)
        {
            var facilityVO = GetAndConvertFacilityValueObject(facilityId);

            var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.ReportingPeriodSupplierId);
            if (periodSupplier is null)
                throw new ArgumentNullException("ReportingPeriodSupplier not found !!");

            var periodFacility = reportingPeriod.AddPeriodFacility(reportingPeriodFacilityDto.Id, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodFacilityDto.ReportingPeriodSupplierId, reportingPeriodFacilityDto.FacilityIsRelevantForPeriod, fercRegion, reportingPeriodFacilityDto.IsActive);

            var periodFacilityEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDomainToEntity(periodFacility);
            _reportingPeriodDataActions.AddPeriodFacility(periodFacilityEntity, reportingPeriodFacilityDto.FacilityIsRelevantForPeriod);
        }

        return "ReportingPeriodFacilities added or removed successfully";
    }

    /// <summary>
    /// LockUnlockPeriodSupplierStatus
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
    {
        var periodSupplierEntity = _reportingPeriodDataActions.GetPeriodSupplierById(periodSupplierId);

        if (periodSupplierEntity is null)
            throw new BadRequestException("PeriodSupplier is not found !!");

        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodSupplierEntity.ReportingPeriodId);
        var periodSupplierStatus = GetAndConvertSupplierPeriodStatuses();

        reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(periodSupplierId, periodSupplierStatus);

        var entities = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSuppliersDomainToEntity(reportingPeriod.PeriodSuppliers);

        _reportingPeriodDataActions.UpdateReportingPeriodSuppliers(entities);

        return "PeriodSupplierStatus is updated successfully...";
    }

    /// <summary>
    /// AddRemove ReportingPeriodFacility ElectricityGridMix Components.
    /// Per PeriodFacility ContentValues should be 100%
    /// Per PeriodFacility allowed maximum components is 9
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    public string AddRemovePeriodFacilityElectricityGridMix(MultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto)
    {
        var electricityGridMixComponent = GetAndConvertElectricityGridMixComponents();
        var unitOfMeasure = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == periodFacilityElectricityGridMixDto.UnitOfMeasureId);

        if (unitOfMeasure is null)
            throw new NotFoundException("UnitOfMeasure is not found !!");

        var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacilityElectricityGridMixDto.FercRegionId);

        if (fercRegion is null)
            throw new NotFoundException("FercRegion is not found !!");

        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodFacilityElectricityGridMixDto.ReportingPeriodId);

        var valueObjectLists = _reportingPeriodDomainDtoMapper.ConvertPeriodElectricityGridMixDtosToValueObjects(periodFacilityElectricityGridMixDto.ReportingPeriodFacilityElectricityGridMixDtos, electricityGridMixComponent);

        var facilityElectricityGridMixDomain = reportingPeriod.AddPeriodFacilityElectricityGridMix(periodFacilityElectricityGridMixDto.ReportingPeriodFacilityId, periodFacilityElectricityGridMixDto.SupplierId, unitOfMeasure, fercRegion, valueObjectLists);

        var entity = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixDomainListToEntities(facilityElectricityGridMixDomain);

        _reportingPeriodDataActions.AddPeriodFacilityElectricityGridMix(entity, periodFacilityElectricityGridMixDto.ReportingPeriodFacilityId, periodFacilityElectricityGridMixDto.FercRegionId);

        return "ReportingPeriodFacility ElectricityGridMix Components added successfully !!";
    }

    /// <summary>
    /// AddRemove ReportingPeriodFacility GasSupplyBreakdown
    /// Per periodSupplier per site ContentValues should be 100
    /// </summary>
    /// <param name="periodFacilityGasSupplyBreakdownDto"></param>
    /// <returns></returns>
    public string AddRemovePeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakdownDto periodFacilityGasSupplyBreakdownDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodFacilityGasSupplyBreakdownDto.ReporingPeriodId);

        var sites = GetAndConvertSites();
        var unitOfMeasures = GetAndConvertUnitOfMeasures();

        var valueObjectList = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityGasSupplyBreakDownDtosToValueObjects(periodFacilityGasSupplyBreakdownDto.PeriodFacilityGasSupplyBreakdowns, sites, unitOfMeasures);

        var gasSupplyDomainList = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(periodFacilityGasSupplyBreakdownDto.ReportingPeriodSupplierId, valueObjectList);

        var gasSupplyBreakdownEntities = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityGasSupplyBreakdownDomainListToEntities(gasSupplyDomainList);

        _reportingPeriodDataActions.AddPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownEntities, periodFacilityGasSupplyBreakdownDto.ReportingPeriodSupplierId);

        return "PeriodFacilityGasSupplyBreakdown is added successfully...";
    }

    /// <summary>
    /// AddUpdate ReportingPeriodFacilityDocument
    /// </summary>
    /// <param name="reportingPeriodDocumentDto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public string AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentDto reportingPeriodDocumentDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodDocumentDto.ReportingPeriodId);

        var documentStatuses = GetDocumentStatuses();
        if (documentStatuses is null)
            throw new NotFoundException("DocumentStatus is not found !!");

        var documentType = GetDocumentTypes().FirstOrDefault(x => x.Id == reportingPeriodDocumentDto.DocumentTypeId);
        if (documentType is null)
            throw new NotFoundException("DocumentType is not found !!");

        var facilityRequiredDocumentTypeVos = GetAndConvertFacilityRequiredDocumentTypeVO();

        var periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(reportingPeriodDocumentDto.SupplierId, reportingPeriodDocumentDto.PeriodFacilityId, reportingPeriodDocumentDto.DocumentFile.FileName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVos);

        var periodFacilityDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDocumentDomainToEntity(periodFacilityDocument);

        if (periodFacilityDocumentEntity.Id == 0)
            _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(periodFacilityDocumentEntity);

        //change status Submitted To InProgress
        var progressDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

        reportingPeriod.UpdatePeriodFacilityDataStatusSubmittedToInProgress(reportingPeriodDocumentDto.SupplierId, periodFacilityDocumentEntity.ReportingPeriodFacilityId, progressDataStatus);

        _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityDocument.ReportingPeriodFacilityId, progressDataStatus.Id);

        var documentPath = _uploadDocuments.UploadFile(reportingPeriodDocumentDto.DocumentFile);

        ValidateFileSignature(documentPath, reportingPeriodDocumentDto.DocumentFile.FileName);

        var error = _uploadDocuments.validationError(reportingPeriodDocumentDto.DocumentFile);

        var updatedPeriodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(reportingPeriodDocumentDto.SupplierId, reportingPeriodDocumentDto.PeriodFacilityId, reportingPeriodDocumentDto.DocumentFile.FileName, documentPath, error, documentStatuses, documentType, facilityRequiredDocumentTypeVos);

        var updatedPeriodFacilityDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDocumentDomainToEntity(updatedPeriodFacilityDocument);

        _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(updatedPeriodFacilityDocumentEntity);

        if (error != null)
            throw new Exception(error);

        //Check facilityRequiredDocumentType is required than update facilityDataStatus is InProgress To Complete
        var requiredDocument = facilityRequiredDocumentTypeVos.Any(x => x.DocumentType.Id == updatedPeriodFacilityDocumentEntity.DocumentTypeId && x.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.Required);
        if (requiredDocument)
            UpdatePeriodFacilityDataStatusInProgressToComplete(reportingPeriodDocumentDto.PeriodFacilityId);

        return "ReportingPeriodFacilityDocument is added successfully...";
    }

    /// <summary>
    /// Update ReportingPeriodFacilityDataStatus Complete To Submitted
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    public string UpdatePeriodFacilityDataStatusCompleteToSubmitted(int reportingPeriodId, int periodSupplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierId);
        var periodFacilities = periodSupplier.PeriodFacilities;

        var facilityDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
        reportingPeriod.UpdatePeriodFacilityDataStatusCompleteToSubmitted(periodSupplierId, facilityDataStatus);

        var entities = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilitiesDomainToEntity(periodFacilities);

        _reportingPeriodDataActions.UpdatePeriodFacilities(entities);

        return "Update PeriodFacilityDataStatus is successfully...";
    }

    public string RemovePeriodFacilityDocument(int documentId)
    {
        var periodFacilityDocument = _reportingPeriodDataActions.GetReportingPeriodFacilityDocumentById(documentId);

        var periodSupplier = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityDocument.ReportingPeriodFacilityId);

        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodSupplier.ReportingPeriodId);

        var isRemoved = reportingPeriod.RemovePeriodFacilityDocument(periodSupplier.Id, periodFacilityDocument.ReportingPeriodFacilityId, documentId);

        if (isRemoved)
            _reportingPeriodDataActions.RemovePeriodFacilityDocument(documentId);

        //Check RequiredDocuments are uploaded or not for PeriodFacility 
        //If not uploaded then change FacilityStatus Submitted to InProgress
        UpdatePeriodFacilityDataStatus(periodFacilityDocument.ReportingPeriodFacilityId);

        return "PeriodFacilityDocument is removed successfully...";
    }

    private void UpdatePeriodFacilityDataStatus(int periodFacilityId)
    {
        var periodFacility = _reportingPeriodDataActions.GetPeriodFacilityById(periodFacilityId);

        var periodFacilityDocuments = periodFacility.ReportingPeriodFacilityDocumentEntities;

        var facilityRequiredDocumentTypeVos = GetAndConvertFacilityRequiredDocumentTypeVO();
        var requiredDocument = facilityRequiredDocumentTypeVos.Where(x => x.ReportingType.Id == periodFacility.ReportingTypeId && x.SupplyChainStage.Id == periodFacility.SupplyChainStageId && x.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.Required).ToList();

        if (requiredDocument.Count() != periodFacilityDocuments.Count())
            UpdatePeriodFacilityDataStatusSubmittedToInprogress(periodFacilityId);

    }

    #endregion

    #region GetMethods

    /// <summary>
    /// Get Active ReportingPeriodList
    /// [Without PeriodSuppliersList]
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
    {
        var activeReportingPeriods = _reportingPeriodDataActions.GetReportingPeriods().Where(x => x.IsActive);
        var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
        var reportingPeriodStatuses = GetAndConvertReportingPeriodStatuses();

        var reportingPeriodDomainList = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntitiesToDomain(activeReportingPeriods, reportingPeriodTypes, reportingPeriodStatuses);

        var reportingPeriodDtos = _reportingPeriodDomainDtoMapper.ConvertReportingPeriodDomainListToDtos(reportingPeriodDomainList);

        return reportingPeriodDtos;
    }

    /// <summary>
    /// Get ReportingPeriodSupplierFacilities
    /// List of PeriodSupplier facilities.
    /// Relevant and NotRelevant both are display with status 'FacilityIsRelevantForPeriod'
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
    {
        var periodSupplierEntity = _reportingPeriodDataActions.GetPeriodSupplierById(periodSupplierId);

        if (periodSupplierEntity == null)
            throw new NotFoundException("ReportingPeriodSupplierEntity is not found !!");

        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodSupplierEntity.ReportingPeriodId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierEntity.Id);

        //Get PeriodSupplierFacilities
        var periodFacilities = periodSupplier.PeriodFacilities.Where(x => x.ReportingPeriodSupplierId == periodSupplierId).ToList();

        var allFacilities = periodSupplierEntity.Supplier.FacilityEntities;
        var inRelaventFacilities = new List<FacilityEntity>();

        foreach (var facility in allFacilities)
        {
            if (facility.ReportingPeriodFacilityEntities.Count() == 0)
            {
                inRelaventFacilities.Add(facility);
            }
        }

        var periodFacilitiesDtos = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityDomainListToDtos(periodFacilities, inRelaventFacilities);
        var supplierFacilitiesDto = _reportingPeriodDomainDtoMapper.ConvertReportingPeriodSupplierFacilitiesDomainToDto(periodSupplier, periodFacilitiesDtos);

        return supplierFacilitiesDto;

    }

    /// <summary>
    /// Get ReportingPeriodSuppliersList
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    public IEnumerable<ReportingPeriodRelevantSupplierDto> GetRelevantSuppliers(int reportingPeriodId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodId);
        var periodSupplierDomainlist = reportingPeriod.PeriodSuppliers;

        var allSuppliers = _supplierDataActions.GetAllSuppliers();
        var supplierList = new List<SupplierEntity>();

        foreach (var inRelevantSuppliers in allSuppliers)
        {
            if (inRelevantSuppliers.ReportingPeriodSupplierEntities.Count() == 0 && inRelevantSuppliers.IsActive)
            {
                supplierList.Add(inRelevantSuppliers);
            }
        }

        var periodsuppliersDtos = _reportingPeriodDomainDtoMapper.ConvertReleventPeriodSupplierDomainToDto(periodSupplierDomainlist, supplierList, reportingPeriod);

        return periodsuppliersDtos;
    }

    /// <summary>
    /// Get PeriodFacilityElectricityGridMixList
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public MultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMixes(int periodFacilityId)
    {
        var periodSupplier = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);

        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("PeriodFacility is not found !!");

        var gridMixDto = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityElectricityGridMixDomainListToDto(periodFacility.periodFacilityElectricityGridMixes, periodFacility, periodSupplier);

        return gridMixDto;

    }

    /// <summary>
    /// Get PeriodFacilityGasSupplyBreakdownList
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    public MultiplePeriodFacilityGasSupplyBreakdownDto GetReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var periodSupplierEntity = _reportingPeriodDataActions.GetPeriodSupplierById(periodSupplierId);
        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodSupplierEntity.ReportingPeriodId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierEntity.Id);

        var periodFacilities = periodSupplier.PeriodFacilities;

        var gasSupplyBreakdownDto = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityGasSupplyBreakdownDoaminListToDto(periodFacilities, periodSupplier);

        return gasSupplyBreakdownDto;
    }

    /// <summary>
    /// Get ReportingPeriodFacility GridMixes And Documents
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int periodFacilityId)
    {
        var periodSupplier = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);

        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("PeriodFacility is not found !!");

        var facilityGridMixAndDocumentDto = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityElectricityGridMixAndDocumentDomainListToDto(periodFacility, periodSupplier);
        return facilityGridMixAndDocumentDto;
    }

    /// <summary>
    /// Download PeriodFacilityDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public FileStreamResult GetReportingPeriodFacilityDocumentDownload(int documentId)
    {
        var document = _reportingPeriodDataActions.GetReportingPeriodFacilityDocumentById(documentId);

        var result = _reportingPeriodDataActions.DownloadFile(document.Path);

        if (document is null)
            throw new NotFoundException("Result not found !!");

        return result;
    }

}

#endregion

