using IslandSanctuaryAgendaPlanner.Type;

namespace IslandSanctuaryAgendaPlanner;

public static class Utility
{
    private static readonly float[] DemandModifier = new[]
    {
        0.8f, // Low
        1.0f, // Average
        1.2f, // High
        1.4f, // Very High
    };
    
    private static readonly float[] SupplyModifier = new[]
    {
        1.6f, // Nonexistent
        1.3f, // Insufficient
        1.0f, // Sufficient
        0.8f, // Surplus
        0.6f, // Overflowing
    };

    private static readonly float[] WorkshopModifier = new[]
    {
        1.0f, // Workshop I
        1.1f, // Workshop II
        1.2f, // Workshop III
    };

    public static float GetDemandModifier(Demand demand) => DemandModifier[(int)demand];

    public static float GetSupplyModifier(Supply supply) => SupplyModifier[(int)supply];

    public static float GetWorkshopModifier(int workshopRank) => WorkshopModifier[workshopRank];
}