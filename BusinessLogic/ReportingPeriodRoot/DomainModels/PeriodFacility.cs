using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacility
    {
        private HashSet<PeriodFacilityElectricityGridMix> _periodFacilityElectricityGridMixes;
        private HashSet<PeriodFacilityGasSupplyBreakdown> _periodFacilityGasSupplyBreakdowns;

        public int Id { get; private set; }
        public FacilityVO FacilityVO { get; private set; }
        public FacilityReportingPeriodDataStatus FacilityReportingPeriodDataStatus { get; private set; }
        public int ReportingPeriodId { get; private set; }
        public int ReportingPeriodSupplierId { get; private set; }
        public FercRegion FercRegion { get; private set; }
        public bool IsActive { get; private set; }

        public IEnumerable<PeriodFacilityElectricityGridMix> periodFacilityElectricityGridMixes
        {
            get
            {
                if (_periodFacilityElectricityGridMixes == null)
                {
                    return new List<PeriodFacilityElectricityGridMix>();
                }
                return _periodFacilityElectricityGridMixes;
            }
        }

        public IEnumerable<PeriodFacilityGasSupplyBreakdown> PeriodFacilityGasSupplyBreakdowns
        {
            get
            {
                if (_periodFacilityGasSupplyBreakdowns == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakdown>();
                }
                return _periodFacilityGasSupplyBreakdowns.ToList();
            }
        }

        internal PeriodFacility() { }

        internal PeriodFacility(FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId,FercRegion fercRegion, bool isActive)
        {
            FacilityVO = facilityVO;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            FercRegion = fercRegion;
            IsActive = isActive;
            _periodFacilityElectricityGridMixes = new HashSet<PeriodFacilityElectricityGridMix>();
            _periodFacilityGasSupplyBreakdowns = new HashSet<PeriodFacilityGasSupplyBreakdown>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId,FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId,fercRegion, isActive)
        {
            Id = id;
        }

        #region PeriodFacilityElecetricityGridMix

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
        {
            switch (FercRegion.Name)
            {
                case FercRegionValues.CustomMix:
                    {
                        if (electricityGridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add PeriodFacilityElectricityGridMixComponents.");

                        _periodFacilityElectricityGridMixes.Clear();

                        foreach (var gridMixComponent in electricityGridMixComponentPercents)
                        {
                            var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, gridMixComponent.Content);

                            var existingGridMixComponent = _periodFacilityElectricityGridMixes.Any(x => x.ElectricityGridMixComponent.Id == gridMixComponent.ElectricityGridMixComponent.Id);

                            if (existingGridMixComponent)
                                throw new BadRequestException("ElectricityGridMixComponent is already exists in this PeriodFacility !!");

                            _periodFacilityElectricityGridMixes.Add(periodFacilityElectricityGridMixDomain);
                        }

                        var contentValue = _periodFacilityElectricityGridMixes.Sum(x => x.Content);
                        if (contentValue != 100)
                            throw new BadRequestException("ContentValues is not 100 !!");

                    }
                    break;
                default:
                    {
                        if (electricityGridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion is not Custom Mix. Please select fercRegion is Custom Mix then add PeriodFacilityElectricityGridMix.");

                        if (_periodFacilityElectricityGridMixes.Count() != 0)
                            _periodFacilityElectricityGridMixes.Clear();
                    }
                    break;
            }

            return _periodFacilityElectricityGridMixes;
        }

        internal bool LoadPeriodFacilityElectricityGridMix(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
        {
            foreach (var gridMixComponent in electricityGridMixComponentPercents)
            {
                var periodFacilityElectricityGridMixDomain = new PeriodFacilityElectricityGridMix(gridMixComponent.Id, Id, gridMixComponent.ElectricityGridMixComponent, unitOfMeasure, gridMixComponent.Content);

                _periodFacilityElectricityGridMixes.Add(periodFacilityElectricityGridMixDomain);
            }
            return true;
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown

        internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
        {
            _periodFacilityGasSupplyBreakdowns.Clear();

            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new BadRequestException("SupplyChainStage is not Production !!");

            foreach (var gasSupplyBreakdown in gasSupplyBreakdownVOs)
            {
                var periodFacilityGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, gasSupplyBreakdown.Site, gasSupplyBreakdown.UnitOfMeasure, gasSupplyBreakdown.Content);

                var existingSite = _periodFacilityGasSupplyBreakdowns.Any(x => x.Site.Id == gasSupplyBreakdown.Site.Id);
                if (existingSite)
                    throw new BadRequestException($"Site '{gasSupplyBreakdown.Site.Name}' is already exists in same PeriodFacility !!");

                _periodFacilityGasSupplyBreakdowns.Add(periodFacilityGasSupplyBreakdown);
            }

            return _periodFacilityGasSupplyBreakdowns;

        }

        internal bool LoadPeriodFacilityGasSupplyBreakdown(Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            var periodFacilityGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, site, unitOfMeasure, content);

            _periodFacilityGasSupplyBreakdowns.Add(periodFacilityGasSupplyBreakdown);

            return true;
        }

        #endregion

    }
}
