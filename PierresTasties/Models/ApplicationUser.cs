using Microsoft.AspNetCore.Identity;

namespace PierresTasties.Models;

public class ApplicationUser : IdentityUser
{
    public int VoteTokens { get; set; }
    public ICollection<Vote> Votes { get; set; }
}
