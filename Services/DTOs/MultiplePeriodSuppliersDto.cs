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

        public bool ActiveForCurrentPeriod { get; set; }

        public bool InitialDataRequest { get; set; }

        public bool ResendInitialDataRequest { get; set; }
        //public IDictionary<int, string> Suppliers { get; set; }

        public MultiplePeriodSuppliersDto(int id, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatusName, IEnumerable<int> supplierIds,bool activeForCurrentPeriod,bool initialDataRequest,bool resendInitialDataRequest)
        {
            Id = id;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
            SupplierIds = supplierIds;
            ActiveForCurrentPeriod = activeForCurrentPeriod;
            InitialDataRequest = initialDataRequest;
            ResendInitialDataRequest= resendInitialDataRequest;


        }
    }
}
