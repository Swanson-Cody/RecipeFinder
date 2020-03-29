using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeIndexModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipeIndexModel(RecipeFinder.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Recipe> Recipe { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string UserId { get; set; }

        public async Task OnGetAsync()
        {
            var recipes = new List<Recipe>();
            UserId = _userManager.GetUserId(User);
            var searchItems = SearchString?.Split(',').Select(x => x.Trim());

            if (!string.IsNullOrEmpty(SearchString))
            {
                recipes = _context.Recipe
                    .Join(_context.Ingredient, a => a.ID, b => b.RecipeId, (a, b) => a).Distinct()
                    .Where(x => x.Ingredients.Select(y => y.Name).Any(z => searchItems.Contains(z)))
                    .Where(x => x.UserRecordNumber.Equals(UserId))
                    .Select(recipe => new Recipe
                    {
                        ID = recipe.ID,
                        UserRecordNumber = recipe.UserRecordNumber,
                        Title = recipe.Title,
                        DateAdded = recipe.DateAdded,
                        Instruction = recipe.Instruction,
                        Ingredients = recipe.Ingredients
                            .Select(ingredient => new Ingredient
                            {
                                ID = ingredient.ID,
                                RecipeId = ingredient.RecipeId,
                                Name = ingredient.Name,
                                Measurement = ingredient.Measurement,
                                Notes = ingredient.Notes,
                                Quantity = ingredient.Quantity
                            }).ToList()
                    }).ToList();
            }
            else
            {
                recipes = _context.Recipe
                    .Join(_context.Ingredient, a => a.ID, b => b.RecipeId, (a, b) => a).Distinct()
                    .Where(x => x.UserRecordNumber.Equals(UserId))
                    .Select(recipe => new Recipe
                    {
                        ID = recipe.ID,
                        UserRecordNumber = recipe.UserRecordNumber.ToString(),
                        Title = recipe.Title,
                        DateAdded = recipe.DateAdded,
                        Instruction = recipe.Instruction,
                        Ingredients = recipe.Ingredients
                            .Select(ingredient => new Ingredient
                            {
                                ID = ingredient.ID,
                                RecipeId = ingredient.RecipeId,
                                Name = ingredient.Name,
                                Measurement = ingredient.Measurement,
                                Notes = ingredient.Notes,
                                Quantity = ingredient.Quantity
                            }).ToList()
                    }).ToList();
            }

            Recipe = recipes;
        }
    }
}
