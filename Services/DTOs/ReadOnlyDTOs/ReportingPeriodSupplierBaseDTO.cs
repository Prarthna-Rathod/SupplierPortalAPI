using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class ReportingPeriodSupplierBaseDTO : SupplierReportingPeriodDTO
{
    public ReportingPeriodSupplierBaseDTO(int id, int supplierId, string supplierName, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatus) : base(id, supplierId, supplierName, reportingPeriodId, reportingPeriodName, supplierReportingPeriodStatusId, supplierReportingPeriodStatus)
    {
    }
}
