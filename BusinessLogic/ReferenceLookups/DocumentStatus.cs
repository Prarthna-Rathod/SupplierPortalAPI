using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class DocumentStatus : ReferenceLookup
    {
        public DocumentStatus() { }

        public DocumentStatus(int value, string displayName) : base(value, displayName) { }
    }
}
