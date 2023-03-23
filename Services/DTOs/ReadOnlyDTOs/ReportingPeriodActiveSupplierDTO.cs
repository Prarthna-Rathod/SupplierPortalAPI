namespace Services.DTOs.ReadOnlyDTOs;

public class ReportingPeriodActiveSupplierDTO
{
    public ReportingPeriodActiveSupplierDTO(int reportingPeriodSupplierId, int supplierId,
        string supplierName, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatus)
    {
        ReportingPeriodSupplierId = reportingPeriodSupplierId;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ReportingPeriodId = reportingPeriodId;
        ReportingPeriodName = reportingPeriodName;
        SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
    }

    public int ReportingPeriodSupplierId { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int ReportingPeriodId { get; set; }
    public string ReportingPeriodName { get; set; }
    public int SupplierReportingPeriodStatusId { get; set; }
    public string SupplierReportingPeriodStatus { get; set; }
}
