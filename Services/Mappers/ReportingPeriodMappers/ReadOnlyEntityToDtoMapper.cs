using DataAccess.Entities;
using Services.DTOs.ReadOnlyDTOs;
using Services.Mappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.ReportingPeriodMappers
{
    public class ReadOnlyEntityToDtoMapper : IReadOnlyEntityToDtoMapper
    {
        public SupplierReportingPeriodDTO ConvertReportingPeriodSupplierEntityToSupplierReportingPeriodDTO(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
        {
            return new SupplierReportingPeriodDTO(
                reportingPeriodSupplierEntity.Id,
                reportingPeriodSupplierEntity.SupplierId,
                reportingPeriodSupplierEntity.Supplier.Name,
                reportingPeriodSupplierEntity.ReportingPeriodId,
                reportingPeriodSupplierEntity.ReportingPeriod.DisplayName,
                reportingPeriodSupplierEntity.SupplierReportingPeriodStatusId,
                reportingPeriodSupplierEntity.SupplierReportingPeriodStatus.Name
                );
        }

        public InternalReportingPeriodDTO ConvertReportingPeriodEntityToInternalPeriodDTO(ReportingPeriodEntity reportingPeriodEntity)
        {
            return new InternalReportingPeriodDTO(
                    reportingPeriodEntity.Id,
                    reportingPeriodEntity.DisplayName,
                    reportingPeriodEntity.ReportingPeriodTypeId,
                    reportingPeriodEntity.ReportingPeriodType.Name,
                    reportingPeriodEntity.CollectionTimePeriod,
                    reportingPeriodEntity.ReportingPeriodStatusId,
                    reportingPeriodEntity.ReportingPeriodStatus.Name,
                    reportingPeriodEntity.StartDate,
                    reportingPeriodEntity.EndDate
                );
        }

        public ReportingPeriodActiveSupplierDTO ConvertReportingPeriodSupplierEntityToReportingPeriodActiveSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity)
        {
            return new ReportingPeriodActiveSupplierDTO(
                    reportingPeriodSupplierEntity.Id,
                    reportingPeriodSupplierEntity.SupplierId,
                    reportingPeriodSupplierEntity.Supplier.Name,
                    reportingPeriodSupplierEntity.ReportingPeriodId,
                    reportingPeriodSupplierEntity.ReportingPeriod.DisplayName,
                    reportingPeriodSupplierEntity.SupplierReportingPeriodStatusId,
                    reportingPeriodSupplierEntity.SupplierReportingPeriodStatus.Name
                );
        }
    }
}
