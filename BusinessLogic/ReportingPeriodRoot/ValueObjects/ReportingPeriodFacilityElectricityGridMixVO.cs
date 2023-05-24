using BusinessLogic.ReferenceLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReportingPeriodRoot.ValueObjects;

public class ReportingPeriodFacilityElectricityGridMixVO
{
    public int Id { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public ElectricityGridMixComponent ElectricityGridMixComponent { get;set; }
    public decimal Content { get; set; }
    public bool IsActive { get; set; }


    public ReportingPeriodFacilityElectricityGridMixVO()
    {

    }

    public ReportingPeriodFacilityElectricityGridMixVO(int id, UnitOfMeasure unitOfMeasure, ElectricityGridMixComponent electricityGridMixComponent,decimal content, bool isActive)
    {
        Id = id;
        UnitOfMeasure = unitOfMeasure;
        ElectricityGridMixComponent = electricityGridMixComponent;
        Content = content;
        IsActive = isActive;
    }
}
