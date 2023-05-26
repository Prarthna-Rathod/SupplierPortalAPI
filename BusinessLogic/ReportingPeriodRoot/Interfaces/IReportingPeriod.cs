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

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate, bool isActive);

        #endregion

        #region PeriodFacility

        PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, FercRegion fercRegion, bool isActive);

        IEnumerable<PeriodFacility> UpdateAllPeriodFacilityDataStatus(int periodSupplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int supplierId, int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);


        #endregion

        #region ElectricityGridMixes

        IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents);

        bool LoadElectricityGridMixComponents(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents);

        #endregion

        #region GasSupplyBreakdown

        IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        bool LoadPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        #endregion

        #region FacilityDocuments

        PeriodFacilityDocument AddUpdatePeriodFacilityDocuments(int supplierId, int periodFacilityId, string displayName, string? path,IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs);

        bool LoadPeriodFacilityDocuments(int documentId, int periodSupplierId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);
        bool RemovePeriodFacilityDocument(int periodFacilityId, int documentId);

        #endregion

        #region SupplierDocument
        PeriodSupplierDocument AddUpdatePeriodSupplierDocument(int supplierId, string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError);

        bool LoadPeriodSupplierDocuments(int periodSupplierId, int documentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);

        bool RemovePeriodSupplierDocument(int reportingPeriodSupplierId, int documentId);

        #endregion

        #region Delete Methods

        int DeletePeriodFacilityElectricityGridMixes(int periodSupplierId, int periodFacilityId);
        int DeletePeriodSupplierGasSupplyBreakdowns(int periodSupplierId);

        #endregion
    }
}
