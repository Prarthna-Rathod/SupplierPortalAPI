using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class DocumentTypeValues
    {
        public static string[] DocumentType =
        {
            Subpart_C,
            Subpart_W,
            Non_GHGRP,
            Supplemental
        };

        public const string Subpart_C="Subpart C";
        public const string Subpart_W = "Subpart W";
        public const string Non_GHGRP="Non-GHGRP";
        public const string Supplemental = "Supplemental";
    }
}
