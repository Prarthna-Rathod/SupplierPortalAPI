
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using Services.Interfaces;
using System.Configuration;

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
        public string AddUpdateReportingPeriod(ReportingPeriodDto reportingPeriodDto)
        {
            return _services.AddUpdateReportingPeriod(reportingPeriodDto);
        }
        [HttpPost("AddMultiplePeriodSuppliers")]
        public string SetMultiplePeriodSuppliers(IEnumerable<MultiplePeriodSuppliersDto> multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }
        [HttpPost("AddPeriodFacilities")]
        public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddPeriodFacilities(reportingPeriodFacilityDto);
        }
        [HttpPost("AddPeriodFacilityElectricityGridMix")]
        public string AddReportingPeriodFacilityElectricityGridMix(AddMultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddPeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }
        [HttpPost("AddPeriodFacilityGasSupplyBreakdown")]
        public string AddReportingPeriodFacilityGasSupplyBreakdown(MultiplePeriodFacilityGasSupplyBreakDownDto multiplePeriodFacilityGasSupplyBreakDownDto)
        {
            return _services.AddPeriodFacilityGasSupplyBreakdown(multiplePeriodFacilityGasSupplyBreakDownDto);
        }
        [HttpPost("AddUpdateReportingPeriodFacilityDocument")]
        public string AddUpdateReportingPeriodFacilityDocument([FromForm] ReportingPeriodFacilityDocumentDto reportingPeriodFacilityDocumentDto)
        {
            return _services.AddUpdateReportingPeriodFacilityDocument(reportingPeriodFacilityDocumentDto);
        }
        [HttpPost("AddUpdateReportingPeriodSupplierDocument")]
        public string AddUpdateReportingperiodSupplierDocument([FromForm] ReportingPeriodSupplierDocumentDto reportingPeriodSupplierDocumentDto)
        {
            return _services.AddUpdateReportingPeriodSupplierDocument(reportingPeriodSupplierDocumentDto);
        }
        [HttpPut("LockUnlockPeriodSupplierStatus")]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }
        [HttpPut("UpdatePeriodFacilityDataStatus")]
        public string UpdatePeriodFacilityDataStatus(int periodSupplierId)
        {
            return _services.UpdatePeriodFacilityDataStatusCompleteToSubmitted(periodSupplierId);
        }
        [HttpPost("SendInitialDataRequestResendDataRequestEmail ")]
        public string SendEmailInitialAndResendDataRequest(int periodSupplierId, string? CCEmail, string? BCCEmail)
        {
            return _services.SendEmailInitialAndResendDataRequest(periodSupplierId, CCEmail,BCCEmail);
        }
        #endregion

        #region GetAll Methods

        [HttpGet("GetActiveReportingPeriods")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }

        [HttpGet("GetReportingPeriodSuppliersList")]
        public IEnumerable<ReportingPeriodRelevantSupplierDto> GetReportingPeriodSuppliersList(int reportingPeriodId)
        {
            return _services.GetRelevantSuppliers(reportingPeriodId);
        }

        [HttpGet("GetPeriodFacilities")]
        public ReportingPeriodSupplierFacilitiesDto GetReportingPeriodFacilities(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilities(periodSupplierId);
        }

        [HttpGet("GetPeriodFacilityElectricityGridMix")]
        public AddMultiplePeriodFacilityElectricityGridMixDto GetPeriodFacilityElecticityGridMix(int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityElectricityGridMix(periodFacilityId);
        }

        [HttpGet("GetPeriodFacilityGasSupplyBreakdown")]
        public MultiplePeriodFacilityGasSupplyBreakDownDto GetPeriodFaclityGasSupplyBreakDown(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilityGasSupplybreakDown(periodSupplierId);
        }
        [HttpGet("GetPeriodFacilityElectricityGridMixesAndPeriodFacilityDocuments")]
        public ReportingPeriodFacilityGridMixAndDocumentDto GetReportingPeriodFacilityGridMixAndDocuments(int supplierId,int periodFacilityId)
        {
            return _services.GetReportingPeriodFacilityGridMixAndDocuments(supplierId,periodFacilityId);
        }
        [HttpGet("GetPeriodFacilityGasSupplyBreakDownAndPeriodSupplierDocument")]
        public ReportingPeriodSupplierGasSupplyAndDocumentDto GetReportingPeriodSupplierGasSupplyAndDocumentsDto(int periodSupplierId)
        {
            return _services.GetReportingPeriodSupplierGasSupplyAndDocuments(periodSupplierId);
        }

        [HttpGet("DownloadPeriodFacilityDocument")]
        public IActionResult DownloadPeriodFaclityDocument(int documentId)
        {
            var document = _services.DownloadFacilityDocument(documentId);
            return document;
        }
        [HttpGet("DownloadPeriodSupplierDocument")]
        public IActionResult DownloadPeriodSupplierDocument(int documentId)
        {
            var document = _services.DownloadSupplierDocument(documentId);
            return document;
        }
        #endregion

        #region Remove Methods

        [HttpDelete("RemovePeriodFacilityElectricityGridMix")]
        public string RemovePeriodFacilityEectricityGridMix(int supplierId,int periodFacilityId)
        {
            return _services.RemovePeriodFacilityElectricityGridMix(supplierId,periodFacilityId);
        }

        [HttpDelete("RemovePeriodFacilityGasSupplyBreakdown")]
        public string RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
        {
            return _services.RemovePeriodFacilityGasSupplyBreakdown(periodSupplierId) ;
        }

        [HttpDelete("RemoveReportingPeriodFacilityDocument")]
        public string RemoveReportingPeriodFacilityDocument(int supplierId,int documentId)
        {
            return _services.RemovePeriodFacilityDocument(supplierId,documentId);
        }
        [HttpDelete("RemoveReportingPeriodSupplierDocument")]
        public string RemoveReportingPeriodSupplierDocument(int periodSupplierId,int documentId)
        {
            return _services.RemoveReportingPeriodSupplierDocument(periodSupplierId,documentId);
        }
        #endregion


    }
}