using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using Services.DTOs;
using Services.Mappers.Interfaces;

namespace Services.Mappers.SupplierMappers
{
    public class SupplierEntityDomainMapper : ISupplierEntityDomainMapper
    {
        public ContactEntity ConvertContactDomainToEntity(Contact contact)
        {
            var user = new UserEntity 
            { 
                Id = contact.UserVO.Id,
                Name = contact.UserVO.Name,
                Email = contact.UserVO.Email,
                ContactNo = contact.UserVO.ContactNo,
                IsActive = contact.UserVO.IsActive,
            };
            return new ContactEntity
            {
                Id = contact.Id,
                SupplierId = contact.SupplierId,
                User = user
            };
        }

        public FacilityEntity ConvertFacilityDomainToEntity(Facility facility)
        {
            return new FacilityEntity
            {
                Id = facility.Id,
                Name = facility.Name,
                Description = facility.Description,
                IsPrimary = facility.IsPrimary,
                GhgrpfacilityId = facility.GHGHRPFacilityId,
                SupplierId = facility.SupplierId,
                AssociatePipelineId = facility.AssociatePipelines.Id,
                ReportingTypeId = facility.ReportingTypes.Id,
                SupplyChainStageId = facility.SupplyChainStages.Id,
                IsActive = facility.IsActive
            };
        }

        public SupplierEntity ConvertSupplierDomainToEntity(Supplier supplier)
        {
            var entity = new SupplierEntity()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Alias = supplier.Alias,
                Email = supplier.Email,
                ContactNo = supplier.ContactNo,
                IsActive = supplier.IsActive
            };

            var supplierContacts = new List<ContactEntity>();
            foreach (var contact in supplier.Contacts)
            {
                var contactEntity = new ContactEntity();
                contactEntity.Id = contact.Id;
                contactEntity.SupplierId = contact.SupplierId;
                contactEntity.UserId = contact.UserVO.Id;

                supplierContacts.Add(contactEntity);
            }

            var supplierFacilities = new List<FacilityEntity>();
            foreach (var facility in supplier.Facilities)
            {
                var facilityEntity = new FacilityEntity();
                facilityEntity.Id = facility.Id;
                facilityEntity.Name = facility.Name;
                facilityEntity.Description = facility.Description;
                facilityEntity.IsPrimary = facility.IsPrimary;                
                facilityEntity.GhgrpfacilityId = facility.GHGHRPFacilityId;
                facilityEntity.SupplierId = facility.SupplierId;
                facilityEntity.AssociatePipelineId = facility.AssociatePipelines.Id;
                facilityEntity.ReportingTypeId = facility.ReportingTypes.Id;
                facilityEntity.SupplyChainStageId = facility.SupplyChainStages.Id;
                facilityEntity.IsActive = facility.IsActive;
                
                supplierFacilities.Add(facilityEntity);
            }

            entity.ContactEntities = supplierContacts;
            entity.FacilityEntities = supplierFacilities;

            return entity;
        }

        public Supplier ConvertSupplierEntityToDomain(SupplierEntity supplierEntity, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<AssociatePipeline> associatePipelines)
        {
            var supplier = new Supplier(supplierEntity.Id, supplierEntity.Name, supplierEntity.Alias, supplierEntity.Email, supplierEntity.ContactNo, supplierEntity.IsActive);

            foreach (var contact in supplierEntity.ContactEntities)
            {
                supplier.AddSupplierContact(contact.Id ,supplier, contact.User.Id, contact.User.Name, contact.User.Email, contact.User.ContactNo, contact.User.IsActive);
            }

            foreach(var facility in supplierEntity.FacilityEntities)
            {
                var reportingType = reportingTypes.Where(x => x.Id == facility.ReportingTypeId).FirstOrDefault();
                var supplyChainStage = supplyChainStages.Where(x => x.Id == facility.SupplyChainStageId).FirstOrDefault();
                var associatePipeline = associatePipelines.Where(x => x.Id == facility.AssociatePipelineId).FirstOrDefault();

                supplier.AddSupplierFacility(facility.Id, facility.Name, facility.Description, facility.IsPrimary, supplier.Id, facility.GhgrpfacilityId, associatePipeline, reportingType, supplyChainStage, facility.IsActive);
            }

            return supplier;

        }

        public IEnumerable<Supplier> ConvertSuppliersListEntityToDomain(IEnumerable<SupplierEntity> supplierEntities, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<AssociatePipeline> associatePipelines)
        {
            var list = new List<Supplier>();
            
            foreach (var entity in supplierEntities)
            {
                list.Add(ConvertSupplierEntityToDomain(entity, reportingTypes, supplyChainStages, associatePipelines));
            }

            return list;
        }
    }
}
