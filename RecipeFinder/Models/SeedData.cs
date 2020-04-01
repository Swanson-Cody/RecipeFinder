using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeFinder.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Recipe.Any())
                {
                    return;   // DB has been seeded
                }

                var user = context.Users.Select(x => x).FirstOrDefault();
                var recipe = new Recipe
                {
                    UserRecordNumber = user?.Id,
                    Title = "Vegetable Medley",
                    DateAdded = DateTime.Parse("2020-03-02"),
                    Instruction = "1) Preheat Oven 350°F. 2) Add all ingredients into a bowl and mix together. 3) Put ingredients into a 9x9 pan and cover with aluminum foil. 4) Bake for 35-40 minutes."
                };

                context.Recipe.Add(recipe);
                context.SaveChanges();

                context.Ingredient.AddRange(
                        new Ingredient()
                        {
                            Name = "Zucchini",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = .5,
                            RecipeId = recipe.ID,
                            Notes = "Chopped"
                        },
                        new Ingredient()
                        {
                            Name = "Bell Pepper",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID,
                            Notes = "Chopped"
                        },
                        new Ingredient()
                        {
                            Name = "Celery",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID,
                            Notes = "Chopped"
                        },
                        new Ingredient()
                        {
                            Name = "Broccoli",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID,
                            Notes = "Chopped"
                        },
                        new Ingredient()
                        {
                            Name = "Chicken Breast",
                            Quantity = 1,
                            RecipeId = recipe.ID,
                            Notes = "Cooked and chopped"
                        },
                        new Ingredient()
                        {
                            Name = "Cream of Mushroom",
                            Measurement = Measurements.Ounce.ToString(),
                            Quantity = 21,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Black Pepper",
                            Measurement = Measurements.Teaspoon.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID
                        }
                    );

                context.SaveChanges();

                recipe = new Recipe
                {
                    UserRecordNumber = user?.Id,
                    Title = "Hot Wings",
                    DateAdded = DateTime.Parse("2020-03-02"),
                    Instruction = "1) Preheat deep fryer to 375°F. 2) Mix flour, cayenne pepper, black pepper, and salt together in a bowl. 3) Coat the chicken wings with mixture and fry it until golden. Set them aside for now. 4) Add butter and hot wing sauce into a small pot. 5) Heat the small pot using medium heat on the stove. Stir until it starts to bubble. 6) Coat the chicken wings with sauce."
                };

                context.Recipe.Add(recipe);
                context.SaveChanges();

                context.Ingredient.AddRange(
                        new Ingredient()
                        {
                            Name = "Chicken Wings",
                            Measurement = Measurements.Pound.ToString(),
                            Quantity = 1,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Flour",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = 1,
                            RecipeId = recipe.ID,
                            Notes = "All-purpose flour is best."
                        },
                        new Ingredient()
                        {
                            Name = "Cayenne Pepper",
                            Measurement = Measurements.Teaspoon.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Salt",
                            Measurement = Measurements.Teaspoon.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Black Pepper",
                            Measurement = Measurements.Teaspoon.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Butter",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = .25,
                            RecipeId = recipe.ID
                        },
                        new Ingredient()
                        {
                            Name = "Hot Wing Sauce",
                            Measurement = Measurements.Cup.ToString(),
                            Quantity = 1,
                            RecipeId = recipe.ID,
                            Notes = "Use favorite hot wing sauce. Frank's Redhot Original Buffalo Wing Sauce is good."
                        }
                    );

                context.SaveChanges();
            }
        }
    }
}
