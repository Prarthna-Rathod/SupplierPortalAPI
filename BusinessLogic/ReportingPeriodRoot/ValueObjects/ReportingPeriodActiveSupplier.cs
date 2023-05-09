using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class ReportingPeriodActiveSupplier
{
    public ReportingPeriodActiveSupplier(int reportingPeriodSupplierId, SupplierVO supplier,
        int periodId, string periodName, SupplierReportingPeriodStatus? supplierPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive)

    {
        ReportingPeriodSupplierId = reportingPeriodSupplierId;
        Supplier = supplier;
        PeriodId = periodId;
        PeriodName = periodName;
        SupplierPeriodStatus = supplierPeriodStatus;
        InitialDataRequestDate = initialDataRequestDate;
        ResendDataRequestDate = resendDataRequestDate;
        IsActive = isActive;



    }
    public ReportingPeriodActiveSupplier()
    {

    }

    public int ReportingPeriodSupplierId { get; set; }

    public SupplierVO Supplier { get; set; }

    public int PeriodId { get; set; }

    public string PeriodName { get; set; }

    public SupplierReportingPeriodStatus? SupplierPeriodStatus { get; set; }

    public DateTime? InitialDataRequestDate { get; set; }
    public DateTime? ResendDataRequestDate { get; set; }
    public bool IsActive{get; set;}
}
