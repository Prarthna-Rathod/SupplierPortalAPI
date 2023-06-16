using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.IO;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels;


public class PeriodSupplier
{
    private HashSet<PeriodFacility> _periodfacilities;
    private HashSet<PeriodSupplierDocument> _periodSupplierDocuments;

    internal PeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus, DateTime? initialDataRequestDate, DateTime? resendDataRequestDate, bool isActive)
    {
        Supplier = supplier;
        ReportingPeriodId = reportingPeriodId;
        SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        InitialDataRequestDate = initialDataRequestDate;
        ResendDataRequestDate = resendDataRequestDate;
        IsActive = isActive;
        _periodfacilities = new HashSet<PeriodFacility>();
        _periodSupplierDocuments = new HashSet<PeriodSupplierDocument>();

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

        var fileTypes = new[] { "xlsx" };

        var fileExtension = Path.GetExtension(fileName).Substring(1);
        if (!fileTypes.Contains(fileExtension))
            throw new BadRequestException("File Extension Is InValid !!");

        var storedName = supplierName + "-" + collectionTimePeriod + "-" + documentType.Name + "-" + version + "." + fileExtension;

        return $"{storedName}";
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

        return periodFacility.RemovePeriodFacilityElectricityGridMix();

    }

    #endregion

    #region PeriodFacilityGasSupplyBreakDown
    internal IEnumerable<PeriodFacilityGasSupplyBreakDown> AddPeriodFacilityGasSupplyBreakDown(IEnumerable<ReportingPeriodFacilityGasSupplyBreakDownVO> reportingPeriodFacilityGasSupplyBreakDownVOs)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("Supplier should be Unlocked !!");

        var list = new List<PeriodFacilityGasSupplyBreakDown>();

        var sites = reportingPeriodFacilityGasSupplyBreakDownVOs.GroupBy(x => x.Site.Id);

        foreach (var site in sites)
        {
            var siteName = site.Select(x => x.Site.Name).FirstOrDefault();
            var contentValue = site.Sum(x => x.Content);

            if (contentValue != 100)
                throw new Exception($"Add more contentValues to site {siteName}");
        }

        var periodFaciities = reportingPeriodFacilityGasSupplyBreakDownVOs.GroupBy(x => x.PeriodFacilityId);
        foreach (var periodFacility in periodFaciities)
        {
            var reportingPeriodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacility.Key);

            if (reportingPeriodFacility is null) throw new Exception("PeriodFacility not found");

            list.AddRange(reportingPeriodFacility.AddPeriodFacilityGasSupplyBreakDown(reportingPeriodFacilityGasSupplyBreakDownVOs));


        }
        return list;
    }

    internal bool LoadPeriodFacilityGasSupplyBreakdown(int id, int supplierId, int periodFacilityId, Site site, UnitOfMeasure unitOfMeasure, decimal content)
    {
        var periodFacility = _periodfacilities.FirstOrDefault(x => x.Id == periodFacilityId);

        return periodFacility.LoadPeriodFacilityGasSupplyBreakdown(id, periodFacilityId, site, unitOfMeasure, content);
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

    #region PeriodFacilityDocument
    internal PeriodFacilityDocument AddPeriodFacilityDocument(int periodFacilityId, string displayName, string? path, string? validationError, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, IEnumerable<FacilityRequiredDocumentType> facilityRequiredDocumentTypes, string collectionTimePeriod)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("PeriodSupplier should be Unlocked !!");

        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.AddPeriodFacilityDocument(displayName, path, validationError, documentStatuses, documentType, facilityRequiredDocumentTypes, collectionTimePeriod);
    }

    internal bool LoadPeriodFacilityDocument(int periodFacilityDocumentId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.LoadPeriodFacilityDocument(periodFacilityDocumentId, version, displayName, storedName, path, documentStatus, documentType, validationError);
    }
    internal bool RemovePeriodFacilityDocument(int periodFacilityId, int periodFacilityDocumentId)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("PeriodSupplier should be Unlocked !!");
        var periodFacility = GetPeriodFacility(periodFacilityId);

        return periodFacility.RemovePeriodFacilityDocument(periodFacilityDocumentId);
    }
    #endregion

    #region UpdatePeriodFacilityDataStatuses

    internal bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        var periodFacility = GetPeriodFacility(periodFacilityId);
        periodFacility.UpdatePeriodFacilityDataStatusSubmittedToInProgress(facilityReportingPeriodDataStatus);
        return true;
    }
    internal IEnumerable<PeriodFacility> UpdatePeriodFacilityDataStatusCompleteToSubmitted(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
            throw new Exception("PeriodSupplier should be Unlocked !!");
        var periodFacilities = _periodfacilities.Where(x => x.FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete).ToList();

        foreach (var periodFacility in periodFacilities)
        {
            periodFacility.UpdatePeriodFacilityDataStatusCompleteToSubmitted(facilityReportingPeriodDataStatus);
        }

        return periodFacilities;
    }

    #endregion

    #region PeriodSupplierDocument
    internal PeriodSupplierDocument AddPeriodSupplierDocument(int periodSupplierId, string? path, string? validationError, string displayName, DocumentType documentType, IEnumerable<DocumentStatus> documentStatuses, string collectionTimePeriod)
    {
        var isExist = _periodSupplierDocuments.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();

        PeriodSupplierDocument? periodSupplierDocument = null;

        var documentStatus = documentStatuses.FirstOrDefault(x => x.Name == DocumentStatusValues.Processing);

        if (documentType.Name != DocumentTypeValues.Supplemental)
            throw new BadRequestException("DocumentType should be Supplemental !!");

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            if (isExist.Count() == 0)
            {
                int version = 1;
                var documentStoredName = GeneratedPeriodSupplierDocumentStoredName(collectionTimePeriod, documentType, version, displayName);
                periodSupplierDocument = new PeriodSupplierDocument(Id, version, displayName, documentStoredName, null, null, documentStatus, documentType);
                _periodSupplierDocuments.Add(periodSupplierDocument);
            }
            else
            {
                periodSupplierDocument = isExist.First();

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
                        throw new Exception("Unable to save the uploaded file at this time.Try again later..!");
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
            periodSupplierDocument = isExist.First();
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
        //Get recipients 
        var emailList = Supplier.Users.Where(x => x.IsActive).Select(x => x.Email).ToList();
        if (emailList.Count() == 0)
            throw new NotFoundException("Recipients are not found !!");
        if (InitialDataRequestDate is null)
            return emailList;
        else if (ResendDataRequestDate is null)
        {
            if (endDate is null)
            {
                var dueDate = InitialDataRequestDate.Value.AddDays(30);
                var currentDate = new DateTime(2023, 07, 16);
                if (dueDate.Date > currentDate)
                    throw new BadRequestException($"Reminder Mail Due date : {dueDate}.");
            }
            else
            {
                if (endDate.Value.Date > DateTime.UtcNow.Date)
                    throw new BadRequestException($"Reminder Mail Due date : {endDate}.");
            }
            return emailList;
        }
        else
            throw new BadRequestException("InitialDataRequest and ResendDataRequest mail is already send !!");
    }

    #endregion
}