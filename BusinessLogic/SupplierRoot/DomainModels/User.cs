
namespace BusinessLogic.SupplierRoot.DomainModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public User()
        {

        }

        public User(string name, string email, string contactNo, int roleId, bool isActive)
        {
            Name = name;
            Email = email;
            ContactNo = contactNo;
            RoleId = roleId;
            IsActive = isActive;
        }

        public User(int id, string name, string email, string contactNo, int roleId, bool isActive) : this(name, email, contactNo, roleId, isActive)
        {
            Id = id;
        }

        public void UpdateUser(string name, string email, string contactNo, int roleId, bool isActive)
        {

            UpdateName(name);
            UpdateEmail(email);
            UpdateContact(contactNo);
            UpdateRole(roleId);
            UpdateIsActive(isActive);

        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Name is required");
            }
            else
                Name = name;

        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdateContact(string contactNo)
        {
            ContactNo = contactNo;
        }

        public void UpdateRole(int roleId)
        {
            RoleId = roleId;
        }

        public void UpdateIsActive(bool isActive)
        {
            IsActive = isActive;
        }

    }

}
