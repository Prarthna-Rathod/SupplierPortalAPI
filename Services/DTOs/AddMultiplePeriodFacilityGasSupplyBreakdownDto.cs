﻿using BusinessLogic.ReportingPeriodRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class AddMultiplePeriodFacilityGasSupplyBreakdownDto
    {
        public int ReportingPeriodSupplierId { get; set; }
        public int ReporingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> PeriodSupplierGasSupplyBreakdowns { get; set; }

        public AddMultiplePeriodFacilityGasSupplyBreakdownDto(int reportingPeriodSupplierId, int reporingPeriodId, int supplierId, string supplierName, IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownDto> periodSupplierGasSupplyBreakdowns)
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            ReporingPeriodId = reporingPeriodId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            PeriodSupplierGasSupplyBreakdowns = periodSupplierGasSupplyBreakdowns;
        }
    }
}
