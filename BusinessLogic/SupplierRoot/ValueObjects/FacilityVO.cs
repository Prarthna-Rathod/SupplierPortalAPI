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

    public int Id { get; private set; }
    public string FacilityName { get; private set; }
    public int SupplierId { get; private set; }
    public string? GHGRPFacilityId { get; private set; }
    public bool IsActive { get; private set; }
    public SupplyChainStage SupplyChainStage { get; private set; }
    public ReportingType ReportingType { get; private set; }
}
