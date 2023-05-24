using BusinessLogic.ReferenceLookups;

namespace BusinessLogic.SupplierRoot.ValueObjects;

public class FacilityVO
{
    public FacilityVO(int id, string facilityName, int supplierId, string? GHGRPfacilityId, bool isActive, SupplyChainStage supplyChainStage, ReportingType reportingType)
    {
        Id = id;
        FacilityName = facilityName;
        SupplierId = supplierId;
        GHGRPFacilityId = GHGRPfacilityId;
        IsActive = isActive;
        SupplyChainStage = supplyChainStage;
        ReportingType = reportingType;

    }

    public int Id { get; set; }
    public string FacilityName { get; set; }
    public int SupplierId { get; set; }
    public string? GHGRPFacilityId { get; set; }
    public bool IsActive { get; private set; }
    public SupplyChainStage SupplyChainStage { get; set; }
    public ReportingType ReportingType { get;  set; }
}
