using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UnitTestMappers.SupplierMappers
{
    public class SupplierDomainDtoMapperUnitTest: BasicTestClass
    {
        [Fact]  //done
        public void ConvertSupplierDomainToDto()
        {
            var supplier = GetSupplierDomain();
            var domainDtoMapper = CreateInstanceOfSupplierDomainToDto();

            var supplierDto = domainDtoMapper.ConvertSupplierDomainToDto(supplier);

            Assert.NotNull(supplierDto);
            Assert.Equal(supplierDto.Id, supplier.Id);
            Assert.Equal(supplierDto.Name, supplier.Name);
            Assert.Equal(supplierDto.Alias, supplier.Alias);
            Assert.Equal(supplierDto.Email, supplier.Email);
            Assert.Equal(supplierDto.ContactNo, supplier.ContactNo);
            Assert.Equal(supplierDto.IsActive, supplier.IsActive);
            Assert.Equal(supplierDto.Contacts.Count(), supplier.Contacts.Count());
            Assert.Equal(supplierDto.Facilities.Count(), supplier.Facilities.Count());
        }

        [Fact]  //done
        public void ConvertContactDomainToDto()
        {
            var supplier = GetSupplierDomain();
            var contact = supplier.AddSupplierContact(0, 1, "TestContact", "test@gmail.com", "+934579348", true);
            var domainDtoMapper = CreateInstanceOfSupplierDomainToDto();
            var contactDto = domainDtoMapper.ConvertContactDomainToDto(supplier.Name, contact);

            Assert.NotNull(contactDto);
            Assert.Equal(contactDto.Id, contact.Id);
            Assert.Equal(contactDto.SupplierId, contact.SupplierId);
            Assert.Equal(contactDto.UserId, contact.UserVO.Id);
            Assert.Equal(contactDto.UserName, contact.UserVO.Name);
            Assert.Equal(contactDto.UserEmail, contact.UserVO.Email);
            Assert.Equal(contactDto.UserContactNo, contact.UserVO.ContactNo);
            Assert.Equal(contactDto.IsActive, contact.UserVO.IsActive);
        }

        [Fact]  //done
        public void ConvertFacilityDomainToDto()
        {
            var supplier = GetSupplierDomain();
            var reportingType = GenerateReportingType().Where(x => x.Id == 1).FirstOrDefault();
            var supplyChainStage = GenerateSupplyChainStage().Where(x => x.Id == 1).FirstOrDefault();

            var facility = supplier.AddSupplierFacility(0, "FacilityDomainToDto", "xyz", true, "123", null, reportingType, supplyChainStage, true);
            var domainDtoMapper = CreateInstanceOfSupplierDomainToDto();

            var facilityDto = domainDtoMapper.ConvertFacilityDomainToDto(supplier.Name, facility);

            Assert.NotNull(facilityDto);
            Assert.Equal(facilityDto.Id, facility.Id);
            Assert.Equal(facilityDto.Name, facility.Name);
            Assert.Equal(facilityDto.Description, facility.Description);
            Assert.Equal(facilityDto.IsPrimary, facility.IsPrimary);
            Assert.Equal(facilityDto.GHGRPFacilityId, facility.GHGHRPFacilityId);
            Assert.Equal(facilityDto.AssociatePipelineId, facility.AssociatePipelines?.Id);
            Assert.Equal(facilityDto.ReportingTypeId, facility.ReportingTypes.Id);
            Assert.Equal(facilityDto.SupplyChainStageId, facility.SupplyChainStages.Id);
            Assert.Equal(facilityDto.IsActive, facility.IsActive);
           
        }
    }
}
