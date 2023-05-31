using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    string AddRemovePeriodFacilityElectricityGridMix(MultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto);

    /// <summary>
    /// Get ReportingPeriodFacilityElectricityGridMix
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMixes(int supplierId, int periodFacilityId);

    /// <summary>
    /// Remove PeriodFacilityElectricityGridMix
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    string RemoveReportingPeriodFacilityElectricityGridMix(int supplierId, int periodFacilityId);

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    /// <summary>
    /// Add PeriodFacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="periodFacilityGasSupplyBreakdownDto"></param>
    /// <returns></returns>
    string AddRemovePeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakdownDto periodFacilityGasSupplyBreakdownDto);

    /// <summary>
    /// Get PeriodFacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityGasSupplyBreakdownDto GetReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId);

    /// <summary>
    /// Remove PeriodFacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    string RemoveReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId);

    #endregion

    #region PeriodFacilityDocuments

    /// <summary>
    /// AddUpdate PeriodFacilityDocument
    /// </summary>
    /// <param name="reportingPeriodFacilityDocumentDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto);

    /// <summary>
    /// Get PeriodFacility GridMixes and Documents
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <param name="reportingPeriodId"></param>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId, int periodFacilityId);

    /// <summary>
    /// Download PeriodFacilityDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    FileStreamResult GetReportingPeriodFacilityDocumentDownload(int documentId);

    /// <summary>
    /// Remove PeriodFacilityDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    string RemovePeriodFacilityDocument(int supplierId, int periodFacilityId, int documentId);

    #endregion

    #region UpdateFacilityDataStatus

    /// <summary>
    /// Update PeriodFacilityDataStatus Complete To Submitted
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    string UpdatePeriodFacilityDataStatusCompleteToSubmitted(int reportingPeriodId, int periodSupplierId);

    #endregion

    #region PeriodSupplierDocument

    /// <summary>
    /// AddUpdate ReportingPeriodSupplierDocument
    /// </summary>
    /// <param name="reportingPeriodSupplierDocumentDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto);

    /// <summary>
    /// Get ReportingPeriodSupplier GasSupply and Documents
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocuments(int periodSupplierId);

    /// <summary>
    /// Download PeriodSupplierDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    FileStreamResult GetReportingPeriodSupplierDocumentDownload(int documentId);

    /// <summary>
    /// Remove PeriodSupplierDocument
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="documentId"></param>
    /// <returns></returns>
    string RemoveReportingPeriodSupplierDocument(int periodSupplierId, int documentId);

    #endregion

    #region SendEmail

    string SendEmailInitialAndResendDataRequest(int periodSupplierId, string? CCEmail, string? BCCEmail);

    #endregion

}
