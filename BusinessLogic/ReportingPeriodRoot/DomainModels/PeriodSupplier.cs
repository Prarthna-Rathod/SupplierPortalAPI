using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;

public class PeriodSupplier
{
    public PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, bool activeForCurrentPeriod, bool initialDataRequest, bool resendInitialDataRequest)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        ActiveForCurrentPeriod = activeForCurrentPeriod;
        InitialDataRequest = initialDataRequest;
        ResendInitialDataRequest = resendInitialDataRequest;


    }

    public PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, bool activeForCurrentPeriod, bool initialDataRequest, bool resendInitialDataRequest) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus, activeForCurrentPeriod, initialDataRequest, resendInitialDataRequest)
    {
        Id = id;
    }

    private PeriodSupplier()
    { }

    public int Id { get; private set; }
    public SupplierVO Supplier { get; private set; }
    public int ReportingPeriodId { get; private set; }
    public SupplierReportingPeriodStatus SupplierReportingPeriodStatus { get; private set; }

    public bool ActiveForCurrentPeriod { get; private set; }

    public bool InitialDataRequest { get; private set; }

    public bool ResendInitialDataRequest { get; private set; }
    public bool IsActive { get; private set; }

    public void UpdateSupplierReportingPeriodStatus(SupplierReportingPeriodStatus supplierReportingPeriodStatus)
    {
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;

    }

}
