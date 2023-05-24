using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class ReportingPeriodRelevantFacilityVO
{
    public int ReportingPeriodFacilityId { get; set; }
    public int ReportingPeriodId { get; set; }
    public FacilityVO FacilityVO { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int ReportingPeriodSupplierId { get; set; }
    public bool FacilityIsRelevantForPeriod { get; set; }
    public int FacilityReportingPeriodDataStatusId { get; set; }
    public ReportingPeriodRelevantFacilityVO()
    {

    }
    public ReportingPeriodRelevantFacilityVO(int reportingPeriodFacilityId, int reportingPeriodId, FacilityVO facilityVO, int supplierId, string  supplierName, int reportingPeriodSupplierId,bool facilityIsRelevantForPeriod, int facilityReportingPeriodDataStatusId)
    {
        ReportingPeriodFacilityId = reportingPeriodFacilityId;
        ReportingPeriodId = reportingPeriodId;
        FacilityVO = facilityVO;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ReportingPeriodSupplierId = reportingPeriodSupplierId;
        FacilityIsRelevantForPeriod = facilityIsRelevantForPeriod;
        FacilityReportingPeriodDataStatusId = facilityReportingPeriodDataStatusId;
    }
}
