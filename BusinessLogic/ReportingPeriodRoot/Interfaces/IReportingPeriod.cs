﻿using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        int Id { get; }
        string DisplayName { get; }
        string CollectionTimePeriod { get; }
        DateTime StartDate { get; }
        DateTime? EndDate { get; }
        bool IsActive { get; }

        ReportingPeriodType ReportingPeriodType { get; }
        ReportingPeriodStatus ReportingPeriodStatus { get;}
        
        IEnumerable<PeriodSupplier> PeriodSuppliers { get; }

        PeriodSupplier AddPeriodSupplier(int periodSupplierId,SupplierVO supplier, SupplierReportingPeriodStatus supplierReportingPeriodStatus,bool activeForCurrentPeriod,bool initialDataRequest,bool resendInitialDataRequest);
        
        bool LoadPeriodSupplier(int reportingPeriodSupplierId,SupplierVO supplierVO,SupplierReportingPeriodStatus supplierReportingPeriodStatus,bool activeForCurrentPeriod,bool initialDataRequest,bool resendInitialDataRequest);

        PeriodFacility AddPeriodFacility(int periodFacilityId,FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus,int periodSupplierId, bool facilityIsRelevantForPeriod);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId);

        PeriodFacility UpdatePeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod);

        /*

        PeriodSupplier UpdatePeriodSupplierStatus(int periodSupplierId, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses);

       // PeriodSupplier RemovePeriodSupplier(int periodSupplierId);
        void AddPeriodFacilityToPeriodSupplier(int supplierId,FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus,ReportingType reportingType,int reportingPeriodSupplierId);

        void AddDocumentToPeriodSupplierFacility(DocumentType documentType,DocumentStatus documentStatus);

        PeriodFacilityDocument RemoveDocumentFromPeriodSupplierFacility(int supplierId,int periodFacilityId,int documentId);

        PeriodFacilityDocument AddDataSubmissionDocumentForReportingPeriod(int supplierId,int periodFacilityId, FacilityRequiredDocumentTypeEntity facilityRequiredDocumentType,IEnumerable<DocumentRequirementStatus> documentRequirementStatus);

        PeriodSupplierDocument AddSupplementalDataDocumentToReportingPeriodSupplier(int supplierId,string documentName,DocumentType documentType,IEnumerable<DocumentStatus> documentStatus);

        PeriodSupplierDocument RemoveSupplementalDataDocumentToReportingPeriodSupplier(int supplierId,int documentId);

        IEnumerable<PeriodFacility> UpdateDataStatusToSubmittedForCompletePeriodFacility(int supplierId,FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);
        */
    }
}
