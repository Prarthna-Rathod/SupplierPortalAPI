
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Interfaces;
using System.Configuration;
using System.Data;

namespace SupplierPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingPeriodController : ControllerBase
    {
        private IReportingPeriodServices _services;
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;

        public ReportingPeriodController(IReportingPeriodServices services,ISendGridClient sendGridClient,IConfiguration configuration)
        {
            _services = services;
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }

        #region Add-Update Methods

        [HttpPost("AddUpdateReportingPeriod")]
        [Authorize(Roles = "Internal")]
        public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
        {
            return _services.AddUpdateReportingPeriod(reportingPeriodDto);
        }
        [HttpPost("AddMultiplePeriodSuppliers")]
        [Authorize(Roles = "Internal")]
        public string SetMultiplePeriodSuppliers(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }
        [HttpPost("AddPeriodFacilities")]
        [Authorize(Roles = "Internal")]
        public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddPeriodFacilities(reportingPeriodFacilityDto);
        }
        [HttpPost("AddPeriodFacilityElectricityGridMix")]
        [Authorize]
        public string AddReportingPeriodFacilityElectricityGridMix(AddMultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddPeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }
        [HttpPost("AddPeriodFacilityGasSupplyBreakdown")]
        [Authorize]
        public string AddReportingPeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakDownDto multiplePeriodFacilityGasSupplyBreakDownDto)
        {
            return _services.AddPeriodFacilityGasSupplyBreakdown(multiplePeriodFacilityGasSupplyBreakDownDto);
        }
        [HttpPost("AddUpdateReportingPeriodFacilityDocument")]
        [Authorize]
        public string AddUpdateReportingPeriodFacilityDocument([FromForm] ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto)
        {
            return _services.AddUpdateReportingPeriodFacilityDocument(reportingPeriodFacilityDocumentDto);
        }
        [HttpPost("AddUpdateReportingPeriodSupplierDocument")]
        [Authorize]
        public string AddUpdateReportingperiodSupplierDocument([FromForm] ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto)
        {
            return _services.AddUpdateReportingPeriodSupplierDocument(reportingPeriodSupplierDocumentDto);
        }
        [HttpPut("LockUnlockPeriodSupplierStatus")]
        [Authorize(Roles = "Internal")]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }
        [HttpPut("UpdatePeriodFacilityDataStatus")]
        [Authorize(Roles = "Internal")]
        public string UpdatePeriodFacilityDataStatus(int periodSupplierId)
        {
            return _services.UpdatePeriodFacilityDataStatusCompleteToSubmitted(periodSupplierId);
        }
        [HttpPost("SendInitialDataRequestResendDataRequestEmail ")]
        [Authorize(Roles = "Internal")]
        public string SendEmailInitialAndResendDataRequest(int periodSupplierId, string? CCEmail, string? BCCEmail)
        {
            return _services.SendEmailInitialAndResendDataRequest(periodSupplierId, CCEmail,BCCEmail);
        }
        #endregion

        #region GetAll Methods

        [HttpGet("GetActiveReportingPeriods")]
        [Authorize(Roles = "Internal")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }
        [HttpGet("GetReportingPeriodSuppliersList")]
        [Authorize(Roles = "Internal")]
        public IEnumerable<ReportingPeriodRelevantSupplierDto> GetReportingPeriodSuppliersList(int reportingPeriodId)
        {
            return _services.GetRelevantSuppliers(reportingPeriodId);
        }
        [HttpGet("GetPeriodFacilities")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilities(periodSupplierId);
        }
        [HttpGet("GetPeriodFacilityElectricityGridMix")]
        [Authorize(Roles = "Internal")]
        public AddMultiplePeriodFacilityElectricityGridMixDto GetPeriodFacilityElecticityGridMix(int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityElectricityGridMix(periodFacilityId);
        }
        [HttpGet("GetPeriodFacilityGasSupplyBreakdown")]
        [Authorize(Roles = "Internal")]
        public MultiplePeriodFacilityGasSupplyBreakDownDto GetPeriodFaclityGasSupplyBreakDown(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilityGasSupplybreakDown(periodSupplierId);
        }
        [HttpGet("GetPeriodFacilityElectricityGridMixesAndPeriodFacilityDocuments")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId,int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityGridMixAndDocuments(supplierId,periodFacilityId);
        }
        [HttpGet("GetPeriodFacilityGasSupplyBreakDownAndPeriodSupplierDocument")]
        [Authorize(Roles = "Internal")]
        public ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocumentsDto(int periodSupplierId)
        {
            return _services.GetReportingPeriodSupplierGasSupplyAndDocuments(periodSupplierId);
        }
        [HttpGet("DownloadPeriodFacilityDocument")]
        [Authorize(Roles = "Internal")]
        public IActionResult DownloadPeriodFaclityDocument(int documentId)
        {
            var document = _services.DownloadFacilityDocument(documentId);
            return document;
        }
        [HttpGet("DownloadPeriodSupplierDocument")]
        [Authorize(Roles = "Internal")]
        public IActionResult DownloadPeriodSupplierDocument(int documentId)
        {
            var document = _services.DownloadSupplierDocument(documentId);
            return document;
        }
        #endregion

        #region Remove Methods

        [HttpDelete("RemovePeriodFacilityElectricityGridMix")]
        [Authorize(Roles = "Internal")]
        public string RemovePeriodFacilityEectricityGridMix(int supplierId,int periodFacilityId)
        {
            return _services.RemovePeriodFacilityElectricityGridMix(supplierId,periodFacilityId);
        }
        [HttpDelete("RemovePeriodFacilityGasSupplyBreakdown")]
        [Authorize(Roles = "Internal")]
        public string RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
        {
            return _services.RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId) ;
        }
        [HttpDelete("RemoveReportingPeriodFacilityDocument")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodFacilityDocument(int supplierId,int documentId)
        {
            return _services.RemovePeriodFacilityDocument(supplierId,documentId);
        }
        [HttpDelete("RemoveReportingPeriodSupplierDocument")]
        [Authorize(Roles = "Internal")]
        public string RemoveReportingPeriodSupplierDocument(int periodSupplierId,int documentId)
        {
            return _services.RemoveReportingPeriodSupplierDocument(periodSupplierId,documentId);
        }
        #endregion


    }
}