using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class EmailTemplateValues
    {
        public static string[] EmailTemplates =
       {
            InitialDataRequest,
            ResendDataRequest,
        };
        public const string InitialDataRequest = "InitialDataRequest";
        public const string ResendDataRequest = "ResendDataRequest";
    }
}
