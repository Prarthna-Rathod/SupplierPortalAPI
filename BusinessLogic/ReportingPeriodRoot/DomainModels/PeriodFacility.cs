using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<Facility> _facilities;
        private HashSet<FacilityReportingPeriodDataStatus> _facilityReportingPeriodDataStatuses;
        private HashSet<ReportingType> _reportingTypes;
        //private HashSet<ReportingPeriodSupplier> ReportingPeriodSuppliers;

        public PeriodFacility()
        {
            _facilities = new HashSet<Facility>();
            _facilityReportingPeriodDataStatuses = new HashSet<FacilityReportingPeriodDataStatus>();
            _reportingTypes = new HashSet<ReportingType>();
        }

        public PeriodFacility(int id) : this()
        {
            Id = id;
        }

        public int Id { get; set; }

        public IEnumerable<Facility> Facilities
        {
            get
            {
                if (_facilities == null)
                {
                    return new List<Facility>();
                }
                return _facilities.ToList();
            }
        }

        public IEnumerable<FacilityReportingPeriodDataStatus> FacilityReportingPeriodDataStatus
        {
            get
            {
                if (_facilityReportingPeriodDataStatuses == null)
                {
                    return new List<FacilityReportingPeriodDataStatus>();
                }
                return _facilityReportingPeriodDataStatuses.ToList();
            }
        }

        public IEnumerable<ReportingType> ReportingType
        {
            get
            {
                if (_reportingTypes == null)
                {
                    return new List<ReportingType>();
                }
                return _reportingTypes.ToList();
            }
        }
    }
}
