using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PierresTasties.Models;

public class DataInitializer
{
    public static void InitializeData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<PierresTastiesContext>();
            context.Database.Migrate();

            // If there's already stuff in the database, don't run.
            if (context.Flavors.Any())
            {
                return;
            }

            // Define Flavors
            var flavors = new List<Flavor>
                {
                    new Flavor { Name = "Sweet" },
                    new Flavor { Name = "Sharp" },
                    new Flavor { Name = "Savory" },
                    new Flavor { Name = "Creamy" },
                    new Flavor { Name = "Spicy" }
                };

            // Save Flavors to the database
            context.Flavors.AddRange(flavors);
            context.SaveChanges();

            // Create a list of treats for every flavor combo.
            var treatCombinations = new List<(string Name, string Flavor1, string Flavor2)>
                {
                    ("Sugar Cookie", "Sweet", null),
                    ("Lemon Meringue Pie", "Sweet", "Sharp"),
                    ("Bacon Scones", "Sweet", "Savory"),
                    ("Strawberry Cheesecake", "Sweet", "Creamy"),
                    ("Ginger Cookies", "Sweet", "Spicy"),
                    ("Raspberry Tart", "Sharp", null),
                    ("Sharp Cheddar Sourdough Bread", "Sharp", "Savory"),
                    ("Lemon Cream Tart", "Sharp", "Creamy"),
                    ("Orange Jalapeño Monkey Bread", "Sharp", "Spicy"),
                    ("Black Truffle Olive Bread", "Savory", null),
                    ("Cheesy Croissant", "Savory", "Creamy"),
                    ("Cayenne Tomato Quiche", "Savory", "Spicy"),
                    ("Vanilla Eclairs", "Creamy", null),
                    ("Mayan Chocolate Truffle", "Creamy", "Spicy"),
                    ("Habanero Potato Empanada", "Spicy", null)
                };


            foreach (var treatCombination in treatCombinations)
            {
                // Make a Treat object for every treat in the list.
                var treat = new Treat
                {
                    Name = treatCombination.Name,
                    Description = "This is a tasty treat.",
                };
                context.Treats.Add(treat);
                context.SaveChanges();

                // Use Flavor1 and Flavor2 strings to get Flavor entity from db.
                var flavor1 = flavors.FirstOrDefault(f => f.Name == treatCombination.Flavor1);
                var flavor2 = flavors.FirstOrDefault(f => f.Name == treatCombination.Flavor2);

                // Match treat and flavor via `FlavorTreat` entity.
                var flavorTreat1 = new FlavorTreat { Flavor = flavor1, Treat = treat };
                context.FlavorTreats.Add(flavorTreat1);

                // If treat has a second flavor, then create another `FlavorTreat`.
                if (flavor2 != null)
                {
                    var flavorTreat2 = new FlavorTreat { Flavor = flavor2, Treat = treat };
                    context.FlavorTreats.Add(flavorTreat2);
                }

                // Save changes to the database
                context.SaveChanges();
            }
        }
    }
}
