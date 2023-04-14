using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Text.RegularExpressions;

namespace BusinessLogic.SupplierRoot.DomainModels
{
    public class Supplier : ISupplier
    {
        private HashSet<Contact> _contacts;
        private HashSet<Facility> _facilities;

        public Supplier(string name, string alias, string email,
            string contactNo, bool isActive)
        {
            Name = name;
            Alias = alias;
            Email = email;
            ValidateUserContactNo(contactNo);
            ContactNo = contactNo;
            IsActive = isActive;

            _facilities = new HashSet<Facility>();
            _contacts = new HashSet<Contact>();
        }

        public Supplier(int id, string name, string alias, string email,
            string contactno, bool isActive) : this(name, alias, email, contactno, isActive)
        {
            Id = id;
        }

        private Supplier() { }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Alias { get; private set; }
        public string Email { get; private set; }
        public string ContactNo { get; private set; }
        public bool IsActive { get; private set; }

        public IEnumerable<Facility> Facilities
        {
            get
            {
                if (_facilities == null)
                {
                    return new List<Facility>();
                }
                return _facilities.ToList();
            }
        }

        public IEnumerable<Contact> Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    return new List<Contact>();
                }
                return _contacts.ToList();
            }
        }

        public void UpdateSupplier(string name, string alias, string email, string contactNo, bool isActive)
        {
            UpdateName(name);
            UpdateAlias(alias);
            UpdateEmail(email);
            UpdateContactNo(contactNo);
            UpdateIsActive(isActive);
        }

        private void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new NoContentException("Name is required");
            }
            else
                Name = name;

        }
        private void UpdateAlias(string alias)
        {
            Alias = alias;
        }

        private void UpdateEmail(string email)
        {
            Email = email;
        }

        private void UpdateContactNo(string contactNo)
        {
            ValidateUserContactNo(contactNo);
            ContactNo = contactNo;
        }

        private void UpdateIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void ValidateUserContactNo(string contactNo)
        {
            Regex format = new Regex(@"^[+]{1}(?:[0-9\-\(\)\/\.]\s?){6,15}[0-9]{1}$");
            var isValidate = format.IsMatch(contactNo.ToString());

            if (isValidate == false)
            {
                throw new NoContentException("Please enter valid ContactNumber !!");
            }
        }

        public Contact AddSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive)
        {
            if (IsActive == true)
            {
                ValidateUserContactNo(contactNo);
                var userVO = new UserVO(userId, userName, email, contactNo, isActive);
                var contact = new Contact(contactId, Id, userVO);
                var x = _contacts.Where(x => x.UserVO.Email == email).FirstOrDefault();
                if (x != null)
                    throw new AlreadyExistException("Email is already exists !!");

                _contacts.Add(contact);

                return contact;
            }
            else
                throw new NoContentException("Supplier is not active for add contacts !!");

        }

        public Contact UpdateSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive)
        {
            var contact = _contacts.FirstOrDefault(x => x.Id == contactId);
            if (contact == null)
                throw new NotFoundException("Contact not found !!");

            contact.UpdateUser(userName, email, contactNo, isActive);

            return contact;
        }

        public bool LoadSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
        {
            var facility = new Facility(facilityId, name, description, isPrimary, Id, ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage, isActive);

            return _facilities.Add(facility);
        }

        public Facility AddSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
         {
            if (!IsActive) throw new NoContentException("Supplier is not active for add facility !!");

            if (isActive == false)
                throw new NoContentException("Can't add InActive facility !!");

            CheckFacilityValidations(ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage);

            if (reportingType.Name == ReportingTypeValues.GHGRP)
            {
                //Check IsPrimaryFacility
                var primaryFacility = _facilities.Where(x =>
                    x.ReportingTypes.Name == ReportingTypeValues.GHGRP &&
                    x.GHGHRPFacilityId == ghgrpFacilityId &&
                    x.IsPrimary &&
                    x.IsActive).FirstOrDefault();

                if (primaryFacility != null && isPrimary == true)
                    primaryFacility.IsPrimary = false;
                else
                    isPrimary = true;
            }
            else
                isPrimary = true;


            var facility = new Facility(facilityId, name, description, isPrimary, Id, ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage, isActive);

            _facilities.Add(facility);
            return facility;

        }

        private void CheckFacilityValidations(string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage)
        {
            if (reportingType.Name == ReportingTypeValues.NonGHGRP)
            {
                if (ghgrpFacilityId != null)
                {
                    throw new BadRequestException("GHGRP-FacilityId is not allowed !!");
                }
            }

            if (supplyChainStage.Name != SupplyChainStagesValues.TransmissionCompression)
            {
                if (associatePipeline != null)
                {
                    throw new BadRequestException("Associate Pipeline is not allowed !!");
                }
            }

        }

        private void CheckUpdateFacilityValidations(string? facilityGHGRPId, string? payLoadGHGRPFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage)
        {
            if (reportingType.Name == ReportingTypeValues.GHGRP && facilityGHGRPId != null)
            {
                if (facilityGHGRPId != payLoadGHGRPFacilityId)
                    throw new BadRequestException("GHGRPFacilityId cannot be change !!");
            }

            if (facilityGHGRPId != null && payLoadGHGRPFacilityId == null)
                throw new BadRequestException("GHGRPFacilityId cannot changed to be null !!");

            if (supplyChainStage.Name != SupplyChainStagesValues.TransmissionCompression)
            {
                if (associatePipeline != null)
                {
                    throw new BadRequestException("Associate Pipeline is not allowed !!");
                }
            }

        }

        private Facility FindExistingFacility(string? ghgrpFacilityId)
        {
            var existingFacility = _facilities.Where(x =>
                        x.ReportingTypes.Name == ReportingTypeValues.GHGRP &&
                        x.GHGHRPFacilityId == ghgrpFacilityId &&
                        x.IsPrimary && x.IsActive)
                        .FirstOrDefault();

            return existingFacility;
        }

        public Facility UpdateSupplierFacility(int facilityId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
        {
            var facility = _facilities.FirstOrDefault(x => x.Id == facilityId);
            if (facility == null) throw new NotFoundException("Can't found Facility !!");

            CheckUpdateFacilityValidations(facility.GHGHRPFacilityId, ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage);

            if (!isActive && facility.IsPrimary && facility.ReportingTypes.Name == ReportingTypeValues.GHGRP)
            {
                throw new BadRequestException("Cannot update GHGRP primary facility to InActive !!");
            }

            if (reportingType.Name == ReportingTypeValues.GHGRP && ghgrpFacilityId != null && isPrimary)
            {
                var existingFacility = FindExistingFacility(ghgrpFacilityId);

                if (existingFacility != null)
                {
                    if (isPrimary)
                        existingFacility.UpdateIsPrimary(false);
                }
                else
                    isPrimary = true;
            }
            else
            {
                var existingNonPrimaryFacility = _facilities.Where(x =>
                    x.ReportingTypes.Name == ReportingTypeValues.GHGRP &&
                    x.GHGHRPFacilityId == ghgrpFacilityId &&
                     x.IsActive && x.Id != facilityId)
                    .FirstOrDefault();

                if (ghgrpFacilityId is not null && reportingType.Name == ReportingTypeValues.NonGHGRP)
                {
                    if (existingNonPrimaryFacility != null)
                    {
                        existingNonPrimaryFacility.UpdateIsPrimary(true);
                    }
                }

                if (reportingType.Name == ReportingTypeValues.GHGRP && ghgrpFacilityId != null && !isPrimary && isActive)
                {
                    if (existingNonPrimaryFacility != null)
                        existingNonPrimaryFacility.UpdateIsPrimary(true);
                    else
                        throw new BadRequestException("You cannot change IsPrimary false because no other facility is available for set it true !!");
                }
                    
            }

            if (reportingType.Name == ReportingTypeValues.GHGRP && facility.GHGHRPFacilityId == null && ghgrpFacilityId != null)
            {
                facility.UpdateGhgrpFacilityId(ghgrpFacilityId);
            }

            facility.UpdateName(name);
            facility.UpdateDescription(description);
            facility.UpdateReportingType(reportingType);
            facility.UpdateIsPrimary(isPrimary);
            facility.UpdateAssociatePipeline(associatePipeline);
            facility.UpdateSupplyChainStage(supplyChainStage);
            facility.UpdateIsActive(isActive);

            return facility;
           

        }
    }
}
