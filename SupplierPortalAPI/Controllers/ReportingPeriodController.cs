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
        public string SetMultiplePeriodSuppliers(MultiplePeriodSuppliersDto multiplePeriodSuppliersDto)
        {
            return _services.SetMultiplePeriodSuppliers(multiplePeriodSuppliersDto);
        }

        [HttpPut("LockUnlockPeriodSupplierStatus")]
        public string LockUnlockPeriodSupplierStatus(int periodSupplierId)
        {
            return _services.LockUnlockPeriodSupplierStatus(periodSupplierId);
        }

        [HttpPost("AddRemovePeriodFacilities")]
        public string AddRemovePeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddRemovePeriodFacilities(reportingPeriodFacilityDto);
        }

        [HttpPost("AddRemovePeriodFacility_ElectricityGridMixComponents")]
        public string AddRemovePeriodFacilityElectricityGridMixComponents(MultiplePeriodFacilityElectricityGridMixDto addMultiplePeriodFacilityElectricityGridMixDto)
        {
            return _services.AddRemovePeriodFacilityElectricityGridMix(addMultiplePeriodFacilityElectricityGridMixDto);
        }

        [HttpPost("AddRemovePeriodFacility_GasSupplyBreakdowns")]
        public string AddRemovePeriodFacilityGasSupplyBreakdowns(MultiplePeriodFacilityGasSupplyBreakdownDto addMultiplePeriodFacilityGasSupplyBreakdownDto)
        {
            return _services.AddRemovePeriodFacilityGasSupplyBreakdown(addMultiplePeriodFacilityGasSupplyBreakdownDto);
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

        [HttpGet("GetReportingPeriodFacility_ElectricityGridMixes")]
        public MultiplePeriodFacilityElectricityGridMixDto GetReportingPeriodFacilityElectricityGridMixes(int periodFacilityId,int reportingPeriodId, int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilityElectricityGridMixes(periodFacilityId,reportingPeriodId,periodSupplierId);
        }

        [HttpGet("GetReportingPeriodFacility_GasSupplyBreakdown")]
        public MultiplePeriodFacilityGasSupplyBreakdownDto GetReportingPeriodFacilityGasSupplyBreakdowns(int periodSupplierId)
        {
            return _services.GetReportingPeriodFacilityGasSupplyBreakdown(periodSupplierId);
        }

        #endregion

    }
}
