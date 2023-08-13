Add Description and Flavors to Treat form.

User
    Many-to-many with Treat (via TastyToken entity)
    TokenCount = 3

TastyToken
    User adds token to treat, to encourage pierre to produce it
    Pierre can generate a token using the user's unique Id (uses Id as salt for a 8-char hash)
    User can add token by redeeming a code

Treat
    Many-to-many with User (via TastyToken entity)
    VoteCount = 0;

Authorize User to Update treat vote-count (add or remove)
Authorize Admin to create a token code
Authorize User to create a token by redeeming a token code.

User can create tokens (by redeeming it), user can add a vote token to a treat, or edit their vote token, see their votes in profile page