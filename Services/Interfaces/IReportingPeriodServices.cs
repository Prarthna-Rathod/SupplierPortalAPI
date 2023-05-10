using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using DataAccess.Entities;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces;

public interface IReportingPeriodServices
{

    #region ReportingPeriod

    /// <summary>
    /// Add Reporting Period
    /// </summary>
    /// <param name="reportingPeriodDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto);

    /// <summary>
    /// Get Active ReportingPeriods
    /// </summary>
    /// <returns></returns>
    IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods();

    #endregion

    #region PeriodSuppliers

    /// <summary>
    /// Add Multiple PeriodSuppliers
    /// </summary>
    /// <param name="multiplePeriodSuppliersDto"></param>
    /// <returns></returns>
    string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto);

    /// <summary>
    /// LockUnlockPeriodSupplierStatus
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    string LockUnlockPeriodSupplierStatus(int periodSupplierId);

    /// <summary>
    /// Get Relevant PeriodSuppliers
    /// </summary>
    /// <returns></returns>
    IEnumerable<ReportingPeriodRelevantSupplierDto> GetRelevantSuppliers(int reportingPeriodId);

    /// <summary>
    /// Remove Period Supplier
    /// </summary>
    /// <param name="PeriodSupplierId"></param>
    /// <returns></returns>
    bool RemovePeriodSupplier(int PeriodSupplierId);

    #endregion

    #region PeriodFacilities


    /// <summary>
    /// Add PeriodFacility
    /// </summary>
    /// <param name="reportingPeriodFacilityDto"></param>
    /// <returns></returns>
     string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto);

    /// <summary>
    /// Get ReportingPeriodFacilities
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId);

    /// <summary>
    /// Add or Replace ReportingPeriodFacility ElectricityGridMix Components
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    string AddRemovePeriodFacilityElectricityGridMix(MultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto);

    /// <summary>
    /// Add or Replace ReportingPeriodSupplierFacility GasSupplyBreakdown sites
    /// </summary>
    /// <param name="multiplePeriodSupplierGasSupplyBreakdownDto"></param>
    /// <returns></returns>
    string AddRemovePeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakdownDto multiplePeriodSupplierGasSupplyBreakdownDto);

    /// <summary>
    /// Get ReportingPeriodFacility ElectricityGridMixComponents data
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityElectricityGridMixDto GetFacilityElectricityGridMixComponents(int periodFacilityId, int reportingPeriodId);


    /// <summary>
    /// Get ReportingPeriodSupplierFacility GasSupplyBreakdown data
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityGasSupplyBreakdownDto GetFacilityGasSupplyBreakdowns(int periodSupplierId, int reportingPeriodId);
    #endregion

    #region ReportingPeriodDocuments
    #endregion



}
