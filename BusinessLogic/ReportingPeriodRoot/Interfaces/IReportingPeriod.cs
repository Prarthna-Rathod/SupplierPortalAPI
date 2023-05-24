using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        #region PeriodSupplier

        PeriodSupplier AddPeriodSupplier(int periodSupplierId, SupplierVO supplier, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate);

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate);

        #endregion

        #region PeriodFacility

        PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, FercRegion fercRegion, bool isActive);

        #endregion

        #region PeriodFacilityElectricityGridMix

        IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents);

        bool LoadPeriodFacilityElectricityGridMix(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents);

        #endregion

        #region PeriodFacilityGasSupplyBreakDown

        IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        bool LoadPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        #endregion

        #region PeriodFacilityDocument

        PeriodFacilityDocument AddPeriodFacilityDocument(int periodSupplierId, int periodFacilityId, string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs);

        bool LoadPeriodFacilityDocument(int periodFacilityDocumentId, int periodSupplierId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);

        bool RemovePeriodFacilityDocument(int periodSupplierId, int periodFacilityId, int periodFacilityDocumentId);

        #endregion

        #region UpdatePeriodFacilityDataStatuses

        IEnumerable<PeriodFacility> UpdatePeriodFacilityDataStatusCompleteToSubmitted(int periodSupplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int supplierId, int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        #endregion


        /*


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
