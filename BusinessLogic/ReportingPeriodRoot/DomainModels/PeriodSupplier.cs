using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;

public class PeriodSupplier
{
    private HashSet<PeriodFacility> _periodfacilities;

    internal PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        InitialDataRequestDate = initialDataRequestDate;
        ResendDataRequestDate = resendDataRequestDate;
        _periodfacilities = new HashSet<PeriodFacility>();
    }

    internal PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime initialDataRequestDate, DateTime resendDataRequestDate) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate)
    {
        Id = id;
    }

    private PeriodSupplier()
    { }

    public int Id { get; private set; }
    public SupplierVO Supplier { get; private set; }
    public int ReportingPeriodId { get; private set; }
    public SupplierReportingPeriodStatus SupplierReportingPeriodStatus { get; private set; }
    public DateTime InitialDataRequestDate { get; private set; }
    public DateTime ResendDataRequestDate { get; private set; }
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

    #region Period Facility

    internal PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, bool facilityIsRelevantForPeriod, bool isActive)
    {
        int counter = 0;
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, Id, isActive);

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            var periodSupplierFacilities = Supplier.Facilities;

            //Check existing PeriodSupplier
            foreach (var existingPeriodFacility in _periodfacilities)
            {
                if (existingPeriodFacility.FacilityVO.Id == facilityVO.Id && existingPeriodFacility.ReportingPeriodId == Id)
                {
                    if (facilityIsRelevantForPeriod)
                        throw new BadRequestException("ReportingPeriodFacility is already exists !!");
                    else
                        _periodfacilities.Remove(existingPeriodFacility);
                }
            }

            if (facilityIsRelevantForPeriod)
            {
                if (facilityReportingPeriodDataStatus.Name != FacilityReportingPeriodDataStatusValues.InProgress)
                    throw new BadRequestException("FacilityReportingPeriodStatus should be InProgress only !!");

                foreach (var supplierFacility in periodSupplierFacilities)
                {
                    if (supplierFacility.Id == facilityVO.Id)
                    {
                        _periodfacilities.Add(periodFacility);
                        counter = 0;
                        break;
                    }
                    else
                        counter++;
                }
            }

            if (counter != 0)
                throw new BadRequestException("The given facility is not relavent with given ReportingPeriodSupplier !!");

        }
        else
            throw new BadRequestException("SupplierReportingPeriodStatus is locked !! Can't add new PeriodFacility !!");

        return periodFacility;
    }


    internal bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int periodSupplierId, bool isActive)
    {
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, Id, periodSupplierId, isActive);

        return _periodfacilities.Add(periodFacility);
    }


    #endregion

    #region PeriodFacilityElectricityGridMix

    internal IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodFacilityId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("PeriodFacility is not found !!");

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new BadRequestException("SupplierReportingPeriodStatus should be Unlocked !!");

        return periodFacility.AddElectricityGridMixComponents(unitOfMeasure, fercRegion, electricityGridMixComponentPercents);
    }

    internal bool LoadPeriodFacilityElectricityGridMix(int periodFacilityId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);
        return periodFacility.LoadPeriodFacilityElectricityGridMix(unitOfMeasure, fercRegion, electricityGridMixComponentPercents);
    }

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new BadRequestException("SupplierReportingPeriodStatus should be Unlocked !!");

        var list = new List<PeriodFacilityGasSupplyBreakdown>();

        //Check site wise content values
        var siteData = gasSupplyBreakdownVOs.GroupBy(x => x.Site.Id);
        foreach (var singleSiteData in siteData)
        {
            var siteName = singleSiteData.Select(x => x.Site.Name);
            var totalContentValue = singleSiteData.Sum(x => x.Content);

            var newCount = singleSiteData.Select(x => x.PeriodFacilityId).Distinct().Count();
            var count = singleSiteData.Select(x => x.PeriodFacilityId).Count();
            var facilityIsExists = newCount < count;
            if (facilityIsExists)
                throw new BadRequestException($"Site '{siteName}' is already exists in same PeriodFacility !!");

            if (totalContentValue != 100)
                throw new Exception($"Please add more content values in site {siteName}");
        }

        var periodFacility = gasSupplyBreakdownVOs.GroupBy(x => x.PeriodFacilityId);
        foreach (var facility in periodFacility)
        {
            var periodFacilityId = facility.First().PeriodFacilityId;
            var periodFacilityDomain = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

            if (periodFacilityDomain is null)
                throw new NotFoundException("PeriodFacility is not found !!");

            list.AddRange(periodFacilityDomain.AddPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownVOs));

        }

        return list;
    }

    internal bool LoadPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
    {
        foreach (var gasSupplyBreakdownVo in gasSupplyBreakdownVOs)
        {
            var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == gasSupplyBreakdownVo.PeriodFacilityId);

            periodFacility.LoadPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownVo.Site, gasSupplyBreakdownVo.UnitOfMeasure, gasSupplyBreakdownVo.Content);
        }

        return true;
    }

    #endregion

}
