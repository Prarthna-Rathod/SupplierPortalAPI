using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class ReportingPeriodRelevantFacility
{
    public int? ReportingPeriodFacilityId { get; set; }

    public int SupplierId { get; set; }

    public string SupplierName { get; set; }

    public int PeriodId { get; set; }

    public string PeriodName { get; set; }

    public int FacilityId { get; set; }

    public string FacilityName { get;set; }

    public string? GHGRPFacilityId { get; set; }

    public string ReportingType { get; set; }

    public string SupplyChainStage { get;set; }

    public bool FacilityIsRelevantForPeriod { get; set; }

    public bool FacilityPeriodIsActive { get; set; }

    public int SupplierPeriodStatusId { get; set; }

    public string SupplierPeriodStatus { get; set; }    

    public bool SupplierActiveForperiod { get; set; }

    public bool IsFacilityHasDocumentForPeriod { get; set; }

    public int FacilityDataSourceId { get; set; }

    public string? FacilityDataSourceName { get;set; }

    public string FacilityReportingPeriodDataStatus { get; set; }

    public int FacilityReportingPeriodDataStatusId { get; set; }

    public ReportingPeriodRelevantFacility(int? reportingPeriodFacilityId, int supplierId, string supplierName, int periodId, string periodName, int facilityId, string facilityName, string? gHGRPFacilityId, string reportingType, string supplyChainStage, bool facilityIsRelevantForPeriod, bool facilityPeriodIsActive, int supplierPeriodStatusId, string supplierPeriodStatus, bool supplierActiveForperiod, bool isFacilityHasDocumentForPeriod, int facilityDataSourceId, string? facilityDataSourceName, string facilityReportingPeriodDataStatus, int facilityReportingPeriodDataStatusId)
    {
        ReportingPeriodFacilityId = reportingPeriodFacilityId;
        SupplierId = supplierId;
        SupplierName = supplierName;
        PeriodId = periodId;
        PeriodName = periodName;
        FacilityId = facilityId;
        FacilityName = facilityName;
        GHGRPFacilityId = gHGRPFacilityId;
        ReportingType = reportingType;
        SupplyChainStage = supplyChainStage;
        FacilityIsRelevantForPeriod = facilityIsRelevantForPeriod;
        FacilityPeriodIsActive = facilityPeriodIsActive;
        SupplierPeriodStatusId = supplierPeriodStatusId;
        SupplierPeriodStatus = supplierPeriodStatus;
        SupplierActiveForperiod = supplierActiveForperiod;
        IsFacilityHasDocumentForPeriod = isFacilityHasDocumentForPeriod;
        FacilityDataSourceId = facilityDataSourceId;
        FacilityDataSourceName = facilityDataSourceName;
        FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
        FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
    }
}
