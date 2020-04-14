using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pluralize.NET;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeIndexModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPluralize _pluralizer;

        public RecipeIndexModel(RecipeFinder.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _pluralizer = new Pluralizer();
        }

        public List<Recipe> Recipe { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public string UserId { get; set; }

        public async Task OnGetAsync()
        {
            var recipes = new List<Recipe>();
            UserId = _userManager.GetUserId(User);
            var searchItems = SearchString?.Split(',').Select(x => x.Trim()).Select(x => x.ToLower()).ToList();
            var robustSearchItems = new List<string>();

            if (searchItems != null)
            {
                foreach (var searchItem in searchItems)
                {
                    robustSearchItems.Add(_pluralizer.Singularize(searchItem));
                    robustSearchItems.Add(_pluralizer.Pluralize(searchItem));
                }
            }

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

            if (!string.IsNullOrEmpty(SearchString))
            {
                var recipesFromSearch = new List<Recipe>();
                recipesFromSearch.AddRange(recipes.Where(x => robustSearchItems.Any(y => x.Title.ToLower().Contains(y))));

                foreach (var recipe in recipes)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (robustSearchItems.Any(x => ingredient.Name.ToLower().Contains(x)))
                        {
                            if (!recipesFromSearch.Contains(recipe))
                            {
                                recipesFromSearch.Add(recipe);
                                break;
                            }
                        }
                    }
                }

                recipes = recipesFromSearch;
            }

            Recipe = recipes;
        }
    }
}
