using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.ReportingPeriodRoot.ValueObjects;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using DataAccess.Extensions;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Net;
using System.Net.Mail;

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

    private PeriodSupplier()
    {
    }

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

    #region Private methods

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

    private string GeneratedReportingPeriodSupplierDocumentName(string collectionTimePeriod, string documentTypeName, int version, string extension)
    {
        var documentName = Supplier.Name + "-" + collectionTimePeriod + "-" + documentTypeName + "-" + version + extension;

        return documentName;
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

    internal IEnumerable<PeriodFacility> UpdatePeriodFacilityDataStatus(FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        var periodFacilities = _periodfacilities.Where(x => x.FacilityReportingPeriodDataStatus.Name == FacilityReportingPeriodDataStatusValues.Complete).ToList();

        foreach (var periodFacility in periodFacilities)
        {
            periodFacility.UpdatePeriodFacilityDataStatus(facilityReportingPeriodDataStatus);
        }

        return periodFacilities;
    }

    internal bool UpdatePeriodFacilityDataStatusSubmittedToInProgress(int periodFacilityId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
    {
        var periodFacility = FindPeriodFacility(periodFacilityId);
        periodFacility.UpdatePeriodFacilityDataStatusSubmittedToInProgress(facilityReportingPeriodDataStatus);
        return true;
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

        foreach (var facilityData in groupByFacility)
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

    internal PeriodFacilityDocument AddUpdatePeriodFacilityDocument(int periodFacilityId, string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, string collectionTimePeriod, IEnumerable<FacilityRequiredDocumentTypeVO> facilityRequiredDocumentTypeVOs)
    {
        CheckSupplierReportingPeriodStatus();
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.AddUpdatePeriodFacilityDocument(displayName, path, documentStatuses, documentType, validationError, collectionTimePeriod, facilityRequiredDocumentTypeVOs);
    }

    internal bool LoadPeriodFacilityDocuments(int documentId, int periodFacilityId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
    {
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.LoadPeriodFacilityDocuments(documentId, version, displayName, storedName, path, documentStatus, documentType, validationError);
    }

    internal bool RemovePeriodFacilityDocument(int periodFacilityId, int documentId)
    {
        CheckSupplierReportingPeriodStatus();
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.RemovePeriodFacilityDocument(documentId);
    }

    #endregion

    #region PeriodSupplierDocument

    private void CheckSupplementalDocumentTypeOrNot(DocumentType documentType)
    {
        if (documentType.Name != DocumentTypeValues.Supplemental)
            throw new BadRequestException("ReportingPeriodSupplierDocument should be SupplementalDocumentType !!");
    }

    internal PeriodSupplierDocument AddUpdatePeriodSupplierDocument(string displayName, string? path, IEnumerable<DocumentStatus> documentStatuses, DocumentType documentType, string? validationError, string collectionTimePeriod)
    {
        CheckSupplementalDocumentTypeOrNot(documentType);

        var existingData = _periodSupplierDocuments.Where(x => x.DocumentType.Id == documentType.Id).OrderByDescending(x => x.Version).ToList();

        //DocumentStatus -> Processing
        var documentStatusProcessing = documentStatuses.First(x => x.Name == DocumentStatusValues.Processing);

        //Retrieve FileExtension from fileName
        FileInfo file = new FileInfo(displayName);
        var extension = file.Extension;

        PeriodSupplierDocument? document = null;

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            if (existingData.Count() == 0)
            {
                var version = 1;
                var storedName = GeneratedReportingPeriodSupplierDocumentName(collectionTimePeriod, documentType.Name, version, extension);
                document = new PeriodSupplierDocument(Id, version, displayName, storedName, null, documentStatusProcessing, documentType, validationError);

                _periodSupplierDocuments.Add(document);
            }
            else
            {
                document = existingData.First();

                if (document.Path is not null)
                    throw new Exception("This file already exists in the system. If you wish to upload a new version of the file, please delete existing file and try the upload again.");

                //Update existing versioned data record
                var version = document.Version;
                DocumentStatus? documentStatus = null;

                if (document.DocumentStatus.Name == DocumentStatusValues.HasErrors)
                    version += 1;

                if (validationError == null)
                {
                    documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.NotValidated);
                }
                else
                {
                    documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.HasErrors);
                    path = null;
                    validationError = "Unable to save the uploaded file at this time.  Please attempt the upload again later.";
                }

                if (validationError == null && path != null)
                {
                    documentStatus = documentStatuses.First(x => x.Name == DocumentStatusValues.Validated);
                }

                var newStoredName = GeneratedReportingPeriodSupplierDocumentName(collectionTimePeriod, documentType.Name, version, extension);
                document.Version = version;
                document.DisplayName = file.Name;
                document.StoredName = newStoredName;
                document.Path = path;
                document.DocumentStatus = documentStatus;
                document.DocumentType = documentType;
                document.ValidationError = validationError;
            }
        }
        else
        {
            document = existingData.First();
            if (document.Path is not null)
                throw new Exception("This file already exists in the system. If you wish to upload a new version of the file, please delete existing file and try the upload again.");

            document.DisplayName = file.Name;
        }

        return document;
    }

    internal bool LoadPeriodSupplierDocuments(int documentId, int version, string displayName, string storedName, string path, DocumentStatus documentStatus, DocumentType documentType, string validationError)
    {
        var supplierDocument = new PeriodSupplierDocument(documentId, Id, version, displayName, storedName, path, documentStatus, documentType, validationError);

        return _periodSupplierDocuments.Add(supplierDocument);
    }

    internal bool RemovePeriodSupplierDocument(int documentId)
    {
        var document = _periodSupplierDocuments.First(x => x.Id == documentId);

        if (document is null)
            throw new NotFoundException("ReportingPeriodDocument not found !!");

        if (SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked)
        {
            _periodSupplierDocuments.Remove(document);
            return true;
        }
        else
            return false;

    }

    #endregion

    #region Delete methods

    internal int DeletePeriodFacilityElectricityGridMixes(int periodFacilityId)
    {
        CheckSupplierReportingPeriodStatus();
        var periodFacility = FindPeriodFacility(periodFacilityId);
        return periodFacility.DeletePeriodFacilityElectricityGridMixes();
    }

    internal int DeletePeriodSupplierGasSupplyBreakdowns()
    {
        CheckSupplierReportingPeriodStatus();
        int count = 0;
        foreach (var periodFacility in _periodfacilities)
        {
            count += periodFacility.DeletePeriodSupplierGasSupplyBreakdowns();
        }

        return count;
    }

    #endregion

    #region SendMailNotification

    internal List<string> CheckInitialOrResendDataRequestDateAndGetContactEmails(DateTime? endDate)
    {
        var contactsEmails = Supplier.Contacts.Where(x => x.IsActive).Select(x => x.Email).ToList();

        if(contactsEmails.Count() == 0)
            throw new NotFoundException("Supplier contacts not found !!");

        if (InitialDataRequestDate is null)
        {
            return contactsEmails;
        }
        else if(ResendDataRequestDate is null)
        {
            if (endDate is null)
            {
                var timeLimit = InitialDataRequestDate.Value.Date.AddDays(30);
                //InitialDataRequestDate : 31-05-2023
                //Added 30 days timelimitDate : 30-06-2023
                //CurrentDate : 1-6-2023
                if (timeLimit.Date > DateTime.UtcNow.Date)
                    throw new Exception($"You can send reminder DataRequest mail after the deadline {timeLimit.Date}!!");
            }
            else
            {
                if(endDate.Value.Date > DateTime.UtcNow.Date)
                    throw new Exception($"You can send reminder DataRequest mail after the deadline {endDate.Value.Date}!!");

            }
            return contactsEmails;
        }
        else
            throw new BadRequestException("Already sended mail for InitialDataRequest and ResendDataRequest !!");
    }

    #endregion

}
