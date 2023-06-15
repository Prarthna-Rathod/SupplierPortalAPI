using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using DataAccess.Extensions;
using DataAccess.LoggingFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataActions
{
    public class SupplierDataActionsManager : ISupplierDataActions
    {
        private readonly SupplierPortalDBContext _context;
        private readonly string REPORTING_TYPE_GHGRP = "GHGRP";
        private readonly ISerilog _logger;
        private readonly IHttpContextAccessor _httpContext;

        public SupplierDataActionsManager(ISerilog logger, SupplierPortalDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContext = httpContextAccessor;
        }

        private string CreatedOrUpdatedByUser()
        {
            var email = _httpContext.HttpContext.FindLoginUserEmail();
            return email;
        }

        #region Dispose Methods

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Add Methods

        //User
        public UserEntity AddUser(UserEntity userEntity)
        {
            IsUniqueEmail(userEntity.Email, "User");

            var roles = _context.RoleEntities.Where(x => x.Name == "External").FirstOrDefault();
            userEntity.RoleId = roles.Id;
            userEntity.CreatedOn = DateTime.UtcNow;
            userEntity.CreatedBy = CreatedOrUpdatedByUser();

            _context.UserEntities.Add(userEntity);
            _context.LogEntryAndSaveChanges(_logger);

            return userEntity;
        }

        //Supplier
        public bool AddSupplier(SupplierEntity supplier)
        {
            IsUniqueEmail(supplier.Email, "Supplier");
            supplier.CreatedOn = DateTime.UtcNow;
            supplier.CreatedBy = CreatedOrUpdatedByUser();
            _context.SupplierEntities.Add(supplier);
            _context.LogEntryAndSaveChanges(_logger);
            return true;
        }

        //Contact
        public bool AddContact(ContactEntity contact)
        {
            var user = AddUser(new UserEntity
            {
                Id = contact.UserId,
                Name = contact.User.Name,
                Email = contact.User.Email,
                ContactNo = contact.User.ContactNo,
                IsActive = contact.User.IsActive
            });

            contact.CreatedOn = DateTime.UtcNow;
            contact.CreatedBy = CreatedOrUpdatedByUser();
            contact.User = user;
            contact.UserId = user.Id;

            _context.ContactEntities.Add(contact);
            _context.LogEntryAndSaveChanges(_logger);
            return true;
        }

        //Facility
        public bool AddFacility(FacilityEntity facility)
        {
            if (facility.AssociatePipelineId != null)
            {
                if (facility.AssociatePipelineId == 0 && facility.AssociatePipeline.Name != null)
                {
                    var associatePipeline = AddAssociatePipeline(facility.AssociatePipeline.Name);
                    facility.AssociatePipeline = associatePipeline;
                    facility.AssociatePipelineId = associatePipeline.Id;
                }
            }

            var reportingType = _context.ReportingTypeEntities.FirstOrDefault(x => x.Id == facility.ReportingTypeId);

            if (reportingType?.Name == REPORTING_TYPE_GHGRP)
            {
                var primaryFacility = _context.FacilityEntities.Where(x =>
                    x.ReportingTypeId == reportingType.Id &&
                    x.GhgrpfacilityId == facility.GhgrpfacilityId &&
                    x.SupplierId == facility.SupplierId &&
                    x.IsPrimary &&
                    x.IsActive).FirstOrDefault();

                if (primaryFacility != null)
                {
                    if (facility.IsPrimary == true)
                    {
                        primaryFacility.IsPrimary = false;
                        _context.FacilityEntities.Update(primaryFacility);
                    }
                }
                else
                    facility.IsPrimary = true;
            }
            else
                facility.IsPrimary = true;


            facility.CreatedOn = DateTime.UtcNow;
            facility.CreatedBy = CreatedOrUpdatedByUser();
            _context.FacilityEntities.Add(facility);
            _context.LogEntryAndSaveChanges(_logger);
            return true;
        }

        //AssociatePipeline
        public AssociatePipelineEntity AddAssociatePipeline(string associatePipelineName)
        {
            var associatePipeline = new AssociatePipelineEntity();
            associatePipeline.Name = associatePipelineName;
            associatePipeline.CreatedOn = DateTime.UtcNow;
            associatePipeline.CreatedBy = CreatedOrUpdatedByUser();
            _context.AssociatePipelineEntities.Add(associatePipeline);
            _context.LogEntryAndSaveChanges(_logger);
            return associatePipeline;
        }
        #endregion

        #region GetAll Methods
        public IEnumerable<UserEntity> GetAllUsers()
        {
            var allUsers = _context.UserEntities.ToList();
            return allUsers;
        }

        public IEnumerable<UserEntity> GetAllUsersByRoleId(int roleId)
        {
            var allUsersByRole = _context.UserEntities.Where(x => x.RoleId == roleId).ToList();
            return allUsersByRole;
        }

        public IEnumerable<SupplierEntity> GetAllSuppliers()
        {
            var allSuppliers = _context.SupplierEntities.Include(x => x.ContactEntities)
                                                            .ThenInclude(x => x.User)
                                                        .Include(x => x.FacilityEntities)
                                                        .Include(x => x.ReportingPeriodSupplierEntities)
                                                        .ToList();
            return allSuppliers;
        }

        public IEnumerable<ContactEntity> GetAllContacts()
        {
            var allContacts = _context.ContactEntities
                                        .Include(x => x.Supplier)
                                        .Include(x => x.User)
                                        .ToList();
            return allContacts;
        }
        public IEnumerable<SupplyChainStageEntity> GetSupplyChainStages()
        {
            var supplyChainStages = _context.SupplyChainStageEntities.ToList();
            return supplyChainStages;
        }
        public IEnumerable<FacilityEntity> GetAllFacilities()
        {
            var allFacility = _context.FacilityEntities.Include(x => x.AssociatePipeline)
                                                       .Include(x => x.ReportingType)
                                                       .Include(x => x.SupplyChainStage)
                                                       .Include(x => x.ReportingPeriodFacilityEntities)
                                                       .ToList();
            return allFacility;
        }
        public IEnumerable<FacilityEntity> GetFacilitiesByReportingType(int reportingTypeId)
        {
            var reportingPeriodFacility =
                _context.FacilityEntities.Include(x => x.AssociatePipeline)
                                         .Include(x => x.ReportingType)
                                         .Include(x => x.SupplyChainStage)
                                         .Where(x => x.ReportingTypeId == reportingTypeId)
                                         .ToList();

            return reportingPeriodFacility;
        }
        public IEnumerable<AssociatePipelineEntity> GetAllAssociatePipeline()
        {
            var allAssociatePipelines = _context.AssociatePipelineEntities.ToList();
            return allAssociatePipelines;
        }

        #endregion

        #region GetById Methods

        public UserEntity GetUserById(int userId)
        {
            var user = _context.UserEntities.Where(x => x.Id == userId).FirstOrDefault();

            if (user == null)
                throw new Exception("User not found !");

            return user;
        }

        public UserEntity GetUserByEmailId(string emailId)
        {
            var user = _context.UserEntities.Where(x => x.Email == emailId)
                                            .Include(x => x.Role)
                                            .FirstOrDefault();

            if (user == null)
                throw new Exception("Invalid EmailId !!");

            return user;
        }

        public SupplierEntity GetSupplierById(int supplierId)
        {
            var supplier =
                _context.SupplierEntities.Where(x => x.Id == supplierId)
                                         .Include(x => x.ContactEntities)
                                            .ThenInclude(x => x.User)
                                         .Include(x => x.FacilityEntities)
                                            .ThenInclude(x => x.ReportingPeriodFacilityEntities)
                                         .Include(x => x.ReportingPeriodSupplierEntities)
                                         .FirstOrDefault();
            return supplier;
        }
        public ContactEntity GetContactById(int contactId)
        {
            var contact = _context.ContactEntities.Where(x => x.Id == contactId)
                                                  .Include(x => x.Supplier)
                                                           .Include(x => x.User)
                                                  .FirstOrDefault();
            return contact;
        }
        public FacilityEntity GetFacilityById(int facilityId)
        {
            var facility = _context.FacilityEntities.Include(x => x.AssociatePipeline)
                                                    .Include(x => x.ReportingType)
                                                    .Include(x => x.SupplyChainStage)
                                                    .Include(x => x.ReportingPeriodFacilityEntities)
                                                    .FirstOrDefault(x => x.Id == facilityId);
            return facility;
        }
        public AssociatePipelineEntity GetAssociatePipelineById(int associatePipelineId)
        {
            var associatePipeline = _context.AssociatePipelineEntities.FirstOrDefault(x => x.Id == associatePipelineId);
            return associatePipeline;
        }

        #endregion

        #region Update Methods

        public UserEntity UpdateUser(UserEntity userEntity)
        {
            var entity = _context.UserEntities.FirstOrDefault(x => x.Id == userEntity.Id);

            if (entity == null)
            {
                throw new Exception("User not found !!");
            }

            if (userEntity.Email != entity.Email)
            {
                var isUnique = IsUniqueEmail(userEntity.Email, "User");
                if (isUnique == false)
                {
                    throw new Exception("Email-Id is already exists !!");
                }
                else
                    goto Case;
            }
            else
                goto Case;

            Case:
            {
                entity.Name = userEntity.Name;
                entity.Email = userEntity.Email;
                entity.ContactNo = userEntity.ContactNo;
                entity.RoleId = entity.RoleId;
                entity.IsActive = userEntity.IsActive;
                entity.UpdatedOn = DateTime.UtcNow;
                entity.UpdatedBy = CreatedOrUpdatedByUser();

                _context.UserEntities.Update(entity);
                _context.LogEntryAndSaveChanges(_logger);
                return entity;
            }
        }

        public bool UpdateSupplier(SupplierEntity supplier)
        {
            var supplierEntity = _context.SupplierEntities.Where(x => x.Id == supplier.Id).FirstOrDefault();


            if (supplier.Email != supplierEntity.Email)
            {
                var isUnique = IsUniqueEmail(supplier.Email, "Supplier");
                if (isUnique == false)
                {
                    throw new Exception("Email-Id is already exists !!");
                }
                else
                    goto Case;
            }
            else
                goto Case;

            Case:
            {
                supplierEntity.Name = supplier.Name;
                supplierEntity.Alias = supplier.Alias;
                supplierEntity.Email = supplier.Email;
                supplierEntity.ContactNo = supplier.ContactNo;
                supplierEntity.IsActive = supplier.IsActive;
                supplierEntity.UpdatedOn = DateTime.UtcNow;
                supplierEntity.UpdatedBy = CreatedOrUpdatedByUser();

                _context.SupplierEntities.Update(supplierEntity);
                _context.LogEntryAndSaveChanges(_logger);
                return true;
            }
        }

        public bool UpdateContact(ContactEntity contact)
        {
            var contactEntity = _context.ContactEntities.Where(x => x.Id == contact
            .Id).FirstOrDefault();

            var oldEntity = new ContactEntity();
            oldEntity = contactEntity;

            if (contactEntity == null)
                throw new Exception("Contact not found !!");

            if (contactEntity.SupplierId != contact.SupplierId)
                throw new Exception("Supplier cannot be changed !!");

            //_logger.BeginScope(LogContext.PushProperty("PreviousValue", contactEntity.User));

            var user = UpdateUser(new UserEntity
            {
                Id = contact.User.Id,
                Name = contact.User.Name,
                Email = contact.User.Email,
                ContactNo = contact.User.ContactNo,
                IsActive = contact.User.IsActive
            });

            contactEntity.SupplierId = contact.SupplierId;
            contactEntity.User = user;
            contactEntity.UserId = user.Id;
            contactEntity.UpdatedOn = DateTime.UtcNow;
            contactEntity.UpdatedBy = CreatedOrUpdatedByUser();

            _context.ContactEntities.Update(contactEntity);
            _logger.PushSerilog(_context.ChangeTracker.Entries());
            _context.LogEntryAndSaveChanges(_logger);

            return true;
        }


        public bool UpdateAllFacilities(IEnumerable<FacilityEntity> facilityEntities)
        {
            var supplierId = facilityEntities.First().SupplierId;
            var facilities = _context.FacilityEntities.Where(x => x.SupplierId == supplierId).ToList();
            foreach (var updateFacility in facilityEntities)
            {
                var facilityEntity = facilities
                                .Where(x => x.Id == updateFacility.Id)
                                .FirstOrDefault();

                if (facilityEntity == null)
                {
                    throw new Exception("Facility not found !!");
                }

                if (updateFacility.AssociatePipeline != null)
                {
                    var associatePipeline = AddAssociatePipeline(updateFacility.AssociatePipeline.Name);
                    updateFacility.AssociatePipeline = associatePipeline;
                    updateFacility.AssociatePipelineId = associatePipeline.Id;
                }

                facilityEntity.AssociatePipeline = updateFacility.AssociatePipeline;
                facilityEntity.AssociatePipelineId = updateFacility.AssociatePipelineId;
                facilityEntity.Name = updateFacility.Name;
                facilityEntity.Description = updateFacility.Description;
                facilityEntity.IsPrimary = updateFacility.IsPrimary;
                facilityEntity.GhgrpfacilityId = updateFacility.GhgrpfacilityId;
                facilityEntity.AssociatePipelineId = updateFacility.AssociatePipelineId;
                facilityEntity.ReportingTypeId = updateFacility.ReportingTypeId;
                facilityEntity.SupplyChainStageId = updateFacility.SupplyChainStageId;
                facilityEntity.IsActive = updateFacility.IsActive;
                facilityEntity.UpdatedOn = DateTime.UtcNow;
                facilityEntity.UpdatedBy = CreatedOrUpdatedByUser();
            }

            _context.LogEntryAndSaveChanges(_logger);
            return true;
        }

        public bool UpdateAssociatePipeline(AssociatePipelineEntity associatePipeline, int associatePipelineId)
        {
            var associatePipelineEntity = _context.AssociatePipelineEntities.Where(x => x.Id == associatePipelineId).FirstOrDefault();
            associatePipelineEntity.Name = associatePipeline.Name;
            associatePipelineEntity.UpdatedOn = DateTime.UtcNow;
            associatePipelineEntity.UpdatedBy = CreatedOrUpdatedByUser();

            _context.AssociatePipelineEntities.Update(associatePipelineEntity);
            _context.LogEntryAndSaveChanges(_logger);
            return true;

        }

        #endregion

        #region Private Method
        private bool IsUniqueEmail(string email, string entity)
        {
            if (entity == "User")
            {
                var allEmailId = _context.UserEntities.Where(x => x.Email == email).ToList();

                if (allEmailId.Count() != 0)
                    throw new Exception("Email-Id is already exists !!");
                else
                    return true;
            }

            if (entity == "Supplier")
            {
                var allEmailId = _context.SupplierEntities.Where(x => x.Email == email).ToList();
                if (allEmailId.Count != 0)
                    throw new Exception("Supplier Email-Id is already exists !!");
                else
                    return true;
            }
            return false;
        }

        #endregion
    }
}
