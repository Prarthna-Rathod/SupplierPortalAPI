using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
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
    private IReportingPeriodEntityDomainMapper _reportingPeriodEntityDomainMapper;
    private IReportingPeriodDataActions _reportingPeriodDataActions;
    private ISupplierDataActions _supplierDataActions;
    private IReferenceLookUpMapper _referenceLookUpMapper;
    private IReportingPeriodDomainDtoMapper _reportingPeriodDomainDtoMapper;

    public ReportingPeriodServices(IReportingPeriodFactory reportingPeriodFactory, IReportingPeriodEntityDomainMapper reportingPeriodEntityDomainMapper,
      IReportingPeriodDataActions reportingPeriodDataActions, ISupplierDataActions supplierDataActions,
      IReferenceLookUpMapper referenceLookUpMapper,
      IReportingPeriodDomainDtoMapper reportingPeriodDomainDtoMapper)
    {
        _reportingPeriodFactory = reportingPeriodFactory;
        _reportingPeriodEntityDomainMapper = reportingPeriodEntityDomainMapper;
        _reportingPeriodDataActions = reportingPeriodDataActions;
        _supplierDataActions = supplierDataActions;
        _referenceLookUpMapper = referenceLookUpMapper;
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
            var supplierVO = GetAndConvertSupplierValueObject();
            var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == periodSupplier.SupplierReportingPeriodStatusId);
            reportingPeriodDomain.LoadPeriodSupplier(periodSupplier.Id, periodSupplier.SupplierId, supplierVO, supplierReportingPeriodStatus, periodSupplier.InitialDataRequestDate, periodSupplier.ResendDataRequestDate, periodSupplier.IsActive);
        }

        foreach (var periodFacility in reportingPeriodEntity.ReportingPeriodFacilityEntities)
        {
            var facilityVO = GetAndConvertFacilityVO(periodFacility.FacilityId);
            var facilityReportingPeriodStatus = GetAndConvertFacilityReportingPeriodDataStatuses().FirstOrDefault(x => x.Id == periodFacility.FacilityReportingPeriodDataStatusId);
            var fercRegion = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacility.FercRegionId);

            var periodSupplier = reportingPeriodDomain.PeriodSuppliers.FirstOrDefault(x => x.Id == periodFacility.ReportingPeriodSupplierId);

            reportingPeriodDomain.LoadPeriodFacility(periodFacility.Id, facilityVO, facilityReportingPeriodStatus, reportingPeriodId, periodFacility.ReportingPeriodSupplierId, fercRegion, periodFacility.IsActive);

            //Load PeriodFacilityElectricityGridMix values
            foreach (var electricityGridMix in periodFacility.ReportingPeriodFacilityElectricityGridMixEntities)
            {
                var periodFacilityDomain = periodSupplier.PeriodFacilities.FirstOrDefault(x => x.Id == electricityGridMix.ReportingPeriodFacilityId);

                var electricityGridMixLookUp = GetAndConvertElectricityGridMixComponents().FirstOrDefault(x => x.Id == electricityGridMix.ElectricityGridMixComponentId);
                var unitOfMesaureLookUp = GetAndConvertUnitOfMeasures().FirstOrDefault(x => x.Id == electricityGridMix.UnitOfMeasureId);
                //var fercRegionLookUp = GetAndConvertFercRegions().FirstOrDefault(x => x.Id == periodFacility.FercRegionId);

                reportingPeriodDomain.LoadPeriodFacilityElectricityGridMix(electricityGridMix.Id,electricityGridMix.ReportingPeriodFacilityId, electricityGridMixLookUp, unitOfMesaureLookUp, electricityGridMix.Content, electricityGridMix.IsActive);
            }
        }

        return reportingPeriodDomain;

    }

    /// <summary>
    /// Get SupplierEntity and Convert it to SupplierVO
    /// </summary>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    private IEnumerable<SupplierVO> GetAndConvertSupplierValueObject()
    {
        var supplierEntity = _supplierDataActions.GetAllSuppliers();
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();



        if (supplierEntity == null)
            throw new BadRequestException("Supplier not found !!");

        var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierEntityToSupplierValueObjectList(supplierEntity, supplyChainStages, reportingTypes);
        return supplierVO;
    }

    /// <summary>
    /// Get FacilityEntity and convert it to FacilityVO
    /// </summary>
    /// <param name="facilityId"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    private IEnumerable<FacilityVO> GetAndConvertFacilityValueObject(IEnumerable<int> facilityIds)
    {

        var facilityEntity = _supplierDataActions.GetFacilityByIds(facilityIds);
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();

        if (facilityEntity == null)
            throw new BadRequestException("Facility not found !!");

        var facilityVO = _reportingPeriodEntityDomainMapper.ConvertFacilityEntityToFacilityValueObjectList(facilityEntity, supplyChainStages, reportingTypes);

        return facilityVO;
    }

    private FacilityVO GetAndConvertFacilityVO(int facilityId)
    {

        var facilityEntity = _supplierDataActions.GetFacilityById(facilityId);
        var reportingTypes = GetAndConvertReportingTypes();
        var supplyChainStages = GetAndConvertSupplyChainStages();

        if (facilityEntity == null)
            throw new BadRequestException("Facility not found !!");

        var facilityVO = _reportingPeriodEntityDomainMapper.ConvertFacilityEntityToFacilityVO(facilityEntity, supplyChainStages, reportingTypes);

        return facilityVO;
    }

    /// <summary>
    /// Configure ReportingPeriod
    /// </summary>
    /// <param name="reportingPeriodEntity"></param>
    /// <param name="reportingPeriodTypes"></param>
    /// <param name="reportingPeriodStatuses"></param>
    /// <returns></returns>
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
    /// <param name="multiplePeriodSuppliersDtos"></param>
    /// <returns></returns>
    public string SetMultiplePeriodSuppliers(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDtos)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(multiplePeriodSuppliersDtos.Select(x => x.ReportingPeriodId).FirstOrDefault());

        var reportingPeriodStatuses = GetAndConvertReportingPeriodStatuses();
        var supplierReportingPeriodStatues = GetAndConvertSupplierPeriodStatuses();

        var suppliers = _reportingPeriodDataActions.GetSuppliers(multiplePeriodSuppliersDtos.Select(x => x.SupplierId));
        var supplyChainStages = GetAndConvertSupplyChainStages();
        var reportingTypes = GetAndConvertReportingTypes();



        var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierEntityToSupplierValueObjectList(suppliers, supplyChainStages, reportingTypes);


        var reportingPeriodSupplierValueObject = _reportingPeriodDomainDtoMapper.ConvertMultiplePeriodSupplierDtosToValueObject(multiplePeriodSuppliersDtos, supplierVO, supplierReportingPeriodStatues);

        var periodSupplierDomain = reportingPeriod.AddRemovePeriodSupplier(reportingPeriodSupplierValueObject);


        var periodSupplierEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSuppliersDomainToEntity(periodSupplierDomain);
        _reportingPeriodDataActions.AddRemovePeriodSupplier(periodSupplierEntity, reportingPeriod.Id);

        return "Multiple PeriodSupplier Added or removed successfully...";

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

        var facilityReportingPeriodDataStatuses = GetAndConvertFacilityReportingPeriodDataStatuses();

        var facilityVOs = GetAndConvertFacilityValueObject(reportingPeriodFacilityDto.reportingPeriodSupplierRelaventFacilityDtos.Select(x => x.FacilityId));

        var fercRegions = GetAndConvertFercRegions();

        var periodSupplier = reportingPeriod.PeriodSuppliers.FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.ReportingPeriodSupplierId);
        if (periodSupplier is null)
            throw new ArgumentNullException("PeriodSupplier not found !!");

        var periodFacilityVO = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityDtoToValueObject(reportingPeriodFacilityDto, facilityVOs, facilityReportingPeriodDataStatuses, periodSupplier);

        var periodFacility = reportingPeriod.AddRemoveUpdatePeriodFacility(periodFacilityVO, fercRegions, facilityReportingPeriodDataStatuses, periodSupplier);

        var periodFacilityEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodFacilityDomainListToEntity(periodFacility, fercRegions);

        _reportingPeriodDataActions.AddRemovePeriodFacility(periodFacilityEntity, reportingPeriodFacilityDto.ReportingPeriodSupplierId);


        return "ReportingPeriodFacilities Added or Removed successfully";
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
    /// Add ReportingPeriodFacility ElectricityGridMix
    /// Per facility UnitOfMeasure should be 100%
    /// Per facility allowed maximum components is 9
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    /// 
    public string AddPeriodFacilityElectricityGridMix(AddMultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDtos)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(addMultiplePeriodFacilityElectricityGridMixDtos.ReportingPeriodId);

        var electricityGridMixComponent = GetAndConvertElectricityGridMixComponents();

        var unitOfMeasure = GetAndConvertUnitOfMeasures();

        var fercRegions = GetAndConvertFercRegions();
        var fercRegion = fercRegions.FirstOrDefault(x=>x.Id==addMultiplePeriodFacilityElectricityGridMixDtos.FercRegionId);

        //Dto to ValueObject
        var periodFacilityElectricGridMixVO = _reportingPeriodDomainDtoMapper.ConvertPeriodFacilityElectricityGridMixDtoToValueObject(addMultiplePeriodFacilityElectricityGridMixDtos,electricityGridMixComponent,unitOfMeasure);
        
        //GridMixDomain
        var periodFacilityacilityElectricityGridMixDomain = reportingPeriod.AddPeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDtos.ReportingPeriodSupplierId,addMultiplePeriodFacilityElectricityGridMixDtos.ReportingPeriodFacilityId,periodFacilityElectricGridMixVO, fercRegion);

        //ELectricityGridMix Domain to Entity
        var entity = _reportingPeriodEntityDomainMapper.ConvertPeriodFacilityElectricityGridMixDomainListToEntity(periodFacilityacilityElectricityGridMixDomain);

        //DataAction to Database
        _reportingPeriodDataActions.AddPeriodFacilityElectricityGridMix(entity,addMultiplePeriodFacilityElectricityGridMixDtos.ReportingPeriodFacilityId,addMultiplePeriodFacilityElectricityGridMixDtos.FercRegionId);


        return "ReportingPeriodFacility ElectricityGridMix added successfully !!";
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
    /// GetReportingPeriodSuppliers
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ReportingPeriodSupplierDto> GetReportingPeriodSuppliers(int reportingPeriodId)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodId);
        var periodSuppliers = reportingPeriod.PeriodSuppliers;

        var supplierReportingPeriodDtos = _reportingPeriodDomainDtoMapper.ConvertPeriodSupplierDomainListToDtos(periodSuppliers, reportingPeriod);

        return supplierReportingPeriodDtos;
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
    /// Get ReportingPeriodFacilityElectricityGridMix
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public AddMultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        var periodFacility = _reportingPeriodDataActions.GetReportingPeriodFacility(periodFacilityId);
        if (periodFacility is null) throw new Exception("PeriodFacility Not Found..!");

        var unitOfMeasure = GetAndConvertUnitOfMeasures();

        var electricityComponent = GetAndConvertElectricityGridMixComponents();

        var electricityGridMixDto = _reportingPeriodDomainDtoMapper.ConvertReportingPeriodFacilityEntityToDto(periodFacility,unitOfMeasure,electricityComponent);

        return electricityGridMixDto;
     
    }
}

#endregion



