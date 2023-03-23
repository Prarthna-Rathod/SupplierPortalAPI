using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class ReportingPeriodRelevantFacilityBaseDTO : ReportingPeriodRelevantFacilityDTO
{
    public ReportingPeriodRelevantFacilityBaseDTO(int id, int facilityId, string facility, int facilityReportingPeriodDataStatusId, string facilityReportingPeriodDataStatus, int reportingTypeId, string reportingType, int reportingPeriodSupplierId, string reportingPeriodSupplier) : base(id, facilityId, facility, facilityReportingPeriodDataStatusId, facilityReportingPeriodDataStatus, reportingTypeId, reportingType, reportingPeriodSupplierId, reportingPeriodSupplier)
    {
    }
}
