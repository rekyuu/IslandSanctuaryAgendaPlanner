// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using IslandSanctuaryAgendaPlanner.Model;
using IslandSanctuaryAgendaPlanner.Type;

namespace IslandSanctuaryAgendaPlanner;

internal class Program
{
    private const int IslandRank = 9;
    private const int WorkshopRank = 2;
    private const int CurrentGroove = 15;
    private const int MaxGroove = 30;
    
    private static ProductData[] Products;
    private static Dictionary<ProductData, ProductData[]> ProductComboMap = new();
    
    static void Main(string[] args)
    {
        float workshopModifier = Utility.GetWorkshopModifier(WorkshopRank);
        
        // Initialize the base product data.
        string productData = File.ReadAllText("Data/ProductData.json");
        Products = JsonSerializer.Deserialize<ProductData[]>(productData)!;
        
        // Read CSV data for supply and demand and store it to the ProductData array.
        int lineNo = 0;
        foreach (string line in File.ReadLines(args[0]))
        {
            if (lineNo == 0)
            {
                lineNo++;
                continue;
            }

            string[] csvData = line.Split(",");

            try
            {
                long id = long.Parse(csvData[0]);

                ProductData product = Products
                    .Where(x => x.Id == id)
                    .Select(x => x)
                    .First();

                product.Demand = (Demand)long.Parse(csvData[2]);
                product.Supply = (Supply)long.Parse(csvData[3]);
                product.DemandShift = (DemandShift)long.Parse(csvData[4]);
                product.PredictedDemand = (Demand)long.Parse(csvData[5]);
            }
            catch
            {
                continue;
            }

            lineNo++;
        }
        
        // Generate product combo map.
        foreach (ProductData product in Products)
        {
            List<ProductData> potentialCombos = new();
            foreach (string category in product.Categories)
            {
                potentialCombos.AddRange(Products.Where(x => x.Categories.Contains(category)));
            }

            potentialCombos = potentialCombos
                .Distinct()
                .ToList();
            potentialCombos.Remove(product);
            
            ProductComboMap.Add(product, potentialCombos.ToArray());
        }

        // Generate all possible agendas.
        List<Agenda> agendas = new();
        foreach (ProductData product in Products)
        {
            List<Agenda> productAgendas = GetPossibleAgendasFromStartingProduct(product, workshopModifier, CurrentGroove);
            
            agendas.AddRange(productAgendas);
        }

        // Remove stuff you can't or don't want to make.
        ProductData sharkOil = Products
            .Single(x => x.Name == "Isleworks Shark Oil");

        ProductData earrings = Products
            .Single(x => x.Name == "Isleworks Earrings");

        ProductData formula = Products
            .Single(x => x.Name == "Isleworks Growth Formula");

        ProductData earCuffs = Products
            .Single(x => x.Name == "Isleworks Silver Ear Cuffs");

        ProductData rapier = Products
            .Single(x => x.Name == "Isleworks Garnet Rapier");

        // Order expected value by descending order and take the top 10.
        Agenda[] topAgendas = agendas
            //.Where(x => !x.ProductSchedule.Contains(sharkOil))
            .Where(x => !x.ProductSchedule.Contains(earrings))
            .Where(x => !x.ProductSchedule.Contains(formula))
            .Where(x => !x.ProductSchedule.Contains(earCuffs))
            .Where(x => !x.ProductSchedule.Contains(rapier))
            .OrderByDescending(x => x.ExpectedValue)
            .Take(10)
            .Reverse()
            .ToArray();

        // Take the top 10 week starter schedules (4-4-4-4-4-4).
        // Agenda[] topStarters = agendas
        //     .Where(x => x.ProductSchedule.Count == 6)
        //     .OrderByDescending(x => x.ExpectedValue)
        //     .Take(10)
        //     .Reverse()
        //     .ToArray();
        
        // Print to console.
        Console.WriteLine($"== SUGGESTIONS ==");
        foreach (Agenda agenda in topAgendas)
        {
            Console.WriteLine($"Total: {agenda.ExpectedValue}");
            foreach (ProductData product in agenda.ProductSchedule)
            {
                Console.WriteLine($"  {product.Time} hrs: {product.Name}");
            }
            
            Console.WriteLine();
        }
    }

    private static List<Agenda> GetPossibleAgendasFromStartingProduct(ProductData startingProduct, float workshopModifier, long groove)
    {
        // Create the initial agenda.
        Agenda initialAgenda = new();
        initialAgenda.AddToAgenda(startingProduct, workshopModifier, groove, false);
        
        // Create the initial results list and get all possible agenda combinations from it.
        List<Agenda> initialResults = new() { initialAgenda };

        return GetPossibleAgendas(initialResults, workshopModifier, groove);

    }

    private static List<Agenda> GetPossibleAgendas(List<Agenda> results, float workshopModifier, long groove)
    {
        List<Agenda> completeResults = new();
        List<Agenda> incompleteResults = new();
        
        foreach (Agenda agenda in results)
        {
            // Sanity check for completed agendas.
            if (agenda.UsedTime == 24)
            {
                completeResults.Add(agenda);
                continue;
            }
        
            // Obtain a list of potential next products.
            foreach (ProductData potentialCombo in ProductComboMap[agenda.ProductSchedule.Last()])
            {
                if (agenda.UsedTime + potentialCombo.Time > 24) continue;
                if (potentialCombo.Rank > IslandRank) continue;

                Agenda nextAgenda = agenda.Clone();
                nextAgenda.AddToAgenda(potentialCombo, workshopModifier, groove, true);

                // No more items can be added if there's less than 4 hours left in a day.
                if (nextAgenda.UsedTime > 20) completeResults.Add(nextAgenda);
                else incompleteResults.Add(nextAgenda);
            }
        }
            
        // Add groove now that a new item has been added to each agenda.
        groove += 1;
        if (groove > MaxGroove) groove = MaxGroove;

        // Recursively process incomplete agendas until they hit more than 20 hours.
        if (incompleteResults.Count > 0)
        {
            List<Agenda> toComplete = GetPossibleAgendas(incompleteResults, workshopModifier, groove);
            completeResults.AddRange(toComplete);
        }
        
        return completeResults;
    }
}