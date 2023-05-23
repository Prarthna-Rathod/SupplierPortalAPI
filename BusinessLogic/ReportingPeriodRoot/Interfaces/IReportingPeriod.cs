using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        PeriodSupplier AddPeriodSupplier(int periodSupplierId, SupplierVO supplier, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate);

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate);

        PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, FercRegion fercRegion, bool isActive);

        IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents);

        bool LoadPeriodFacilityElectricityGridMix(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents);

        IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        PeriodFacilityDocument AddPeriodFacilityDocument(int periodSupplierId, int periodFacilityId, string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs);

        bool LoadPeriodFacilityDocument(int periodFacilityDocumentId, int periodSupplierId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);

        IEnumerable<PeriodFacility> UpdatePeriodFacilityDataStatusCompleteToSubmitted(int periodSupplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int supplierId, int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        /*

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
