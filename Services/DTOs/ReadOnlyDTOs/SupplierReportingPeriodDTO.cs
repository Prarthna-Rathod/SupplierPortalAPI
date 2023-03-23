using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class SupplierReportingPeriodDTO
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int ReportingPeriodId { get; set; }
    public string ReportingPeriodName { get; set; }
    public int SupplierReportingPeriodStatusId { get; set; }
    public string SupplierReportingPeriodStatus { get; set; }

    public SupplierReportingPeriodDTO(int id, int supplierId, string supplierName, 
        int reportingPeriodId, string reportingPeriodName,int supplierReportingPeriodStatusId,
        string supplierReportingPeriodStatus)
    {
        Id = id;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ReportingPeriodId = reportingPeriodId;
        ReportingPeriodName = reportingPeriodName;
        SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
    }

    public SupplierReportingPeriodDTO()
    {

    }
}
