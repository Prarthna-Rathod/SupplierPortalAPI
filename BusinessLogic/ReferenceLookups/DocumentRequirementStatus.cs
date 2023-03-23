using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class DocumentRequirementStatus : ReferenceLookup
    {
        public DocumentRequirementStatus() { }

        public DocumentRequirementStatus(int value,string displayName) : base(value,displayName) { }
    }
}
