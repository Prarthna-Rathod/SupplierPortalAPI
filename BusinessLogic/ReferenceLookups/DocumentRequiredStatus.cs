using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class DocumentRequiredStatus : ReferenceLookup
    {
        public DocumentRequiredStatus() { }

        public DocumentRequiredStatus(int value, string displayName) : base(value,displayName) { }
    }
}
