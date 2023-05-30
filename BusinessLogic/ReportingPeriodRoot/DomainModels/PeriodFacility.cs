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
        private HashSet<PeriodFacilityGasSupplyBreakDown> _periodFacilityGasSupplyBreakDown;

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
                if (_periodFacilityElectricityGridMix == null)
                {
                    return new List<PeriodFacilityElectricityGridMix>();
                }
                return _periodFacilityElectricityGridMix;
            }
        }

        public IEnumerable<PeriodFacilityGasSupplyBreakDown> periodFacilityGasSupplyBreakDowns
        {
            get
            {
                if (_periodFacilityGasSupplyBreakDown == null)
                {
                    return new List<PeriodFacilityGasSupplyBreakDown>();
                }
                return _periodFacilityGasSupplyBreakDown;
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
            _periodFacilityElectricityGridMix = new HashSet<PeriodFacilityElectricityGridMix>();
            _periodFacilityGasSupplyBreakDown = new HashSet<PeriodFacilityGasSupplyBreakDown>();
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }



        #region PeriodFacilityElectricityGridMix
        internal IEnumerable<PeriodFacilityElectricityGridMix> AddElectricityGridMix(IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> reportingPeriodFacilityElectricityGridMixVOs, FercRegion fercRegion)
        {

            FercRegion = fercRegion;

            switch (fercRegion.Name)
            {
                case FercRegionvalues.Custom_Mix:
                    {
                        decimal contentValue = 0;
                        _periodFacilityElectricityGridMix.Clear();


                        foreach (var facilityElectricityGridMix in reportingPeriodFacilityElectricityGridMixVOs)
                        {
                            var existingElectricGridMixComponant = _periodFacilityElectricityGridMix.FirstOrDefault(x => x.ElectricityGridMixComponent.Id == facilityElectricityGridMix.ElectricityGridMixComponent.Id);

                            if (existingElectricGridMixComponant != null)
                                throw new Exception("ElectricityGridMix Component is Already Exists!!");


                            var gridmix = new PeriodFacilityElectricityGridMix(Id, facilityElectricityGridMix.ElectricityGridMixComponent, facilityElectricityGridMix.UnitOfMeasure, facilityElectricityGridMix.Content, facilityElectricityGridMix.IsActive);

                            _periodFacilityElectricityGridMix.Add(gridmix);

                            contentValue = contentValue + facilityElectricityGridMix.Content;


                        }
                        if (contentValue != 100)
                        {
                            throw new Exception("Content Value should be 100!!!");
                        }


                    }
                    break;
                default:
                    {
                        if (reportingPeriodFacilityElectricityGridMixVOs.Count() > 0)
                            throw new BadRequestException("FercRegion Should be Custom-Mix for Adding PeriodFacilityElectricityGridMix.");

                        if (_periodFacilityElectricityGridMix.Count() != 0)
                            _periodFacilityElectricityGridMix.Clear();
                    }
                    break;
            }
            return _periodFacilityElectricityGridMix;




        }

        

        internal bool LoadPeriodFacilityElecticGridMix(int reportingPeriodFacilityId, ElectricityGridMixComponent electricityComponent, UnitOfMeasure unitOfMeasure, decimal content, bool isActive)
        {

            var electricityGridMix = new PeriodFacilityElectricityGridMix(reportingPeriodFacilityId, electricityComponent, unitOfMeasure, content, isActive);

            return _periodFacilityElectricityGridMix.Add(electricityGridMix);

        }


        internal bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
        {
            var periodFacilityElectricityGridMixes = _periodFacilityElectricityGridMix.Where(x => x.PeriodFacilityId == periodFacilityId).ToList();

            foreach (var periodFacilityElectricityGridMix in periodFacilityElectricityGridMixes)
            {
                _periodFacilityElectricityGridMix.Remove(periodFacilityElectricityGridMix);
            }

            return true;
        }

        #endregion

        #region PeriodFacilityGasSupplyBreakdown
        internal IEnumerable<PeriodFacilityGasSupplyBreakDown> AddPeriodFacilityGasSupplyBreakDown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> periodFacilityGasSupplyBreakDownVOs)
        {
            if (FacilityVO.SupplyChainStage.Name != SupplyChainStagesValues.Production)
                throw new Exception("SupplychainStage Should be Production !!");

            _periodFacilityGasSupplyBreakDown.Clear();

            foreach(var gasSupplyBreakdown in periodFacilityGasSupplyBreakDownVOs)
            {
                var existingSite = _periodFacilityGasSupplyBreakDown.Any(x => x.Site.Id == gasSupplyBreakdown.Site.Id);
                if (existingSite)
                    throw new BadRequestException("Site is Already Exist in PeriodFacility !!");

                var periodFacilityGasSupplyBreakDown = new PeriodFacilityGasSupplyBreakDown(Id,gasSupplyBreakdown.PeriodFacilityId,gasSupplyBreakdown.UnitOfMeasure,gasSupplyBreakdown.Site,gasSupplyBreakdown.Content);

                _periodFacilityGasSupplyBreakDown.Add(periodFacilityGasSupplyBreakDown);
            }

            return periodFacilityGasSupplyBreakDowns;

            
        }

        internal bool LoadPeriodFacilityGasSupplyBreakdown(int id, int periodFacilityId, Site site,UnitOfMeasure unitOfMeasure, decimal content)
        {
            var gasSupplyBreakdown = new PeriodFacilityGasSupplyBreakDown(id, Id, unitOfMeasure,site, content);

            return _periodFacilityGasSupplyBreakDown.Add(gasSupplyBreakdown);
        }

        internal bool RemovePeriodSupplierGasSupplyBreakdown(int periodFacilityId)
        {
            var periodFacilityGasSupplyBreakdowns = _periodFacilityGasSupplyBreakDown.Where(x => x.PeriodFacilityId == periodFacilityId).ToList();

            foreach (var gasSupply in periodFacilityGasSupplyBreakdowns)
            {
                _periodFacilityGasSupplyBreakDown.Remove(gasSupply);
            }

            return true;
        }
        #endregion

    }
}

