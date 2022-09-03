using IslandSanctuaryAgendaPlanner.Type;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IslandSanctuaryAgendaPlanner.Model;

public class ProductData
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public long Rank { get; set; }
    
    public long Time { get; set; }
    
    public long Quantity { get; set; }
    
    public long Value { get; set; }
    
    public string[] Categories { get; set; }

    public ProductMaterialData[] Materials { get; set; }

    public Demand Demand { get; set; }

    public Supply Supply { get; set; }
    
    public DemandShift DemandShift { get; set; }

    public Demand PredictedDemand { get; set; }
    
    public long CraftedToday { get; set; }

    public long BaseExpectedValue => GetExpectedValueFromCurrentDemand(1, 0, false);

    public long GetExpectedValueFromCurrentDemand(float workshopModifier, long groove, bool combo)
    {
        float demandModifier = Utility.GetDemandModifier(Demand);
        float supplyModifier = Utility.GetSupplyModifier(Supply);
        
        return GetValue(workshopModifier, groove, combo, demandModifier, supplyModifier);
    }

    public long GetExpectedValueFromPredictedDemand(float workshopModifier, long groove, bool combo)
    {
        float demandModifier = Utility.GetDemandModifier(Demand);
        float supplyModifier = Utility.GetSupplyModifier(GetExpectedSupply());
        
        return GetValue(workshopModifier, groove, combo, demandModifier, supplyModifier);
    }

    private long GetValue(float workshopModifier, long groove, bool combo, float demandModifier, float supplyModifier)
    {
        long efficiencyBonus = combo ? 2 : 1;
        return efficiencyBonus * (long)Math.Floor(demandModifier * supplyModifier * Math.Floor(Value * workshopModifier * (1 + groove / 100)));
    }

    private Supply GetExpectedSupply()
    {
        if (Supply == Supply.Nonexistent && DemandShift == DemandShift.Skyrocketing) return Supply.Surplus;
        
        long totalUnits = Utility.GetAverageSupplyAmount(Supply) + Utility.GetSupplyShiftAmount(DemandShift) + CraftedToday;

        return totalUnits switch
        {
            >= 18 => Supply.Overflowing,
            <= 17 and >= 10 => Supply.Surplus,
            <= 9 and >= 2 => Supply.Sufficient,
            <= 1 and >= -6 => Supply.Insufficient,
            <= -5 => Supply.Nonexistent
        };
    }
}