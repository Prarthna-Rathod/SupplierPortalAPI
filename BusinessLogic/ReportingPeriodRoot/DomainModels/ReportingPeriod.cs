using BusinessLogic.ReferenceLookups;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.SupplierRoot.ValueObjects;
using BusinessLogic.ValueConstants;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.Text.RegularExpressions;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class ReportingPeriod : IReportingPeriod
    {
        private HashSet<PeriodSupplier> _periodSupplier;

        private readonly string REPORTING_PERIOD_NAME_PREFIX = "Reporting Period Data";
        public ReportingPeriod(ReportingPeriodType reportingPeriodType, string collectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool isActive)
        {
            CheckCollectionTimePeriodData(reportingPeriodType.Name, collectionTimePeriod);
            DisplayName = GeneratedReportingPeriodName(reportingPeriodType, collectionTimePeriod);
            CollectionTimePeriod = collectionTimePeriod;
            ReportingPeriodType = reportingPeriodType;
            ReportingPeriodStatus = reportingPeriodStatus;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            _periodSupplier = new HashSet<PeriodSupplier>();
        }

        public ReportingPeriod(int id, string displayName, ReportingPeriodType types, string collectionTimePeriod, ReportingPeriodStatus status, DateTime startDate, DateTime? endDate, bool isActive) : this(types, collectionTimePeriod, status, startDate, endDate, isActive)
        {
            Id = id;
        }
        public ReportingPeriod()
        {

        }

        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public string CollectionTimePeriod { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsActive { get; private set; }

        public ReportingPeriodType ReportingPeriodType { get; private set; }

        public ReportingPeriodStatus ReportingPeriodStatus { get; private set; }


        private string[] SplitCollectionTimePeriod()
        {
            return CollectionTimePeriod.Split(" ");
        }

        private string GeneratedReportingPeriodName(ReportingPeriodType reportingPeriodType, string collectionTimePeriod)
        {
            var reportingPeriodName = REPORTING_PERIOD_NAME_PREFIX;
            switch (reportingPeriodType.Name)
            {
                case ReportingPeriodTypeValues.Annual:
                    reportingPeriodName = $"{reportingPeriodName} Year {collectionTimePeriod}";
                    break;
                case ReportingPeriodTypeValues.Quartly:
                    reportingPeriodName = $"{reportingPeriodName} {collectionTimePeriod}";
                    break;
                case ReportingPeriodTypeValues.Monthly:
                    reportingPeriodName = $"{reportingPeriodName} {collectionTimePeriod}";
                    break;
                default:
                    reportingPeriodName = $"{reportingPeriodName} {SplitCollectionTimePeriod().FirstOrDefault()}";
                    break;
            }
            return $"{reportingPeriodName}";
        }

        private void CheckCollectionTimePeriodData(string reportingPeriodTypeName, string collectionTimePeriod)
        {
            switch (reportingPeriodTypeName)
            {
                case ReportingPeriodTypeValues.Annual:
                    {
                        Regex format = new Regex("^[0-9]{4}$");
                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Year(ex: 2023)");
                        }
                    }
                    break;

                case ReportingPeriodTypeValues.Quartly:
                    {
                        Regex format = new Regex("^[Q]{1}[1-4]{1}[ ][0-9]{4}$");
                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Quarter(ex: 'Q1 2023')");
                        }
                    }
                    break;

                case ReportingPeriodTypeValues.Monthly:
                    {
                        Regex format = new Regex("^[A-Z]{3}[ ][0-9]{4}$");

                        if (!format.IsMatch(collectionTimePeriod))
                        {
                            throw new NoContentException("Collection time period should be in Month(ex: 'Jan 2023')");
                        }
                    }
                    break;
                default:
                    throw new NoContentException("Please enter valid CollectionTimePeriod for ReportingPeriodType !!");

            }

        }

        public IEnumerable<PeriodSupplier> PeriodSuppliers
        {
            get
            {
                if (_periodSupplier == null)
                {
                    return new List<PeriodSupplier>();
                }
                return _periodSupplier.ToList();
            }
        }

        public void UpdateReportingPeriod(ReportingPeriodType reportingPeriodType, string collectionTimePeriod, ReportingPeriodStatus reportingPeriodStatus, DateTime startDate, DateTime? endDate, bool isActive, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            switch (ReportingPeriodStatus.Name)
            {
                case ReportingPeriodStatusValues.InActive:
                    {
                        CheckCollectionTimePeriodData(reportingPeriodType.Name, collectionTimePeriod);
                        DisplayName = GeneratedReportingPeriodName(reportingPeriodType, collectionTimePeriod);

                        if (StartDate.Date != startDate.Date && startDate.Date < DateTime.UtcNow.Date)
                        {
                                throw new Exception("StartDate should be in future !!");
                        }

                        if (endDate != null && endDate < startDate)
                        {
                            
                                throw new BadRequestException("EndDate should be greater than startDate !!");
                        }


                        if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Open || reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                        {
                            if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                            {
                                //ReportingPeriodStatus = reportingPeriodStatus;
                                isActive = false;
                            }
                        }
                        else if (reportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                            throw new BadRequestException("You cannot set ReportingPeriodStatus to Complete !!");

                        CollectionTimePeriod = collectionTimePeriod;
                        ReportingPeriodStatus = reportingPeriodStatus;
                        StartDate = startDate;
                        EndDate = endDate;
                        IsActive = isActive;

                    }
                    break;
                case ReportingPeriodStatusValues.Open:
                    {
                        if (ReportingPeriodType.Id != reportingPeriodType.Id || CollectionTimePeriod != collectionTimePeriod ||
                            StartDate.Date != startDate.Date || IsActive != isActive)
                        {
                            throw new BadRequestException("You can't update any other data except ReportingPeriodStatus and EndDate !!");
                        }

                        if (ReportingPeriodStatus.Name != reportingPeriodStatus.Name && reportingPeriodStatus.Name == ReportingPeriodStatusValues.Close)
                        {
                            ReportingPeriodStatus = reportingPeriodStatus;

                            var periodSuppliersList = _periodSupplier.Where(x => x.IsActive && x.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Unlocked).ToList();

                            var supplierReportingPeriodLockedStatus = supplierReportingPeriodStatuses.First(x => x.Name == SupplierReportingPeriodStatusValues.Locked);
                            foreach (var periodSupplier in periodSuppliersList)
                            {
                                periodSupplier.UpdateSupplierReportingPeriodStatus(supplierReportingPeriodLockedStatus);
                            }
                        }
                        else
                            throw new BadRequestException("ReportingPeriodStatus can be Open as it is or else you can set it to Close only !!");


                        if (endDate != null)
                        {
                            if (endDate < startDate)
                                throw new BadRequestException("EndDate should be greater than startDate !!");
                        }
                        EndDate = endDate;

                    }
                    break;
                case ReportingPeriodStatusValues.Close:
                    {
                        if (ReportingPeriodType.Id != reportingPeriodType.Id || CollectionTimePeriod != collectionTimePeriod ||
                            StartDate.Date != startDate.Date || IsActive != isActive || EndDate != endDate)
                            throw new BadRequestException("You can't update any other data except ReportingPeriodStatus !!");

                        if (ReportingPeriodStatus.Name != reportingPeriodStatus.Name && reportingPeriodStatus.Name == ReportingPeriodStatusValues.Complete)
                        {
                            ReportingPeriodStatus = reportingPeriodStatus;
                        }
                        else
                            throw new BadRequestException("ReportingPeriodStatus can be Close as it is or else you can set it to Complete only !!");
                    }
                    break;
                default:
                    throw new BadRequestException("You cannot update any data because ReportingPeriod is Completed !!");
            }

        }


        public bool LoadPeriodSupplier(int reportingPeriodSupplierId, SupplierVO supplierVO, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
        {
            var reportingPeriodSupplier = new PeriodSupplier(reportingPeriodSupplierId, supplierVO, reportingPeriodId, supplierReportingPeriodStatus);

            return _periodSupplier.Add(reportingPeriodSupplier);
        }   

        public PeriodSupplier AddPeriodSupplier(SupplierVO supplier, int reportingPeriodId, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
        {
            var reportingPeriodSupplier = new PeriodSupplier(supplier, reportingPeriodId, supplierReportingPeriodStatus);


/*
            if (_periodSupplier.Contains(reportingPeriodSupplier))
            {
                throw new Exception("Supplier Already Exist!");
            }
            else
            {*/
                if(supplier.IsActive && ReportingPeriodStatus.Name == ReportingPeriodStatusValues.InActive)
                {
                    _periodSupplier.Add(reportingPeriodSupplier);
                }
               
           // }

            return reportingPeriodSupplier;
        }

        /*
        public PeriodSupplier UpdatePeriodSupplierStatus(int periodSupplierId, IEnumerable<SupplierReportingPeriodStatus> supplierReportingPeriodStatuses)
        {
            var periodSupplier = _periodSupplier.Where(x => x.Id == periodSupplierId).FirstOrDefault();

            if (periodSupplier is null)
            {
                throw new ArgumentNullException("Unable to retrieve Period Supplier");
            }

            if (periodSupplier.Id == periodSupplierId)
            {
                if (periodSupplier.SupplierReportingPeriodStatus.Name == SupplierReportingPeriodStatusValues.Locked)
                {
                    var unlockedStatus = supplierReportingPeriodStatuses.Where(x => x.Name == "Unlocked").FirstOrDefault();
                    periodSupplier.UpdateSupplierReportingPeriodStatus(unlockedStatus);
                }
                else
                {
                    periodSupplier.SupplierReportingPeriodStatus.Name = "Locked";
                }
            }
            else
            {
                throw new ArgumentNullException("Unable to retrieve Period Supplier");

            }


            return periodSupplier;
        }

        public PeriodFacilityDocument AddDataSubmissionDocumentForReportingPeriod(int supplierId, int periodFacilityId, FacilityRequiredDocumentTypeEntity facilityRequiredDocumentType, IEnumerable<DocumentRequirementStatus> documentRequirementStatus)
        {
            throw new NotImplementedException();
        }

        public void AddDocumentToPeriodSupplierFacility(DocumentType documentType, DocumentStatus documentStatus)
        {
            throw new NotImplementedException();
        }

        public void AddPeriodFacilityToPeriodSupplier(int supplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus, ReportingType reportingType, int reportingPeriodSupplierId)
        {
            var reportingPeriodFacility = new PeriodFacility();
        }


        public PeriodSupplierDocument AddSupplementalDataDocumentToReportingPeriodSupplier(int supplierId, string documentName, DocumentType documentType, IEnumerable<DocumentStatus> documentStatus)
        {
            throw new NotImplementedException();
        }

        public PeriodFacilityDocument RemoveDocumentFromPeriodSupplierFacility(int supplierId, int periodFacilityId, int documentId)
        {
            throw new NotImplementedException();
        }

        public PeriodSupplierDocument RemoveSupplementalDataDocumentToReportingPeriodSupplier(int supplierId, int documentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodFacility> UpdateDataStatusToSubmittedForCompletePeriodFacility(int supplierId, FacilityReportingPeriodDataStatus facilityReportingPeriodDataStatus)
        {
            throw new NotImplementedException();
        }

        */
    }
}
