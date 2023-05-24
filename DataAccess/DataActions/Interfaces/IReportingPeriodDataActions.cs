using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.DataActions.Interfaces
{
    public interface IReportingPeriodDataActions : IDisposable
    {

        #region ReportingPeriod

        bool AddReportingPeriod(ReportingPeriodEntity reportingPeriodEntity);

        bool UpdateReportingPeriod(ReportingPeriodEntity reportingPeriod);

        #endregion

        #region PeriodSupplier

        bool AddPeriodSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity);

        IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSuppliers(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers);

        #endregion

        #region PeriodFacility

        bool AddPeriodFacility(ReportingPeriodFacilityEntity reportingPeriodFacilityEntity, bool facilityIsRelaventForPeriod);

        IEnumerable<ReportingPeriodFacilityEntity> UpdatePeriodFacilities(IEnumerable<ReportingPeriodFacilityEntity> periodFacilityEntities);

        bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId, int periodFacilityDataStatusId);

        #endregion

        #region PeriodFacilityElectricityGridMix

        bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntities, int periodFacilityId, int fercRegionId);

        bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId);

        #endregion

        #region PeriodFacilityGasSupplyBreakDown

        bool AddPeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownEntity> periodFacilityGasSupplyBreakDownEntities, int periodSupplierId);

        bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId);

        #endregion

        #region PeriodFacilityDocument

        bool AddUpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocumentEntity);

        FileStreamResult DownloadFile(string path);

        bool RemovePeriodFacilityDocument(int documentId);

        #endregion

        
        Task<bool> AddReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument);

        Task<bool> UpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument);

        
        #region GetAll Methods

        IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypes();

        IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatus();

        IEnumerable<ReportingPeriodEntity> GetReportingPeriods();

        IEnumerable<ReportingPeriodSupplierEntity> GetPeriodSuppliers();

        IEnumerable<SupplierReportingPeriodStatusEntity> GetSupplierReportingPeriodStatus();

        IEnumerable<FacilityReportingPeriodDataStatusEntity> GetFacilityReportingPeriodDataStatus();

        IEnumerable<DocumentRequiredStatusEntity> GetDocumentRequiredStatus();

        IEnumerable<DocumentStatusEntity> GetDocumentStatusEntities();
        IEnumerable<DocumentTypeEntity> GetDocumentTypeEntities();
        
        IEnumerable<ReportingTypeEntity> GetReportingTypes();

        IEnumerable<ElectricityGridMixComponentEntity> GetElectricityGridMixComponentEntities();

        IEnumerable<UnitOfMeasureEntity> GetUnitOfMeasureEntities();
        IEnumerable<FercRegionEntity> GetFercRegionEntities();
        IEnumerable<SiteEntity> GetSiteEntities();

        IEnumerable<FacilityRequiredDocumentTypeEntity> GetFacilityRequiredDocumentTypeEntities();


        #endregion

        #region GetById Methods

        Task<IEnumerable<ReportingPeriodEntity>> GetReportingPeriods(int ReportingPeriodId);

        ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId);

        ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId);

        ReportingPeriodFacilityEntity GetPeriodFacilityById(int periodFacilityId);

        ReportingPeriodFacilityDocumentEntity GetReportingPeriodFacilityDocumentById(int documentId);

        IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypeById(int reportingPeriodTypeId);

        IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatusById(int reportingPeriodStatusId);

        Task<IEnumerable<ReportingPeriodFacilityEntity>> GetReportingPeriodFacilities(int SupplierId, int ReportingPeriodId);

        Task<IEnumerable<ReportingPeriodSupplierDocumentEntity>> GetReportingPeriodSuppliersDocument(int DocumentId);

        IEnumerable<ReportingPeriodSupplierEntity> GetReportingPeriodSuppliers(int ReportingPeriodId);

        #endregion

    }
}
