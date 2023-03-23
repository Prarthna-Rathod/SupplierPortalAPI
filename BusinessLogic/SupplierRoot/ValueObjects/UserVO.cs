
namespace BusinessLogic.SupplierRoot.ValueObjects
{
    public class UserVO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }


        public UserVO(int id, string name, string email, string contactNo, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            ContactNo = contactNo;
            IsActive = isActive;
        }

    }
}
