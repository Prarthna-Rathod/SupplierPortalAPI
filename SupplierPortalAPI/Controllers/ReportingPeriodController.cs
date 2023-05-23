﻿using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Interfaces;

namespace SupplierPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingPeriodController : ControllerBase
    {
        private IReportingPeriodServices _services;

        public ReportingPeriodController(IReportingPeriodServices services)
        {
            _services = services;
        }

        #region Add-Update Methods

        [HttpPost("AddUpdateReportingPeriod")]
        public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
        {
            return _services.AddUpdateReportingPeriod(reportingPeriodDto);
        }

        [HttpPost("AddMultiplePeriodSuppliers")]
        public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }

        [HttpPost("AddPeriodFacilities")]
        public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddPeriodFacilities(reportingPeriodFacilityDto);
        }

        [HttpPost("AddRemoveReportingPeriodFacility_ElectricityGridMixComponents")]
        public string AddRemoveReportingPeriodFacilityElectricityGridMixComponents(MultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddRemovePeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }

        [HttpPost("AddReportingPeriodSupplier_GasSupplyBreakdowns")]
        public string AddRemoveReportingPeriodSupplierGasSupplyBreakdowns(MultiplePeriodFacilityGasSupplyBreakdownDto addMultiplePeriodSupplierGasSupplyBreakdownDto)
        {
            return _services.AddRemovePeriodFacilityGasSupplyBreakdown(addMultiplePeriodSupplierGasSupplyBreakdownDto);
        }

        [HttpPost("AddUpdateReportingPeriodDocument")]
        public string AddUpdateReportingPeriodDocument([FromForm] ReportingPeriodDocumentDto reportingPeriodDocumentDto)
        {
            return _services.AddUpdateReportingPeriodDocument(reportingPeriodDocumentDto);
        }

        [HttpPut("UpdateReportingPeriodFacilityStatusSubmitted")]
        public string UpdateReportingPeriodFacilityStatusSubmitted(int reportingPeriodId, int supplierId)
        {
            return _services.UpdatePeriodFacilityStatusSubmitted(reportingPeriodId, supplierId);
        }


        [HttpPut("LockUnlockPeriodSupplierStatus")]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }

        #endregion

        #region GetAll Methods

        [HttpGet("GetActiveReportingPeriods")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }

        [HttpGet("GetReportingPeriodSuppliersList")]
        public IEnumerable<ReportingPeriodRelevantSupplierDto> GetReportingPeriodSuppliersList(int reportingperiodId)
        {
            return _services.GetRelevantSuppliers(reportingperiodId);
        }

        [HttpGet("GetReportingPeriodFacilities")]
        public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilities(periodSupplierId);
        }

        [HttpGet("GetReportingPeriodSupplierFacility_GasSupplyBreakdowns")]
        public MultiplePeriodFacilityGasSupplyBreakdownDto GetPeriodSupplierGasSupplyBreakdown(int reportingPeriodId, int supplierId)
        {
            return _services.GetFacilityGasSupplyBreakdowns(reportingPeriodId, supplierId);
        }

        [HttpGet("GetReportingPeriodFacility_ElectricityGridMixes")]
        public MultiplePeriodFacilityElectricityGridMixDto GetPeriodFacilityElectricityGridMixes(int reportingPeriodFacilityId)
        {
            return _services.GetFacilityElectricityGridMixComponents(reportingPeriodFacilityId);
        }

        [HttpGet("GetPeriodFacilityElectricityGridMixAndDocuments")]
        public ReportingPeriodFacilityElectricityGridMixAndDocumentDto GetReportingPeriodFacilityElectricityGridMixAndDocuments(int reportingPeriodFacilityId)
        {
            return _services.GetPeriodFacilityDocuments(reportingPeriodFacilityId);
        }

        [HttpGet("DownloadReportingPeriodFacilityDocument")]
        public IActionResult DownloadReportingPeriodFacilityDocumentById(int documentId)
        {
            return _services.DownloadPeriodFacilityDocument(documentId);
        }

        #endregion

        #region Remove Methods

        [HttpDelete("RemoveReportingPeriodFacilityDocuments")]
        public string RemoveReportingPeriodFacilityDocuments(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int documentId)
        {
            return _services.RemoveReportingPeriodFacilityDocument(reportingPeriodFacilityId, reportingPeriodId, supplierId, documentId);
        }

        #endregion

    }
}
