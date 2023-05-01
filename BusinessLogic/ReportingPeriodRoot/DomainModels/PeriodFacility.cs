using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<PeriodFacilityElectricityGridMix> _periodFacilityElectricityGridMix;

        public int Id { get; private set; }
        public FacilityVO FacilityVO { get; private set; }
        public FacilityReportingPeriodDataStatus FacilityReportingPeriodDataStatus { get; private set; }
        public int ReportingPeriodId { get; private set; }
        public int ReportingPeriodSupplierId { get; private set; }
        public bool IsActive { get; private set; }

        public IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes
        {
            get
            {
                if (_periodFacilityElectricityGridMix == null)
                {
                    return new List<PeriodFacilityElectricityGridMix>();
                }
                return _periodFacilityElectricityGridMix;
            }
        }

        internal PeriodFacility() { }

        internal PeriodFacility(FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, bool isActive)
        {
            FacilityVO = facilityVO;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            IsActive = isActive;
            _periodFacilityElectricityGridMix = new HashSet<PeriodFacilityElectricityGridMix>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, isActive)
        {
            Id = id;
        }

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents, bool isActive)
        {
            decimal sum = 0;

            if (fercRegion.Name != FercRegionValues.Custom_Mix)
                throw new BadRequestException("FercRegion should be CustomMix for add electricityGridMix components !!");
            else if (gridMixComponentPercents.Count() == 0)
                throw new BadRequestException("Please add gridMixComponents !!");

            foreach (var facilityGridMix in gridMixComponentPercents)
            {
                var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, fercRegion, facilityGridMix.Content, isActive);

                foreach (var existingComponent in _periodFacilityElectricityGridMix)
                {
                    if (existingComponent.ElectricityGridMixComponent.Id == facilityGridMix.ElectricityGridMixComponent.Id)
                        throw new BadRequestException($"{facilityGridMix.ElectricityGridMixComponent.Name} component is already exists in this periodFacility !!");

                }
                sum = sum + facilityGridMix.Content;

                _periodFacilityElectricityGridMix.Add(periodFacilityGridMixDomain);
            }

            if (sum != 100)
                throw new BadRequestException("Please adjust ContentValues because total should be 100 !!");


            return _periodFacilityElectricityGridMix;
        }

    }
}
