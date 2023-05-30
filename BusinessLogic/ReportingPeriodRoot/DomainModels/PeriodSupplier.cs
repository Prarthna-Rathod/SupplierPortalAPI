using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;


public class PeriodSupplier
{
    private HashSet<PeriodFacility> _periodfacilities;

    internal PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        InitialDataRequestDate = initialDataRequestDate;
        ResendDataRequestDate = resendDataRequestDate;
        IsActive = isActive;
        _periodfacilities = new HashSet<PeriodFacility>();
    }

    internal PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate, isActive)
    {
        Id = id;
    }

    internal PeriodSupplier()
    { }

    public int Id { get; private set; }
    public SupplierVO Supplier { get; private set; }
    public int ReportingPeriodId { get; private set; }
    public SupplierReportingPeriodStatus SupplierReportingPeriodStatus { get; private set; }
    public DateTime? InitialDataRequestDate { get; private set; }
    public DateTime? ResendDataRequestDate { get; private set; }
    public bool IsActive { get; private set; }

    public IEnumerable<PeriodFacility> PeriodFacilities
    {
        get
        {
            if (_periodfacilities == null)
            {
                return new List<PeriodFacility>();
            }
            return _periodfacilities.ToList();
        }
    }

    internal void UpdateSupplierReportingPeriodStatus(SupplierReportingPeriodStatus supplierReportingPeriodStatus)
    {
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;

    }

    private PeriodFacility GetPeriodFacility(int periodFacilityId)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("PeriodFacility is not found !!");

        return periodFacility;
    }

    #region Period Facility

    internal string AddRemoveUpdatePeriodFacility(IEnumerable<ReportingPeriodRelevantFacilityVO> reportingPeriodRelevantFacilities, IEnumerable<FercRegion> fercRegions, IEnumerable<FacilityReportingPeriodDataStatus> facilityReportingPeriodDataStatuses)
    {
        var checkFacility = "";

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("Supplier ReportingPeriodStatus is Locked..");


        var fercRegion = fercRegions.FirstOrDefault(x => x.Name == FercRegionvalues.None);

        var facilityReportingPeriodDataStatus = facilityReportingPeriodDataStatuses.FirstOrDefault(x => x.Name == FacilityReportingPeriodDataStatusValues.InProgress);

        foreach (var facility in reportingPeriodRelevantFacilities)
        {
            var isExist = _periodfacilities.FirstOrDefault(x => x.FacilityVO.Id == facility.FacilityVO.Id);

            if (facility.FacilityIsRelevantForPeriod && facility.ReportingPeriodFacilityId == 0)
            {
                if (isExist is null)
                {
                    var periodFacility = new PeriodFacility(facility.FacilityVO, facilityReportingPeriodDataStatus, facility.ReportingPeriodId, facility.ReportingPeriodSupplierId, fercRegion, facility.FacilityIsRelevantForPeriod);

                    _periodfacilities.Add(periodFacility);
                }
                else
                {
                    checkFacility = checkFacility + "," + $"{facility.FacilityVO.Id}";
                }

            }
            if (isExist is not null)
            {
                if (!facility.FacilityIsRelevantForPeriod)
                {
                    _periodfacilities.Remove(isExist);
                }
                if (facility.FacilityIsRelevantForPeriod)
                {
                    isExist.FacilityVO.ReportingType = facility.FacilityVO.ReportingType;
                }

            }

        }


        return "This facility is not performed Successfull.." + checkFacility;
    }


    internal bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int periodSupplierId, FercRegion fercRegion, bool isActive)
    {
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, periodSupplierId, fercRegion, isActive);

        return _periodfacilities.Add(periodFacility);
    }





    #endregion

    #region PeriodFacilityElectricityGridMix

    internal IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodFacilityId, IEnumerable<ReportingPeriodFacilityElectricityGridMixVO> reportingPeriodFacilityElectricityGridMixVOs, FercRegion fercRegion)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("Supplier Should be Unlocked");

        var periodFacility = _periodfacilities.Where(x => x.Id == periodFacilityId).FirstOrDefault();

        return periodFacility.AddElectricityGridMix(reportingPeriodFacilityElectricityGridMixVOs, fercRegion);

    }

    internal bool LoadPeriodFacilityElectricityGridMix(int supplierId, int reportingPeriodFacilityId, ElectricityGridMixComponent electricityComponent, UnitOfMeasure unitOfMeasure, decimal content, bool isActive)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == reportingPeriodFacilityId);

        return periodFacility.LoadPeriodFacilityElecticGridMix(reportingPeriodFacilityId, electricityComponent, unitOfMeasure, content, isActive);
    }

    internal bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked) 
            throw new Exception("Supplier Should be Unlocked");

        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.RemovePeriodFacilityElectricityGridMix(periodFacilityId);

    }

    #endregion

    #region PeriodFacilityGasSupplyBreakDown
    internal IEnumerable<PeriodFacilityGasSupplyBreakDown> AddPeriodFacilityGasSupplyBreakDown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> reportingPeriodFacilityGasSupplyBreakDownVOs)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("Supplier should be Unlocked !!");

        var list = new List<PeriodFacilityGasSupplyBreakDown>();

        var sites = reportingPeriodFacilityGasSupplyBreakDownVOs.GroupBy(x => x.Site.Id);

        foreach(var site in sites)
        {
            var siteName = site.Select(x=>x.Site.Name).FirstOrDefault();
            var contentValue = site.Sum(x=>x.Content);

            if (contentValue != 100)
                throw new Exception($"Add more contentValues to site {siteName}");
        }

        var periodFaciities = reportingPeriodFacilityGasSupplyBreakDownVOs.GroupBy(x => x.PeriodFacilityId);
        foreach(var periodFacility in periodFaciities)
        {
            var reportingPeriodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacility.Key);

            if (reportingPeriodFacility is null) throw new Exception("PeriodFacility not found");

            list.AddRange(reportingPeriodFacility.AddPeriodFacilityGasSupplyBreakDown(reportingPeriodFacilityGasSupplyBreakDownVOs));


        }
         return list;
    }

    internal bool LoadPeriodFacilityGasSupplyBreakdown(int id,int supplierId, int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        return periodFacility.LoadPeriodFacilityGasSupplyBreakdown(id,periodFacilityId,site,unitOfMeasure,content);
    }

    internal bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var periodFacilities = _periodfacilities.Where(x => x.ReportingPeriodSupplierId == periodSupplierId).ToList();

        

        foreach (var periodFacility in periodFacilities)
        {
            periodFacility.RemovePeriodSupplierGasSupplyBreakdown(periodFacility.Id);
        }

        return true;
    }
    #endregion
}





