using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ReportingPeriodFacilityGridMixAndDocumentDto
    {
        public int ReportingPeriodFacilityId { get; set; }
        public int ReportingPeriodId { get; set; }
        public int SupplierId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public int FercRegionId { get; set; }
        public string FercRegionName { get; set; }
        public IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> ReportingPeriodFacilityElectricityGridMixDtos { get; set; } = new List<ReportingPeriodFacilityElectricityGridMixDto>();
        public IEnumerable<PeriodFacilityDocumentDto> PeriodFacilityDocumentDtos { get; set; } = new List<PeriodFacilityDocumentDto>();

        public ReportingPeriodFacilityGridMixAndDocumentDto(int reportingPeriodFacilityId, int reportingPeriodId, int supplierId, int unitOfMeasureId, string unitOfMeasureName, int fercRegionId, string fercRegionName, IEnumerable<ReportingPeriodFacilityElectricityGridMixDto> reportingPeriodFacilityElectricityGridMixDtos,IEnumerable<PeriodFacilityDocumentDto> periodFacilityDocumentDtos)
        {
            ReportingPeriodFacilityId = reportingPeriodFacilityId;
            ReportingPeriodId = reportingPeriodId;
            SupplierId = supplierId;
            UnitOfMeasureId = unitOfMeasureId;
            UnitOfMeasureName = unitOfMeasureName;
            FercRegionId = fercRegionId;
            FercRegionName = fercRegionName;
            ReportingPeriodFacilityElectricityGridMixDtos = reportingPeriodFacilityElectricityGridMixDtos;
            PeriodFacilityDocumentDtos = periodFacilityDocumentDtos;
        }
    }
}
