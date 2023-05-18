using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class DocumentTypeValues
    {
        public static string[] DocumentTypes =
        {
            SubpartC,
            SubpartW,
            NonGHGRP,
            Supplemental
        };
        public const string SubpartC = "Subpart C";
        public const string SubpartW = "Subpart W";
        public const string NonGHGRP = "Non-GHGRP";
        public const string Supplemental = "Supplemental";
    }
}
