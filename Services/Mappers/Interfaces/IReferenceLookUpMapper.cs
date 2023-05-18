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

    IEnumerable<FacilityReportingPeriodDataStatus> GetFacilityReportingPeriodDataStatusLookUp(IEnumerable<FacilityReportingPeriodDataStatusEntity> facilityReportingPeriodDataStatusEntities);

    IEnumerable<ElectricityGridMixComponent> GetElectricityGridMixComponentsLookUp(IEnumerable<ElectricityGridMixComponentEntity> electricityGridMixComponentEntities);

    IEnumerable<UnitOfMeasure> GetUnitOfMeasuresLookUp(IEnumerable<UnitOfMeasureEntity> unitOfMeasureEntities);

    IEnumerable<FercRegion> GetFercRegionsLookUp(IEnumerable<FercRegionEntity> fercRegionsEntities);

    IEnumerable<Site> GetSitesLookUp(IEnumerable<SiteEntity> siteEntities);

    IEnumerable<DocumentStatus> GetDocumentStatusesLookUp(IEnumerable<DocumentStatusEntity> documentStatusEntities);

    IEnumerable<DocumentType> GetDocumentTypesLookUp(IEnumerable<DocumentTypeEntity> documentTypeEntities);

    //Supplier
    IEnumerable<AssociatePipeline> GetAssociatePipelinesLookUp(IEnumerable<AssociatePipelineEntity> associatePipelineEntities);
    IEnumerable<ReportingType> GetReportingTypeLookUp(IEnumerable<ReportingTypeEntity> reportingTypeEntities);
    IEnumerable<SupplyChainStage> GetSupplyChainStagesLookUp(IEnumerable<SupplyChainStageEntity> supplyChainStageEntities);

}
