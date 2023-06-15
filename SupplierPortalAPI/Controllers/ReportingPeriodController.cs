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
        private readonly ILogger _logger;

        public ReportingPeriodController(ILoggerFactory loggerFactory, IReportingPeriodServices services)
        {
            _logger = loggerFactory.CreateLogger<SupplierController>();
            _services = services;
        }

        #region Add-Update Methods

        [Authorize(Roles = "Internal")]
        [HttpPost("AddUpdateReportingPeriod")]
        public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
        {
            return _services.AddUpdateReportingPeriod(reportingPeriodDto);
        }

        [Authorize(Roles = "Internal")]
        [HttpPost("AddMultiplePeriodSuppliers")]
        public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }

        [Authorize(Roles = "Internal")]
        [HttpPost("AddPeriodFacilities")]
        public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddPeriodFacilities(reportingPeriodFacilityDto);
        }

        [Authorize]
        [HttpPost("AddRemoveReportingPeriodFacility_ElectricityGridMixComponents")]
        public string AddRemoveReportingPeriodFacilityElectricityGridMixComponents(MultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddRemovePeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }


        [Authorize]
        [HttpPost("AddReportingPeriodSupplier_GasSupplyBreakdowns")]
        public string AddRemoveReportingPeriodSupplierGasSupplyBreakdowns(MultiplePeriodFacilityGasSupplyBreakdownDto addMultiplePeriodSupplierGasSupplyBreakdownDto)
        {
            return _services.AddRemovePeriodFacilityGasSupplyBreakdown(addMultiplePeriodSupplierGasSupplyBreakdownDto);
        }

        [Authorize]
        [HttpPost("AddUpdateReportingPeriodDocument")]
        public string AddUpdateReportingPeriodDocument([FromForm] ReportingPeriodDocumentDto reportingPeriodDocumentDto)
        {
            return _services.AddUpdateReportingPeriodDocument(reportingPeriodDocumentDto);
        }

        [Authorize]
        [HttpPost("AddUpdateReportingPeriodSupplierDocument")]
        public string AddUpdateReportingPeriodSupplierDocument([FromForm] ReportingPeriodSupplierDocumentDto periodSupplierDocumentDto)
        {
            return _services.AddUpdateReportingPeriodSupplierDocument(periodSupplierDocumentDto);
        }

        [Authorize(Roles = "Internal")]
        [HttpPut("SendEmailForInitialDataRequestOrResendDataRequest")]
        public string SendDataRequestEmailNotification(int periodSupplierId, string? ccMailId, string? bccMailId)
        {
            return _services.SendInitialOrResendDataRequestEmailNotification(periodSupplierId, ccMailId, bccMailId);
        }

        [Authorize(Roles = "Internal")]
        [HttpPut("UpdateReportingPeriodFacilityStatusSubmitted")]
        public string UpdateReportingPeriodFacilityStatusSubmitted(int reportingPeriodId, int supplierId)
        {
            return _services.UpdatePeriodFacilityStatusSubmitted(reportingPeriodId, supplierId);
        }

        [Authorize(Roles = "Internal")]
        [HttpPut("LockUnlockPeriodSupplierStatus")]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }

        #endregion

        #region GetAll Methods

        [Authorize(Roles = "Internal")]
        [HttpGet("GetActiveReportingPeriods")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetReportingPeriodSuppliersList")]
        public IEnumerable<ReportingPeriodRelevantSupplierDto> GetReportingPeriodSuppliersList(int reportingperiodId)
        {
            return _services.GetRelevantSuppliers(reportingperiodId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetReportingPeriodFacilities")]
        public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilities(periodSupplierId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetReportingPeriodSupplierFacility_GasSupplyBreakdowns")]
        public MultiplePeriodFacilityGasSupplyBreakdownDto GetPeriodSupplierGasSupplyBreakdown(int reportingPeriodSupplierId)
        {
            return _services.GetFacilityGasSupplyBreakdowns(reportingPeriodSupplierId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetReportingPeriodFacility_ElectricityGridMixes")]
        public MultiplePeriodFacilityElectricityGridMixDto GetPeriodFacilityElectricityGridMixes(int reportingPeriodFacilityId)
        {
            return _services.GetFacilityElectricityGridMixComponents(reportingPeriodFacilityId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetPeriodFacilityElectricityGridMixAndDocuments")]
        public ReportingPeriodFacilityElectricityGridMixAndDocumentDto GetReportingPeriodFacilityElectricityGridMixAndDocuments(int reportingPeriodFacilityId)
        {
            return _services.GetPeriodFacilityDocuments(reportingPeriodFacilityId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("DownloadReportingPeriodFacilityDocument")]
        public IActionResult DownloadReportingPeriodFacilityDocumentById(int documentId)
        {
            return _services.DownloadPeriodFacilityDocument(documentId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("GetReportingPeriodSupplierGasSupplyBreakdownAndDocuments")]
        public ReportingPeriodSupplierGasSupplyBreakdownAndDocumentDto GetReportingPeriodSupplierGasSupplyBreakdownAndDocumentDto(int reportingPeriodSupplierId)
        {
            return _services.GetPeriodSupplierSupplyBreakdownAndDocumentDto(reportingPeriodSupplierId);
        }

        [Authorize(Roles = "Internal")]
        [HttpGet("DownloadReportingPeriodSupplierDocumentById")]
        public IActionResult DownloadReportingPeriodSupplierDocumentById(int documentId)
        {
            return _services.DownloadPeriodSupplierDocument(documentId);
        }

        #endregion

        #region Remove Methods

        [Authorize(Roles = "Internal")]
        [HttpDelete("RemoveReportingPeriodFacilityDocuments")]
        public string RemoveReportingPeriodFacilityDocuments(int reportingPeriodFacilityId, int documentId)
        {
            return _services.RemoveReportingPeriodFacilityDocument(reportingPeriodFacilityId, documentId);
        }

        [Authorize(Roles = "Internal")]
        [HttpDelete("RemoveReportingPeriodSupplierDocument")]
        public string RemoveReportingPeriodSupplierDocument(int reportingPeriodSupplierId, int documentId)
        {
            return _services.RemoveReportingPeriodSupplierDocument(reportingPeriodSupplierId, documentId);
        }

        [Authorize(Roles = "Internal")]
        [HttpDelete("DeleteReportingPeriodFacilityElectricityGridMixes")]
        public string DeleteReportingPeriodFacilityElectricityGridMixes(int periodFacilityId)
        {
            return _services.DeletePeriodFacilityElectricityGridMixes(periodFacilityId);
        }

        [Authorize(Roles = "Internal")]
        [HttpDelete("DeleteReportingPeriodSupplierGasSupplyBreakdown")]
        public string DeleteReportingPeriodSupplierGasSupplyBreakdown(int periodSupplierId)
        {
            return _services.DeletePeriodSupplierGasSupplyBreakdowns(periodSupplierId);
        }

        #endregion
    }
}
