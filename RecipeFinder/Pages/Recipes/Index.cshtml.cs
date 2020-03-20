using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;

        public IndexModel(RecipeFinder.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Recipe> Recipe { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var recipesToReturn = new Dictionary<int, Recipe>();
            var enumerableRecipes = from r in _context.Recipe select r;
            var allRecipes = await enumerableRecipes.Select(x => new Recipe
            {
                ID = x.ID,
                IngredientsWithAmount = x.IngredientsWithAmount,
                DateAdded = x.DateAdded,
                Instruction = x.Instruction,
                Title = x.Title
            }).ToListAsync();

            if (!string.IsNullOrEmpty(SearchString))
            {
                var strings = SearchString.Split(',');
                foreach (var splitString in strings)
                {
                    var foundRecipes = allRecipes.Where(s => s.IngredientsWithAmount.Contains(splitString)).ToList();
                    //foundRecipes.ForEach(x => recipeList.Add(x));
                    foreach (var recipe in foundRecipes)
                    {
                        if (!recipesToReturn.ContainsKey(recipe.ID))
                        {
                            recipesToReturn.Add(recipe.ID, recipe);
                        }
                    }
                }
                
                Recipe = recipesToReturn.Values.ToList();
            }
            else
            {
                Recipe = allRecipes;
            }

        }
    }
}
