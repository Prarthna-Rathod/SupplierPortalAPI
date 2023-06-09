﻿using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using DataAccess.Entities;
using Services.Mappers.Interfaces;

namespace Services.Mappers.SupplierMappers
{
    public class SupplierEntityDomainMapper : ISupplierEntityDomainMapper
    {
        /// <summary>
        /// Convert ContactDomain to ContactEntity mapper
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert FacilityDomainList to FacilityEntityList mapper
        /// </summary>
        /// <param name="facilities"></param>
        /// <returns></returns>
        public IEnumerable<FacilityEntity> ConvertFacilitiesDomainToEntity(IEnumerable<Facility> facilities)
        {
            var facilityEntities = new List<FacilityEntity>();

            foreach (var facility in facilities)
            {
                facilityEntities.Add(ConvertFacilityDomainToEntity(facility));
            }
            return facilityEntities;
        }

        /// <summary>
        /// Convert FacilityDomain to FacilityEntity mapper
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public FacilityEntity ConvertFacilityDomainToEntity(Facility facility)
        {
            var facilityEntity = new FacilityEntity();
            facilityEntity.Id = facility.Id;
            facilityEntity.Name = facility.Name;
            facilityEntity.Description = facility.Description;
            facilityEntity.IsPrimary = facility.IsPrimary;
            facilityEntity.GhgrpfacilityId = facility.GHGHRPFacilityId;
            facilityEntity.SupplierId = facility.SupplierId;

            if (facility.AssociatePipelines != null)
            {
                if (facility.AssociatePipelines.Id == 0 && facility.AssociatePipelines.Name != null)
                {
                    var associatedPipelineEntity = new AssociatePipelineEntity();
                    associatedPipelineEntity.Id = facility.AssociatePipelines.Id;
                    associatedPipelineEntity.Name = facility.AssociatePipelines.Name;

                    facilityEntity.AssociatePipeline = associatedPipelineEntity;
                    facilityEntity.AssociatePipelineId = associatedPipelineEntity.Id;
                }
                else
                {
                    facilityEntity.AssociatePipelineId = facility.AssociatePipelines.Id;
                   // facilityEntity.AssociatePipeline = facility.AssociatePipelines;
                }
                    
            }
            facilityEntity.ReportingTypeId = facility.ReportingTypes.Id;
            facilityEntity.SupplyChainStageId = facility.SupplyChainStages.Id;
            facilityEntity.IsActive = facility.IsActive;

            return facilityEntity;
        }

        /// <summary>
        /// Convert SupplierDomain to SupplierEntity
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
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
            return entity;
            /*var supplierContacts = new List<ContactEntity>();
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
            entity.FacilityEntities = supplierFacilities;   */

        }

        /// <summary>
        /// Convert SupplierEntity to SupplierDomain mapper
        /// </summary>
        /// <param name="supplierEntity"></param>
        /// <param name="reportingTypes"></param>
        /// <param name="supplyChainStages"></param>
        /// <param name="associatePipelines"></param>
        /// <returns></returns>
        public Supplier ConvertSupplierEntityToDomain(SupplierEntity supplierEntity, IEnumerable<ReportingType> reportingTypes, IEnumerable<SupplyChainStage> supplyChainStages, IEnumerable<AssociatePipeline> associatePipelines)
        {
            var supplier = new Supplier(supplierEntity.Id, supplierEntity.Name, supplierEntity.Alias, supplierEntity.Email, supplierEntity.ContactNo, supplierEntity.IsActive);

            foreach (var contact in supplierEntity.ContactEntities)
            {
                supplier.AddSupplierContact(contact.Id, contact.User.Id, contact.User.Name, contact.User.Email, contact.User.ContactNo, contact.User.IsActive);
            }

            foreach (var facility in supplierEntity.FacilityEntities)
            {
                var reportingType = reportingTypes.Where(x => x.Id == facility.ReportingTypeId).FirstOrDefault();
                var supplyChainStage = supplyChainStages.Where(x => x.Id == facility.SupplyChainStageId).FirstOrDefault();
                var associatePipeline = associatePipelines.Where(x => x.Id == facility.AssociatePipelineId).FirstOrDefault();

                supplier.LoadSupplierFacility(facility.Id, facility.Name, facility.Description, facility.IsPrimary, facility.GhgrpfacilityId, associatePipeline, reportingType, supplyChainStage, facility.IsActive);
            }
            return supplier;
        }

        /// <summary>
        /// Convert SupplierEntityList to SupplierDomainList
        /// </summary>
        /// <param name="supplierEntities"></param>
        /// <param name="reportingTypes"></param>
        /// <param name="supplyChainStages"></param>
        /// <param name="associatePipelines"></param>
        /// <returns></returns>
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
