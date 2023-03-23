using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class SupplierReportingPeriod
{
    public int ReportingPeriodSupplierId { get; private set; }

    public SupplierVO SupplierVO { get; private set; }

    public SupplierReportingPeriodStatus SupplierPeriodStatus { get; private set;}

    public bool IsActive { get; private set; }

    public int PeriodId { get; private set; }

    public string PeriodName { get; private set; }

    public DateTime OpenDate { get; private set; }

    public DateTime? CloseDate { get; private set; }

    public string CollectionTimePeriod { get; private set; }

    public bool PeriodIsActive { get; private set; }    

    public ReportingPeriodType PeriodType { get; private set; }

    public ReportingPeriodStatus PeriodStatus { get; private set; }

    public PeriodSupplierDocument SupplementDataDocument { get; private set; }

    public SupplierReportingPeriod(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierPeriodStatus, bool isActive, int periodId, string periodName, DateTime openDate, DateTime? closeDate, string collectionTimePeriod, bool periodIsActive, ReportingPeriodType periodType, ReportingPeriodStatus periodStatus, PeriodSupplierDocument supplementDataDocument)
    {
        ReportingPeriodSupplierId = reportingPeriodSupplierId;
        SupplierVO = supplierVO;
        SupplierPeriodStatus = supplierPeriodStatus;
        IsActive = isActive;
        PeriodId = periodId;
        PeriodName = periodName;
        OpenDate = openDate;
        CloseDate = closeDate;
        CollectionTimePeriod = collectionTimePeriod;
        PeriodIsActive = periodIsActive;
        PeriodType = periodType;
        PeriodStatus = periodStatus;
        SupplementDataDocument = supplementDataDocument;
    }
}
