using BusinessLogic.ReferenceLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class FacilityRequiredDocumentType
{
    public ReportingType ReportingType { get; set; }
    public SupplyChainStage SupplyChainStage { get; set; }
    public DocumentType DocumentType { get; set; }
    public DocumentRequiredStatus DocumentRequiredStatus { get; set; }

    public FacilityRequiredDocumentType(ReportingType reportingType, SupplyChainStage supplyChainStage, DocumentType documentType, DocumentRequiredStatus documentRequiredStatus)
    {
        ReportingType = reportingType;
        SupplyChainStage = supplyChainStage;
        DocumentType = documentType;
        DocumentRequiredStatus = documentRequiredStatus;
    }
}
