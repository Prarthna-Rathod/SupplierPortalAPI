using BusinessLogic.ReferenceLookups;
using BusinessLogic.SupplierRoot.ValueObjects;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects
{
    public class ActiveSupplier
    {
        public int PeriodId { get; set; }
        public SupplierVO SupplierVO { get; set; }
        public bool IsActiveForPeriod { get; set; }
        public SupplierReportingPeriodStatus SupplierReportingPeriodStatus { get; set; }


        public ActiveSupplier(int periodId, SupplierVO supplierVO, bool isActiveForPeriod, SupplierReportingPeriodStatus supplierReportingPeriodStatus)
        {
            PeriodId = periodId;
            SupplierVO = supplierVO;
            IsActiveForPeriod = isActiveForPeriod;
            SupplierReportingPeriodStatus = supplierReportingPeriodStatus;
        }
          


    }
}
