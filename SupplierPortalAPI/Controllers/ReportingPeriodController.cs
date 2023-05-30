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
        #endregion


    }
}