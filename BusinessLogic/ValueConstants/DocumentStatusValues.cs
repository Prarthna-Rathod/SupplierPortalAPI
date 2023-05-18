using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class DocumentStatusValues
    {
        public static string[] DocumentStatuses =
        {
            NotValidated,
            Validated,
            HasErrors,
            Processing
        };
        public const string NotValidated = "Not-validated";
        public const string Validated = "Validated";
        public const string HasErrors = "Has errors";
        public const string Processing = "Processing";
    }
}
