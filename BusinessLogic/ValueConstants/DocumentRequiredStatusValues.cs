using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class DocumentRequiredStatusValues
    {
        public static string[] DocumentRequiredStatuses =
       {
            Optional,
            Required,
            NotAllowed,
        };
        public const string Optional = "Optional";
        public const string Required = "Required";
        public const string NotAllowed = "Not-allowed";
    }
}
