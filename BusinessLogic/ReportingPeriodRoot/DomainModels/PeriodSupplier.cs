using BusinessLogic.ReferenceLookups;
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

    internal PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus,DateTime initialDataRequestDate, DateTime resendDataRequestDate) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate)
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

}
