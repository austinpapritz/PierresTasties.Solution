Add Admin Role
Authorize only Admin to CUD flavors and treats

Add Patron Role and class
Authorize Patron to Read flavors and treats

Figure out relationship between treats, vote tokens, token codes, patrons, and admin

Token
    User gets 3 tokens
    User adds token to treat, to encourage production
    Pierre can generate a token using the user's unique Id (uses Id as salt for a 8-char hash)
    User can add token by redeeming a code

Authorize Patron to Update treat vote-count (add or remove)
Authorize Admin to create a token code
Authorize Patron to create a token by redeeming a token code.

Admin can CUD flavors and treats
Patron can create tokens (by redeeming it), user can add a vote token to a treat, or edit their vote token, see their votes in profile page