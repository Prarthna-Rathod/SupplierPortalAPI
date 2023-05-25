using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        PeriodSupplier AddPeriodSupplier(int periodSupplierId, SupplierVO supplier, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate);

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate, bool isActive);

        PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, FercRegion fercRegion, bool isActive);

        IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents);

        bool LoadElectricityGridMixComponents(int periodFacilityId, int periodSupplierId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents);

        IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        bool LoadPeriodFacilityGasSupplyBreakdown(int periodSupplierId, IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs);

        PeriodFacilityDocument AddUpdatePeriodFacilityDocuments(int supplierId, int periodFacilityId, string displayName, string? path,IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs);

        bool LoadPeriodFacilityDocuments(int documentId, int periodSupplierId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);

       IEnumerable<PeriodFacility> UpdateAllPeriodFacilityDataStatus(int periodSupplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int supplierId, int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus);

        bool RemovePeriodFacilityDocument(int supplierId, int periodFacilityId, int documentId);

        PeriodSupplierDocument AddUpdatePeriodSupplierDocument(int supplierId, string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError);

        bool LoadPeriodSupplierDocuments(int periodSupplierId, int documentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError);

        bool RemovePeriodSupplierDocument(int supplierId, int documentId);
    }
}
