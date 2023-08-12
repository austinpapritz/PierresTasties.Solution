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
                    new Flavor { Name = "Savory" },
                    new Flavor { Name = "Creamy" },
                    new Flavor { Name = "Spicy" }
                };

            // Save Flavors to the database
            context.Flavors.AddRange(flavors);
            context.SaveChanges();

            // Define Treats with combinations of flavors
            var treatNames = new List<string>
                {
                    "Sweet Raspberry Tart", "Sweet Lemon Cookies", "Sweet Almond Cake", "Sweet Chocolate Muffins",
                    "Savory Cheese Croissant", "Savory Olive Bread", "Savory Tomato Quiche", "Savory Bacon Scones",
                    "Creamy Vanilla Eclairs", "Creamy Coffee Tiramisu", "Creamy Strawberry Cheesecake", "Creamy Pistachio Mousse",
                    "Spicy Ginger Cookies", "Spicy Chili Brownies", "Spicy Cinnamon Rolls", "Spicy Cayenne Chocolate Cake"
                };

            // Create FlavorTreat relationships
            for (int i = 0; i < flavors.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var treat = new Treat
                    {
                        Name = treatNames[i * 4 + j],
                        Description = "This is a tasty treat.",
                        ImageURL = $"/{treatNames[i * 4 + j]}.jpg"
                    };
                    var flavorTreat = new FlavorTreat { Flavor = flavors[i], Treat = treat };
                    context.FlavorTreats.Add(flavorTreat);
                }
            }

            // Save changes to the database
            context.SaveChanges();
        }
    }

}
