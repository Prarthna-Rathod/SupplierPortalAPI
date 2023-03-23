using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class ReportingPeriodActiveSupplier
    {
        public ReportingPeriodActiveSupplier(int? reportingPeriodSupplierId, int supplierId,string supplierName,
            int periodId,string periodName,string periodCollectionTimePeriod,string reportingPeriodType,
            int periodStatusId,string periodStatus,int? supplierPeriodStatusId,string? supplierPeriodStatus , 
            bool supplierActiveForCurrentPeriod,int reportingPeriodTypeId,bool activeForPreviousPeriod,
            bool activeForCurrentPeriod) 
        {
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            SupplierId = supplierId;
            SupplierName = supplierName;
            PeriodId = periodId;
            PeriodName = periodName;
            PeriodCollectionTimePeriod = periodCollectionTimePeriod;
            ReportingPeriodType = reportingPeriodType;
            PeriodStatusId = periodStatusId;
            Periodstatus = periodStatus;
            SupplierPeriodStatusId= supplierPeriodStatusId;
            SupplierPeriodStatus= supplierPeriodStatus;
            SupplierActiveForCurrentPeriod= supplierActiveForCurrentPeriod;
            ReportingPeriodTypeId= reportingPeriodTypeId;
            ActiveForCurrentPeriod= activeForCurrentPeriod;
            ActiveForPreviousPeriod= activeForPreviousPeriod;

        
        
        }

        public int? ReportingPeriodSupplierId { get;set; }

        public int SupplierId { get;set; }

        public string SupplierName { get;set;}

        public int PeriodId { get;set; }

        public string PeriodName { get;set;}

        public string PeriodCollectionTimePeriod { get;set;}

        public string ReportingPeriodType { get;set; }

        public int? PeriodStatusId { get; set; }

        public string Periodstatus { get; set; }

        public int? SupplierPeriodStatusId { get; set; }

        public string? SupplierPeriodStatus { get; set; }

        public bool SupplierActiveForCurrentPeriod { get; set; }

        public int ReportingPeriodTypeId { get;set; }

        public bool ActiveForCurrentPeriod { get; set; }

        public bool ActiveForPreviousPeriod { get;set; }






    }
}
