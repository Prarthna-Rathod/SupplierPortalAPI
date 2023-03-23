using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ValueConstants
{
    public class SupplyChainStagesValues
    {
        public static string[] SupplyChainStages =
        {
            Production,
            Processing,
            TransmissionCompression,
            TransmissionStorage,
            TransportPipeline,
            GatheringAndBoosting
        };
        public const string Production = "Production";
        public const string Processing = "Processing";
        public const string TransmissionCompression = "Transmission Compression";
        public const string TransmissionStorage = "Transmission Storage";
        public const string TransportPipeline = "Transport Pipeline";
        public const string GatheringAndBoosting = "Gathering and Boosting";
    }
}
