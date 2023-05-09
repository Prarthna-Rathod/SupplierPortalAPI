using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;

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
    /// Add PeriodSupplier
    /// </summary>
    /// <param name="reportingPeriodSupplierDto"></param>
    /// <returns></returns>
    /*string SetPeriodSupplier(ReportingPeriodSupplierDto reportingPeriodSupplierDto);*/

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
    /// Get ReportingPeriodSuppliers
    /// </summary>
    /// <returns></returns>
    /// 
    IEnumerable<ReportingPeriodSupplierDto> GetReportingPeriodSuppliers(int reportingPeriodId);

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
    string AddRemovePeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto);

    /// <summary>
    /// Get ReportingPeriodFacilities
    /// </summary>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId);

    #endregion

    #region PeriodFacilityElectricityGridMix

    /// <summary>
    /// AddRemove PeriodFacilityElectricityGridMix
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    string AddRemovePeriodFacilityElectricityGridMix(AddMultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto);

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    /// <summary>
    /// Add PeriodFacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="periodFacilityGasSupplyBreakdownDto"></param>
    /// <returns></returns>
    string AddRemovePeriodFacilityGasSupplyBreakdown(AddMultiplePeriodFacilityGasSupplyBreakdownDto periodFacilityGasSupplyBreakdownDto);

    #endregion

    #region ReportingPeriodDocuments
    #endregion



}
