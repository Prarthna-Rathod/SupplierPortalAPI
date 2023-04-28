namespace Services.DTOs
{
    public class MultiplePeriodSuppliersDto
    {
        public int Id { get; set; }
        public int ReportingPeriodId { get; set; }
        public string ReportingPeriodName { get; set; }
        public int SupplierReportingPeriodStatusId { get; set; }
        public string SupplierReportingPeriodStatusName { get; set; }
        public IEnumerable<int> SupplierIds { get; set; }
        public DateTime InitialDataRequestDate { get; set; }
        public DateTime ResendDataRequestDate { get; set; }

        public MultiplePeriodSuppliersDto(int id, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatusName, IEnumerable<int> supplierIds,DateTime initialDataRequestDate,DateTime resendDataRequestDate)
        {
            Id = id;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
            SupplierIds = supplierIds;
            InitialDataRequestDate = initialDataRequestDate;
            ResendDataRequestDate = resendDataRequestDate;
        }
    }
}
