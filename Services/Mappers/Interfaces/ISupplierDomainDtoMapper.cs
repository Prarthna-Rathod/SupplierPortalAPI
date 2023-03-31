using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using Services.DTOs;

namespace Services.Mappers.Interfaces
{
    public interface ISupplierDomainDtoMapper
    {
        SupplierDto ConvertSupplierDomainToDto(Supplier supplier);
        List<SupplierDto> ConvertSuppliersToDtos(IEnumerable<Supplier> suppliers);

        List<FacilityDto> ConvertFacilitiesToDto(string supplierName, IEnumerable<Facility> facilities);
        FacilityDto ConvertFacilityDomainToDto(string supplierName, Facility facility);

        List<ContactDto> ConvertContactsToDto(string supplierName, IEnumerable<Contact> contacts);
        ContactDto ConvertContactDomainToDto(string supplierName, Contact contact);
    }
}
