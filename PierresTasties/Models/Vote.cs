namespace PierresTasties.Models;
public class Vote
{
    public int VoteId { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public int TreatId { get; set; }
    public Treat Treat { get; set; }
    public DateTime VoteDate { get; set; }
}
