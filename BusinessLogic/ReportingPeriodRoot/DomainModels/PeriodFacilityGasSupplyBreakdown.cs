using BusinessLogic.ReferenceLookups;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityGasSupplyBreakdown
    {
        public int Id { get; set; }
        public int PeriodFacilityId { get; set; }
        public Site Site { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public decimal Content { get; set; }

        internal PeriodFacilityGasSupplyBreakdown(int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            PeriodFacilityId = periodFacilityId;
            Site = site;
            UnitOfMeasure = unitOfMeasure;
            Content = content;
        }

        internal PeriodFacilityGasSupplyBreakdown(int id, int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content): this(periodFacilityId, site, unitOfMeasure, content)
        {
            Id = id;
        }
    }

}
