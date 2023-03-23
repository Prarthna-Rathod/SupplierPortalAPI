using BusinessLogic.ReferenceLookups;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.Interfaces;

public interface IReferenceLookUpMapper
{
    IEnumerable<ReportingPeriodType> GetReportingPeriodTypesLookUp(IEnumerable<ReportingPeriodTypeEntity> reportingPeriodTypeEntities);
    
    IEnumerable<ReportingPeriodStatus> GetReportingPeriodStatusesLookUp(IEnumerable<ReportingPeriodStatusEntity> reportingPeriodStatusEntities);
    
    IEnumerable<SupplierReportingPeriodStatus> GetSupplierReportingPeriodStatusesLookUp(IEnumerable<SupplierReportingPeriodStatusEntity> supplierReportingPeriodStatusEntities);


    //Supplier
    IEnumerable<AssociatePipeline> GetAssociatePipelinesLookUp(IEnumerable<AssociatePipelineEntity> associatePipelineEntities);
    IEnumerable<ReportingType> GetReportingTypeLookUp(IEnumerable<ReportingTypeEntity> reportingTypeEntities);
    IEnumerable<SupplyChainStage> GetSupplyChainStagesLookUp(IEnumerable<SupplyChainStageEntity> supplyChainStageEntities);

    IEnumerable<FacilityReportingPeriodDataStatus> GetFacilityReportingPeriodDataStatusLookUp(IEnumerable<FacilityReportingPeriodDataStatusEntity> facilityReportingPeriodDataStatusEntities);

    IEnumerable<ReportingType> GetReportingTypeLookUp(IEnumerable<ReportingTypeEntity> reportingTypeEntities);

}
