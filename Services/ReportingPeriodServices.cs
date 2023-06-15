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
    private ISendEmailServices _sendEmailServices;

    public ReportingPeriodServices(IReportingPeriodFactory reportingPeriodFactory, ILoggerFactory loggerFactory, IReportingPeriodEntityDomainMapper reportingPeriodEntityDomainMapper,
            ISupplierEntityDomainMapper supplierEntityDomainMapper,
            ISupplierDomainDtoMapper supplierDomainDtoMapper,
           IReportingPeriodDataActions reportingPeriodDataActions, IUploadDocuments uploadDocuments, ISupplierDataActions supplierDataActions, IReferenceLookUpMapper referenceLookUpMapper,
           IReadOnlyEntityToDtoMapper readOnlyEntityToDtoMapper,
           IReportingPeriodDomainDtoMapper reportingPeriodDomainDtoMapper, ISendEmailServices sendEmailServices)
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
        _sendEmailServices = sendEmailServices;

    }

    #region IEnumerable GetAndConvert methods

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

    #endregion

    #region UpdateFacilityDataStatus methods

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

    private void UpdatePeriodFacilityDataStatus(int periodFacilityId)
    {
        var periodFacility = _reportingPeriodDataActions.GetPeriodFacilityById(periodFacilityId);

        var periodFacilityDocuments = periodFacility.ReportingPeriodFacilityDocumentEntities;

        var facilityRequiredDocumentTypeVos = GetAndConvertFacilityRequiredDocumentTypeVO();
        var requiredDocument = facilityRequiredDocumentTypeVos.Where(x => x.ReportingType.Id == periodFacility.ReportingTypeId && x.SupplyChainStage.Id == periodFacility.SupplyChainStageId && x.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.Required).ToList();

        if (requiredDocument.Count() != periodFacilityDocuments.Count())
            UpdatePeriodFacilityDataStatusSubmittedToInprogress(periodFacilityId);

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

    #endregion

    #region FileSignature

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

    #endregion

    #region RetrieveAndConvert methods

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

            GetAndLoadPeriodSupplierDocuments(periodSupplier, reportingPeriodDomain);

        }
        foreach (var periodFacility in reportingPeriodEntity.ReportingPeriodFacilityEntities)
        {
            GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(periodFacility, reportingPeriodDomain);

            GetAndLoadPeriodFacilityDocuments(periodFacility, reportingPeriodDomain);

        }

        return reportingPeriodDomain;

    }

    private ReportingPeriod RetrieveAndConvertReportingPeriodSupplierFacility(int periodFacilityId)
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

        GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(periodFacilityEntity, reportingPeriodDomain);
        GetAndLoadPeriodFacilityDocuments(periodFacilityEntity, reportingPeriodDomain);

        return reportingPeriodDomain;
    }

    private ReportingPeriod RetrieveAndConvertReportingPeriodSupplier(int periodSupplierId)
    {
        var periodSupplierEntity = _reportingPeriodDataActions.GetPeriodSupplierById(periodSupplierId);
        var reportingPeriodEntity = periodSupplierEntity.ReportingPeriod;

        if (periodSupplierEntity is null)
            throw new NotFoundException("ReportingPeriodFacilityEntity not found !!");

        var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses();

        var reportingPeriodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

        //Load PeriodSupplier
        var supplierVO = GetAndConvertSupplierValueObject(periodSupplierEntity.SupplierId);
        var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == periodSupplierEntity.SupplierReportingPeriodStatusId);
        reportingPeriodDomain.LoadPeriodSupplier(periodSupplierEntity.Id, supplierVO, supplierReportingPeriodStatus, periodSupplierEntity.InitialDataRequestDate, periodSupplierEntity.ResendDataRequestDate);

        var periodFacilityEntities = periodSupplierEntity.ReportingPeriodFacilityEntities;
        foreach (var periodFacilityEntity in periodFacilityEntities)
        {
            GetAndLoadPeriodFacilityWithGridMixesAndGasSupply(periodFacilityEntity, reportingPeriodDomain);
        }

        //GetAndLoadPeriodFacilityDocuments(periodFacilityEntity, reportingPeriodDomain);
        GetAndLoadPeriodSupplierDocuments(periodSupplierEntity, reportingPeriodDomain);

        return reportingPeriodDomain;
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

    private void GetAndLoadPeriodSupplierDocuments(ReportingPeriodSupplierEntity periodSupplierEntity, ReportingPeriod reportingPeriod)
    {
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierEntity.Id);

        var periodSupplierDocumentEntities = periodSupplierEntity.ReportingPeriodSupplierDocumentEntities;

        if (periodSupplierDocumentEntities.Count() != 0)
        {
            foreach (var periodSupplierDocumentEntity in periodSupplierDocumentEntities)
            {
                var documentStatus = GetDocumentStatuses().First(x => x.Id == periodSupplierDocumentEntity.DocumentStatusId);
                var documentType = GetDocumentTypes().First(x => x.Id == periodSupplierDocumentEntity.DocumentTypeId);

                reportingPeriod.LoadPeriodSupplierDocument(periodSupplierDocumentEntity.Id, periodSupplier.Supplier.Id, periodSupplierDocumentEntity.Version, periodSupplierDocumentEntity.DisplayName, periodSupplierDocumentEntity.StoredName, periodSupplierDocumentEntity.Path, periodSupplierDocumentEntity.ValidationError, documentStatus, documentType);
            }
        }
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

    private ReportingPeriod ConfigureReportingPeriod(ReportingPeriodEntity reportingPeriodEntity, IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses)
    {
        var reportingPeriodType = reportingPeriodTypes.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodTypeId).ToList();
        var reportingPeriodStatus = reportingPeriodStatuses.Where(x => x.Id == reportingPeriodEntity.ReportingPeriodStatusId).ToList();

        //Convert entity to domain
        var reportingPeriodDomain = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodType, reportingPeriodStatus);
        return reportingPeriodDomain;

    }

    #endregion

    #region ReportingPeriod

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

    #endregion

    #region PeriodSupplier

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
    /// LockUnlockPeriodSupplierStatus
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);
        var periodSupplierStatus = GetAndConvertSupplierPeriodStatuses();

        var periodSupplier = reportingPeriod.UpdateLockUnlockPeriodSupplierStatus(periodSupplierId, periodSupplierStatus);

        var entity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplier);

        _reportingPeriodDataActions.UpdateReportingPeriodSupplierLockUnlockStatus(entity);

        return "PeriodSupplierStatus is updated successfully...";
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

    #endregion

    #region PeriodFacility

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

    #endregion

    #region PeriodFacilityElectricityGridMix

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
    /// Get PeriodFacilityElectricityGridMixList
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public MultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMixes(int supplierId, int periodFacilityId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);

        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Supplier.Id == supplierId);

        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        var gridMixDto = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityElectricityGridMixDomainListToDto(periodFacility.periodFacilityElectricityGridMixes, periodFacility, periodSupplier);

        return gridMixDto;

    }

    /// <summary>
    /// Remove ReportingPeriodFacility ElectricityGridMix
    /// </summary>
    /// <param name="supplierId"></param>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    public string RemoveReportingPeriodFacilityElectricityGridMix(int supplierId, int periodFacilityId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);
        var isRemoved = reportingPeriod.RemovePeriodFacilityElectricityGridMix(supplierId, periodFacilityId);

        if (isRemoved)
            _reportingPeriodDataActions.RemovePeriodFacilityElectricityGridMix(periodFacilityId);

        return "PeriodFacility electricityGridMix is removed successfully...";
    }

    #endregion

    #region PeriodFacilityGasSupplyBreakDown

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
    /// Get PeriodFacilityGasSupplyBreakdownList
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    public MultiplePeriodFacilityGasSupplyBreakdownDto GetReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierId);

        var periodFacilities = periodSupplier.PeriodFacilities;

        var gasSupplyBreakdownDto = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityGasSupplyBreakdownDoaminListToDto(periodFacilities, periodSupplier);

        return gasSupplyBreakdownDto;
    }

    /// <summary>
    /// Remove PeriodFacilityGasSupplyBreakDown
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    public string RemoveReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);
        var isRemoved = reportingPeriod.RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId);

        if (isRemoved)
            _reportingPeriodDataActions.RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId);

        return "GasSupplyBreakdown is removed successfully...";
    }

    #endregion

    #region PeriodFacilityDocument

    /// <summary>
    /// AddUpdate ReportingPeriodFacilityDocument
    /// </summary>
    /// <param name="reportingPeriodFacilityDocumentDto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public string AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodFacilityDocumentDto.ReportingPeriodId);

        var documentStatuses = GetDocumentStatuses();
        if (documentStatuses is null)
            throw new NotFoundException("DocumentStatus is not found !!");

        var documentType = GetDocumentTypes().FirstOrDefault(x => x.Id == reportingPeriodFacilityDocumentDto.DocumentTypeId);
        if (documentType is null)
            throw new NotFoundException("DocumentType is not found !!");

        var facilityRequiredDocumentTypeVos = GetAndConvertFacilityRequiredDocumentTypeVO();

        var periodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(reportingPeriodFacilityDocumentDto.SupplierId, reportingPeriodFacilityDocumentDto.PeriodFacilityId, reportingPeriodFacilityDocumentDto.DocumentFile.FileName, null, null, documentStatuses, documentType, facilityRequiredDocumentTypeVos);

        var periodFacilityDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDocumentDomainToEntity(periodFacilityDocument);

        if (periodFacilityDocumentEntity.Id == 0)
            _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(periodFacilityDocumentEntity);

        //change status Submitted To InProgress
        var progressDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

        reportingPeriod.UpdatePeriodFacilityDataStatusSubmittedToInProgress(reportingPeriodFacilityDocumentDto.SupplierId, periodFacilityDocumentEntity.ReportingPeriodFacilityId, progressDataStatus);

        _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityDocument.ReportingPeriodFacilityId, progressDataStatus.Id);

        var documentPath = _uploadDocuments.UploadFile(reportingPeriodFacilityDocumentDto.DocumentFile, periodFacilityDocumentEntity.StoredName);

        ValidateFileSignature(documentPath, reportingPeriodFacilityDocumentDto.DocumentFile.FileName);

        var error = _uploadDocuments.validationError(reportingPeriodFacilityDocumentDto.DocumentFile);

        var updatedPeriodFacilityDocument = reportingPeriod.AddPeriodFacilityDocument(reportingPeriodFacilityDocumentDto.SupplierId, reportingPeriodFacilityDocumentDto.PeriodFacilityId, reportingPeriodFacilityDocumentDto.DocumentFile.FileName, documentPath, error, documentStatuses, documentType, facilityRequiredDocumentTypeVos);

        var updatedPeriodFacilityDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDocumentDomainToEntity(updatedPeriodFacilityDocument);

        _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(updatedPeriodFacilityDocumentEntity);

        if (error != null)
            throw new Exception(error);

        //Check facilityRequiredDocumentType is required than update facilityDataStatus is InProgress To Complete
        var requiredDocument = facilityRequiredDocumentTypeVos.Any(x => x.DocumentType.Id == updatedPeriodFacilityDocumentEntity.DocumentTypeId && x.DocumentRequiredStatus.Name == DocumentRequiredStatusValues.Required);
        if (requiredDocument)
            UpdatePeriodFacilityDataStatusInProgressToComplete(reportingPeriodFacilityDocumentDto.PeriodFacilityId);

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

        //update facilityDataStatus
        var facilityDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);
        reportingPeriod.UpdatePeriodFacilityDataStatusCompleteToSubmitted(periodSupplierId, facilityDataStatus);

        var entities = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilitiesDomainToEntity(periodFacilities);

        _reportingPeriodDataActions.UpdatePeriodFacilities(entities);

        return "Update PeriodFacilityDataStatus is successfully...";
    }

    /// <summary>
    /// Remove PeriodFacilityDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    public string RemovePeriodFacilityDocument(int supplierId, int periodFacilityId, int documentId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);

        var isRemoved = reportingPeriod.RemovePeriodFacilityDocument(supplierId, periodFacilityId, documentId);

        if (isRemoved)
            _reportingPeriodDataActions.RemovePeriodFacilityDocument(documentId);

        //Check RequiredDocuments are uploaded or not for PeriodFacility 
        //If not uploaded then change FacilityStatus Submitted to InProgress
        UpdatePeriodFacilityDataStatus(periodFacilityId);

        return "PeriodFacilityDocument is removed successfully...";
    }

    /// <summary>
    /// Get ReportingPeriodFacility GridMixes And Documents
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId, int periodFacilityId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);

        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Supplier.Id == supplierId);

        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);

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

    #endregion

    #region PeriodSupplierDocument

    /// <summary>
    /// AddUpdate ReportingPeriodSupplierDocument
    /// </summary>
    /// <param name="reportingPeriodSupplierDocumentDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodSupplierDocumentDto.ReportingPeriodId);

        var documentStatuses = GetDocumentStatuses();
        var documentType = GetDocumentTypes().FirstOrDefault(x => x.Id == reportingPeriodSupplierDocumentDto.DocumentTypeId);

        var periodSupplierDocument = reportingPeriod.AddUpdatePeriodSupplierDocument(reportingPeriodSupplierDocumentDto.SupplierId, reportingPeriodSupplierDocumentDto.DocumentFile.FileName, null, null, documentStatuses, documentType);

        var periodSupplierDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDocumentDomainToEntity(periodSupplierDocument);

        if (periodSupplierDocumentEntity.Id == 0)
            _reportingPeriodDataActions.AddUpdateReportingPeriodSupplierDocument(periodSupplierDocumentEntity);

        var documentPath = _uploadDocuments.UploadFile(reportingPeriodSupplierDocumentDto.DocumentFile, periodSupplierDocumentEntity.StoredName);

        ValidateFileSignature(documentPath, reportingPeriodSupplierDocumentDto.DocumentFile.FileName);

        var error = _uploadDocuments.validationError(reportingPeriodSupplierDocumentDto.DocumentFile);

        var updatedPeriodSupplierDocumentDomain = reportingPeriod.AddUpdatePeriodSupplierDocument(reportingPeriodSupplierDocumentDto.SupplierId, reportingPeriodSupplierDocumentDto.DocumentFile.FileName, documentPath, error, documentStatuses, documentType);

        var updatedPeriodSupplierDocumentEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDocumentDomainToEntity(updatedPeriodSupplierDocumentDomain);

        _reportingPeriodDataActions.AddUpdateReportingPeriodSupplierDocument(updatedPeriodSupplierDocumentEntity);

        if (error != null)
            throw new Exception(error);

        return "PeriodSupplierDocument is added successfully...";
    }

    /// <summary>
    /// Get ReportingPeriodSupplier GasSupply and Documents
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    public ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocuments(int periodSupplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == periodSupplierId);

        var periodFacilities = periodSupplier.PeriodFacilities;

        var periodSupplierDocumentDto = _reportingPeriodDomainDtoMapper.ConvertPeriodSupplierGasSupplyAndDocumentDomainToDto(periodFacilities, periodSupplier);

        return periodSupplierDocumentDto;
    }

    /// <summary>
    /// Download PeriodSupplierDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public FileStreamResult GetReportingPeriodSupplierDocumentDownload(int documentId)
    {
        var document = _reportingPeriodDataActions.GetReportingPeriodSupplierDocumentById(documentId);

        var result = _reportingPeriodDataActions.DownloadFile(document.Path);

        if (document is null)
            throw new NotFoundException("Result not found !!");

        return result;
    }

    /// <summary>
    /// Remove ReportingPeriodSupplierDocument
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="documentId"></param>
    /// <returns></returns>
    public string RemoveReportingPeriodSupplierDocument(int periodSupplierId, int documentId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);

        var isRemoved = reportingPeriod.RemovePeriodSupplierDocument(periodSupplierId, documentId);

        if (isRemoved)
            _reportingPeriodDataActions.RemovePeriodSupplierDocument(documentId);

        return "PeriodSupplierDocument is removed successfully...";
    }

    #endregion

    #region SendEmail

    private (string, string, string?) GetEmailTemplateCodeSubjectAndBody(string nameCode, string supplierName, ReportingPeriod reportingPeriod)
    {
        var emailTemplates = _reportingPeriodDataActions.GetEmailTemplateBynameCode();
        var emailTemplate = emailTemplates.First(x => x.NameCode == nameCode);
        if (emailTemplate == null)
            throw new NotFoundException("EmailTemplate is not found !!");

        var emailBody = emailTemplate.Body;

        var endDate = (reportingPeriod.EndDate != null ? reportingPeriod.EndDate.Value.Date : DateTime.UtcNow.Date.AddDays(30));

        emailBody = emailTemplate.Body.Replace("<<supplierName>>", supplierName).Replace("<<closeDate>>", endDate.ToString()).Replace("<<ReportingPeriodName>>", reportingPeriod.DisplayName);

        return (emailTemplate.Subject, emailBody, emailTemplate.DocumentPath);
    }

    /// <summary>
    /// SendMail for InitialDataRequest Or ResendDataRequest notification for users
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="CCEmail"></param>
    /// <param name="BCCEmail"></param>
    /// <returns></returns>
    public string SendEmailInitialAndResendDataRequest(int periodSupplierId, string? CCEmail, string? BCCEmail)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriodSupplier(periodSupplierId);

        var emails = reportingPeriod.CheckInitialAndResendDataRequest(periodSupplierId);

        var periodSupplier = reportingPeriod.PeriodSuppliers.First(x => x.Id == periodSupplierId);

        (string subject, string body, string? filePath) emailSubjectAndBody = (periodSupplier.InitialDataRequestDate == null ? GetEmailTemplateCodeSubjectAndBody(EmailTemplateValues.InitialDataRequest, periodSupplier.Supplier.Name, reportingPeriod) : GetEmailTemplateCodeSubjectAndBody(EmailTemplateValues.ResendDataRequest, periodSupplier.Supplier.Name, reportingPeriod));

        var isInitialDataRequest = _sendEmailServices.SendEmailRequest(emails, emailSubjectAndBody.subject, emailSubjectAndBody.body, CCEmail, BCCEmail, emailSubjectAndBody.filePath);

        if (isInitialDataRequest)
            _reportingPeriodDataActions.SendEmailInitialAndResendDataRequest(periodSupplierId);

        return "InitialDataRequest and ResendDataRequest send email successfully...";

    }

    #endregion

}
