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

    public long BaseExpectedValue => GetExpectedValue(1, 0, false);

    public long GetExpectedValue(float workshopModifier, long groove, bool combo)
    {
        long efficiencyBonus = combo ? 2 : 1;
        float demandModifier = Utility.GetDemandModifier(Demand);
        float supplyModifier = Utility.GetSupplyModifier(Supply);
        
        return efficiencyBonus * (long)Math.Floor(demandModifier * supplyModifier * Math.Floor(Value * workshopModifier * (1 + groove / 100)));
    }
}