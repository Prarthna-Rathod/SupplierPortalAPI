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
    string SetMultiplePeriodSuppliers(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDtos);

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
   // bool RemovePeriodSupplier(int PeriodSupplierId);

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

    #endregion

    #region PeriodFacilityElectricityGridMix

    /// <summary>
    /// Add PeriodFacilityElectricGridMix
    /// </summary>
    /// <param name="periodFacilityElectricityGridMixDto"></param>
    /// <returns></returns>
    string AddPeriodFacilityElectricityGridMix(AddMultiplePeriodFacilityElectricityGridMixDto periodFacilityElectricityGridMixDto);

    /// <summary>
    /// GetReportingPeriodFacilityElecticityGridMix
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    AddMultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMix(int periodFacilityId);

    /// <summary>
    /// Remove PeriodFacilityElectricityGrid Mix
    /// </summary>
    /// <param name="supplierId"></param>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    string RemovePeriodFacilityElectricityGridMix(int supplierId, int periodFacilityId);
    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    /// <summary>
    /// AddPeriodfacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="multiplePeriodFacilityGasSupplyBreakDownDto"></param>
    /// <returns></returns>
    string AddPeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakDownDto multiplePeriodFacilityGasSupplyBreakDownDto);

    /// <summary>
    /// GetPeriodFacilityGasSupplyBreakdown
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    MultiplePeriodFacilityGasSupplyBreakDownDto GetReportingPeriodFacilityGasSupplybreakDown(int periodSupplierId);

    /// <summary>
    ///RemovePeriodFacilityGasSupplyBreakdown 
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    string RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId);

    #endregion

    #region PeriodFacilityDocument

    /// <summary>
    /// SetReportingPeriodFacilityDocument
    /// </summary>
    /// <param name="reportingPeriodFacilityDocumentDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto);
    /// <summary>
    /// Remove PeriodFacility Document
    /// </summary>
    /// <param name="DocumentId"></param>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    string RemovePeriodFacilityDocument(int supplierId,int documentId);

    /// <summary>
    /// Get PeriodFacility ElectricityGridMix & Document
    /// </summary>
    /// <param name="periodFacilityId"></param>
    /// <returns></returns>
    ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId,int periodFacilityId);
    /// <summary>
    /// Document FacilityDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    FileStreamResult DownloadFacilityDocument(int documentId);
    /// <summary>
    /// Update FacilityDataStatus
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    string UpdatePeriodFacilityDataStatusCompleteToSubmitted(int periodSupplierId);


    #endregion

    #region PeriodSupplierDocument
    /// <summary>
    /// AddUpdate periodSupplierDocument
    /// </summary>
    /// <param name="reportingPeriodSupplierDocumentDto"></param>
    /// <returns></returns>
    string AddUpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto);
    /// <summary>
    /// Remove PeriodSupplierDocument
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="documentId"></param>
    /// <returns></returns>
    string RemoveReportingPeriodSupplierDocument(int periodSupplierId, int documentId);
    /// <summary>
    /// Get PeriodSupplierGasSupplyBreakdownAndDocuments
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <returns></returns>
    ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocuments(int periodSupplierId);
    /// <summary>
    /// DownloadPeriodSupplierDocument
    /// </summary>
    /// <param name="documentId"></param>
    /// <returns></returns>
    FileStreamResult DownloadSupplierDocument(int documentId);
    /// <summary>
    /// SendEmail InitialDataRequest and ResendDataRequest
    /// </summary>
    /// <param name="periodSupplierId"></param>
    /// <param name="cCEmail"></param>
    /// <param name="bCCEmail"></param>
    /// <returns></returns>
    string SendEmailInitialAndResendDataRequest(int periodSupplierId, string? cCEmail, string? bCCEmail);
    #endregion

}
