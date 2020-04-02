using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder
{
    [Authorize]
    public class RecipeEditModel : PageModel
    {
        private readonly RecipeFinder.Data.ApplicationDbContext _context;

        public RecipeEditModel(RecipeFinder.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public List<Ingredient> Ingredients { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.ID == id);
            Ingredients = await _context.Ingredient.Where(x => x.RecipeId == Recipe.ID).ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostSaveRecipeAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _context.Attach(Recipe).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _context.Ingredient.UpdateRange(Ingredients);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(Recipe.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task OnPostAddIngredientAsync()
        {
            Ingredients.Add(new Ingredient());
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.ID == id);
        }

        public async Task OnPostRemoveIngredientAsync(int index)
        {
            Ingredients.RemoveAt(index);
            ModelState.Clear();
        }
    }
}
