using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.SupplierRoot.Interfaces
{
    public interface ISupplier
    {
        Facility AddSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive);
        Facility UpdateSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive);
        /* IEnumerable<Facility> UpdateSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive);*/
        Contact AddSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive);
        Contact UpdateSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive);
        void ValidateUserContactNo(string contactNo);

    }
}
