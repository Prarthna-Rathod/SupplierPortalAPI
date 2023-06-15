using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        #region Add methods

        [HttpPost("AddUpdateReportingPeriod")]
        [Authorize(Roles = "Internal")]
        public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
        {
            return _services.AddUpdateReportingPeriod(reportingPeriodDto);
        }

        [HttpPost("AddMultiplePeriodSuppliers")]
        [Authorize(Roles = "Internal")]
        public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }

        [HttpPost("AddRemovePeriodFacilities")]
        [Authorize(Roles = "Internal")]
        public string AddRemovePeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddRemovePeriodFacilities(reportingPeriodFacilityDto);
        }

        [HttpPost("AddRemovePeriodFacility_ElectricityGridMixComponents")]
        [Authorize]
        public string AddRemovePeriodFacilityElectricityGridMixComponents(MultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddRemovePeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }

        [HttpPost("AddRemovePeriodFacility_GasSupplyBreakdowns")]
        [Authorize]
        public string AddRemovePeriodFacilityGasSupplyBreakdowns(MultiplePeriodFacilityGasSupplyBreakdownDto addMultiplePeriodFacilityGasSupplyBreakdownDto)
        {
            return _services.AddRemovePeriodFacilityGasSupplyBreakdown(addMultiplePeriodFacilityGasSupplyBreakdownDto);
        }

        [HttpPost("AddUpdateReportingPeriodFacilityDocument")]
        [Authorize]
        public string AddUpdateReportingPeriodFacilityDocument([FromForm] ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto)
        {
            return _services.AddUpdateReportingPeriodFacilityDocument(reportingPeriodFacilityDocumentDto);
        }

        [HttpPost("AddUpdateReportingPeriodSupplierDocument")]
        [Authorize]
        public string AddUpdateReportingPeriodSupplierDocument([FromForm] ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto)
        {
            return _services.AddUpdateReportingPeriodSupplierDocument(reportingPeriodSupplierDocumentDto);
        }

        [HttpPost("SendEmailInitialOrResendDataRequest")]
        [Authorize(Roles = "Internal")]
        public string SendEmailInitialAndResendDataRequest(int periodSupplierId,string? CCEmail,string? BCCEmail)
        {
            return _services.SendEmailInitialAndResendDataRequest(periodSupplierId, CCEmail, BCCEmail);
        }

        #endregion

        #region Update methods

        [HttpPut("LockUnlockPeriodSupplierStatus")]
        [Authorize]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }

        [HttpPut("UpdatePeriodFacilityDataStatus")]
        [Authorize(Roles = "Internal")]
        public string UpdatePeriodFacilityDataStatus(int reportingPeriodId, int periodSupplierId)
        {
            return _services.UpdatePeriodFacilityDataStatusCompleteToSubmitted(reportingPeriodId, periodSupplierId);
        }

        #endregion

        #region Get methods

        [HttpGet("GetActiveReportingPeriods")]
        [Authorize(Roles = "Internal")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }

        [HttpGet("GetReportingPeriodSuppliersList")]
        [Authorize(Roles = "Internal")]
        public IEnumerable<ReportingPeriodRelevantSupplierDto> GetReportingPeriodSuppliersList(int reportingperiodId)
        {
            return _services.GetRelevantSuppliers(reportingperiodId);
        }

        [HttpGet("GetReportingPeriodFacilities")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilities(periodSupplierId);
        }

        [HttpGet("GetReportingPeriodFacility_ElectricityGridMixes")]
        [Authorize(Roles = "Internal")]
        public MultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMixes(int supplierId,int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityElectricityGridMixes(supplierId,periodFacilityId);
        }

        [HttpGet("GetReportingPeriodFacility_GasSupplyBreakdown")]
        [Authorize(Roles = "Internal")]
        public MultiplePeriodFacilityGasSupplyBreakdownDto GetReportingPeriodFacilityGasSupplyBreakdowns(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilityGasSupplyBreakdown(periodSupplierId);
        }

        [HttpGet("ReportingPeriodFacility_ElectricityGridMixes_Documents")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId, int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityGridMixAndDocuments(supplierId,periodFacilityId);
        }

        [HttpGet("ReportingPeriodFacilityDocumentDownload")]
        [Authorize(Roles = "Internal")]
        public IActionResult GetReportingPeriodFacilityDocumentDownload(int documentId)
        {
            var result = _services.GetReportingPeriodFacilityDocumentDownload(documentId);
            return result;
        }

        [HttpGet("ReportingPeriodSupplier_GasSupplyBreakDowns_Documents")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocuments(int periodSupplierId)
        {
            return _services.GetReportingPeriodSupplierGasSupplyAndDocuments(periodSupplierId);
        }

        [HttpGet("ReportingPeriodSupplierDocumentDownload")]
        [Authorize(Roles = "Internal")]
        public IActionResult GetReportingPeriodSupplierDocumentDownload(int documentId)
        {
            var result = _services.GetReportingPeriodSupplierDocumentDownload(documentId);
            return result;
        }

        #endregion

        #region Remove methods

        [HttpDelete("RemoveReportingPeriodFacilityDocument")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodFacilityDocument(int supplierId, int periodFacilityId, int documentId)
        {
            return _services.RemovePeriodFacilityDocument(supplierId,periodFacilityId,documentId);
        }

        [HttpDelete("RemoveReportingPeriodSupplierDocument")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodSupplierDocument(int periodSupplierId, int documentId)
        {
            return _services.RemoveReportingPeriodSupplierDocument(periodSupplierId, documentId);
        }

        [HttpDelete("RemoveReportingPeriodFacilityElectricityGridMix")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodFacilityElectricityGridMix(int supplierId,int periodFacilityId)
        {
            return _services.RemoveReportingPeriodFacilityElectricityGridMix(supplierId,periodFacilityId);
        }

        [HttpDelete("RemoveReportingPeriodFacilityGasSupplyBreakdown")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodFacilityGasSupplyBreakdown(int periodSupplierId)
        {
            return _services.RemoveReportingPeriodFacilityGasSupplyBreakdown(periodSupplierId);
        }

        #endregion

    }
}
