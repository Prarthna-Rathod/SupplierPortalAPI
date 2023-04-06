using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.DTOs;

namespace Services.Mappers.Interfaces
{
    public interface ISupplierEntityDomainMapper
    {
        SupplierEntity ConvertSupplierDomainToEntity(Supplier supplier);
        Supplier ConvertSupplierEntityToDomain(SupplierEntity supplierEntity, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<AssociatePipeline> associatePipelines);
        ContactEntity ConvertContactDomainToEntity(Contact contact);
        IEnumerable<Supplier> ConvertSuppliersListEntityToDomain(IEnumerable<SupplierEntity> supplierEntities, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<AssociatePipeline> associatePipelines);

        IEnumerable<FacilityEntity> ConvertFacilitiesDomainToEntity(IEnumerable<Facility> facilities);
        FacilityEntity ConvertFacilityDomainToEntity(Facility facility);

    }
}
