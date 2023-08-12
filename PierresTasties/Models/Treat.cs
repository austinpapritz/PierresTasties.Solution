namespace PierresTasties.Models;

public class Treat
{
    public int TreatId { get; set; }
    public string Name { get; set; }
    public List<FlavorTreat> FlavorTreats { get; set; }
    public List<Vote> Votes { get; set; }
}