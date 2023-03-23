using Services.DTOs;

namespace Services.Interfaces
{
    public interface ISupplierServices
    {
        /// <summary>
        /// Add Update Supplier
        /// </summary>
        /// <param name="supplierDto"></param>
        /// <returns></returns>
        string AddUpdateSupplier(SupplierDto supplierDto);

        /// <summary>
        /// Get All Supplier
        /// </summary>
        /// <returns></returns>
        IEnumerable<SupplierDto> GetAllSuppliers();

        /// <summary>
        /// Get Supplier By Id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        SupplierDto GetSupplierById(int supplierId);

        /// <summary>
        /// Add Update Supplier Contact
        /// </summary>
        /// <param name="contactDto"></param>
        /// <returns></returns>
        string AddUpdateContact(ContactDto contactDto);

        /// <summary>
        /// Add Supplier Faciltiy
        /// </summary>
        /// <param name="facilityDto"></param>
        /// <returns></returns>
        string AddFacility(FacilityDto facilityDto);
    }
}
