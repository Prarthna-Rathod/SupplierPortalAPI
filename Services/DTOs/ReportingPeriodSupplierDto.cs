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
        public DateTime InitialDataRequestDate { get; set; }
        public DateTime ResendDataRequestDate { get; set; }

        public ReportingPeriodSupplierDto(int id, int supplierId, string supplierName, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatusName,DateTime initialDataRequestDate, DateTime resendDataRequestDate)
        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
            InitialDataRequestDate = initialDataRequestDate;
            ResendDataRequestDate = resendDataRequestDate;
        }


    }
}
