using BusinessLogic.SupplierRoot.ValueObjects;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BusinessLogic.SupplierRoot.DomainModels
{

    public class Contact
    {
        public int Id { get; private set; }
        public int SupplierId { get; private set; }
        public UserVO UserVO { get; private set; }

        internal Contact()
        { }

        internal Contact(int supplierId, UserVO userVO)
        {
            SupplierId = supplierId;
            UserVO = userVO;
        }

        internal Contact(int id, int supplierId, UserVO userVO) : this(supplierId, userVO)
        {
            Id = id;
        }

        internal void UpdateUser(string name, string email, string contactNo, bool isActive)
        {
            UpdateName(name);
            UpdateEmail(email);
            UpdateContactNo(contactNo);
            UpdateIsActive(isActive);
        }

        internal void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Name is required");
            }
            else
                UserVO.Name = name;

        }

        internal void UpdateEmail(string email)
        {
            UserVO.Email = email;
        }

        internal void UpdateContactNo(string contactNo)
        {
            Regex format = new Regex(@"^[+]{1}(?:[0-9\-\(\)\/\.]\s?){6,15}[0-9]{1}$");
            var isValidate = format.IsMatch(contactNo.ToString());

            if (isValidate == false)
            {
                throw new Exception("Please enter valid ContactNumber !!");
            }
            else
                UserVO.ContactNo = contactNo;
        }

        internal void UpdateIsActive(bool isActive)
        {
            UserVO.IsActive = isActive;
        }
    }
}
