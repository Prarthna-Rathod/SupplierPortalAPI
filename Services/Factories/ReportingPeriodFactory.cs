using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.ValueConstants;
using Microsoft.Extensions.Logging;
using Services.Factories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.Factories;

public class ReportingPeriodFactory : IReportingPeriodFactory
{
    private readonly ILogger _logger;

    public ReportingPeriodFactory(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ReportingPeriodFactory>();
    }

    public ReportingPeriod CreateNewReportingPeriod(ReportingPeriodType reportingPeriodType,string collectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool isActive)
    {
        if (!isActive)
            throw new Exception("InActive ReportingPeriod cannot be added !!");

        if (string.IsNullOrWhiteSpace(collectionTimePeriod))
            throw new ArgumentNullException("CollectionTimePeriod can't be null !!");

        if (startDate == null)
            throw new ArgumentNullException("StartDate can't be null !!");

        //Check StartDate and EndDate
        if (startDate < DateTime.UtcNow.Date)
        {
            throw new Exception("StartDate should be in future");
        }
        if (endDate != null)
        {
            if (endDate < startDate)
            {
                // 21-4-23 <  22-4-23
                throw new Exception("EndDate should be greater than StartDate");
            }
        }

        //Check ReportingPeriodStatus
        if (reportingPeriodStatus.Name != ReportingPeriodStatusValues.InActive && reportingPeriodStatus.Name != ReportingPeriodStatusValues.Open)
        {
            throw new Exception("ReportingPeriodStatus should be InActive or Open !!");
        }


        var reportingPeriod = new ReportingPeriod(reportingPeriodType, collectionTimePeriod, reportingPeriodStatus, startDate, endDate, isActive);
        return reportingPeriod;
    }
}
