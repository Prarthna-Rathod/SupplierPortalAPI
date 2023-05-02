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

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents, bool isActive)
        {
            decimal sum = 0;

            if (fercRegion.Name != FercRegionValues.CustomMix)
                throw new BadRequestException("FercRegion is not Custom Mix. Please select fercRegion is Custom Mix then add PeriodFacilityElectricityGridMix.");
            else if (electricityGridMixComponentPercents.Count() == 0)
                throw new BadRequestException("Please add gridMixComponents");

            foreach (var gridMixComponent in electricityGridMixComponentPercents)
            {
                var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, fercRegion, gridMixComponent.Content, isActive);
                foreach (var existingGridMixComponent in _periodFacilityElectricityGridMix)
                {
                    if (existingGridMixComponent.ElectricityGridMixComponent.Id == gridMixComponent.ElectricityGridMixComponent.Id)
                        throw new BadRequestException("ElectricityGridMixComponent is already exists in this PeriodFacility !!");

                }
                sum = sum + gridMixComponent.Content;
                _periodFacilityElectricityGridMix.Add(periodFacilityElectricityGridMixDomain);
            }

            if (sum != 100)
                throw new BadRequestException("ContentValues is not 100. Please add more content values.");

            return _periodFacilityElectricityGridMix;
        }

    }
}
