using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Factories.Interfaces;
using Services.Interfaces;
using Services.Mappers.Interfaces;

namespace Services
{
    public class SupplierServices : ISupplierServices
    {
        private ISupplierFactory _supplierFactory;
        private readonly ILogger _logger;
        private ISupplierEntityDomainMapper _supplierEntityDomainMapper;
        private ISupplierDomainDtoMapper _supplierDomainDtoMapper;
        private ISupplierDataActions _persister;
        private IReferenceLookUpMapper _referenceLookUpMapper;
        private IReportingPeriodDataActions _reportingPeriodDataActions;

        public SupplierServices(ILoggerFactory loggerFactory, ISupplierFactory supplierFactory,
            ISupplierEntityDomainMapper supplierEntityDomainMapper,
            ISupplierDomainDtoMapper supplierDomainDtoMapper,
            ISupplierDataActions persister, IReferenceLookUpMapper referenceLookUpMapper,
            IReportingPeriodDataActions reportingPeriodDataActions)
        {
            _logger = loggerFactory.CreateLogger<SupplierServices>();
            _supplierFactory = supplierFactory;
            _supplierEntityDomainMapper = supplierEntityDomainMapper;
            _supplierDomainDtoMapper = supplierDomainDtoMapper;
            _persister = persister;
            _referenceLookUpMapper = referenceLookUpMapper;
            _reportingPeriodDataActions = reportingPeriodDataActions;
        }

        #region AddUpdateMethods

        /// <summary>
        /// AddUpdate Supplier
        /// </summary>
        /// <param name="supplierDto"></param>
        /// <returns></returns>
        public string AddUpdateSupplier(SupplierDto supplierDto)
        {
            if (supplierDto.Id == 0)
            {
                var supplier = _supplierFactory.CreateNewSupplier(supplierDto.Name, supplierDto.Alias, supplierDto.Email, supplierDto.ContactNo, supplierDto.IsActive);

                var entity = _supplierEntityDomainMapper.ConvertSupplierDomainToEntity(supplier);
                _persister.AddSupplier(entity);
            }
            else
            {
                //Fetch record by Id
                var supplier = RetrieveAndConvertSupplier(supplierDto.Id ?? 0);
                supplier.UpdateSupplier(supplierDto.Name, supplierDto.Alias, supplierDto.Email, supplierDto.ContactNo, supplierDto.IsActive);
                //Convert Domain to Entity
                var entity = _supplierEntityDomainMapper.ConvertSupplierDomainToEntity(supplier);
                _persister.UpdateSupplier(entity);

            }
            return "Supplier added successfully....";
        }

        private Supplier RetrieveAndConvertSupplier(int supplierId)
        {
            var supplierEntity = _persister.GetSupplierById(supplierId);
            if (supplierEntity == null)
            {
                throw new Exception("Supplier not found !!");
            }
            return ConfigureSupplier(supplierEntity);
        }

        private Supplier ConfigureSupplier(SupplierEntity supplierEntity)
        {
            var reportingTypes = GetAndConvertReportingType();
            var supplyChainStages = GetAndConvertSupplyChainStages();
            var associatePipelines = GetAndConvertAssociatePipelines();

            //Convert Entity to Domain
            var supplier = _supplierEntityDomainMapper.ConvertSupplierEntityToDomain(supplierEntity, reportingTypes, supplyChainStages, associatePipelines);
            return supplier;
        }
        /// <summary>
        /// AddUpdateContact
        /// Supplier should be active for add contacts
        /// Add user details and put foreign key in contact
        /// </summary>
        /// <param name="contactDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string AddUpdateContact(ContactDto contactDto)
        {
            var supplier = RetrieveAndConvertSupplier(contactDto.SupplierId);

            if (contactDto.Id == 0)
            {
                var contact = supplier.AddSupplierContact(contactDto.Id, contactDto.UserId, contactDto.UserName, contactDto.UserEmail, contactDto.UserContactNo, contactDto.IsActive);

                var contactEntity = _supplierEntityDomainMapper.ConvertContactDomainToEntity(contact);
                _persister.AddContact(contactEntity);
            }
            else
            {
                var contact = supplier.UpdateSupplierContact(contactDto.Id, contactDto.UserId, contactDto.UserName, contactDto.UserEmail, contactDto.UserContactNo, contactDto.IsActive);

                var contactEntity = _supplierEntityDomainMapper.ConvertContactDomainToEntity(contact);
                _persister.UpdateContact(contactEntity);

            }

            return "Contact done Successfully";
        }

