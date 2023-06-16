﻿namespace Services.DTOs;

public class PeriodFacilityDocumentDto
{
    public int DocumentId { get; set; }
    public int Version { get; set; }
    public string DisplayName { get; set; }
    public int DocumentStatusId { get; set; }
    public string DocumentStatusName { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; }
    public string ValidationError { get; set; }

    public PeriodFacilityDocumentDto(int documentId, int version, string displayName, int documentStatusId, string documentStatusName, int documentTypeId, string documentTypeName, string validationError)
    {
        DocumentId = documentId;
        Version = version;
        DisplayName = displayName;
        DocumentStatusId = documentStatusId;
        DocumentStatusName = documentStatusName;
        DocumentTypeId = documentTypeId;
        DocumentTypeName = documentTypeName;
        ValidationError = validationError;
    }
}