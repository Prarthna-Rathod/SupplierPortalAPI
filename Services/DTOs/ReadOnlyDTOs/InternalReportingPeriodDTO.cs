using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class InternalReportingPeriodDTO
{
    public InternalReportingPeriodDTO(int reportingPeriodId,string reportingPeriodName,int reportingPeriodTypeId,string reportingPeriodTypeName,string collectionTimePeriod,int reportingPeriodStatusId,string reportingPeriodStatusName,DateTime startDate,DateTime? endDate)
    {
        ReportingPeriodId = reportingPeriodId;
        ReportingPeriodName = reportingPeriodName;
        ReportingPeriodTypeId = reportingPeriodTypeId;
        ReportingPeriodTypeName = reportingPeriodTypeName;
        CollectionTimePeriod = collectionTimePeriod;
        ReportingPeriodStatusId = reportingPeriodStatusId;
        ReportingPeriodStatusName = reportingPeriodStatusName;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int ReportingPeriodId { get; set; }
    public string ReportingPeriodName { get; set; }
    public int ReportingPeriodTypeId { get; set; }
    public string ReportingPeriodTypeName { get; set; }
    public string CollectionTimePeriod { get; set; }
    public int ReportingPeriodStatusId { get; set; }
    public string ReportingPeriodStatusName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

}
