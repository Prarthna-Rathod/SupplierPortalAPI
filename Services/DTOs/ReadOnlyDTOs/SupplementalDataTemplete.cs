using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class SupplementalDataTemplete
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public int ReportingPeriodSupplierDocumentId { get; set; }
    public string DocumentName { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentType { get; set; }
    public int DocumentStatusId { get; set; }
    public string DocumentStatus { get; set; }

    public SupplementalDataTemplete(int id,int supplierId,string supplierName,int reportingPeriodSupplierDocumentId,string documentName,
                        int documentTypeId,string documentType,int documentStatusId,string documentStatus)
    {
        Id = id;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ReportingPeriodSupplierDocumentId = reportingPeriodSupplierDocumentId;
        DocumentName = documentName;
        DocumentTypeId = documentTypeId;
        DocumentStatusId = documentStatusId;
        DocumentStatus = documentStatus;
    }
}
