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

        public string ReportingPeriod { get; set; }

        public int SupplierReportingPeriodStatusId { get; set; }

        public string SupplierReportingPeriodStatus { get;set; }

       // public bool IsActive { get; set; }

        public ReportingPeriodSupplierDto(int id, int supplierId, string supplierName, int reportingPeriodId, string reportingPeriod, int supplierReportingPeriodStatusId, string supplierReportingPeriodStatus)

        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriod = reportingPeriod;
            SupplierReportingPeriodStatusId = supplierReportingPeriodStatusId;
            SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
          //  IsActive = isActive;
        }


    }
}
