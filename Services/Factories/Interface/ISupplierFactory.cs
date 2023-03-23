using BusinessLogic.SupplierRoot.DomainModels;
using Services.DTOs;

namespace Services.Factories.Interfaces
{
    public interface ISupplierFactory
    {
        Supplier CreateNewSupplier(string name, string alias, string email, string contactNo, bool isActive);    
    }
}
