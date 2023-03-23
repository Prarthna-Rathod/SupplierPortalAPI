using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;

public class PeriodSupplier
{
    internal PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;

    }

    internal PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus)
    {
        Id = id;
    }

    internal PeriodSupplier()
    { }

    public int Id { get; private set; }
    public SupplierVO Supplier { get; private set; }
    public int ReportingPeriodId { get; private set; }
    public SupplierReportingPeriodStatus SupplierReportingPeriodStatus { get; private set; }
    public bool IsActive { get; private set; }

    internal void UpdateSupplierReportingPeriodStatus(SupplierReportingPeriodStatus unlockedStatus)
    {
        SupplierReportingPeriodStatus = unlockedStatus;
    }

}
