﻿using BusinessLogic.ReferenceLookups;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("AddPeriodSupplier")]
        public string SetPeriodSupplier(ReportingPeriodSupplierDto reportingPeriodSupplierDto)
        {
            return _services.SetPeriodSupplier(reportingPeriodSupplierDto);
        }

        [HttpPost("AddPeriodFacilities")]
        public string AddPeriodFacilities(ReportingPeriodFacilityDto reportingPeriodFacilityDto)
        {
            return _services.AddPeriodFacilities(reportingPeriodFacilityDto);
        }
/*
        /* [HttpPut("UpdateLockUnlokckStatusOfPeriodSupplier")]
         public bool PeriodSupplierLockUnlock(int periodSupplierId)
         {
             return _services.UpdateLockUnlockPeriodSupplier(periodSupplierId);
         }*/

        #endregion

        #region GetAll Methods

        [HttpGet("GetActiveReportingPeriods")]
        public IEnumerable<ReportingPeriodDto> GetActiveReportingPeriods()
        {
            return _services.GetActiveReportingPeriods();
        }

        /* [HttpGet("GetActivePeriodSuppliers")]
         public IEnumerable<ReportingPeriodActiveSupplierDTO> GetActivePeriodSuppliers()
         {
             return _services.GetActivePeriodSuppliers();
         }*/

        [HttpGet("GetReportingPeriodRelaventSuppliers")]
        public IEnumerable<ReportingPeriodSupplierDto> GetReportingPeriodSuppliers(int reportingPeriodId)
        {
            return _services.GetReportingPeriodSuppliers(reportingPeriodId);
        }
        #endregion

        #region Remove_PeriodSupplier

        /*[HttpDelete("RemovePeriodSupplier")]
        public string RemovePeriodSupplier(int PeriodSupplierId)
        {
            var result = _services.RemovePeriodSupplier(PeriodSupplierId);

            if (result == true)
            {
                return "Record deleted successfully..";
            }
            else
                return "PeriodSupplier is not exists !!";
        }
*/
        #endregion                   
    }
}
