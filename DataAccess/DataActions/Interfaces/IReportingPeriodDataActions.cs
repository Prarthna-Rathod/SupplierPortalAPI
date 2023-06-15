using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
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
        bool AddPeriodFacilityElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixEntity> periodFacilityElectricityGridMixEntity, int PeriodFacilityId, int fercRegionId);
        bool AddPeriodFacilityGasSupplyBreakdown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakdownEntity> gasSupplyBreakdownEntity, int reportingPeriodSupplierId);
        int AddReportingPeriodFacilityDocument(ReportingPeriodFacilityDocumentEntity reportingPeriodFacilityDocument);
        int AddReportingPeriodSupplierDocument(ReportingPeriodSupplierDocumentEntity reportingPeriodSupplierDocument);
        #endregion

        #region Update Methods
        bool UpdateReportingPeriod(ReportingPeriodEntity reportingPeriod);
        bool UpdateReportingPeriodFacilityDataStatus(int periodFacilityId, int periodFacilityDataStatusId);
        IEnumerable<ReportingPeriodFacilityEntity> UpdatePeriodFacilities(IEnumerable<ReportingPeriodFacilityEntity> periodFacilityEntities);
        IEnumerable<ReportingPeriodSupplierEntity> UpdateReportingPeriodSuppliers(IEnumerable<ReportingPeriodSupplierEntity> periodSuppliers);
        #endregion

        #region Remove Methods

        bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId);

        bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId);

        bool RemovePeriodFacilityDocument(int documentId);

        bool RemovePeriodSupplierDocument(int documentId);

        #endregion

        #region SendMail
        bool SendInitialAndResendDataRequestEmail(int periodSupplierId);
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
        IEnumerable<SiteEntity> GetSiteEntities();
        IEnumerable<EmailTemplateEntity> GetEmailBlueprints();
        IEnumerable<EmailTemplateEntity> GetEmailTemplateBynameCode();
        #endregion

        #region GetById Methods

        ReportingPeriodEntity GetReportingPeriodById(int reportingPeriodId);
        ReportingPeriodSupplierEntity GetPeriodSupplierById(int periodSupplierId);
        IEnumerable<SupplierEntity> GetSuppliers(IEnumerable<int> id);
        ReportingPeriodFacilityEntity GetReportingPeriodFacility(int periodFacilityId);
        ReportingPeriodFacilityDocumentEntity GetReportingPeriodFacilityDocumentById(int documentId);
        ReportingPeriodFacilityEntity GetPeriodFacilityById(int periodFacilityId);
        ReportingPeriodSupplierDocumentEntity GetReportingPeriodSuppliersDocumentById(int documentId);
        #endregion

    }
}
