﻿using BusinessLogic.ReferenceLookups;
using BusinessLogic.ValueConstants;

namespace BusinessLogic.SupplierRoot.DomainModels;

public class Facility
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsPrimary { get; set; }
    public int SupplierId { get; private set; }
    public string? GHGHRPFacilityId { get; private set; }
    public AssociatePipeline? AssociatePipelines { get; private set; }
    public ReportingType ReportingTypes { get; private set; }
    public SupplyChainStage SupplyChainStages { get; private set; }
    public bool IsActive { get; set; }

    internal Facility()
    { }

    internal Facility(string name, string description, bool isPrimary, int supplierId, string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
    {
        Name = name;
        Description = description;
        IsPrimary = isPrimary;
        SupplierId = supplierId;
        GHGHRPFacilityId = ghgrpFacilityId;
        AssociatePipelines = associatePipeline;
        ReportingTypes = reportingType;
        SupplyChainStages = supplyChainStage;
        IsActive = isActive;
    }

    internal Facility(int id, string name, string description, bool isPrimary, int supplierId, string? ghgrpFacilityId, AssociatePipeline? associatePipeline, ReportingType reportingType, SupplyChainStage supplyChainStage, bool isActive)
        : this(name, description, isPrimary, supplierId, ghgrpFacilityId, associatePipeline, reportingType, supplyChainStage, isActive)
    {
        Id = id;
    }

    internal void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("FacilityName is required");
        }
        else
            Name = name;
    }

    internal void UpdateDescription(string description)
    {
        Description = description;
    }

    internal void UpdateIsPrimary(bool isPrimary)
    {
        IsPrimary = ReportingTypes.Name == ReportingTypeValues.GHGRP ? isPrimary : true;
    }

    internal void UpdateGhgrpFacilityId(string? ghgrpFacilityId)
    {       
        GHGHRPFacilityId = ghgrpFacilityId;
    }

    internal void UpdateAssociatePipeline(AssociatePipeline associatePipeline)
    {
        AssociatePipelines = associatePipeline;
    }

    internal void UpdateReportingType(ReportingType reportingType)
    {
        ReportingTypes = reportingType;
    }

    internal void UpdateSupplyChainStage(SupplyChainStage supplyChainStage)
    {
        SupplyChainStages = supplyChainStage;
    }

    internal void UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
    }

}
