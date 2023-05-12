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
        private HashSet<PeriodFacilityGasSupplyBreakdown> _periodSupplierGasSupplyBreakdowns;
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

        public IEnumerable<PeriodFacilityGasSupplyBreakdown> periodFacilityGasSupplyBreakdowns
        {
            get
            {
                if (_periodSupplierGasSupplyBreakdowns == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakdown>();
                }
                return _periodSupplierGasSupplyBreakdowns;
            }
        }

        internal PeriodFacility() { }

        internal PeriodFacility(FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive)
        {
            FacilityVO = facilityVO;
            FacilityReportingPeriodDataStatus = facilityReportingPeriodDataStatus;
            ReportingPeriodId = reportingPeriodId;
            ReportingPeriodSupplierId = reportingPeriodSupplierId;
            FercRegion = fercRegion;
            IsActive = isActive;
            _periodFacilityElectricityGridMixes = new HashSet<PeriodFacilityElectricityGridMix>();
            _periodSupplierGasSupplyBreakdowns = new HashSet<PeriodFacilityGasSupplyBreakdown>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }

        internal IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            switch (FercRegion.Name)
            {
                case FercRegionValues.Custom_Mix:
                    {
                        if (gridMixComponentPercents.Count() == 0)
                            throw new BadRequestException("Please add gridMixComponents !!");

                        _periodFacilityElectricityGridMixes.Clear();

                        foreach (var facilityGridMix in gridMixComponentPercents)
                        {
                            var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, facilityGridMix.Content);

                            var checkExistsComponent = _periodFacilityElectricityGridMixes.Any(x => x.ElectricityGridMixComponent.Id == facilityGridMix.ElectricityGridMixComponent.Id);

                            if (checkExistsComponent)
                                throw new BadRequestException($"{facilityGridMix.ElectricityGridMixComponent.Name} component is already exists in this periodFacility !!");

                            _periodFacilityElectricityGridMixes.Add(periodFacilityGridMixDomain);
                        }

                        var totalContentValues = _periodFacilityElectricityGridMixes.Sum(x => x.Content);
                        if (totalContentValues != 100)
                            throw new BadRequestException("Please adjust ContentValues because total should be 100 !!");
                    }
                    break;
                default:
                    {
                        if (gridMixComponentPercents.Count() > 0)
                            throw new BadRequestException("FercRegion should be CustomMix for add electricityGridMix components !!");

                        //If user want to update only fercRegion then need to update that region and clear the gridMix data
                        if (_periodFacilityElectricityGridMixes.Count() != 0)
                            _periodFacilityElectricityGridMixes.Clear();
                            
                    }
                    break;
            }

            return _periodFacilityElectricityGridMixes;
        }

        internal bool LoadElectricityGridMixComponents(UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
        {
            foreach (var facilityGridMix in gridMixComponentPercents)
            {
                var periodFacilityGridMixDomain = new PeriodFacilityElectricityGridMix(facilityGridMix.Id, Id, facilityGridMix.ElectricityGridMixComponent, unitOfMeasure, facilityGridMix.Content);

                _periodFacilityElectricityGridMixes.Add(periodFacilityGridMixDomain);
            }

            return true;
        }

        #region GasSupplyBreakdown

        internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> facilityDataVos)
        {
            _periodSupplierGasSupplyBreakdowns.Clear();

            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new BadRequestException("Facility SupplyChainStage is not Production !!");

            foreach (var singleVo in facilityDataVos)
            {
                var periodSupplierGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, singleVo.Site, singleVo.UnitOfMeasure, singleVo.Content);

                var isSiteExists = _periodSupplierGasSupplyBreakdowns.Any(x => x.Site.Id == singleVo.Site.Id);

                if (isSiteExists)
                    throw new Exception($"Duplicate Site '{singleVo.Site.Name}' exists in same facility !!");

                _periodSupplierGasSupplyBreakdowns.Add(periodSupplierGasSupplyBreakdown);
            }

            return _periodSupplierGasSupplyBreakdowns;
        }

        internal PeriodFacilityGasSupplyBreakdown LoadPeriodFacilityGasSupplyBreakdowns(Site site, UnitOfMeasure unitOfMeasure, decimal content)
        {
            var periodSupplierGasSupplyBreakdown = new PeriodFacilityGasSupplyBreakdown(Id, site, unitOfMeasure, content);

            _periodSupplierGasSupplyBreakdowns.Add(periodSupplierGasSupplyBreakdown);

            return periodSupplierGasSupplyBreakdown;
        }


        #endregion
    }
}
