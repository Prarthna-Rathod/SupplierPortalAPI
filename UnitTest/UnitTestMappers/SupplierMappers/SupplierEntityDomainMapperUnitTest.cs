using BusinessLogic.SupplierRoot.DomainModels;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.SupplierMappers
{
    public class SupplierEntityDomainMapperUnitTest: BasicTestClass
    {
        [Fact]  //done
        public void ConvertSupplierDomainToEntitySucceed()
        {
            //Arrange
            var supplier = GetSupplierDomain();
            var mapper = CreateInstanceOfSupplierEntityToDomain();

            //Act
            var supplierEntity = mapper.ConvertSupplierDomainToEntity(supplier);

            //Assert
            Assert.NotNull(supplierEntity);
            Assert.Equal(supplier.Name, supplierEntity.Name);
            Assert.Equal(supplier.Alias, supplierEntity.Alias);
            Assert.Equal(supplier.Email, supplierEntity.Email);
            Assert.Equal(supplier.ContactNo, supplierEntity.ContactNo);
            Assert.Equal(supplier.IsActive, supplierEntity.IsActive);
        }

        [Fact]  //done
        public void ConvertSupplierEntityToDomain()
        {
            var supplierEntity = CreateSupplierEntity();
            var reportingTypes = GenerateReportingType();
            var supplyChainStages = GenerateSupplyChainStage();
            var associatePipelines = GenerateAssociatePipelines();
            var mapper = CreateInstanceOfSupplierEntityToDomain();

            //act
            var supplier = mapper.ConvertSupplierEntityToDomain(supplierEntity, reportingTypes, supplyChainStages, associatePipelines);

            Assert.NotNull(supplier);
            Assert.Equal(supplier.Id, supplierEntity.Id);
            Assert.Equal(supplier.Name, supplierEntity.Name);
            Assert.Equal(supplier.Alias, supplierEntity.Alias);
            Assert.Equal(supplier.Email, supplierEntity.Email);
            Assert.Equal(supplier.ContactNo, supplierEntity.ContactNo);
            Assert.Equal(supplier.Contacts.Count(), supplierEntity.ContactEntities.Count());
            Assert.Equal(supplier.Facilities.Count(), supplierEntity.FacilityEntities.Count());
            
        }

        [Fact]  //done
        public void ConvertContactDomainToEntitySucceed()
        {
            int exceptionCounter = 0;
            string? exceptionMessage = null;
            var supplier = GetSupplierDomain();
            var contact = supplier.AddSupplierContact(0, 1, "alexa", "alexa@gmail.com", "+8345934789", true);
            var mapper = CreateInstanceOfSupplierEntityToDomain();
            var result = new ContactEntity();

            try
            {
                result = mapper.ConvertContactDomainToEntity(contact);

            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                exceptionCounter++;
            }

            Assert.Equal(result.User.Name, contact.UserVO.Name);
            Assert.Equal(result.User.Email, contact.UserVO.Email);
            Assert.Equal(result.User.ContactNo, contact.UserVO.ContactNo);
            Assert.Equal(result.User.IsActive, contact.UserVO.IsActive);
            Assert.Equal(result.SupplierId, contact.SupplierId);
            Assert.Equal(0, exceptionCounter);
            Assert.Null(exceptionMessage);

        }

        [Fact]  //done
        public void ConvertFacilityDomainToEntitySucceed()
        {
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            var supplier = GetSupplierDomain();

            var facility = supplier.AddSupplierFacility(0, "facility test", "Test facility", true, "123", null, reportingType, supplyChainStage, true);
            var domainToEntityMapper = CreateInstanceOfSupplierEntityToDomain();

            var facilityEntity = domainToEntityMapper.ConvertFacilityDomainToEntity(facility);

            Assert.NotNull(facilityEntity);
            Assert.Equal(facilityEntity.Id, facility.Id);
            Assert.Equal(facilityEntity.Name, facility.Name);
            Assert.Equal(facilityEntity.Description, facility.Description);
            Assert.Equal(facilityEntity.IsPrimary, facility.IsPrimary);
            Assert.Equal(facilityEntity.GhgrpfacilityId, facility.GHGHRPFacilityId);
            Assert.Equal(facilityEntity.SupplierId, facility.SupplierId);
            Assert.Equal(facilityEntity.AssociatePipelineId, facility.AssociatePipelines?.Id);
            Assert.Equal(facilityEntity.ReportingTypeId, facility.ReportingTypes.Id);
            Assert.Equal(facilityEntity.SupplyChainStageId, facility.SupplyChainStages.Id);
            Assert.Equal(facilityEntity.IsActive, facility.IsActive);
        }

    }
}
