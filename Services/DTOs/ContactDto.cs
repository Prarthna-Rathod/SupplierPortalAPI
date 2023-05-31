using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int UserId { get; set; }

        [RegularExpression("(^[A-Za-z]{2,}[ ]{0,1}[A-Za-z]{2,}$)", ErrorMessage = "Name should be characters with allowed only 1 space and minimum length is 2 !!")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter valid Email-ID !!")]
        public string UserEmail { get; set; }

        public string UserContactNo { get; set; }
        public bool IsActive { get; set; }

        public ContactDto(int id, int supplierId, string supplierName, int userId, string userName, string userEmail, string userContactNo, bool isActive)
        {
            Id = id;
            SupplierId = supplierId;
            SupplierName = supplierName;
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            UserContactNo = userContactNo;
            IsActive = isActive;
        }

    }
}