        /// <summary>
        /// AddFacility
        /// </summary>
        /// <param name="facilityDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string AddFacility(FacilityDto facilityDto)
        {
            AssociatePipeline? associatePipeline = null;
            if (facilityDto.AssociatePipelineId != null)
            {
                if (facilityDto.AssociatePipelineId == 0 && facilityDto.AssociatePipelineName is not null)
                {
                    associatePipeline = new AssociatePipeline(0, facilityDto.AssociatePipelineName);
                }
                else
                {
                    associatePipeline = GetAndConvertAssociatePipelines().FirstOrDefault(x => x.Id == facilityDto.AssociatePipelineId);
                    if (associatePipeline is null) 
                        throw new Exception("Can't found AssociatePipeline !!");
                }
            }

            var supplier = RetrieveAndConvertSupplier(facilityDto.SupplierId);

            var reportingType = GetAndConvertReportingType()
               .Where(x => x.Id == facilityDto.ReportingTypeId).FirstOrDefault();
            var supplyChainStage = GetAndConvertSupplyChainStages()
               .Where(x => x.Id == facilityDto.SupplyChainStageId).FirstOrDefault();

            var facility = supplier.AddSupplierFacility(facilityDto.Id, facilityDto.Name, facilityDto.Description, facilityDto.IsPrimary, facilityDto.GHGRPFacilityId, associatePipeline, reportingType, supplyChainStage, facilityDto.IsActive);

            //Convert Domain to Entity
            var facilityEntity = _supplierEntityDomainMapper.ConvertFacilityDomainToEntity(facility);

            _persister.AddFacility(facilityEntity);

            return "Facility added successfully..";
        }

        /// <summary>
        /// Update Facility
        /// </summary>
        /// <returns></returns>
        public string UpdateFacility(FacilityDto facilityDto)
        {
            var associatePipeline = new AssociatePipeline();

            var reportingType = GetAndConvertReportingType()
               .Where(x => x.Id == facilityDto.ReportingTypeId).FirstOrDefault();
            var supplyChainStage = GetAndConvertSupplyChainStages()
               .Where(x => x.Id == facilityDto.SupplyChainStageId).FirstOrDefault();

            var supplier = RetrieveAndConvertSupplier(facilityDto.SupplierId);
            var facility = supplier.UpdateSupplierFacility(facilityDto.Id, facilityDto.Name, facilityDto.Description, facilityDto.IsPrimary, associatePipeline, reportingType, supplyChainStage);

            var facilityEntity = _supplierEntityDomainMapper.ConvertFacilityDomainToEntity(facility);

            _persister.UpdateFacility(facilityEntity);

            return "Facility updated successfully";
        }
        #endregion

        #region GetAllMethods
        public IEnumerable<SupplierDto> GetAllSuppliers()
        {
            var supplierList = _persister.GetAllSuppliers().Where(x => x.IsActive == true);

            var reportingTypes = GetAndConvertReportingType();
            var supplyChainStages = GetAndConvertSupplyChainStages();
            var associatePipelines = GetAndConvertAssociatePipelines();

            var allSuppliers = _supplierEntityDomainMapper.ConvertSuppliersListEntityToDomain(supplierList, reportingTypes, supplyChainStages, associatePipelines);
            var suppliers = _supplierDomainDtoMapper.ConvertSuppliersToDtos(allSuppliers);
            return suppliers;
        }

        #endregion

        #region GetById Methods
        public SupplierDto GetSupplierById(int supplierId)
        {
            var supplier = RetrieveAndConvertSupplier(supplierId);

            var supplierDto = _supplierDomainDtoMapper.ConvertSupplierDomainToDto(supplier);
            return supplierDto;
        }
        #endregion

        #region Private Methods

        private IEnumerable<ReportingType> GetAndConvertReportingType()
        {
            var reportingTypeEntity = _reportingPeriodDataActions.GetReportingTypes();

            return _referenceLookUpMapper.GetReportingTypeLookUp(reportingTypeEntity);
        }

        private IEnumerable<SupplyChainStage> GetAndConvertSupplyChainStages()
        {
            var supplyChainStageEntity = _persister.GetSupplyChainStages();
            return _referenceLookUpMapper.GetSupplyChainStagesLookUp(supplyChainStageEntity);
        }

        private IEnumerable<AssociatePipeline> GetAndConvertAssociatePipelines()
        {
            var associatePipelineEntity = _persister.GetAllAssociatePipeline();
            return _referenceLookUpMapper.GetAssociatePipelinesLookUp(associatePipelineEntity);
        }

        #endregion
    }
}
