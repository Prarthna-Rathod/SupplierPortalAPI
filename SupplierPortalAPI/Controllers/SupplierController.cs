using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.DTOs;
using Services.Interfaces;

namespace SupplierPortalAPI.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private ISupplierServices _service;

        public SupplierController(ISupplierServices supplierServices)
        {
            _service = supplierServices;
        }

        #region Add-Update Methods

        [HttpPost("AddUpdateSupplier")]
        public string AddUpdateSupplier(SupplierDto supplierDto)
        {
            return _service.AddUpdateSupplier(supplierDto);
        }

        [HttpPost("AddUpdateContact")]
        public string AddUpdateContact(ContactDto contactDto)
        {
            return _service.AddUpdateContact(contactDto);
        }

        [HttpPost("AddFacility")]
        public string AddFacility(FacilityDto facilityDto) 
        {
            return _service.AddFacility(facilityDto);
        }
        #endregion

        #region Get Methods

        [HttpGet("GetSupplierById")]
        public SupplierDto GetSupplierById(int supplierId) 
        {
            return _service.GetSupplierById(supplierId);
        }

        [HttpGet("GetAllSuppliers")]
        public IEnumerable<SupplierDto> GetAllSuppliers()
        {
            var list = _service.GetAllSuppliers();
            return list;
        }

        #endregion
    }
}
