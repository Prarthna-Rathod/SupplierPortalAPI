using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups
{
    public class EmailBlueprints :ReferenceLookup
    {
        public EmailBlueprints()
        { }
        public EmailBlueprints(int value,string displayName):base(value, displayName) { }
    }
}
