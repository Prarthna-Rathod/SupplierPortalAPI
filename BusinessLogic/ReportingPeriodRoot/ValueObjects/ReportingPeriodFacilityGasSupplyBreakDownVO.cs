using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class ReportingPeriodFacilityGasSupplyBreakDownVO
    {
        public int Id { get; set; }
        public int PeriodFacilityId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public Site Site { get; set; }
        public decimal Content { get; set; }

        public ReportingPeriodFacilityGasSupplyBreakDownVO()
        {

        }

        public ReportingPeriodFacilityGasSupplyBreakDownVO(int id,int periodFacilityId,UnitOfMeasure unitOfMeasure,Site site,decimal content)
        {
            Id = id;
            PeriodFacilityId = periodFacilityId;
            UnitOfMeasure = unitOfMeasure;
            Site = site;
            Content = content;
        }
    }
}
