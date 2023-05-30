using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        IEnumerable<PeriodSupplier> AddRemovePeriodSupplier(IEnumerable<ReportingPeriodActiveSupplierVO> reportingPeriodActiveSuppliers);

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, int SupplierId, IEnumerable<SupplierVO> supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive);


        
       IEnumerable<PeriodFacility> AddRemoveUpdatePeriodFacility(IEnumerable<ReportingPeriodRelevantFacilityVO> reportingPeriodRelevantFacilityVO, IEnumerable<FercRegion> fercRegion, IEnumerable<FacilityReportingPeriodDataStatus> facilityReportingPeriodDataStatus,PeriodSupplier periodSupplier);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int periodSupplierId, FercRegion fercRegion, bool isActive);

        IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodSupplierId, int periodFacilityId, IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> reportingPeriodFacilityElectricityGridMixVOs, FercRegion fercRegion);

        bool LoadPeriodFacilityElectricityGridMix(int supplierId, int periodfacilityid, ElectricityGridMixComponent electricityGridMixComponent, UnitOfMeasure UnitOfMeasure, decimal Content, bool IsActive);

        public IEnumerable<PeriodFacilityGasSupplyBreakDown> AddPeriodFacilityGasSupplyBreakdown(int ReportingPeriodSupplierId, IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> reportingPeriodFacilityGasSupplyBreakDownVOs);

        bool LoadPeriodFacilityGasSupplyBreakdown(int id, int supplierId, int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content);




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
