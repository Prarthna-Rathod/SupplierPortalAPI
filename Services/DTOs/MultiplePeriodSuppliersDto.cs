using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using System.Security.Cryptography.X509Certificates;

namespace Services.DTOs
{
    public class MultiplePeriodSuppliersDto
    {
        public int Id { get; set; }
        public int ReportingPeriodId { get; set; }
        public string ReportingPeriodName { get; set; }
        public int SupplierReportingPeriodStatusId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public bool ActiveForCurrentPeriod { get; set; }
        public DateTime? InitialDataRequestDate { get; set; } 
        public DateTime? ResendDataRequestDate { get; set; }

        //public IEnumerable<ReportingPeriodActiveSupplier> Suppliers { get; set; }

        public MultiplePeriodSuppliersDto(int id, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId,int supplierId,string supplierName,bool activeForCurrentPeriod,DateTime? initialDataRequestDate,DateTime? resendDataRequestDate/*, IEnumerable<ReportingPeriodActiveSupplier> suppliers*/)
        {
            Id = id;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ActiveForCurrentPeriod = activeForCurrentPeriod;
            InitialDataRequestDate = initialDataRequestDate;
            ResendDataRequestDate = resendDataRequestDate;
            //Suppliers = suppliers;
        }
    }
}
