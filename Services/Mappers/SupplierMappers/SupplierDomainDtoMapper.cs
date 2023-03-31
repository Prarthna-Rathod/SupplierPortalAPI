using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using Services.DTOs;
using Services.Mappers.Interfaces;

namespace Services.Mappers.SupplierMappers
{
    public class SupplierDomainDtoMapper : ISupplierDomainDtoMapper
    {
        public ContactDto ConvertContactDomainToDto(string supplierName, Contact contact)
        {
            return new ContactDto(contact.Id, contact.SupplierId, supplierName, contact.UserVO.Id, contact.UserVO.Name, contact.UserVO.Email, contact.UserVO.ContactNo, contact.UserVO.IsActive);
        }

        public List<ContactDto> ConvertContactsToDto(string supplierName, IEnumerable<Contact> contacts)
        {
            var contactDtos = new List<ContactDto>();
            foreach (var contact in contacts)
            {
                contactDtos.Add(ConvertContactDomainToDto(supplierName, contact));
            }
            return contactDtos;
        }

        public List<FacilityDto> ConvertFacilitiesToDto(string supplierName, IEnumerable<Facility> facilities)
        {
            var facilityDtos = new List<FacilityDto>();
            foreach (var facility in facilities)
            {
                facilityDtos.Add(ConvertFacilityDomainToDto(supplierName, facility));
            }
            return facilityDtos;
        }

        public FacilityDto ConvertFacilityDomainToDto(string supplierName, Facility facility)
        {
            return new FacilityDto(facility.Id, facility.Name, facility.Description, facility.IsPrimary, facility.SupplierId, supplierName, facility.GHGHRPFacilityId, facility.AssociatePipelines?.Id, facility.AssociatePipelines?.Name, facility.ReportingTypes.Id, facility.ReportingTypes.Name, facility.SupplyChainStages.Id, facility.SupplyChainStages.Name, facility.IsActive);
        }

        public SupplierDto ConvertSupplierDomainToDto(Supplier supplier)
        {
            var contactDtos = new List<ContactDto>();
            var facilityDtos = new List<FacilityDto>();
            foreach (var item in supplier.Contacts)
            {
                contactDtos.Add(new ContactDto(item.Id, item.SupplierId, supplier.Name, item.UserVO.Id, item.UserVO.Name, item.UserVO.Email, item.UserVO.ContactNo, item.UserVO.IsActive));
            }
            foreach (var item in supplier.Facilities)
            {   
                facilityDtos.Add(new FacilityDto(item.Id, item.Name, item.Description, item.IsPrimary, item.SupplierId, supplier.Name, item.GHGHRPFacilityId, item.AssociatePipelines?.Id, item.AssociatePipelines?.Name, item.ReportingTypes.Id, item.ReportingTypes.Name, item.SupplyChainStages.Id, item.SupplyChainStages.Name, item.IsActive));
            }
            return new SupplierDto(supplier.Id, supplier.Name, supplier.Alias, supplier.Email, supplier.ContactNo, supplier.IsActive, facilityDtos, contactDtos);
        }

        public List<SupplierDto> ConvertSuppliersToDtos(IEnumerable<Supplier> suppliers)
        {
            var supplierDtos = new List<SupplierDto>();
            foreach (var supplier in suppliers)
            {
                supplierDtos.Add(ConvertSupplierDomainToDto(supplier));
            }
            return supplierDtos;
        }

    }
}
