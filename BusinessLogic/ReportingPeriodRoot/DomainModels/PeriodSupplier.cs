using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.DomainModels;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System;

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

    private void CheckSupplierReportingPeriodStatus()
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new BadRequestException("SupplierReportingPeriodStatus should be UnLocked !!");
    }

    private PeriodFacility FindPeriodFacility(int periodFacilityId)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("ReportingPeriodFacility is not found !!");

        return periodFacility;
    }

    #region Period Facility

    internal PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive)
    {
        int counter = 0;
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, Id, fercRegion, isActive);

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            var periodSupplierFacilities = Supplier.Facilities;

            //Check existing PeriodFacility
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


    internal bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int periodSupplierId, FercRegion fercRegion, bool isActive)
    {
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, periodSupplierId, fercRegion, isActive);

        return _periodfacilities.Add(periodFacility);
    }

    internal IEnumerable<PeriodFacilityElectricityGridMix> AddRemoveElectricityGridMixComponents(int periodFacilityId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
    {
        CheckSupplierReportingPeriodStatus();
        var periodFacility = FindPeriodFacility(periodFacilityId);

        return periodFacility.AddRemoveElectricityGridMixComponents(unitOfMeasure, gridMixComponentPercents, fercRegion);
    }

    internal bool LoadElectricityGridMixComponents(int periodFacilityId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> gridMixComponentPercents)
    {
        var periodFacility = FindPeriodFacility(periodFacilityId);

        return periodFacility.LoadElectricityGridMixComponents(unitOfMeasure, gridMixComponentPercents);
    }

    #endregion

    #region GasSupplyBreakdown

    internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
    {
        CheckSupplierReportingPeriodStatus();
         var list = new List<PeriodFacilityGasSupplyBreakdown>();
        
        //Check total content values 100
        var groupBySiteData = gasSupplyBreakdownVOs.GroupBy(x => x.Site.Id);
        foreach (var singleSiteData in groupBySiteData)
        {
            var siteName = singleSiteData.First().Site.Name;
            var totalContent = singleSiteData.Sum(x => x.Content);
            if (totalContent != 100)
                throw new Exception($"Please add more values in site {siteName}!! ");
        }

        //Add GasSupplyBreakdown data in periodFacility
        var groupByFacility = gasSupplyBreakdownVOs.GroupBy(x => x.PeriodFacilityId);

        foreach(var facilityData in groupByFacility)
        {
            var periodFacility = FindPeriodFacility(facilityData.Key);

            list.AddRange(periodFacility.AddPeriodFacilityGasSupplyBreakdown(facilityData));
        }

        return list;
    }

    internal bool LoadPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> periodFacilityGasSupplyBreakdowns)
    {
        //var list = new List<PeriodFacilityGasSupplyBreakdown>();
        foreach (var item in periodFacilityGasSupplyBreakdowns)
        {
            var periodFacility = FindPeriodFacility(item.PeriodFacilityId);
            periodFacility.LoadPeriodFacilityGasSupplyBreakdowns(item.Site, item.UnitOfMeasure, item.Content);
        }
        return true;
    }

    #endregion

    #region PeriodDocument

    internal PeriodFacilityDocument AddUpdatePeriodFacilityDocument(int periodFacilityId, string displayName, string? path, IEnumerable< DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, string collectionTimePeriod)
    {
        CheckSupplierReportingPeriodStatus();
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.AddUpdatePeriodFacilityDocument(displayName, path, documentStatuses, documentType, validationError, collectionTimePeriod);
    }

    internal bool LoadPeriodFacilityDocuments(int documentId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
    {
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.LoadPeriodFacilityDocuments(documentId, version, displayName, storedName, path, documentStatus, documentType, validationError);
    }

    #endregion

}
