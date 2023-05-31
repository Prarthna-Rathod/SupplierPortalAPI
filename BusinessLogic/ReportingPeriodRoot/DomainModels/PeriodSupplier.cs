using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;

public class PeriodSupplier
{
    private HashSet<PeriodFacility> _periodfacilities;
    private HashSet<PeriodSupplierDocument> _periodSupplierDocuments;

    internal PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        InitialDataRequestDate = initialDataRequestDate;
        ResendDataRequestDate = resendDataRequestDate;
        _periodfacilities = new HashSet<PeriodFacility>();
        _periodSupplierDocuments = new HashSet<PeriodSupplierDocument>();
    }

    internal PeriodSupplier(int id, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate) : this(supplierVO, reportingPeriodId, supplierReportingPeriodStatus, initialDataRequestDate, resendDataRequestDate)
    {
        Id = id;
    }

    private PeriodSupplier()
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

    public IEnumerable<PeriodSupplierDocument> PeriodSupplierDocuments
    {
        get
        {
            if (_periodSupplierDocuments == null)
            {
                return new List<PeriodSupplierDocument>();
            }
            return _periodSupplierDocuments.ToList();
        }
    }

    internal void UpdateSupplierReportingPeriodStatus(SupplierReportingPeriodStatus supplierReportingPeriodStatus)
    {
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;

    }

    #region private methods

    private void CheckPeriodSupplierStatus()
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new BadRequestException("SupplierReportingPeriodStatus should be Unlocked !!");
    }

    private PeriodFacility GetPeriodFacility(int periodFacilityId)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        if (periodFacility is null)
            throw new NotFoundException("PeriodFacility is not found !!");

        return periodFacility;
    }

    private string GeneratedPeriodSupplierDocumentStoredName(string collectionTimePeriod, DocumentType documentType, int version, string fileName)
    {
        var supplierName = Supplier.Name;

        var fileTypes = new[] { "xml", "xlsx" };

        var fileExtension = Path.GetExtension(fileName).Substring(1);
        if (!fileTypes.Contains(fileExtension))
            throw new BadRequestException("File Extension Is InValid !!");

        var storedName = supplierName + "-" + collectionTimePeriod + "-" + documentType.Name + "-" + version + "." + fileExtension;

        return $"{storedName}";
    }

    #endregion

    #region UpdateFacilityDataStatus

    internal IEnumerable<PeriodFacility> UpdatePeriodFacilityDataStatusCompleteToSubmitted(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        var periodFacilities = _periodfacilities.Where(x => x.FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete).ToList();

        foreach (var periodFacility in periodFacilities)
        {
            periodFacility.UpdatePeriodFacilityDataStatusCompleteToSubmitted(facilityReportingPeriodDataStatus);
        }

        return periodFacilities;
    }

    internal bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);
        periodFacility.UpdatePeriodFacilityDataStatusSubmittedToInProgress(facilityReportingPeriodDataStatus);
        return true;
    }

    #endregion

    #region Period Facility

    internal PeriodFacility AddPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, bool facilityIsRelevantForPeriod, FercRegion fercRegion, bool isActive)
    {
        int counter = 0;
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, Id, fercRegion, isActive);

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

    internal bool LoadPeriodFacility(int periodFacilityId, FacilityVO facilityVO, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, int reportingPeriodId, int periodSupplierId, FercRegion fercRegion, bool isActive)
    {
        var periodFacility = new PeriodFacility(periodFacilityId, facilityVO, facilityReportingPeriodDataStatus, reportingPeriodId, periodSupplierId, fercRegion, isActive);

        return _periodfacilities.Add(periodFacility);
    }

    #endregion

    #region PeriodFacilityElectricityGridMix

    internal IEnumerable<PeriodFacilityElectricityGridMix> AddPeriodFacilityElectricityGridMix(int periodFacilityId, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);

        CheckPeriodSupplierStatus();

        return periodFacility.AddElectricityGridMixComponents(unitOfMeasure, electricityGridMixComponentPercents, fercRegion);
    }

    internal bool LoadPeriodFacilityElectricityGridMix(int periodFacilityId, UnitOfMeasure unitOfMeasure, IEnumerable<ElectricityGridMixComponentPercent> electricityGridMixComponentPercents)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);
        return periodFacility.LoadPeriodFacilityElectricityGridMix(unitOfMeasure, electricityGridMixComponentPercents);
    }

    internal bool RemovePeriodFacilityElectricityGridMix(int periodFacilityId)
    {
        CheckPeriodSupplierStatus();

        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.RemovePeriodFacilityElectricityGridMix(periodFacilityId);

    }

    #endregion

    #region PeriodFacilityGasSupplyBreakdown

    internal IEnumerable<PeriodFacilityGasSupplyBreakdown> AddPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
    {
        CheckPeriodSupplierStatus();

        var list = new List<PeriodFacilityGasSupplyBreakdown>();

        //Check site wise content values
        var siteData = gasSupplyBreakdownVOs.GroupBy(x => x.Site.Id);
        foreach (var singleSiteData in siteData)
        {
            var siteName = singleSiteData.Select(x => x.Site.Name);
            var totalContentValue = singleSiteData.Sum(x => x.Content);

            if (totalContentValue != 100)
                throw new Exception($"Please add more content values in site {siteName}");
        }

        var periodFacility = gasSupplyBreakdownVOs.GroupBy(x => x.PeriodFacilityId);
        foreach (var facility in periodFacility)
        {
            var periodFacilityDomain = GetPeriodFacility(facility.Key);

            list.AddRange(periodFacilityDomain.AddPeriodFacilityGasSupplyBreakdown(facility));

        }

        return list;
    }

    internal bool LoadPeriodFacilityGasSupplyBreakdown(IEnumerable<GasSupplyBreakdownVO> gasSupplyBreakdownVOs)
    {
        foreach (var gasSupplyBreakdownVo in gasSupplyBreakdownVOs)
        {
            var periodFacility = GetPeriodFacility(gasSupplyBreakdownVo.PeriodFacilityId);

            periodFacility.LoadPeriodFacilityGasSupplyBreakdown(gasSupplyBreakdownVo.Site, gasSupplyBreakdownVo.UnitOfMeasure, gasSupplyBreakdownVo.Content);
        }

        return true;
    }

    internal bool RemovePeriodFacilityGasSupplyBreakdown(int periodSupplierId)
    {
        var periodFacilities = _periodfacilities.Where(x => x.ReportingPeriodSupplierId == periodSupplierId).ToList();

        CheckPeriodSupplierStatus();

        foreach (var periodFacility in periodFacilities)
        {
            periodFacility.RemovePeriodSupplierGasSupplyBreakdown(periodFacility.Id);
        }

        return true;
    }

    #endregion

    #region PeriodFacilityDocument

    internal PeriodFacilityDocument AddPeriodFacilityDocument(int periodFacilityId, string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string collectionTimePeriod, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs)
    {
        CheckPeriodSupplierStatus();

        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.AddPeriodFacilityDocument(displayName, path, validationError, documentStatuses, documentType, collectionTimePeriod, facilityRequiredDocumentTypeVOs);
    }

    internal bool LoadPeriodFacilityDocument(int periodFacilityDocumentId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.LoadPeriodFacilityDocument(periodFacilityDocumentId, version, displayName, storedName, path, documentStatus, documentType, validationError);
    }

    internal bool RemovePeriodFacilityDocument(int periodFacilityId, int periodFacilityDocumentId)
    {
        CheckPeriodSupplierStatus();
        var periodFacility = GetPeriodFacility(periodFacilityId);
        return periodFacility.RemovePeriodFacilityDocument(periodFacilityDocumentId);
    }

    #endregion

    #region PeriodSupplierDocument

    internal PeriodSupplierDocument AddUpdatePeriodSupplierDocument(string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string collectionTimePeriod)
    {
        //check existingDocument
        var existingDocument = _periodSupplierDocuments.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();

        PeriodSupplierDocument? periodSupplierDocument = null;

        var documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Processing);

        if (documentType.Name != DocumentTypeValues.Supplemental)
            throw new BadRequestException("DocumentType should be Supplemental !!");

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            if (existingDocument.Count() == 0)
            {
                int version = 1;
                var documentStoredName = GeneratedPeriodSupplierDocumentStoredName(collectionTimePeriod, documentType, version, displayName);
                periodSupplierDocument = new PeriodSupplierDocument(Id, version, displayName, documentStoredName, null, null, documentStatus, documentType);
                _periodSupplierDocuments.Add(periodSupplierDocument);
            }
            else
            {
                periodSupplierDocument = existingDocument.First();

                var version = periodSupplierDocument.Version;

                if (periodSupplierDocument.DocumentStatus.Name == DocumentStatusValues.HasErrors)
                    version += 1;

                if (validationError is null)
                    documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.NotValidated);
                else
                {
                    documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.HasErrors);
                    path = null;
                    try
                    {
                        throw new Exception("Unable to save the uploaded file at this time.Please attempt the upload again later.");
                    }
                    catch (Exception ex)
                    {
                        validationError = ex.Message;
                    }
                }

                if (path is not null && validationError is null)
                {
                    documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Validated);
                }

                var documentStoredName = GeneratedPeriodSupplierDocumentStoredName(collectionTimePeriod, documentType, version, displayName);

                periodSupplierDocument.ReportingPeriodSupplierId = Id;
                periodSupplierDocument.Version = version;
                periodSupplierDocument.DisplayName = displayName;
                periodSupplierDocument.StoredName = documentStoredName;
                periodSupplierDocument.Path = path;
                periodSupplierDocument.ValidationError = validationError;
                periodSupplierDocument.DocumentStatus = documentStatus;
                periodSupplierDocument.DocumentType = documentType;
            }
        }
        else
        {
            periodSupplierDocument = existingDocument.First();
            if (periodSupplierDocument.Path is not null)
                throw new BadRequestException("This file already exists in the system.If you wish to upload a new version of the file, please delete the file and try the upload again.");

            periodSupplierDocument.DisplayName = displayName;
        }

        return periodSupplierDocument;

    }

    internal bool LoadPeriodSupplierDocument(int periodSupplierDocumentId, int version, string displayName, string storedName, string path, string validationError, DocumentStatus documentStatus, DocumentType documentType)
    {
        var periodSupplierDocument = new PeriodSupplierDocument(periodSupplierDocumentId, Id, version, displayName, storedName, path, validationError, documentStatus, documentType);

        _periodSupplierDocuments.Add(periodSupplierDocument);

        return true;
    }

    internal bool RemovePeriodSupplierDocument(int documentId)
    {
        var document = _periodSupplierDocuments.FirstOrDefault(x => x.Id == documentId);

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            _periodSupplierDocuments.Remove(document);
            return true;
        }
        else
            throw new BadRequestException("Can't deleted document because supplierReportingPeriodStatus is locked !!");

    }

    #endregion

    #region SendEmail

    internal List<string> CheckInitialAndResendDataRequest(DateTime? endDate)
    {
        var emails = Supplier.Users.Where(x => x.IsActive == true).Select(x => x.Email).ToList();

        if (emails.Count() == 0)
            throw new NotFoundException("Supplier contact is not found !!");

        if (InitialDataRequestDate is null)
            return emails;

        else if (ResendDataRequestDate is null)
        {
            if (endDate is null)
            {
                var timeLimit = InitialDataRequestDate.Value.AddDays(30);

                //DateTime checkDate = new DateTime(2023, 6, 30);

                if (timeLimit.Date > DateTime.UtcNow.Date)
                    throw new BadRequestException($"Please reminder send mail after deadline {timeLimit}.");
            }
            else
            {
                //DateTime checkDate = new DateTime(2024, 4, 12);
                if (endDate.Value.Date > DateTime.UtcNow.Date)
                    throw new BadRequestException($"Please reminder send mail after deadline {endDate}.");
            }
            return emails;
        }
        else
            throw new BadRequestException("InitialDataRequest and ResendDataRequest mail is already send !!");
        
    }

    #endregion

}


