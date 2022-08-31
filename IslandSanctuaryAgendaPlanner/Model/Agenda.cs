namespace IslandSanctuaryAgendaPlanner.Model;

public class Agenda
{
    public List<ProductData> ProductSchedule { get; set; }
    
    public long ExpectedValue { get; set; }
    
    public long UsedTime { get; set; }

    public Agenda()
    {
        ProductSchedule = new List<ProductData>();
        ExpectedValue = 0;
        UsedTime = 0;
    }

    public void AddToAgenda(ProductData productToAdd, float workshopModifier, long groove, bool combo)
    {
        if (UsedTime + productToAdd.Time > 24) return;
        
        ProductSchedule.Add(productToAdd);
        UsedTime += productToAdd.Time;
        ExpectedValue += productToAdd.GetExpectedValue(workshopModifier, groove, combo);
    }

    public Agenda Clone()
    {
        return new Agenda
        {
            ProductSchedule = new List<ProductData>(this.ProductSchedule),
            ExpectedValue = this.ExpectedValue,
            UsedTime = this.UsedTime
        };
    }
}