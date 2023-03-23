using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class SupplierDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }

    [EmailAddress(ErrorMessage = "Please enter valid Email-ID !!")]
    public string Email { get; set; }

    [RegularExpression("^[+]{1}(?:[0-9\\-\\(\\)\\/\\.]\\s?){6,15}[0-9]{1}$", ErrorMessage = "Please enter valid ContactNumber !!")]
    public string ContactNo { get; set; }
    public bool IsActive { get; set; }

    public IEnumerable<FacilityDto>? Facilities { get; set; }
    public IEnumerable<ContactDto>? Contacts { get; set; }

    public SupplierDto(int? id, string name, string alias, string email, string contactNo, bool isActive, IEnumerable<FacilityDto>? facilities, IEnumerable<ContactDto>? contacts)
    {
        Id = id;
        Name = name;
        Alias = alias;
        Email = email;
        ContactNo = contactNo;
        IsActive = isActive;
        Facilities = facilities;
        Contacts = contacts;
    }
};
