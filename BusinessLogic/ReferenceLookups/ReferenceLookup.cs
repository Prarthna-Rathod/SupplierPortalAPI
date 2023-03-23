using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ReferenceLookups;

public abstract class ReferenceLookup
{
    protected ReferenceLookup()
    {
        Id = 0;
        Name = string.Empty;

    }

    public ReferenceLookup(int Value, string DisplayName)
    {
        Id = Value;
        Name = DisplayName;
    }

    public int Id { get; set; }

    public string Name { get; set; }
}
public abstract class ReferenceLookupWithDescription : ReferenceLookup
{
    protected ReferenceLookupWithDescription()
    {
        Description= string.Empty;
    }

    public ReferenceLookupWithDescription(int Value,string DisplayName,string? description) : base(Value,DisplayName)
    {
        Description = description ?? string.Empty;
    }

    public string Description { get; set; } 
}
