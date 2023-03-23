using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReadOnlyDTOs;

public class DocumentTypeRequirementStatusDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Active { get; set; }

    public int DocumentTypeId { get; set; }

    public string DocumentType { get; set; }

    public DocumentTypeRequirementStatusDTO(int id, string name, string description, bool active, int documentTypeId, string documentType)
    {
        Id = id;
        Name = name;
        Description = description;
        Active = active;
        DocumentTypeId = documentTypeId;
        DocumentType = documentType;
    }
}
