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
        }

        internal PeriodFacility(int id, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int reportingPeriodSupplierId, FercRegion fercRegion, bool isActive) : this(facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, reportingPeriodSupplierId, fercRegion, isActive)
        {
            Id = id;
        }




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

        internal bool LoadPeriodFacilityElecticGridMix(int id, int reportingPeriodFacilityId, ElectricityGridMixComponent electricityComponent, UnitOfMeasure unitOfMeasure, decimal content, bool isActive)
        {

            var electricityGridMix = new PeriodFacilityElectricityGridMix(reportingPeriodFacilityId, electricityComponent, unitOfMeasure, content, isActive);

            return _periodFacilityElectricityGridMix.Add(electricityGridMix);

        }

    }
}
