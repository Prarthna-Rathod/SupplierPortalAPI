using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class ReportingTypeValues
    {
        public static string[] ReportingTypes =
        {
            GHGRP,
            NonGHGRP
        };
        public const string GHGRP = "GHGRP";
        public const string NonGHGRP = "NonGHGRP";
    }
}
