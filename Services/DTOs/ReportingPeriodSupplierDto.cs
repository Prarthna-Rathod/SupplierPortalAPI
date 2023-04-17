using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string SupplierReportingPeriodStatusName { get;set; }
       // public bool IsActive { get; set; }

        public ReportingPeriodSupplierDto(int id, int supplierId, string supplierName, int reportingPeriodId, string reportingPeriodName, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatusName)
        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodName = reportingPeriodName;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatusName = supplierReportingPeriodStatusName;
          //  IsActive = isActive;
        }


    }
}
