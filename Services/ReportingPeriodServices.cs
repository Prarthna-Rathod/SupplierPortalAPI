using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
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
    private IReadOnlyEntityToDtoMapper _readOnlyEntityToDtoMapper;
    private IReportingPeriodDataActions _reportingPeriodDataActions;
    private ISupplierDataActions _supplierDataActions;
    private IReferenceLookUpMapper _referenceLookUpMapper;
    private IReportingPeriod _reportingPeriod;
    private IReportingPeriodDomainDtoMapper _reportingPeriodDomainDtoMapper;

    public ReportingPeriodServices(IReportingPeriodFactory reportingPeriodFactory, ILoggerFactory loggerFactory,
        IReportingPeriodDomainDtoMapper reportingPeriodDomainMapper, IReportingPeriodEntityDomainMapper reportingPeriodEntityDomainMapper, IReadOnlyEntityToDtoMapper readOnlyEntityToDtoMapper, IReportingPeriodDataActions reportingPeriodDataActions, ISupplierDataActions supplierDataActions, IReferenceLookUpMapper referenceLookUpMapper, IReportingPeriod reportingPeriod, IReportingPeriodDomainDtoMapper reportingPeriodDomainDtoMapper)
    {
        _reportingPeriodFactory = reportingPeriodFactory;
        _logger = loggerFactory.CreateLogger<SupplierServices>();
        _reportingPeriodEntityDomainMapper = reportingPeriodEntityDomainMapper;
        _readOnlyEntityToDtoMapper = readOnlyEntityToDtoMapper;
        _reportingPeriodDataActions = reportingPeriodDataActions;
        _supplierDataActions = supplierDataActions;
        _referenceLookUpMapper = referenceLookUpMapper;
        _reportingPeriod = reportingPeriod;
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
            reportingPeriodDomain.LoadPeriodSupplier(periodSupplier.Id, supplierVO, supplierReportingPeriodStatus);
        }

        return reportingPeriodDomain;

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

        var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierToSupplierValueObject(supplierEntity, supplyChainStages, reportingTypes);
        return supplierVO;
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
    /// Add PeriodSupplier (Where supplier should be active & ReportingPeriodStatus should be InActive)
    /// </summary>
    /// <param name="reportingPeriodSupplierDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /*public string SetPeriodSupplier(ReportingPeriodSupplierDto reportingPeriodSupplierDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(reportingPeriodSupplierDto.ReportingPeriodId);

        var supplierPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == reportingPeriodSupplierDto.SupplierReportingPeriodStatusId);

        var periodSupplier = reportingPeriod.AddPeriodSupplier(GetAndConvertSupplierValueObject(reportingPeriodSupplierDto.SupplierId), supplierPeriodStatus ?? new SupplierReportingPeriodStatus());
        var periodSupplierEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplier);
        _reportingPeriodDataActions.AddPeriodSupplier(periodSupplierEntity);

        return "ReportingPeriodSupplier added successfully....";
    }*/

    public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
    {
        var reportingPeriod = RetrieveAndConvertReportingPeriod(multiplePeriodSuppliersDto.ReportingPeriodId);

        var supplierPeriodStatus = GetAndConvertSupplierPeriodStatuses().FirstOrDefault(x => x.Id == multiplePeriodSuppliersDto.SupplierReportingPeriodStatusId);

        foreach (var supplierId in multiplePeriodSuppliersDto.SupplierIds)
        {
            var supplierVO = GetAndConvertSupplierValueObject(supplierId);
            var periodSupplier = reportingPeriod.AddPeriodSupplier(multiplePeriodSuppliersDto.Id,supplierVO, supplierPeriodStatus ?? new SupplierReportingPeriodStatus());

            var periodSupplierEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplier);
            _reportingPeriodDataActions.AddPeriodSupplier(periodSupplierEntity);
        }

        return "Multiple PeriodSupplier added successfully...";
    }

    /// <summary>
    /// Set PeriodFacility
    /// </summary>
    /// <param name="reportingPeriodFacilityDto"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /* public async Task<string> SetPeriodFacility(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
     {
         var reportingPeriodData = new SupplierReportingPeriodDTO();

         var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodData.ReportingPeriodId);

         if (reportingPeriodEntity == null)
         {
             throw new ArgumentNullException("Unable to retrive ReportingPeriod");
         }

         var facilityEntity = _supplierDataActions.GetFacilityById(reportingPeriodFacilityDto.FacilityId);
         if(facilityEntity == null)
         {
             throw new Exception("Unable to retrive Facility");
         }

         var facilityReportingPeriodDataStatus = GetAndConvertFacilityReportingPeriodDataStatus();
         var reportingTypes = GetAndConvertReportingType();

         var periodSupplierEntity = _reportingPeriodDataActions.GetPeriodSupplierById(reportingPeriodFacilityDto.ReportingPeriodSupplierId);
         if(periodSupplierEntity == null)
         {
             throw new Exception("Unable to retrive PeriodSupplier");
         }

         var reportingPeriodTypes = GetAndConvertReportingPeriodType();
         var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();
         var supplierPeriodStatuses = GetAndConvertSupplierPeriodStatus();

         var reportingPeriodDomain = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntityToDomain(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatuses);

         var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatus();

         var supplierEntity = _supplierDataActions.GetSupplierById(reportingPeriodData.SupplierId);

         if (supplierEntity == null)
         {
             throw new ArgumentNullException("Unable to retrive Supplier");
         }

         var supplierVo = _supplierDataActions.GetSupplierById(periodSupplierEntity.SupplierId);
         if(supplierVo == null)
         {
             throw new Exception("Unable to retrive Supplier");
         }

         _reportingPeriodEntityDomainMapper.ConvertPeriodSupplierEntityToDomain(reportingPeriodDomain,reportingPeriodEntity.ReportingPeriodSupplierEntities,supplierReportingPeriodStatus);

         if (supplierVo.IsActive == true)
         {

             var facilityPeriodDataStatus = facilityReportingPeriodDataStatus.FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.FacilityReportingPeriodDataStatusId);

             var reportingType = reportingTypes.FirstOrDefault(x => x.Id == reportingPeriodFacilityDto.ReportingTypeId);
             *//*var supplierPeriodStatus = supplierPeriodStatuses.FirstOrDefault(x => x.Id == reportingPeriodData.SupplierReportingPeriodStatusId);*/

    /*

            var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierToSupplierValueObject(supplierEntity);

            var periodSupplierDomain = _reportingPeriodEntityDomainMapper.ConvertPeriodSuppliersEntityToDomain(reportingPeriodDomain, periodSupplierEntity, supplierPeriodStatuses, supplierVO);

            var periodSupplierList = _reportingPeriodDataActions.GetPeriodSuppliers();

            ///First Entry In Database
            if (periodSupplierList.Count() == 0)
            {
                goto AddCase;
            }
            var counter = 0;

            foreach (var periodSupplier in periodSupplierList)
            {
                if (periodSupplier.SupplierId == supplierVO.Id && periodSupplier.ReportingPeriodId == reportingPeriodDomain.Id)
                {
                    throw new Exception("ReportingPeriod Supplier already exists!!");
                }
                else
                {
                    counter++;
                }
            }

            if (counter > 0)
            {
                goto AddCase;
            }

        AddCase:
            {
                _reportingPeriod.AddPeriodFacilityToPeriodSupplier(supplierVO.Id,facilityPeriodDataStatus,reportingType,periodSupplierEntity.Id);
                var periodFacilityEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplierDomain);

                //_reportingPeriodDataActions.AddPeriodFacility(periodFacilityEntity);

            }
        }
        return "Success";

    }
*/

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
    /// GetActivePeriodSuppliers
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ReportingPeriodActiveSupplierDTO> GetActivePeriodSuppliers()
    {
        var activePeriodSuppliers = _reportingPeriodDataActions.GetPeriodSuppliers();
        var periodSupplierDto = new List<ReportingPeriodActiveSupplierDTO>();

        foreach (var periodSupplierEntity in activePeriodSuppliers)
        {
            if (periodSupplierEntity.Supplier.IsActive == true)
            {
                periodSupplierDto.Add(_readOnlyEntityToDtoMapper.ConvertReportingPeriodSupplierEntityToReportingPeriodActiveSupplier(periodSupplierEntity));
            }

        }

        return periodSupplierDto;
    }

    #endregion
}
