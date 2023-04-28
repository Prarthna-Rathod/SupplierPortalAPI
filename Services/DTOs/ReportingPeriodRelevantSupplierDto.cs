using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodRelevantSupplierDto
    {
        public int? Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? ReportingPeriodId { get; set; }
        public int? SupplierReportingPeriodStatusId { get; set; }
        public string? SupplierReportingPeriodStatusName { get; set; }
        public bool ActiveForCurrentPeriod { get; set; }
        public DateTime? InitialDataRequestDate { get; set; }
        public DateTime? ResendDataRequestDate { get; set; }

        public ReportingPeriodRelevantSupplierDto(int? id, int supplierId, string supplierName, int?reportingPeriodId, int? supplierReportingPeriodStatusId, string? supplierReportingPeriodStatusName, bool activeForCurrentPeriod, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate)
        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodId = reportingPeriodId;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
            ActiveForCurrentPeriod = activeForCurrentPeriod;
            InitialDataRequestDate = initialDataRequestDate;
            ResendDataRequestDate = resendDataRequestDate;
        }

    }
}
