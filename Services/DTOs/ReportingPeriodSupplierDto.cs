namespace Services.DTOs
{
    public class ReportingPeriodSupplierDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ReportingPeriodId { get; set; }
        public string ReportingPeriodName { get; set; }
        public int SupplierReportingPeriodStatusId { get; set; }
        public string SupplierReportingPeriodStatusName { get; set; }
        public bool ActiveForCurrentPeriod { get; set; }
        public bool InitialDataRequest { get; set; }
        public bool ResendInitialDataRequest { get; set; }

        public ReportingPeriodSupplierDto(int id, int supplierId, string supplierName, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatusName, bool activeForCurrentPeriod, bool initialDataRequest, bool resendInitialDataRequest)
        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
            ActiveForCurrentPeriod = activeForCurrentPeriod;
            InitialDataRequest = initialDataRequest;
            ResendInitialDataRequest = resendInitialDataRequest;
        }


    }
}
