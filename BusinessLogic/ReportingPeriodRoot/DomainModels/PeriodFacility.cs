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

        #region PeriodFacilityElecetricityGridMix

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
        {
            switch (fercRegion.Name)
            {
                case FercRegionValues.CustomMix:
                    {
                        if (electricityGridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add PeriodFacilityElectricityGridMixComponents.");

                        _periodFacilityElectricityGridMix.Clear();

                        foreach (var gridMixComponent in electricityGridMixComponentPercents)
                        {
                            var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, fercRegion, gridMixComponent.Content, true);

                            var existingGridMixComponent = _periodFacilityElectricityGridMix.Any(x => x.ElectricityGridMixComponent.Id == gridMixComponent.ElectricityGridMixComponent.Id);

                            if (existingGridMixComponent)
                                throw new BadRequestException("ElectricityGridMixComponent is already exists in this PeriodFacility !!");

                            _periodFacilityElectricityGridMix.Add(periodFacilityElectricityGridMixDomain);
                        }

                        var contentValue = _periodFacilityElectricityGridMix.Sum(x => x.Content);
                        if (contentValue != 100)
                            throw new BadRequestException("ContentValues is not 100 !!");

                    }
                    break;
                default:
                    {
                        if (electricityGridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion is not Custom Mix. Please select fercRegion is Custom Mix then add PeriodFacilityElectricityGridMix.");

                        if (_periodFacilityElectricityGridMix.Count() != 0)
                        {
                            var existingFercRegion = _periodFacilityElectricityGridMix.FirstOrDefault().FercRegion.Id;

                            //If user want to update only fercRegion then need to update that region and clear the periodFacilityElectricityGridMix data
                            if (existingFercRegion != fercRegion.Id && _periodFacilityElectricityGridMix.Count() != 0)
                            {
                                foreach(var item in _periodFacilityElectricityGridMix)
                                {
                                    item.FercRegion.Id = fercRegion.Id;
                                    item.FercRegion.Name = fercRegion.Name;
                                }
                            }
                        }
                    }
                    break;
            }

            return _periodFacilityElectricityGridMix;
        }

        internal bool LoadPeriodFacilityElectricityGridMix(UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
        {
            foreach (var gridMixComponent in electricityGridMixComponentPercents)
            {
                var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(gridMixComponent.Id, Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, fercRegion, gridMixComponent.Content, true);

                _periodFacilityElectricityGridMix.Add(periodFacilityElectricityGridMixDomain);
            }
            return true;
        }

        #endregion
    }
}
