﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class FercRegionValues
    {
        public static string[] FercRegions =
        {
            None,
            CAISO,
            MISO,
            PJM,
            SPP,
            ISONE,
            NYISO,
            ERCOT,
            Northwest,
            Southwest,
            Southeast,
            CustomMix
        };

        public const string None = "None";
        public const string CAISO = "CAISO";
        public const string MISO = "MISO";
        public const string PJM = "PJM";
        public const string SPP = "SPP";
        public const string ISONE = "ISO-NE";
        public const string NYISO = "NYISO";
        public const string ERCOT = "ERCOT";
        public const string Northwest = "Northwest";
        public const string Southwest = "Southwest";
        public const string Southeast = "Southeast";
        public const string CustomMix = "Custom Mix";
    }
}
