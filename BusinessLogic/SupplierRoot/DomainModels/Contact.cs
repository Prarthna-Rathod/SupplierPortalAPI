using BusinessLogic.SupplierRoot.ValueObjects;

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
    }
}
