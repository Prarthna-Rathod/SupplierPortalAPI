﻿using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using System.Text.RegularExpressions;

namespace BusinessLogic.SupplierRoot.DomainModels
{
    public class Supplier : ISupplier
    {
        //
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }

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
                throw new Exception("Name is required");
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
                throw new Exception("Please enter valid ContactNumber !!");
            }
        }

        public Contact AddSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive)
        {
            if (IsActive == true)
            {
                ValidateUserContactNo(contactNo);
                var userVO = new UserVO(userId, userName, email, contactNo, isActive);
                var contact = new Contact(contactId, Id, userVO);
                _contacts.Add(contact);

                return contact;
            }
            else
                throw new Exception("Supplier is not active for add contacts !!");

        }

        public Contact UpdateSupplierContact(int contactId, int userId, string userName, string email, string contactNo, bool isActive)
        {
            var contact = _contacts.FirstOrDefault(x => x.Id == contactId);
            contact.UpdateUser(userName, email, contactNo, isActive);

            return contact;
        }

        public Facility AddSupplierFacility(int faciltiyId, string name, string description, bool isPrimary, string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
        {
            if (IsActive == true)
            {
                CheckFacilityValidations(ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage, isActive);

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


                var facility = new Facility(faciltiyId, name, description, isPrimary, Id, ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage, isActive);

                _facilities.Add(facility);
                return facility;
            }
            else
                throw new Exception("Supplier is not active for add facility !!");
        }

        private void CheckFacilityValidations(string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
        {
            if (reportingType.Name == ReportingTypeValues.NonGHGRP)
            {
                if (ghgrpFacilityId != null)
                {
                    throw new Exception("GHGRP-FacilityId is not allowed !!");
                }
            }

            if (supplyChainStage.Name != SupplyChainStagesValues.TransmissionCompression)
            {
                if (associatePipeline != null)
                {
                    throw new Exception("Associate Pipeline is not allowed !!");
                }
            }

            if(isActive == false)
            {
                throw new Exception("Can't add InActive facility !!");
            }
        }

        public Facility UpdateSupplierFacility(int facilityId, string name, string description, bool isPrimary, AssociatePipeline associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage)
        {
            throw new NotImplementedException();
        }
    }
}
