using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Factories.Interface;
using Services.Interfaces;
using Services.Mappers.Interfaces;
using System.ComponentModel.Design;

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

    private IEnumerable<ReportingPeriodType> GetAndConvertReportingPeriodType()
    {
        var reportingPeriodTypeEntity = _reportingPeriodDataActions.GetReportingPeriodTypes().Where(x => x.IsActive).ToList();

        return _referenceLookUpMapper.GetReportingPeriodTypesLookUp(reportingPeriodTypeEntity);
    }

    private IEnumerable<ReportingPeriodStatus> GetAndConvertReportingPeriodStatus()
    {
        var reportingPeriodStatusEntity = _reportingPeriodDataActions.GetReportingPeriodStatus().Where(x => x.IsActive).ToList();

        return _referenceLookUpMapper.GetReportingPeriodStatusesLookUp(reportingPeriodStatusEntity);
    }

    private IEnumerable<SupplierReportingPeriodStatus> GetAndConvertSupplierPeriodStatus()
    {
        var supplierPeriodStatusEntity = _reportingPeriodDataActions.GetSupplierReportingPeriodStatus();

        return _referenceLookUpMapper.GetSupplierReportingPeriodStatusesLookUp(supplierPeriodStatusEntity);
    }

    private IEnumerable<FacilityReportingPeriodDataStatus> GetAndConvertFacilityReportingPeriodDataStatus()
    {
        var facilityReportingPeriodDataStatus = _reportingPeriodDataActions.GetFacilityReportingPeriodDataStatus();

        return _referenceLookUpMapper.GetFacilityReportingPeriodDataStatusLookUp(facilityReportingPeriodDataStatus);
    }

    private IEnumerable<ReportingType> GetAndConvertReportingType()
    {
        var reportingType = _reportingPeriodDataActions.GetReportingTypes();

        return _referenceLookUpMapper.GetReportingTypeLookUp(reportingType);
    }

    /*
    private ReportingPeriod GetAndConvertReportingPeriodSupplierToDomain(int reportingPeriodId)
    {
        var reportingPeriodSuppliers = _reportingPeriodDataActions.GetReportingPeriodSuppliers(reportingPeriodId);
        var periodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);
        var reportingPeriodTypes = GetAndConvertReportingPeriodType();
        var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();

        ReportingPeriod reportingPeriod = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntityToDomain(periodEntity, reportingPeriodTypes, reportingPeriodStatuses);

        var supplierPeriodStatuses = GetAndConvertSupplierPeriodStatus();

        foreach (var reportingPeriodSupplier in reportingPeriodSuppliers)
        {
            var supplierEntity = reportingPeriodSupplier.Supplier;
            var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierToSupplierValueObject(supplierEntity);
            var supplierPeriodStatus = supplierPeriodStatuses.FirstOrDefault(x => x.Id == reportingPeriodSupplier.SupplierReportingPeriodStatusId);

            _reportingPeriod.LoadPeriodSupplier(reportingPeriodSupplier.Id, supplierVO, reportingPeriodId, supplierPeriodStatus ?? new SupplierReportingPeriodStatus());
        }
        return reportingPeriod;

    }*/

    private ReportingPeriod RetrieveAndConvertReportingPeriod(int reportingPeriodId)
    {
        var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);
        var reportingPeriodTypes = GetAndConvertReportingPeriodType();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatus();

        if (reportingPeriodEntity is null)
            throw new ArgumentNullException("ReportingPeriodEntity not found !!");

        var reportingPeriodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

        var supplierReportingPeriodStatuses = GetAndConvertSupplierPeriodStatus();

        /*_reportingPeriodEntityDomainMapper.ConvertPeriodSupplierEntityToDomain(periodDomain, reportingPeriodEntity.ReportingPeriodSupplierEntities, supplierReportingPeriodStatuses);*/

        return reportingPeriodDomain;

    }

    /* private ReportingPeriod RetrieveAndConvertReportingPeriodSupplierToReportingPeriod(int reportingPeriodId,int supplierId)
     {
         var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);
         var reportingPeriodTypes = GetAndConvertReportingPeriodType();
         var reportingPeriodStatus = GetAndConvertReportingPeriodStatus();

         var supplierEntity = _supplierDataActions.GetSupplierById(supplierId);
         var supplierReportingPeriodStatus = GetAndConvertSupplierPeriodStatus();

         if (reportingPeriodEntity is null && supplierEntity is null)
             throw new ArgumentNullException("Unable to retrieve ReportingPeriod and Supplier");

         var periodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

         _reportingPeriodEntityDomainMapper.ConvertPeriodSupplierEntityToDomain(periodDomain, reportingPeriodEntity.ReportingPeriodSupplierEntities, supplierReportingPeriodStatus);

         return periodDomain;

     }*/

   /* private ReportingPeriod RetrieveAndConvertReportingPeriodAndReportingPeriodSupplier(int reportingPeriodId)
    {
        var reportingPeriodEntity = _reportingPeriodDataActions.GetReportingPeriodById(reportingPeriodId);
        var reportingPeriodTypes = GetAndConvertReportingPeriodType();
        var reportingPeriodStatus = GetAndConvertReportingPeriodStatus();

        if (reportingPeriodEntity is null)
            throw new ArgumentNullException("Unable to retrieve reporting period entity");

        var periodDomain = ConfigureReportingPeriod(reportingPeriodEntity, reportingPeriodTypes, reportingPeriodStatus);

        var supplierReportingPeriodStatuses = GetAndConvertSupplierPeriodStatus();

        _reportingPeriodEntityDomainMapper.ConvertPeriodSupplierEntityToDomain(periodDomain, reportingPeriodEntity.ReportingPeriodSupplierEntities, supplierReportingPeriodStatuses);
        return periodDomain;

    }*/

    private SupplierVO GetAndConvertSupplierValueObject(int supplierId)
    {
        var supplierEntity = _supplierDataActions.GetSupplierById(supplierId);

        if (supplierEntity == null)
        {
            throw new ArgumentNullException("Unable to retrive Supplier");
        }
        var supplierVO = _reportingPeriodEntityDomainMapper.ConvertSupplierToSupplierValueObject(supplierEntity);
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

            var reportingPeriodType = GetAndConvertReportingPeriodType().FirstOrDefault(x => x.Id == reportingPeriodDto.ReportingPeriodTypeId);
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().FirstOrDefault(x => x.Id == reportingPeriodDto.ReportingPeriodStatusId);

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
            var reportingPeriodStatus = GetAndConvertReportingPeriodStatus().Where(x => x.Id == reportingPeriodDto.ReportingPeriodStatusId).FirstOrDefault();
            var reportingPeriodType = GetAndConvertReportingPeriodType().Where(x => x.Id == reportingPeriodDto.ReportingPeriodTypeId).FirstOrDefault();
            var supplierReportingPeriodStatuses = GetAndConvertSupplierPeriodStatus();

            if (reportingPeriodType is null)
                throw new Exception("ReportingPeriodType not found !!");
            if (reportingPeriodStatus is null)
                throw new Exception("ReportingPeriodStatus not found !!");

            reportingPeriod.UpdateReportingPeriod(reportingPeriodType, reportingPeriodDto.CollectionTimePeriod, reportingPeriodStatus, reportingPeriodDto.StartDate, reportingPeriodDto.EndDate, reportingPeriodDto.IsActive, supplierReportingPeriodStatuses);

            //Convert domain to entity
            var entity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodDomainToEntity(reportingPeriod);
            _reportingPeriodDataActions.UpdateReportingPeriod(entity);
            
        }

        return "ReportingPeriod added successfully !!";
    }

    /// <summary>
    /// Add PeriodSupplier (Where Supplier should be active & ReportingPeriod should be InActive)
    /// </summary>
    /// <param name="reportingPeriodSupplierDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
   /* public string SetPeriodSupplier(ReportingPeriodSupplierDto reportingPeriodSupplierDto)
    {
        var reportingPeriodEntity = RetrieveAndConvertReportingPeriodAndReportingPeriodSupplier(reportingPeriodSupplierDto.ReportingPeriodId);
        
        var supplierPeriodStatus = GetAndConvertSupplierPeriodStatus().FirstOrDefault(x => x.Id == reportingPeriodSupplierDto.SupplierReportingPeriodStatusId);
          
        var periodSupplierList = _reportingPeriodDataActions.GetPeriodSuppliers();

        ///First Entry In Database
        if (periodSupplierList.Count() == 0)
            goto AddCase;
            
        var counter = 0;

        foreach (var periodSupplier in periodSupplierList)
        {
            if (periodSupplier.SupplierId == reportingPeriodSupplierDto.SupplierId && periodSupplier.ReportingPeriodId == reportingPeriodEntity.Id)
                throw new Exception("ReportingPeriod Supplier already exists!!");
            else
                counter++;
        }

        if (counter > 0)
            goto AddCase;

        AddCase:
            {
                var periodSupplier = reportingPeriodEntity.AddPeriodSupplier(GetAndConvertSupplierValueObject(reportingPeriodSupplierDto.SupplierId), reportingPeriodSupplierDto.ReportingPeriodId, supplierPeriodStatus ?? new SupplierReportingPeriodStatus());
                var periodSupplierEntity = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodSupplierDomainToEntity(periodSupplier);
                await _reportingPeriodDataActions.AddPeriodSupplier(periodSupplierEntity);
            }

        return "Success";
    }
*/
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
            *//*var supplierPeriodStatus = supplierPeriodStatuses.FirstOrDefault(x => x.Id == reportingPeriodData.SupplierReportingPeriodStatusId);*//*

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
    public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
    {
        var activeReportingPeriods = _reportingPeriodDataActions.GetReportingPeriods().Where(x => x.IsActive);
        var reportingPeriodTypes = GetAndConvertReportingPeriodType();
        var reportingPeriodStatuses = GetAndConvertReportingPeriodStatus();

        var reportingPeriodDomainList = _reportingPeriodEntityDomainMapper.ConvertReportingPeriodEntitiesToDomain(activeReportingPeriods, reportingPeriodTypes, reportingPeriodStatuses);

        var reportingPeriodDtos = _reportingPeriodDomainDtoMapper.ConvertReportingPeriodDomainListToDtos(reportingPeriodDomainList);

        return reportingPeriodDtos;
    }

    /// <summary>
    /// GetReportingPeriodSuppliers
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SupplierReportingPeriodDTO> GetReportingPeriodSuppliers(int ReportingPeriodId)
    {
        var periodSuppliers = _reportingPeriodDataActions.GetReportingPeriodSuppliers(ReportingPeriodId);
        var supplierReportingPeriodDTO = new List<SupplierReportingPeriodDTO>();

        foreach (var periodSupplierEntity in periodSuppliers)
        {
            supplierReportingPeriodDTO.Add(_readOnlyEntityToDtoMapper.
                ConvertReportingPeriodSupplierEntityToSupplierReportingPeriodDTO(periodSupplierEntity));
        }
        return supplierReportingPeriodDTO;
    }

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
