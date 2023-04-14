using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.Interfaces;

public interface IReportingPeriodEntityDomainMapper
{
    ReportingPeriodEntity ConvertReportingPeriodDomainToEntity(ReportingPeriod reportingPeriod);

    //PeriodSupplier ConvertPeriodSuppliersEntityToDomain(ReportingPeriod reportingPeriod, ReportingPeriodSupplierEntity reportingPeriodSupplierEntity, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses, SupplierVO supplierVO);

    ReportingPeriod ConvertReportingPeriodEntityToDomain(ReportingPeriodEntity reportingPeriodEntity,IEnumerable<ReportingPeriodType> reportingPeriodTypes, IEnumerable<ReportingPeriodStatus> reportingPeriodStatuses);

    IEnumerable<ReportingPeriodSupplierEntity> ConvertReportingPeriodSuppliersDomainToEntity(IEnumerable<PeriodSupplier> periodSuppliers);
    ReportingPeriodSupplierEntity ConvertReportingPeriodSupplierDomainToEntity(PeriodSupplier periodSupplier);
   /* void ConvertPeriodSupplierEntityToDomain(ReportingPeriod periodDomain, ICollection<ReportingPeriodSupplierEntity> reportingPeriodSupplierEntities, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses);*/
    SupplierVO ConvertSupplierToSupplierValueObject(SupplierEntity supplierEntity,IEnumerable<SupplyChainStage>? supplyChainStages = null,IEnumerable<ReportingType>? reportingTypes = null);

    IEnumerable<SupplierVO> ConvertSupplierEntityToSupplierValueObject(IEnumerable<SupplierEntity> supplierEntities);

    PeriodSupplier ConvertPeriodSuppliersEntityToDomain(ReportingPeriod reportingPeriod, ReportingPeriodSupplierEntity reportingPeriodSupplierEntity, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses, SupplierVO supplierVO);

}
