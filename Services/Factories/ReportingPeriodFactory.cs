
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
    public ReportingPeriod CreateNewReportingPeriod(ReportingPeriodType reportingPeriodType,string CollectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool active)
    {
        var type = reportingPeriodType;
        var status= reportingPeriodStatus;

        if(reportingPeriodStatus.Name!= ReportingPeriodStatusValues.InActive)
        {
            throw new Exception("Reporting Period Is InActive");
        }

        if(startDate< DateTime.Now.Date)
        {
            throw new Exception("Date Should be in Future");
        }
        if(endDate != null) 
        {
            if(endDate <= DateTime.Now.Date) 
            {
                throw new Exception("EndDate Should be in Future");
            }          
            if(endDate < startDate) 
            {
                throw new Exception("EndDate should be Greaterthan StartDate");
            }
        }

        var ReportingPeriod = new ReportingPeriod(type,CollectionTimePeriod, status, startDate, endDate, active);
        return ReportingPeriod;
    }
}
