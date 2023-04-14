
using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Factories.Interface;

public interface IReportingPeriodFactory
{
    ReportingPeriod CreateNewReportingPeriod(ReportingPeriodType reportingPeriodType,string CollectionTimePeriod,ReportingPeriodStatus reportingPeriodStatus,DateTime startDate,DateTime? endDate, bool isActive);
}

