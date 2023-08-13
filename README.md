# _Pierre's Tasties Web App_

#### By _Austin Papritz_

#### _This web application serves to organize Pierre's Bakery new tasty treats and flavors!_

![Screenshot of Ginger Cookies detail page.](/PierresTasties/wwwroot/tasties_ss.png)

## Technologies Used

* _C#_
* _ASP.NET Core_
* _JavaScript_
* _HTML/CSS_
* _MySQL_
* _Visual Studio Code_
* _Entity Framework Core_
* _EF Core Identity_
* _JQuery_

## Description

_Pierre's Tasties is a web app that allows users to see what new treats and flavor combos are in the works at Pierre's Bakery. Admins can easily add, edit, and delete new treats and flavors._

## Project Setup

* _Navigate to where you want to save the repo using your favorite terminal app (e.g., GitBash)_
* _Enter in terminal:_ 

    ```$ git clone https://github.com/austinpapritz/PierresTasties.Solution.git```
* _Navigate into the repo folder and then open it in your favorite IDE (e.g., VS Code, Xcode, Atom)._

## Database Setup

* _Search online to install MySQL on your computer. Remember your username and password._
* _Add `appsettings.json` file to project folder. Paste the following code, inserting your own information where {indicated}._

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database={DATABASENAME};uid={USERNAME};pwd={PASSWORD};"
    }
}
```

* _Build the project by entering `$ dotnet build`._
* _Complete the database setup by entering `$ dotnet EF database update`._

## Run Web App

* _Enter `$ dotnet watch run` to run the web app._
* _Your browser should automatically open._
* _You may need to give yourself security certs by entering `$ dotnet dev-certs https --trust`._
* _There will be a confirmation pop-up in your browser, you might also need to click `Advanced` and then click to proceed to site_
* _Enjoy!_

## New Features In Progress

* _User can use a Tasty Token to vote for new treat products._
* _Admin can generate a unique code that only a specific user can redeem._
* _User can redeem code to get more Tasty Tokens._

## Known Bugs

* _New Flavors need to have styling added for Treat detail pages to display correctly._

## License

_This app is not licensed and is free to use and distribute._

_If you run in to any problems or have any suggestions, feel free to contact me on [linkedIn](https://www.linkedin.com/in/austin-papritz)!_