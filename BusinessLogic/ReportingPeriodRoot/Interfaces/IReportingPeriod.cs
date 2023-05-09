using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.Interfaces
{
    public interface IReportingPeriod
    {
        IEnumerable<PeriodSupplier> AddRemovePeriodSupplier(IEnumerable<ReportingPeriodActiveSupplier> reportingPeriodActiveSuppliers);

        bool LoadPeriodSupplier(int reportingPeriodSupplierId, int SupplierId, IEnumerable<SupplierVO> supplierVO, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive);
        PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool facilityIsRelevantForPeriod, bool isActive);

        bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int periodSupplierId, bool isActive);




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
