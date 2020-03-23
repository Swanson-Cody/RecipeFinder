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

                context.Recipe.AddRange(
                    new Recipe
                    {
                        UserId = user?.Id ?? null,
                        Title = "Vegetable Medley",
                        DateAdded = DateTime.Parse("2020-03-02"),
                        Instruction = "1) Preheat Oven 350°F. 2) Add all ingredients into a bowl and mix together. 3) Put ingredients into a 9x9 pan and cover with aluminum foil. 4) Bake for 35-40 minutes."
                        //"zucchini (chopped) - 1/2 cup, bell pepper (chopped) - 1/4 cup, celery (chopped) - 1/4 cup, broccoli (chopped) - 1/4 cup, chicken breast (cooked and chopped) - 1, cream of mushroom - 2 cans, black pepper - 1/4 teaspoon",
                    }

                    //new Recipe
                    //{
                    //    Title = "Hot Wings",
                    //    DateAdded = DateTime.Parse("2020-03-02"),
                    //    IngredientsWithAmount = "chicken wings - 1 lb., all purpose flour - 1 cup, cayenne pepper - 1/4 teaspoon, salt - 1/4 teaspoon, black pepper - 1/4 teaspoon, butter - 1/4 cup, preferred hot wing sauce - 1 cup",
                    //    Instruction = "1) Preheat deep fryer to 375°F. 2) Mix flour, cayenne pepper, black pepper, and salt together in a bowl. 3) Coat the chicken wings with mixture and fry it until golden. Set them aside for now. 4) Add butter and hot wing sauce into a small pot. 5) Heat the small pot using medium heat on the stove. Stir until it starts to bubble. 6) Coat the chicken wings with sauce."
                    //},

                    //new Recipe
                    //{
                    //    Title = "Mash Potato with Chicken Bowl",
                    //    DateAdded = DateTime.Parse("2020-03-02"),
                    //    IngredientsWithAmount = "potatoes - 2 medium sized, butter - 1/4 cup, milk - 1/4 cup, salt - taste preference, pepper - taste preference, cheese - 1/4 cup, corn - 1/4 cup, popcorn chicken - 1 cup",
                    //    Instruction = "1) Boil potatoes until they are soft. 2) Mash together in a mixing bowl the softened potatoes, butter, milk, salt, and pepper. 3) Cook popcorn chicken in deep fryer at 370°F or air fryer for 400°F. 4) Add cheese, corn, and popcorn chicken on top of the mashed potatoes in order listed."
                    //},

                    //new Recipe
                    //{
                    //    Title = "Parmesan Chicken",
                    //    DateAdded = DateTime.Parse("2020-03-02"),
                    //    IngredientsWithAmount = "chicken breast - 2 pieces, parmesan cheese (grated) - 1/2 cup, salt - dash, black pepper - dash, parsley (flakes) - 1 teaspoon, butter (softened) - 2/3 stick",
                    //    Instruction = "1) Preheat oven to 350°F. 2) Mix butter, parsley, salt, and black pepper together in a bowl. 3) Coat chicken with mixture from step 2 and then coat with parmesan cheese. 4) Put chicken in baking pan, cover with aluminum foil. Bake for 15-20 minutes. 5) Remove aluminum foil. Bake until golden."
                    //}
                );

                context.SaveChanges();
                var recipe = context.Recipe.Select(x => x).FirstOrDefault();

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

                var ingredientId = context.SaveChanges();
            }
        }
    }
}
