using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class AssociatePipeline: ReferenceLookup
    {
        public AssociatePipeline()
        { }

        public AssociatePipeline(int value, string displayName): base(value, displayName) { }
        
    }
}
