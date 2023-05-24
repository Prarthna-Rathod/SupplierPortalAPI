using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface IReportingPeriodDataActions : IDisposable
    {

        #region Add Methods

        bool AddReportingPeriod(ReportingPeriodEntity reportingPeriodEntity);

        bool AddRemovePeriodSupplier(IEnumerable<ReportingPeriodSupplierEntity> reportingPeriodSupplierEntity,int id);

        bool AddRemovePeriodFacility(IEnumerable<ReportingPeriodFacilityEntity> reportingPeriodFacilityEntity,int periodSupplierId);

        bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntity, int PeriodFacilityId);

        Task<bool> AddReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument);

        Task<bool> AddReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument);

        #endregion

        #region Update Methods

        bool UpdateReportingPeriod(ReportingPeriodEntity reportingPeriod);

        Task<bool> UpdateReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument);

        Task<bool> UpdateReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument);

        IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSuppliers(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers);

        #endregion

        #region Remove Methods

        bool RemovePeriodFacilityElectricityGridMix(int periodFacilityElectricityGridMixId);

        #endregion

        #region GetAll Methods

        IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypes();

        IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatus();

        IEnumerable<ReportingPeriodEntity> GetReportingPeriods();

        IEnumerable<ReportingPeriodSupplierEntity> GetPeriodSuppliers();

        IEnumerable<SupplierReportingPeriodStatusEntity> GetSupplierReportingPeriodStatus();

        IEnumerable<FacilityReportingPeriodDataStatusEntity> GetFacilityReportingPeriodDataStatus();

        IEnumerable<DocumentRequiredStatusEntity> GetDocumentRequiredStatus();

        IEnumerable<DocumentStatusEntity> GetDocumentStatus();
        IEnumerable<DocumentTypeEntity> GetDocumentType();
        IEnumerable<FacilityRequiredDocumentTypeEntity> GetFacilityRequiredDocumentType();
        IEnumerable<ReportingTypeEntity> GetReportingTypes();

        IEnumerable<ElectricityGridMixComponentEntity> GetElectricityGridMixComponentEntities();

        IEnumerable<UnitOfMeasureEntity> GetUnitOfMeasureEntities();
        IEnumerable<FercRegionEntity> GetFercRegionEntities();
        

        #endregion

        #region GetById Methods

        Task<IEnumerable<ReportingPeriodEntity>> GetReportingPeriods(int ReportingPeriodId);

        ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId);

        ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId);

        IEnumerable<ReportingPeriodTypeEntity> GetReportingPeriodTypeById(int reportingPeriodTypeId);
        
        IEnumerable<ReportingPeriodStatusEntity> GetReportingPeriodStatusById(int reportingPeriodStatusId);

        IEnumerable<SupplierEntity> GetSuppliers(IEnumerable<int> id);

        ReportingPeriodFacilityEntity GetReportingPeriodFacility(int periodFacilityId);

        Task<IEnumerable<ReportingPeriodFacilityEntity>> GetReportingPeriodFacilities(int SupplierId, int ReportingPeriodId);

        Task<IEnumerable<ReportingPeriodFacilityDocumentEntity>> GetReportingPeriodFacilitiesDocument(int DocumentId);

        Task<IEnumerable<ReportingPeriodSupplierDocumentEntity>> GetReportingPeriodSuppliersDocument(int DocumentId);

        IEnumerable<ReportingPeriodSupplierEntity> GetReportingPeriodSuppliers(int ReportingPeriodId);

        #endregion

    }
}
