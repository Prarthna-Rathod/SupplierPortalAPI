using DataAccess.Entities;

namespace DataAccess.DataActions.Interfaces
{
    public interface ISupplierDataActions : IDisposable
    {
        #region Add Methods
        UserEntity AddUser(UserEntity userEntity);
        bool AddSupplier(SupplierEntity supplier);
        bool AddContact(ContactEntity contact);
        bool AddFacility(FacilityEntity facility);
        AssociatePipelineEntity AddAssociatePipeline(string associatePipelineName);

        #endregion

        #region GetAll Methods
        IEnumerable<UserEntity> GetAllUsers();
        IEnumerable<UserEntity> GetAllUsersByRoleId(int roleId);
        IEnumerable<SupplierEntity> GetAllSuppliers();
        IEnumerable<ContactEntity> GetAllContacts();
        IEnumerable<SupplyChainStageEntity> GetSupplyChainStages();
        IEnumerable<FacilityEntity> GetAllFacilities();
        IEnumerable<AssociatePipelineEntity> GetAllAssociatePipeline();
        IEnumerable<FacilityEntity> GetFacilitiesByReportingType(int reportingTypeId);
        #endregion

        #region GetById Methods
        UserEntity GetUserById(int userId);
        SupplierEntity GetSupplierById(int supplierId);
        ContactEntity GetContactById(int contactId);
        FacilityEntity GetFacilityById(int facilityId);
        AssociatePipelineEntity GetAssociatePipelineById(int associatePipelineId);
        #endregion

        #region Update Methods
        UserEntity UpdateUser(UserEntity user);
        bool UpdateSupplier(SupplierEntity supplier);
        bool UpdateContact(ContactEntity contact);
        bool UpdateFacility(FacilityEntity facility, int facilityId);
        bool UpdateAssociatePipeline(AssociatePipelineEntity associatePipeline, int associatePipelineId);

        #endregion

    }
}
