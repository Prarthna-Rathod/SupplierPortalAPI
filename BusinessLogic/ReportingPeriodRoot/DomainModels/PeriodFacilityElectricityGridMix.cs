using BusinessLogic.ReferenceLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.DomainModels
{
    public class PeriodFacilityElectricityGridMix
    {
        public int Id { get; private set; }
        public int PeriodFacilityId { get; private set; }
        public ElectricityGridMixComponent ElectricityGridMixComponent { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public FercRegion FercRegion { get; private set; }
        public decimal Content { get; private set; }
        public bool IsActive { get; private set; }

        internal PeriodFacilityElectricityGridMix()  {  }

        internal PeriodFacilityElectricityGridMix(int periodFacilityId, ElectricityGridMixComponent electricityGridMixComponent, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, decimal content, bool isActive)
        {
            PeriodFacilityId = periodFacilityId;
            ElectricityGridMixComponent = electricityGridMixComponent;
            UnitOfMeasure = unitOfMeasure;
            FercRegion = fercRegion;
            Content = content;
            IsActive = isActive;
        }

        internal PeriodFacilityElectricityGridMix(int id,int periodFacilityId, ElectricityGridMixComponent electricityGridMixComponent, UnitOfMeasure unitOfMeasure, FercRegion fercRegion, decimal content, bool isActive): this(periodFacilityId, electricityGridMixComponent, unitOfMeasure, fercRegion, content, isActive)
        {
            Id = id;
        }

        
    }
}
