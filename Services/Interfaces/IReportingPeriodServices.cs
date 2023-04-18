using BusinessLogic.ReferenceLookups;
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
    /// <summary>
    /// Add PeriodSupplier
    /// </summary>
    /// <param name="reportingPeriodSupplierDto"></param>
    /// <returns></returns>
    string SetPeriodSupplier(ReportingPeriodSupplierDto reportingPeriodSupplierDto);

    /// <summary>
    /// Add Reporting Period
    /// </summary>
    /// <param name="reportingPeriodDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto);

    /// <summary>
    /// Add PeriodFacility
    /// </summary>
    /// <param name="reportingPeriodFacilityDto"></param>
    /// <returns></returns>
  //  Task<string> SetPeriodFacility(ReportingPeriodFacilityDto reportingPeriodFacilityDto);

    /// <summary>
    /// Get Active ReportingPeriods
    /// </summary>
    /// <returns></returns>
    IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods();

    /// <summary>
    /// Get Active ReportingPeriodSuppliers
    /// </summary>
    /// <returns></returns>
    IEnumerable<ReportingPeriodActiveSupplierDTO> GetActivePeriodSuppliers();

    /// <summary>
    /// Get Active Period Suppliers
    /// </summary>
    /// <returns></returns>
    IEnumerable<SupplierReportingPeriodDTO> GetReportingPeriodSuppliers(int ReportingPeriodId);

    /// <summary>
    /// Remove Period Supplier
    /// </summary>
    /// <param name="PeriodSupplierId"></param>
    /// <returns></returns>
    bool RemovePeriodSupplier(int PeriodSupplierId);

    /// <summary>
    /// UpdateLockUnlockStatus
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
   // bool UpdateLockUnlockPeriodSupplier(int periodSupplierId);




}