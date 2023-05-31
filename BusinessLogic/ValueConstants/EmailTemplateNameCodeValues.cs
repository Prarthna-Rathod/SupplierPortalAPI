using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class EmailTemplateNameCodeValues
    {
        public static string[] EmailTemplateNameCodes =
        {
            InitialDataRequest,
            ReminderDataRequest,
        };
        public const string InitialDataRequest = "Initial Data Collection Request";
        public const string ReminderDataRequest = "Reminder for Data Collection Request";
    }
}
