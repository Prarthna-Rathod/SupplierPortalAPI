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

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {

            switch(fercRegion.Name)
            {
                case FercRegionValues.Custom_Mix:
                    {
                        if(gridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add gridMixComponents !!");

                        _periodFacilityElectricityGridMix.Clear();

                        foreach (var facilityGridMix in gridMixComponentPercents)
                        {
                            var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, fercRegion, facilityGridMix.Content, true);

                            var checkExistsComponent = _periodFacilityElectricityGridMix.Any(x => x.ElectricityGridMixComponent.Id == facilityGridMix.ElectricityGridMixComponent.Id);

                            if (checkExistsComponent)
                                throw new BadRequestException($"{facilityGridMix.ElectricityGridMixComponent.Name} component is already exists in this periodFacility !!");

                            _periodFacilityElectricityGridMix.Add(periodFacilityGridMixDomain);
                        }

                        var totalContentValues = _periodFacilityElectricityGridMix.Sum(x => x.Content);
                        if (totalContentValues != 100)
                            throw new BadRequestException("Please adjust ContentValues because total should be 100 !!");
                    }
                    break;
                default:
                    {
                        if(gridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion should be CustomMix for add electricityGridMix components !!");

                        //If user want to update only fercRegion then need to update that region and clear the gridMix data
                        if (_periodFacilityElectricityGridMix.Count() != 0)
                        {
                            var oldFercRegionId = _periodFacilityElectricityGridMix.FirstOrDefault().FercRegion.Id;

                            if (oldFercRegionId != fercRegion.Id && fercRegion.Name != FercRegionValues.Custom_Mix)
                            {
                                foreach(var i in _periodFacilityElectricityGridMix)
                                {
                                    i.FercRegion.Id = fercRegion.Id;
                                    i.FercRegion.Name = fercRegion.Name;
                                }                                
                            }
                        }
                    }
                    break;
            }
            
           
            return _periodFacilityElectricityGridMix;
        }

        internal bool LoadElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            foreach (var facilityGridMix in gridMixComponentPercents)
            {
                var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(facilityGridMix.Id, Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, fercRegion, facilityGridMix.Content, true);

                _periodFacilityElectricityGridMix.Add(periodFacilityGridMixDomain);
            }

            return true;
        }


    }
}
