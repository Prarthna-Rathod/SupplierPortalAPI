using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
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
    private ISupplierDataActions _supplierDataActions;
    private IReferenceLookUpMapper _referenceLookUpMapper;
    private IReadOnlyEntityToDtoMapper _readOnlyEntityToDtoMapper;
    private IReportingPeriodDomainDtoMapper _reportingPeriodDomainDtoMapper;
    private IFileUploadDataActions _fileUploadDataActions;

    public ReportingPeriodServices(IReportingPeriodFactory reportingPeriodFactory, ILoggerFactory loggerFactory, IReportingPeriodEntityDomainMapper reportingPeriodEntityDomainMapper,
            ISupplierEntityDomainMapper supplierEntityDomainMapper,
            ISupplierDomainDtoMapper supplierDomainDtoMapper,
           IReportingPeriodDataActions reportingPeriodDataActions, ISupplierDataActions supplierDataActions, IReferenceLookUpMapper referenceLookUpMapper,
           IReadOnlyEntityToDtoMapper readOnlyEntityToDtoMapper,
           IReportingPeriodDomainDtoMapper reportingPeriodDomainDtoMapper, IFileUploadDataActions fileUploadDataActions)
    {
        _reportingPeriodFactory = reportingPeriodFactory;
        _logger = loggerFactory.CreateLogger<SupplierServices>();
        _reportingPeriodEntityDomainMapper = reportingPeriodEntityDomainMapper;
        _supplierEntityDomainMapper = supplierEntityDomainMapper;
        _supplierDomainDtoMapper = supplierDomainDtoMapper;
        _reportingPeriodDataActions = reportingPeriodDataActions;
        _supplierDataActions = supplierDataActions;
        _referenceLookUpMapper = referenceLookUpMapper;
        _readOnlyEntityToDtoMapper = readOnlyEntityToDtoMapper;
        _reportingPeriodDomainDtoMapper = reportingPeriodDomainDtoMapper;
        _fileUploadDataActions = fileUploadDataActions;
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

    private IEnumerable<AssociatePipeline> GetAndConvertAssociatePipelines()
    {
        var associatePipelineEntity = _supplierDataActions.GetAllAssociatePipeline();
        return _referenceLookUpMapper.GetAssociatePipelinesLookUp(associatePipelineEntity);
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
        return _referenceLookUpMapper.GetSiteLookUp(sites);
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
        var documentRequiredStatuses = _reportingPeriodDataActions.GetDocumentRequiredStatus();
        return _referenceLookUpMapper.GetDocumentRequiredStatuses(documentRequiredStatuses);
    }

    private IEnumerable<FacilityRequiredDocumentTypeVO> GetFacilityRequiredDocumentTypeVOs()
    {
        var facilityRequiredDocumentTypes = _reportingPeriodDataActions.GetFacilityRequiredDocumentTypes();
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();
        var documentTypes = GetDocumentTypes();
        var documentRequiredStatuses = GetDocumentRequiredStatuses();

        var voList = _reportingPeriodEntityDomainMapper.ConvertFacilityRequiredDocumentTypeEntitiesToValueObjectList(facilityRequiredDocumentTypes, reportingTypes, supplyChainStages, documentTypes, documentRequiredStatuses);

        return voList;
    }

    /// <summary>
    /// Retrieve ReportingPeriod Entity and Convert it to DomainModel
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private ReportingPeriod RetrieveAndConvertReportingPeriod(int reportingPeriodId)
    {
        var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);

        if (reportingPeriodEntity is null)
            throw new NotFoundException("ReportingPeriodEntity not found !!");

        var reportingPeriodTypes = GetAndConvertReportingPeriodTypes();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatuses();

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
            GetAndLoadPeriodFacilityWithGridMixesAndGasSupplyData(periodFacility, reportingPeriodDomain);

            GetAndLoadPeriodFacilityDocuments(periodFacility, reportingPeriodDomain);
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

        if (periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities.Count() == 0)
            throw new NotFoundException("ElectricityGridMixes not available in this ReportingPeriodFacility !!");

        GetAndLoadPeriodFacilityWithGridMixesAndGasSupplyData(periodFacilityEntity, reportingPeriodDomain);
        GetAndLoadPeriodFacilityDocuments(periodFacilityEntity, reportingPeriodDomain);
        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        return periodSupplier;
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

    private void GetAndLoadPeriodFacilityWithGridMixesAndGasSupplyData(ReportingPeriodFacilityEntity periodFacilityEntity, ReportingPeriod reportingPeriodDomain)
    {
        var facilityVO = GetAndConvertFacilityValueObject(periodFacilityEntity.FacilityId);
        var facilityReportingPeriodStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Id == periodFacilityEntity.FacilityReportingPeriodDataStatusId);
        var fercRegion = GetAndConvertFercRegions().First(x => x.Id == periodFacilityEntity.FercRegionId);

        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.First(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        reportingPeriodDomain.LoadPeriodFacility(periodFacilityEntity.Id, facilityVO, facilityReportingPeriodStatus, periodSupplier.Id, fercRegion, periodFacilityEntity.IsActive);

        //Load PeriodFacilityElectricityGridMixes
        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityEntity.Id);
        var gridMixEntities = periodFacilityEntity.ReportingPeriodFacilityElectricityGridMixEntities;

        if (gridMixEntities.Count() != 0)
        {
            var gridMixComponents = GetAndConvertElectricityGridMixComponents();

            var gridMixValueObjectList = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixEntityListToValueObjectList(gridMixEntities, gridMixComponents);
            var unitOfMeasureId = gridMixEntities.First().UnitOfMeasureId;

            var unitOfMeasure = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == unitOfMeasureId);

            reportingPeriodDomain.LoadElectricityGridMixComponents(periodFacility.Id, periodFacility.ReportingPeriodSupplierId, unitOfMeasure, gridMixValueObjectList);
        }

        //Load PeriodFacilityGasSupplyBreakdowns
        var gasSupplyEntities = periodFacilityEntity.ReportingPeriodFacilityGasSupplyBreakDownEntities;

        if (gasSupplyEntities.Count() != 0)
        {
            var sites = GetAndConvertSites();
            var unitOfMeasures = GetAndConvertUnitOfMeasures();
            var gasSupplyValueObjectList = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityGasSupplyBreakdownEntitiesToValueObjectList(gasSupplyEntities, sites, unitOfMeasures);

            reportingPeriodDomain.LoadPeriodFacilityGasSupplyBreakdown(periodFacility.ReportingPeriodSupplierId, gasSupplyValueObjectList);
        }
    }

    private void GetAndLoadPeriodFacilityDocuments(ReportingPeriodFacilityEntity periodFacilityEntity, ReportingPeriod reportingPeriodDomain)
    {
        var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacilityEntity.ReportingPeriodSupplierId);

        // Load PeriodFacilityDocuments
        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityEntity.Id);
        var documentEntities = periodFacilityEntity.ReportingPeriodFacilityDocumentEntities;

        if (documentEntities.Count() != 0)
        {
            foreach (var documentEntity in documentEntities)
            {
                var documentStatus = GetDocumentStatuses().First(x => x.Id == documentEntity.DocumentStatusId);
                var documentType = GetDocumentTypes().First(x => x.Id == documentEntity.DocumentTypeId);

                reportingPeriodDomain.LoadPeriodFacilityDocuments(documentEntity.Id, periodSupplier.Id, documentEntity.ReportingPeriodFacilityId, documentEntity.Version, documentEntity.DisplayName, documentEntity.StoredName, documentEntity.Path, documentStatus, documentType, documentEntity.ValidationError);
            }
        }
    }

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

    private static readonly Dictionary<string, List<byte[]>> _excelFileSignature =
    new Dictionary<string, List<byte[]>>
    {
        { ".xlsx", new List<byte[]>
            {
                new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                new byte[] { 0x50, 0x4B, 0x07, 0x08 },
            }
        },
    };

    private static readonly Dictionary<string, List<byte[]>> _xmlFileSignature =
    new Dictionary<string, List<byte[]>>
    {
        { ".xml", new List<byte[]>
            {
                new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20 },
                new byte[] { 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20 },
                new byte[] { 0x00, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20 },
                new byte[] { 0x4C, 0x6F, 0xA7, 0x94, 0x93, 0x40 },
                new byte[] { 0x3C, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00 },
                new byte[] { 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00, 0x3F, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00, 0x20 },
            }
        },
    };

    private string CheckFileSignature(string path, string extension)
    {
        var isExcelFile = false;
        var isXmlFile = false;

        if (extension == ".xlsx")
        {
            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                var excelSignatures = _excelFileSignature[extension];
                var headerByte = reader.ReadBytes(excelSignatures.Max(m => m.Length));

                isExcelFile = excelSignatures.Any(signature =>
                    headerByte.Take(signature.Length).SequenceEqual(signature));

                reader.Close();
                reader.Dispose();
            }
        }

        if (extension == ".xml")
        {
            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                var xmlSignatures = _xmlFileSignature[extension];
                var headerByte = reader.ReadBytes(xmlSignatures.Max(m => m.Length));

                isXmlFile = xmlSignatures.Any(signature =>
                    headerByte.Take(signature.Length).SequenceEqual(signature));

                reader.Close();
                reader.Dispose();
            }
        }

        string? fileError = null;

        if (!isExcelFile && !isXmlFile)
            fileError = "File signature is not match with file extension !!";

        return fileError;
    }

    private string ValidateFile(string fileType, long fileSize, string path)
    {
        string? error = null;
        var fileTypes = new List<string>();
        fileTypes.Add(".xlsx");
        fileTypes.Add(".xml");

        var isCorrect = fileTypes.Contains(fileType);
        if (!isCorrect)
            error += "FileType is not match.";

        //Check file signature
        var fileError = CheckFileSignature(path, fileType);
        if (fileError != null)
            error += fileError;

        //Check File size (should be 20MB)
        long sizeInBytes = fileSize;
        long maxSizeInBytes = 20971520;
        if (sizeInBytes > maxSizeInBytes)
            error += "Filesize should be in 20Mb";

        return error;
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
    public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodFacilityDto.ReportingPeriodId);

        var facilityReportingPeriodDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.FacilityReportingPeriodDataStatusId);
        var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Name == FercRegionValues.None);

        if (facilityReportingPeriodDataStatus is null)
            throw new NotFoundException("FacilityReportingPeriodDataStatus not found !!");

        if (fercRegion is null)
            throw new NotFoundException("FercRegion not found !!");

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
    /// Remove PeriodSupplier
    /// </summary>
    /// <param name="PeriodSupplierId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool RemovePeriodSupplier(int periodSupplierId)
    {
        return _reportingPeriodDataActions.RemovePeriodSupplier(periodSupplierId);
    }

    /// <summary>
    /// AddRemove ReportingPeriodFacility ElectricityGridMix Components.
    /// Per facility ContentValues should be 100%
    /// Per facility gridMix component can't be repeated
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    public string AddRemovePeriodFacilityElectricityGridMix(MultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto)
    {
        var electricityGridMixComponents = GetAndConvertElectricityGridMixComponents();
        var unitOfMeasure = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == periodFacilityElectricityGridMixDto.UnitOfMeasureId);
        var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacilityElectricityGridMixDto.FercRegionId);

        if (unitOfMeasure is null)
            throw new NotFoundException("UnitOfMeasure not found !!");

        if (fercRegion is null)
            throw new NotFoundException("FercRegion not found !!");

        var reportingPeriod = RetrieveAndConvertReportingPeriod(periodFacilityElectricityGridMixDto.ReportingPeriodId);

        var valueObjectList = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityElectricityGridMixDtosToValueObjectList(periodFacilityElectricityGridMixDto.ReportingPeriodFacilityElectricityGridMixDtos, electricityGridMixComponents);

        var facilityElectricityGridMixDomainList = reportingPeriod.AddRemoveElectricityGridMixComponents(periodFacilityElectricityGridMixDto.ReportingPeriodFacilityId, periodFacilityElectricityGridMixDto.SupplierId, unitOfMeasure, fercRegion, valueObjectList);

        var entities = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixDomainListToEntities(facilityElectricityGridMixDomainList);

        _reportingPeriodDataActions.AddPeriodFacilityElectricityGridMix(entities, periodFacilityElectricityGridMixDto.ReportingPeriodFacilityId, fercRegion.Id);

        return "ReportingPeriodFacility ElectricityGridMix Components added successfully !!";
    }

    /// <summary>
    /// AddRemove ReportingPeriodSupplierFacility GasSupplyBreakdown sites.
    /// In this, Per PeriodSupplier per site Content should be 100
    /// Per PeriodSupplier per facility site could not be repeated
    /// </summary>
    /// <param name="multiplePeriodSupplierGasSupplyBreakdownDto"></param>
    /// <returns></returns>
    public string AddRemovePeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakdownDto multiplePeriodSupplierGasSupplyBreakdownDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(multiplePeriodSupplierGasSupplyBreakdownDto.ReporingPeriodId);

        var sites = GetAndConvertSites();
        var unitOfMeasures = GetAndConvertUnitOfMeasures();

        var valueObjectList = _reportingPeriodDomainDtoMapper.ConvertPeriodSupplierGasSupplyBreakdownDtosToValueObjectList(multiplePeriodSupplierGasSupplyBreakdownDto.PeriodSupplierGasSupplyBreakdowns, sites, unitOfMeasures);

        var domainList = reportingPeriod.AddPeriodFacilityGasSupplyBreakdown(multiplePeriodSupplierGasSupplyBreakdownDto.ReportingPeriodSupplierId, valueObjectList);

        var entities = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityGasSupplyBreakdownDomainListToEntities(domainList);

        _reportingPeriodDataActions.AddRemovePeriodFacilityGasSupplyBreakdown(entities, multiplePeriodSupplierGasSupplyBreakdownDto.ReportingPeriodSupplierId);

        return "ReportingPeriodSupplier GasSupplyBreakdown added successfully !!";
    }

    /// <summary>
    /// Update all PeriodFacilities dataStatus from "Complete" to "Submitted" for given PeriodSupplierId
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    public string UpdatePeriodFacilityStatusSubmitted(int reportingPeriodId, int supplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodId);

        var facilityPeriodDataStatusSubmitted = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.Submitted);

        var periodFacilities = reportingPeriod.UpdateAllPeriodFacilityDataStatus(supplierId, facilityPeriodDataStatusSubmitted);

        foreach (var periodFacility in periodFacilities)
        {
            var periodFacilityEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDomainToEntity(periodFacility);

            _reportingPeriodDataActions.UpdateReportingPeriodFacilityDataStatus(periodFacilityEntity.Id, facilityPeriodDataStatusSubmitted.Id);
        }

        return "FacilityReportingPeriodDataStatus changed successfully...";
    }

    #endregion

    #region ReportingPeriodFacility Document

    /// <summary>
    /// ********Case 1*********
    /// AddUpdate ReportingPeriodFacilityDocument
    /// First add record in DB with Document status "InProcessing"
    /// Upload the attached file and updated the Document status.
    /// If no errors occured in file upload then Document status is "Validated" otherwise status is "HasErrors" with stored the errors also.
    /// 
    /// ********Case 2*********
    /// If PeriodFacility dataStatus is "Submitted" and user want to upload file
    /// First persist record with document status "InProcessing".
    /// Update PeriodFacility dataStatus to "InProgress" for only fileUpload.
    /// Upload file and update the existing record with appropriate documentStatus.
    /// 
    /// *********Check facilityDocuments and update facilityDataStatus*********
    /// Check that all required documents are uploaded for periodFacility or not.
    /// If all documents are uploaded then Update PeriodFacilityDataStatus "InProgress" to "Complete".
    /// Use 'UpdatePeriodFacilityStatusSubmitted API' to change facilityDataStatus "Complete" to "Submitted"
    /// </summary>
    /// <param name="reportingPeriodDocumentDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string AddUpdateReportingPeriodDocument(ReportingPeriodDocumentDto reportingPeriodDocumentDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodDocumentDto.ReportingPeriodId);

        var documentStatuses = GetDocumentStatuses();
        var documentType = GetDocumentTypes().First(x => x.Id == reportingPeriodDocumentDto.DocumentTypeId);
        var facilityRequiredDocumentTypes = GetFacilityRequiredDocumentTypeVOs();

        //First Add new Record with document status "Processing"
        var periodDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(reportingPeriodDocumentDto.SupplierId, reportingPeriodDocumentDto.PeriodFacilityId, reportingPeriodDocumentDto.DocumentFile.FileName, null, documentStatuses, documentType, null, facilityRequiredDocumentTypes);

        var entity = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityDocumentDomainToEntity(periodDocument);

        _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(entity);

        //If FacilityReportingPeriodStatus is submitted and user comes to upload document then change FacilityReportingPeriodStatus to InProgess in Domain and Database
        var inProgressDataStatus = GetAndConvertFacilityReportingPeriodDataStatuses().First(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);
        
        reportingPeriod.UpdatePeriodFacilityDataStatusSubmittedToInProgress(reportingPeriodDocumentDto.SupplierId, entity.ReportingPeriodFacilityId, inProgressDataStatus);
        
        _reportingPeriodDataActions.CheckAndUpdateReportingPeriodFacilityStatus(periodDocument.ReportingPeriodFacilityId, inProgressDataStatus.Id);

        //Upload file
        var path = "";
        string? fileUploadError = null;
        string? errors = null;
        try
        {
            path = _fileUploadDataActions.UploadReportingPeriodDocument(reportingPeriodDocumentDto.DocumentFile);
        }
        catch (Exception ex)
        {
            fileUploadError = $"Some error occured during fileUpload !!";
        }

        if (fileUploadError == null)
        {
            //Check FileSize and FileType before upload
            FileInfo file = new FileInfo(path);
            var fileType = file.Extension;
            long fileSize = file.Length;

            errors = ValidateFile(fileType, fileSize, path);
        }

        if (fileUploadError != null)
            errors += fileUploadError;

        //Update record with documentStatus "NotValidated, HasErrors or Validated"
        var uploadedPeriodDocument = reportingPeriod.AddUpdatePeriodFacilityDocuments(reportingPeriodDocumentDto.SupplierId, reportingPeriodDocumentDto.PeriodFacilityId, reportingPeriodDocumentDto.DocumentFile.FileName, path, documentStatuses, documentType, errors, facilityRequiredDocumentTypes);

        var documentEntity = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityDocumentDomainToEntity(uploadedPeriodDocument);

        _reportingPeriodDataActions.AddUpdateReportingPeriodFacilityDocument(documentEntity);

        if (errors != null)
            throw new Exception(errors);

        //If all required documents are uploaded then change FacilityReportingPeriodDataStatus "Complete"
        UpdatePeriodFacilityStatusToComplete(documentEntity.ReportingPeriodFacilityId);

        return "ReportingPeriodFacilityDocument uploaded successfully....";
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
            throw new BadRequestException("ReportingPeriodSupplierEntity is not found !!");

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
        var supplierFacilitiesDto = _reportingPeriodDomainDtoMapper.ConvertReportingPeriodSupplierFacilityDomainToDto(periodSupplier, periodFacilitiesDtos);

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
    /// Get ReportingPeriodFacility ElectricityGridMixes data
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    public MultiplePeriodFacilityElectricityGridMixDto GetFacilityElectricityGridMixComponents(int periodFacilityId)
    {
        var periodSupplier = RetrieveAndConvertReportingPeriodSupplierFacility(periodFacilityId);
        var periodFacility = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == periodFacilityId);
        var dto = _reportingPeriodDomainDtoMapper.GetAndConvertPeriodFacilityElectricityGridMixDomainListToDto(periodFacility, periodSupplier.Supplier.Id);

        return dto;
    }

    /// <summary>
    /// Get all site data for ReportingPeriodSupplierFacilities
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    public MultiplePeriodFacilityGasSupplyBreakdownDto GetFacilityGasSupplyBreakdowns(int reportingPeriodId, int supplierId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodId);
        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Supplier.Id == supplierId);
        if (periodSupplier == null)
            throw new NotFoundException("ReportingPeriodSupplier not found !!");

        var periodFacilities = periodSupplier.PeriodFacilities;

        var dto = _reportingPeriodDomainDtoMapper.GetAndConvertPeriodFacilityGasSupplyBreakdownDomainListToDto(periodFacilities, periodSupplier);

        return dto;
    }

}

#endregion

