using Microsoft.AspNetCore.Identity;

namespace PierresTasties.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        VoteTokens = 3;
    }
    public int VoteTokens { get; set; }
    public ICollection<Vote> Votes { get; set; }
}
