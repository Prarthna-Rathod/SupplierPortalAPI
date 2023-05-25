using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
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
    MultiplePeriodFacilityElectricityGridMixDto GetFacilityElectricityGridMixComponents(int periodFacilityId);

    /// <summary>
    /// Get ReportingPeriodSupplierFacility GasSupplyBreakdown data
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityGasSupplyBreakdownDto GetFacilityGasSupplyBreakdowns(int reportingPeriodId, int supplierId);

    /// <summary>
    /// Update AllReportingPeriodFacilities
    /// FacilityReportingPeriodDataStatus is changed from Complete to Submitted
    /// </summary>
    /// <param name="reportingPeriodDocumentDto"></param>
    /// <returns></returns>

    string UpdatePeriodFacilityStatusSubmitted(int reportingPeriodId, int supplierId);

    #endregion

    #region ReportingPeriodDocuments

    /// <summary>
    /// AddUpdate ReportingPeriodDocument
    /// </summary>
    /// <param name="reportingPeriodDocumentDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriodDocument(ReportingPeriodDocumentDto reportingPeriodDocumentDto);

    /// <summary>
    /// Get ReportingPeriodFacility ElectricityGridMix and document details
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    ReportingPeriodFacilityElectricityGridMixAndDocumentDto GetPeriodFacilityDocuments(int periodFacilityId);

    FileContentResult DownloadPeriodFacilityDocument(int documentId);

    string RemoveReportingPeriodFacilityDocument(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int documentId);

    #endregion

    #region ReportingPeriodSupplierDocument

    string AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentDto periodSupplierDocumentDto);

    ReportingPeriodSupplierGasSupplyBreakdownAndDocumentDto GetPeriodSupplierSupplyBreakdownAndDocumentDto(int reportingPeriodId, int supplierId);

    FileContentResult DownloadPeriodSupplierDocument(int documentId);

    string RemoveReportingPeriodSupplierDocument(int reportingPeriodId, int supplierId, int documentId);

    #endregion

}
