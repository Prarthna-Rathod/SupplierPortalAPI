using DataAccess.Entities;
using Services.DTOs;
using Services.DTOs.ReadOnlyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers.Interfaces
{
    public interface IReadOnlyEntityToDtoMapper
    {
        SupplierReportingPeriodDTO ConvertReportingPeriodSupplierEntityToSupplierReportingPeriodDTO(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity);

        InternalReportingPeriodDTO ConvertReportingPeriodEntityToInternalPeriodDTO(ReportingPeriodEntity reportingPeriodEntity);

        ReportingPeriodActiveSupplierDTO ConvertReportingPeriodSupplierEntityToReportingPeriodActiveSupplier(ReportingPeriodSupplierEntity reportingPeriodSupplierEntity);

    }
}
