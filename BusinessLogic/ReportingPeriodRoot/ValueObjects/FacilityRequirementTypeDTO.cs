using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class FacilityRequirementTypeDTO
{
    public int Id { get; set; }

    public int ReportingPeriodId { get; set; }

    public int SupplyChainStageId{  get; set; }

    public int DocumentTypeId { get; set; }

    public int DocumentRequirementStatusId { get; set; }

    public FacilityRequirementTypeDTO(int reportingPeriodId, int supplyChainStageId, int documentTypeId, int documentRequirementStatusId)
    {
        ReportingPeriodId = reportingPeriodId;
        SupplyChainStageId = supplyChainStageId;
        DocumentTypeId = documentTypeId;
        DocumentRequirementStatusId = documentRequirementStatusId;
    }
}
