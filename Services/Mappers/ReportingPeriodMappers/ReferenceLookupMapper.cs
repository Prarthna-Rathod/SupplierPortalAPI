using BusinessLogic.ReferenceLookups;
using DataAccess.Entities;
using Services.Mappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.ReportingPeriodMappers
{
    public class ReferenceLookupMapper : IReferenceLookUpMapper
    {
        public IEnumerable<FacilityReportingPeriodDataStatus> GetFacilityReportingPeriodDataStatusLookUp(IEnumerable<FacilityReportingPeriodDataStatusEntity> facilityReportingPeriodDataStatusEntities)
        {
            foreach (var item in facilityReportingPeriodDataStatusEntities)
            {
                yield return new FacilityReportingPeriodDataStatus(item.Id, item.Name);
            }
        }


        public IEnumerable<ReportingPeriodStatus> GetReportingPeriodStatusesLookUp(IEnumerable<ReportingPeriodStatusEntity> reportingPeriodStatusEntities)
        {
            foreach (var item in reportingPeriodStatusEntities)
            {
                yield return new ReportingPeriodStatus(item.Id, item.Name);
            }
        }

        public IEnumerable<ReportingPeriodType> GetReportingPeriodTypesLookUp(IEnumerable<ReportingPeriodTypeEntity> reportingPeriodTypeEntities)
        {
            foreach (var item in reportingPeriodTypeEntities)
            {
                yield return new ReportingPeriodType(item.Id, item.Name);
            }
        }

        public IEnumerable<SupplierReportingPeriodStatus> GetSupplierReportingPeriodStatusesLookUp(IEnumerable<SupplierReportingPeriodStatusEntity> supplierReportingPeriodStatusEntities)
        {
            foreach (var item in supplierReportingPeriodStatusEntities)
            {
                yield return new SupplierReportingPeriodStatus(item.Id, item.Name);
            }
        }

        public IEnumerable<ElectricityGridMixComponent> GetElectricityGridMixComponentsLookUp(IEnumerable<ElectricityGridMixComponentEntity> electricityGridMixComponentEntities)
        {
            foreach (var item in electricityGridMixComponentEntities)
            {
                yield return new ElectricityGridMixComponent(item.Id, item.Name);
            }
        }

        public IEnumerable<UnitOfMeasure> GetUnitOfMeasuresLookUp(IEnumerable<UnitOfMeasureEntity> unitOfMeasureEntities)
        {
            foreach (var item in unitOfMeasureEntities)
            {
                yield return new UnitOfMeasure(item.Id, item.Name);
            }
        }

        public IEnumerable<FercRegion> GetFercRegionsLookUp(IEnumerable<FercRegionEntity> fercRegionsEntities)
        {
            foreach (var item in fercRegionsEntities)
            {
                yield return new FercRegion(item.Id, item.Name);
            }
        }

        public IEnumerable<Site> GetSiteLookUp(IEnumerable<SiteEntity> siteEntities)
        {
            foreach (var item in siteEntities)
            {
                yield return new Site(item.Id, item.Name);
            }
        }

        public IEnumerable<DocumentType> GetDocumentTypesLookUp(IEnumerable<DocumentTypeEntity> documentTypesEntities)
        {
            foreach (var item in documentTypesEntities)
            {
                yield return new DocumentType(item.Id, item.Name);
            }
        }

        public IEnumerable<DocumentStatus> GetDocumentStatusesLookUp(IEnumerable<DocumentStatusEntity> documentStatusesEntities)
        {
            foreach(var item in documentStatusesEntities)
            {
                yield return new DocumentStatus(item.Id, item.Name);
            }
        }

        //SupplierFacility
        public IEnumerable<AssociatePipeline> GetAssociatePipelinesLookUp(IEnumerable<AssociatePipelineEntity> associatePipelineEntities)
        {
            foreach (var item in associatePipelineEntities)
            {
                yield return new AssociatePipeline(item.Id, item.Name);
            }
        }

        public IEnumerable<ReportingType> GetReportingTypeLookUp(IEnumerable<ReportingTypeEntity> reportingTypeEntities)
        {
            foreach (var item in reportingTypeEntities)
            {
                yield return new ReportingType(item.Id, item.Name);
            }
        }

        public IEnumerable<SupplyChainStage> GetSupplyChainStagesLookUp(IEnumerable<SupplyChainStageEntity> supplyChainStageEntities)
        {
            foreach (var item in supplyChainStageEntities)
            {
                yield return new SupplyChainStage(item.Id, item.Name);
            }
        }
    }
}
