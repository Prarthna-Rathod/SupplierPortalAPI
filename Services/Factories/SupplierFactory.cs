using BusinessLogic.SupplierRoot.DomainModels;
using Services.DTOs;
using Services.Factories.Interfaces;

namespace Services.Factories
{
    public class SupplierFactory : ISupplierFactory
    {
        public Supplier CreateNewSupplier(string name, string alias, string email, string contactNo, bool isActive)
        {
            var supplier = new Supplier(name, alias, email, contactNo, isActive);
            return supplier;
        }
    }
}
