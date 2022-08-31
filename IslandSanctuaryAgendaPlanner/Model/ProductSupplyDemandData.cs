using IslandSanctuaryAgendaPlanner.Type;

namespace IslandSanctuaryAgendaPlanner.Model;

public class ProductSupplyDemandData
{
    public long Id { get; set; }
    
    public string Name { get; set; }

    public Demand Demand { get; set; }

    public Supply Supply { get; set; }
    
    public DemandShift DemandShift { get; set; }

    public Demand PredictedDemand { get; set; }
}