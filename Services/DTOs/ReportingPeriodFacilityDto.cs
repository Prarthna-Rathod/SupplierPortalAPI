using Services.DTOs.ReadOnlyDTOs;

namespace Services.DTOs;

public class ReportingPeriodFacilityDto
{
    public int Id { get; set; }

    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int ReportingPeriodId { get; set; }
    public int ReportingPeriodSupplierId { get; set; }
    public IEnumerable<ReportingPeriodSupplierRelaventFacilityDto> reportingPeriodSupplierRelaventFacilityDtos { get; set; }


    public ReportingPeriodFacilityDto()
    {

    }

    public ReportingPeriodFacilityDto(int id, int supplierId,
        string supplierName, int reportingPeriodId, int reportingPeriodSupplierId)
    {
        Id = id;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ReportingPeriodId = reportingPeriodId;
        ReportingPeriodSupplierId = reportingPeriodSupplierId;


    }
}