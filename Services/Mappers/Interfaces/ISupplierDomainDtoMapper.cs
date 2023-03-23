using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using Services.DTOs;

namespace Services.Mappers.Interfaces
{
    public interface ISupplierDomainDtoMapper
    {
        SupplierDto ConvertSupplierDomainToDto(Supplier supplier);
        List<SupplierDto> ConvertSuppliersToDtos(IEnumerable<Supplier> suppliers);
    }
}
